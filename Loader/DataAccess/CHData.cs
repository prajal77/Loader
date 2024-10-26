using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.DataAccess
{
    public class CHData
    {
        public class DataTableWithPager
        {
            public DataTable DataTable { get; set; }
            public Paging Pager { get; set; }

            public DataTableWithPager()
            {
                Pager = new Paging();
                DataTable = new DataTable();
            }
        }

        private static string _ConnectionString = "";
        private static string _ConnectionStringNav = "";
        private static SqlConnection _Connection = new SqlConnection();
        private static SqlConnection _ConnectionNav = new SqlConnection();

        private static ConnectionState PreviousConnectionState = ConnectionState.Closed;

        public static string ConnectionStringNav
        {
            get
            {
                if (_ConnectionStringNav == "")
                {
                    SetServerNameNav();
                }

                return _ConnectionStringNav;
            }
        }

        public static string ConnectionString
        {
            get
            {
                if (_ConnectionString == "")
                {
                    SetServerName();
                }

                return _ConnectionString;
            }
        }

        public static SqlConnection Connection
        {
            get
            {
                if (_Connection.ConnectionString == "")
                {
                    SetServerName();
                    _Connection.ConnectionString = ConnectionString;
                }

                return _Connection;
            }
        }

        public static SqlConnection ConnectionNav
        {
            get
            {
                if (_ConnectionNav.ConnectionString == "")
                {
                    SetServerNameNav();
                    _ConnectionNav.ConnectionString = ConnectionStringNav;
                }

                return _ConnectionNav;
            }
        }
        public static string DBName => Connection.Database;

        public static string DBNameNav => ConnectionNav.Database;

        private static void SetServerName()
        {
            try
            {
                _ConnectionString = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
            }
            catch
            {
                //Persist Security Info=True; Pooling=False
                _ConnectionString = "Data Source=182.93.88.252;Initial Catalog=SchoolMG; Integrated Security=False;User ID=invbilling;Password=invbilling;";
            }
        }

        private static void SetServerNameNav()
        {
            try
            {
                _ConnectionStringNav = ConfigurationManager.ConnectionStrings["ChaInvBillingEntities"].ConnectionString;
            }
            catch
            {
                _ConnectionStringNav = "Data Source=182.93.88.252;Initial Catalog=SchoolMG; Integrated Security=False;User ID=invbilling;Password=invbilling;";
            }
        }

        public static bool OpenConnection()
        {
            if (Connection == null)
            {
                _Connection = new SqlConnection(ConnectionString);
            }

            PreviousConnectionState = Connection.State;
            if (PreviousConnectionState != ConnectionState.Open)
            {
                try
                {
                    Connection.Open();
                    return true;
                }
                catch
                {
                    throw;
                }
            }

            return true;
        }

        public static void CloseConnection()
        {
            PreviousConnectionState = Connection.State;
            if (PreviousConnectionState != 0)
            {
                Connection.Close();
            }
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            try
            {
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CloseConnection(SqlConnection cnn)
        {
            if (cnn != null && cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
        }
        //end database connection

        public static DataTable GetDataTable(string SelectQuery, SqlTransaction SQLTrans = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlDataAdapter sqlDataAdapter;
                if (SQLTrans == null)
                {
                    sqlDataAdapter = new SqlDataAdapter(SelectQuery, ConnectionString);
                }
                else
                {
                    SqlCommand selectCommand = new SqlCommand(SelectQuery, SQLTrans.Connection, SQLTrans);
                    sqlDataAdapter = new SqlDataAdapter(selectCommand);
                }

                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dataTable;
        }

        public static DataTable GetDataTable(SqlCommand command, SqlTransaction SQLTrans = null)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            try
            {
                if (SQLTrans == null)
                {
                    SqlConnection connection = new SqlConnection(ConnectionString);
                    command.Connection = connection;
                    sqlDataAdapter = new SqlDataAdapter();
                    sqlDataAdapter.SelectCommand = command;
                }
                else
                {
                    command.Connection = SQLTrans.Connection;
                    command.Transaction = SQLTrans;
                    sqlDataAdapter = new SqlDataAdapter(command);
                }

                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlDataAdapter.Dispose();
            }

            return dataTable;
        }

        public static object ExecuteCommand(string strQry, SqlTransaction SQLTrans = null)
        {
            object result = null;
            try
            {
                if (SQLTrans != null)
                {
                    SqlCommand sqlCommand = new SqlCommand(strQry, SQLTrans.Connection, SQLTrans);
                    sqlCommand.CommandTimeout = 600;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    DataTable dataTable = new DataTable();
                    try
                    {
                        sqlDataAdapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                        {
                            result = dataTable.Rows[0][0];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    SqlConnection connection = new SqlConnection(ConnectionString);
                    SqlCommand sqlCommand = new SqlCommand(strQry, connection);
                    sqlCommand.CommandTimeout = 600;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    DataTable dataTable = new DataTable();
                    try
                    {
                        sqlDataAdapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                        {
                            result = dataTable.Rows[0][0];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static object ExecuteCommand(SqlCommand Command, SqlTransaction SQLTrans = null)
        {
            object result = null;
            try
            {
                if (SQLTrans != null)
                {
                    Command.Connection = SQLTrans.Connection;
                    Command.Transaction = SQLTrans;
                    Command.CommandTimeout = 600;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(Command);
                    DataTable dataTable = new DataTable();
                    try
                    {
                        sqlDataAdapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                        {
                            result = dataTable.Rows[0][0];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(ConnectionString);
                        Command.Connection = connection;
                        Command.CommandTimeout = 600;
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(Command);
                        DataTable dataTable = new DataTable();
                        if (dataTable.Rows.Count > 0)
                        {
                            result = dataTable.Rows[0][0];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SelectListItem> GetStatusList()
        {
            string query = "select distinct StatusId, StatusName from FgetMasterGrade()";
            DataTable dt = CHData.GetDataTable(query);
            List<SelectListItem> lists = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lists.Add(new SelectListItem { Value = dr["StatusId"].ToString(), Text = dr["StatusName"].ToString() });
                }
            }
            return lists;
        }

    }
}