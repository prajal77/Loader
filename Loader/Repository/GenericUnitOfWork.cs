using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Loader.Repository
{
    public sealed class GenericUnitOfWork : IGenericUnitOfWork, IDisposable
    {
        private ApplicationDbContext entities = null;



        public GenericUnitOfWork()
        {
            entities = new ApplicationDbContext();
        }


        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IGenericRepository<T>;
            }
            IGenericRepository<T> repo = new GenericRepository<T>(entities);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public int ExecuteStoreProcedure(string query, params object[] parameters)
        {
            return entities.Database.ExecuteSqlCommand("EXEC " + query, parameters);
        }

        public string ExecuteStoreProcedurecommand(string query)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            String SqlconString = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
            if (SqlconString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(SqlconString);
                SqlconString = efBuilder.ProviderConnectionString;
            }
            var result = "";

            using (SqlConnection conn = new SqlConnection(SqlconString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    result = (string)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return (string)result;

        }


        public int Commit()
        {
            return entities.SaveChanges();
        }

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ApplicationDbContext GetContext()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
        
            return ctx;
        }

        public int ExecuteStoreProcedureCommandWithOuput(string query)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            String SqlconString = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
            if (SqlconString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(SqlconString);
                SqlconString = efBuilder.ProviderConnectionString;
            }
            int result = 0; // Change the type to int to hold the integer value of Rid

            using (SqlConnection conn = new SqlConnection(SqlconString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    // Execute the query and retrieve the result as an integer
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result; // Return the integer value of Rid
        }

        public decimal ExecuteStoreProcedureToGetReturnedId(string query, params object[] parameters)
        {
            var result = entities.Database.SqlQuery<decimal>("EXEC " + query, parameters).FirstOrDefault();
            return result;
        }

    }

}