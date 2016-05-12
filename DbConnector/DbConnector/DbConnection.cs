using System;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using System.Linq;
using System.Web;

namespace DbConnector
{

    public class DbConnection
    {
        private DbConnectorInfo dbConnectorInfo; //The source connection information
        private MySqlConnection MySqlSourceConn;
        private SqlConnection SqlServerSourceConn;  //The source database connection
        private string connectionString;   //connection string to a database when connecting the source or destination
        private List<string> sourceTables;
        private bool _connectionOpen;
        public bool ConnectionOpen
        {
            get { return _connectionOpen; }
        }

        /*
         * Constructor
         */
        public DbConnection(DbConnectorInfo connectionInfo)
        {
            dbConnectorInfo = connectionInfo;
            _connectionOpen = false;
            connectionString = "";
        }

        #region Utility

        private static void ParameterizedQueries
        {

        }

        private static void QueryBuilder
        {

        }

        #endregion

        /*
         * Method Name: CloseDBConnectionLeft/CloseDBConnectionRight
         * Parameters: void
         * Return: void
         * Description: The method will close any lingering connections.
         */
        public void CloseDBConnection()
        {
            //Close the connections before the application closes
            if (SqlServerSourceConn != null &&
                SqlServerSourceConn.State == ConnectionState.Open && dbConnectorInfo.dbtype == "SQL Server")
            {
                SqlServerSourceConn.Close();
            }
            else if (MySqlSourceConn != null &&
                MySqlSourceConn.State == ConnectionState.Open && dbConnectorInfo.dbtype == "MySQL")
            {
                MySqlSourceConn.Close();
            }

            _connectionOpen = false;
        }

        /*
         * Method Name: ExtractTables
         * Parameters: SqlConnection conn
         * Return: List<string>
         * Description: This will extract a list of table names from the MySql database from the connection.
         */
        public List<string> ExtractTables(SqlConnection conn)
        {
            List<string> tempList = new List<string>();

            if (conn != null &&
                conn.State == ConnectionState.Open)
            {
                //This retrieves the table names from the database.
                DataTable schema = conn.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    tempList.Add(row[2].ToString());
                }
            }

            //Return the list of the table names from the database connection
            return tempList;
        }

        /*
        * Method Name: MsInitData/MyInitData
        * Parameters: string server, string userid, string password, string database
        * Return: SqlConnection
        * Description: The method will create a SQL Server connection that it will return based on the passed parameters.
        */
        private SqlConnection MsInitData(string server, string userid, string password, string database)
        {
            //Setup the connection string
            SqlConnection conn = null;
            connectionString = @"server=" + server + ";user id=" + userid + ";password=" + password + ";database=" + database;

            //Try to connect to the database based on the connection string
            //Also fill the table list immediately since the database is currently selected
            try
            {
                //Open connection to database
                conn = new SqlConnection(connectionString);
                conn.Open();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            //Return the new MySql Connection
            return conn;
        }

        private MySqlConnection MyInitData(string server, string userid, string password, string database)
        {
            //Setup the connection string
            MySqlConnection conn = null;
            connectionString = @"server=" + server + "; database=" + database + "; uid=" + userid + "; pwd=" + password;

            //Try to connect to the database based on the connection string
            //Also fill the table list immediately since the database is currently selected
            try
            {
                //Open connection to database
                conn = new MySqlConnection(connectionString);
                conn.Open();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            //Return the new MySql Connection
            return conn;
        }

        /*
         * Method Name: OpenDBConnectionLeft/OpenDbConnectionRight
         * Parameters: void
         * Return: void
         * Description: The method will open a connection to a database.
         */
        public void OpenDBConnection()
        {
            //If logging into the source database connect to it
            if (dbConnectorInfo.dbtype == "SQL Server")
            {
                SqlServerSourceConn = MsInitData(dbConnectorInfo.server, dbConnectorInfo.userid,
                                      dbConnectorInfo.password, dbConnectorInfo.database);
            }
            else if (dbConnectorInfo.dbtype == "MySQL")
            {
                MySqlSourceConn = MyInitData(dbConnectorInfo.server, dbConnectorInfo.userid,
                                      dbConnectorInfo.password, dbConnectorInfo.database);
            }

            _connectionOpen = true;
        }


    }
}