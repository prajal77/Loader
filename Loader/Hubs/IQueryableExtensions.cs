using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Web;

namespace Loader.Hubs
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        //public static string ToTraceQuery<T>(this IQueryable<T> query)
        //{
        //    ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

        //    var result = objectQuery.ToTraceString();
        //    foreach (var parameter in objectQuery.Parameters)
        //    {
        //        var name = "@" + parameter.Name;
        //        var value = "'" + parameter.Value.ToString() + "'";
        //        result = result.Replace(name, value);
        //    }

        //    return result;
        //}

        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL and Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceString<T>(this DbContext dbContext, IQueryable<T> query) where T :EntityBase
        {
            ObjectQuery objectQuery = dbContext.GetObjectQuery<T>(query);

            var traceString = new StringBuilder();

            traceString.AppendLine(objectQuery.ToTraceString());
            traceString.AppendLine();

            foreach (var parameter in objectQuery.Parameters)
            {
                traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
            }

            return traceString.ToString();
        }
        public static ObjectQuery GetObjectQuery<T>(DbContext dbContext, IQueryable query) where T : EntityBase
        {
            if (query is ObjectQuery)
                return query as ObjectQuery;

            if (dbContext == null)
                throw new ArgumentException("dbContext cannot be null");

            var objectSet = dbContext.UnderlyingContext().CreateObjectSet<T>();
            var iQueryProvider = ((IQueryable)objectSet).Provider;

            // Use the provider and expression to create the ObjectQuery.
            return (ObjectQuery)iQueryProvider.CreateQuery(query.Expression);
        }
        //private static System.Data.Entity.Core.Objects.ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
        //{
        //    var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
        //    var internalQuery = internalQueryField.GetValue(query);
        //    var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
        //    return objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery<T>;
        //}
    }
}