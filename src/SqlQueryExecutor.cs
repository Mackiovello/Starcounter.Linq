﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Remotion.Linq;

namespace Starcounter.Linq
{
    public class EmptyQueryExecutor : SqlQueryExecutor
    {
        public override T ExecuteScalar<T>(QueryModel queryModel)
        {
            ExecuteCollection<T>(queryModel);
            return default(T);
        }

        // Executes a query with a single result object, i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
        public override T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            ExecuteCollection<T>(queryModel);
            return default(T);
        }

        public override IEnumerable<T1> ExecuteCollection<T1>(QueryModel queryModel)
        {
            var commandData = SqlGeneratorQueryModelVisitor.GenerateSqlQuery(queryModel);
            var args = commandData.QueryVariables.Select(n => n.Value).ToArray();

            return null;
        }
    }

    public class SqlQueryExecutor : IQueryExecutor
    {
        public SqlQueryExecutor()
        {
            
        }
        // Executes a query with a scalar result, i.e. a query that ends with a result operator such as Count, Sum, or Average.
        public virtual T ExecuteScalar<T>(QueryModel queryModel)
        {
            if (typeof(T) == typeof(int))
            {
                var res = ExecuteCollection<long>(queryModel).Single();
                return (T) Convert.ChangeType(res, typeof(int));
            }
            if (typeof(T) == typeof(double))
            {
                var res = ExecuteCollection<decimal>(queryModel).Single();
                return (T) Convert.ChangeType(res, typeof(double));
            }
            else
            {
                var res = ExecuteCollection<T>(queryModel);
                return res.Single();
            }
        }

        // Executes a query with a single result object, i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
        public virtual T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            if (typeof(T) == typeof(int))
            {
                var res = ExecuteCollection<long>(queryModel).Single();
                return (T) Convert.ChangeType(res, typeof(int));
            }
            if (typeof(T) == typeof(double))
            {
                var res = ExecuteCollection<decimal>(queryModel).Single();
                return (T) Convert.ChangeType(res, typeof(double));
            }
            else
            {
                var res = ExecuteCollection<T>(queryModel);
                return returnDefaultWhenEmpty ? res.SingleOrDefault() : res.Single();
            }
        }

        // Executes a query with a collection result.
        public virtual IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var commandData = SqlGeneratorQueryModelVisitor.GenerateSqlQuery(queryModel);
            var args = commandData.QueryVariables.Select(n => n.Value).ToArray();
            //Debug.WriteLine($"SQL: {commandData.Statement}");

            var res = Db.SQL<T>(commandData.Statement, args);
            return res;
        }
    }
}