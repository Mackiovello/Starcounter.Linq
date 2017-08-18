﻿using System;
using System.Linq.Expressions;

namespace Starcounter.Linq.Visitors
{
    public class WhereVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static WhereVisitor<TEntity> Instance = new WhereVisitor<TEntity>();

        public override void VisitBinary(BinaryExpression node, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("(");
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    Visit(node.Left, state);
                    state.WriteWhere(" > ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    Visit(node.Left, state);
                    state.WriteWhere(" >= ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.LessThan:
                    Visit(node.Left, state);
                    state.WriteWhere(" < ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.LessThanOrEqual:
                    Visit(node.Left, state);
                    state.WriteWhere(" <= ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.Equal:
                    Visit(node.Left, state);
                    state.WriteWhere(" = ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.NotEqual:
                    state.WriteWhere("NOT ");
                    Visit(node.Left, state);
                    state.WriteWhere(" = ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    Visit(node.Left, state);
                    state.WriteWhere(" AND ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    Visit(node.Left, state);
                    state.WriteWhere(" OR ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Add:
                    Visit(node.Left, state);
                    state.WriteWhere(" + ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Subtract:
                    Visit(node.Left, state);
                    state.WriteWhere(" - ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Multiply:
                    Visit(node.Left, state);
                    state.WriteWhere(" * ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Divide:
                    Visit(node.Left, state);
                    state.WriteWhere(" / ");
                    Visit(node.Right, state);
                    break;

                default:
                    throw new NotSupportedException();
            }

            state.WriteWhere(")");
        }

        public override void VisitBlock(BlockExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitBlock(node, state);
        }

        public override void VisitConditional(ConditionalExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitConditional(node, state);
        }

        public override void VisitConstant(ConstantExpression node, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("?");
            state.AddVariable(node.Value);
        }

        public override void VisitDebugInfo(DebugInfoExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitDebugInfo(node, state);
        }

        public override void VisitDynamic(DynamicExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitDynamic(node, state);
        }

        public override void VisitDefault(DefaultExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitDefault(node, state);
        }

        public override void VisitInvocation(InvocationExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitInvocation(node, state);
        }

        public override void VisitLambda(LambdaExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitLambda(node, state);
        }

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Expression is ParameterExpression param)
            {
                state.WriteWhere(param.Type.SourceName());
            }
            else
            {
                Visit(node.Expression, state);
            }
            state.WriteWhere("." + node.Member.Name);
        }

        public override void VisitIndex(IndexExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitIndex(node, state);
        }

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Method == KnownMethods.StringContains)
            {
                state.WriteWhere("(");
                Visit(node.Object, state);
                state.WriteWhere(" LIKE '%' || ? || '%')");
            }
            if (node.Method == KnownMethods.StringStartsWith)
            {
                state.WriteWhere("(");
                Visit(node.Object, state);
                state.WriteWhere(" LIKE ? || '%')");
            }
            if (node.Method == KnownMethods.StringEndsWith)
            {
                state.WriteWhere("(");
                Visit(node.Object, state);
                state.WriteWhere(" LIKE '%' || ?)");
            }
        }

        public override void VisitNewArray(NewArrayExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitNewArray(node, state);
        }

        public override void VisitNew(NewExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitNew(node, state);
        }

        public override void VisitParameter(ParameterExpression node, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("?");
        }

        public override void VisitRuntimeVariables(RuntimeVariablesExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitRuntimeVariables(node, state);
        }

        public override void VisitUnary(UnaryExpression node, QueryBuilder<TEntity> state)
        {
            var body = (node.Operand as LambdaExpression).Body;
            Visit(body,state);
         //   base.VisitUnary(node, state);
        }
    }
}