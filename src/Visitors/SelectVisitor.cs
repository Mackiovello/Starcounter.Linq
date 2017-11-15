﻿using System.Linq.Expressions;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq.Visitors
{
    public class SelectVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static SelectVisitor<TEntity> Instance = new SelectVisitor<TEntity>();

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            if (node.Expression is ParameterExpression param)
                state.WriteSelect(param.Type.SourceName());
            else
                Visit(node.Expression, state);
            state.WriteSelect("." + SqlHelper.EscapeIfReservedWord(node.Member.Name));
        }
    }
}
