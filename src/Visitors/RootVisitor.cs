﻿using System.Linq.Expressions;

namespace Starcounter.Linq.Visitors
{
    public class RootVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static readonly RootVisitor<TEntity> Instance = new RootVisitor<TEntity>();
        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            var method = node.Method;
            if (method == KnownMethods<TEntity>.QueryableFirstOrDefaultPred)
            {
                state.Fetch(1);
                var expression = node.Arguments[0];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<TEntity>.IQueryableFirstOrDefaultPred)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                state.Fetch(1);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<TEntity>.IQueryableWhere)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<TEntity>.IQueryableCountPredicate)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
                state.WriteSelect($"COUNT({state.GetSourceName()})");
            }
            else if (method == KnownMethods<TEntity>.IQueryableTake)
            {
                //TODO: hack'ish, this assumes the Take argument is a constant int
                // ReSharper disable once PossibleNullReferenceException
                var value = (int)(node.Arguments[1] as ConstantExpression).Value;
                state.Fetch(value);
                Visit(node.Arguments[0], state);
            }
            else if (method == KnownMethods<TEntity>.IQueryableSkip)
            {
                //TODO: hack'ish, this assumes the Skip argument is a constant int
                // ReSharper disable once PossibleNullReferenceException
                var value = (int)(node.Arguments[1] as ConstantExpression).Value;
                state.Offset(value);
                Visit(node.Arguments[0], state);
            }
            else if (method == KnownMethods<TEntity>.IQueryableGroupBy)
            {
                var left = node.Arguments[0];
                Visit(left, state);
                var expression = node.Arguments[1];
                GroupByVisitor<TEntity>.Instance.Visit(expression, state);
            }
            else if (method.IsGenericMethod)
            {
                var gen = method.GetGenericMethodDefinition();

                //Why cannot this be done like the above?
                //Because OrderBy use Func<T,TKeySelector> where TKeySelector is unknown to us here
                //That is, it is the return type of the property
                if (gen == KnownMethods.IQueryableOrderBy)
                {
                    VisitOrderBy(node, state, true);
                }
                else if (gen == KnownMethods.IQueryableOrderByDesc)
                {
                    VisitOrderBy(node, state, false);
                }
                else if (gen == KnownMethods.IQueryableThenBy)
                {
                    VisitOrderBy(node, state, true);
                }
                else if (gen == KnownMethods.IQueryableThenByDesc)
                {
                    VisitOrderBy(node, state, false);
                }
                else if (gen == KnownMethods.IQueryableCount)
                {
                    var left = node.Arguments[0];
                    Visit(left, state);
                    state.WriteSelect($"COUNT({state.GetSourceName()})");
                }
                else if (gen == KnownMethods.IQueryableAverage)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("AVG(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
                }
                else if (gen == KnownMethods.IQueryableMin)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("MIN(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
                }
                else if (gen == KnownMethods.IQueryableMax)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("MAX(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
                }
                else if (gen == KnownMethods.IQueryableSum)
                {
                    var left = node.Arguments[0];
                    var right = node.Arguments[1];
                    Visit(left, state);
                    state.WriteSelect("SUM(");
                    SelectVisitor<TEntity>.Instance.Visit(right, state);
                    state.WriteSelect(")");
                }
            }
            else
            {
             //   throw new NotSupportedException();
            }
        }

        private void VisitOrderBy(MethodCallExpression node, QueryBuilder<TEntity> state, bool asc)
        {
            var left = node.Arguments[0];
            Visit(left, state);
            state.BeginOrderBySection();
            var arg = node.Arguments[1];
            OrderByVisitor<TEntity>.Instance.Visit(arg, state);
            state.EndOrderBySection(asc);
        }

        private static void VisitWhere(Expression expression, QueryBuilder<TEntity> state)
        {
            state.BeginWhereSection();
            WhereVisitor<TEntity>.Instance.Visit(expression, state);
            state.EndWhereSection();
        }

        public override void VisitLambda(LambdaExpression node, QueryBuilder<TEntity> state)
        {
            var arg = node.Parameters[0];

            WhereVisitor<TEntity>.Instance.Visit(node.Body, state);
        }
    }
}
