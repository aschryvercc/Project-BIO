using System;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace DbConnector
{

    public class DbConnector
    {
        //tracks connection info
        private DbConnectorInfo cnnInfo = new DbConnectorInfo();

        private SqlConnection sourceConn = null;  //The source database connection
        private SqlConnection destinationConn = null; //The destination database connection
        private SqlDataAdapter dbDataAdapter = null;  //A data adapter used to bridge the data to a data set object
        private string connectionString = "";   //connection string to a database when connecting the source or destination
        private string sourceDBName = "";   //Name of the source database
        private string destinationDBName = "";  //Name of the destination database
        private List<string> sourceTables = new List<string>(); //List of the names of the source database's tables
        private List<string> destinationTables = new List<string>();    //List of the names of the destination database's tables

        //build select statement.
        private string buildSelect(bool hasJoins, Dictionary<string,string> leftPair, Dictionary<string, string> rightPair, List<string> columns, List<string> conditions)
        {
            string query = "select  ";

            foreach (string column in columns)
            {
                query += column + ", ";
            }
            query = query.Remove(query.Length - 3);

            query += " from " + leftPair.Keys.ElementAt(0) + " ";

            if (hasJoins)
            {
                for (int i = 0; i <= leftPair.Count; i++)
                {
                    query += " joins " + rightPair.Keys.ElementAt(i) + " on " +
                             leftPair.Values.ElementAt(i) + " = " + rightPair.Values.ElementAt(i);
                }
            }

            query += "where ";

            for (int i = 0; i <= conditions.Count; i++)
            {
                query += conditions[i] + ", ";
            }

            query += ";";

            return query;
        }

        //build insert statement
        private string buildInsertUpdate(string table, DataTable data, char useMode)
        {
            string query = "";
            if (useMode == 'i')
            {
                query = "insert into " + table + " (";
            }
            else if (useMode == 'u')
            {
                query = "update " + table + " (";
            }

            //insert columns to be inserted into
            foreach (DataColumn column in data.Columns)
            {
                query += column.ColumnName + ", ";
            }
            query = query.Remove(query.Length - 3);

            query += " values ";
            //insert data
            foreach (DataRow row in data.Rows)
            {
                query += "(";
                foreach (String val in row.ItemArray)
                {
                    query += val + ", ";
                }
                query = query.Remove(query.Length - 3);
                query += "), ";
            }
            query = query.Remove(query.Length - 3);
            query += ";";

            return query;
        }
        
        //execute pull into datatable
        public DataTable PullData(bool hasJoins, Dictionary<string, string> leftPair, Dictionary<string, string> rightPair, List<string> columns, List<string> conditions)
        {
            DataTable pulledContents = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = makeCnnString();
            SqlCommand cmd = new SqlCommand(buildSelect(hasJoins, leftPair, rightPair, columns, conditions), cnn);
           
            try
            {
                cnn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(pulledContents);
                cnn.Close();
                da.Dispose();
            }
            catch (Exception ex)
            {
                //Add exception handling...
            }

            return pulledContents;
        }

        public void insertUpdate(string table, DataTable data, char useMode)
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = makeCnnString();
            SqlCommand cmd = new SqlCommand(buildInsertUpdate(table, data, useMode), cnn);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch
            {
                //Add exception handling...
            }
        }

        //creates connection string
        private string makeCnnString()
        {
            return
                "Data Source = " + cnnInfo.serverName + ";" +
                "Initial Catalog = " + cnnInfo.serverName + "; " +
                "User ID = " + cnnInfo.userName + ";" +
                "Password = " + cnnInfo.password;
        }

        /*
         * Method Name: InitData
         * Parameters: string server, string userid, string password, string database
         * Return: SqlConnection
         * Description: This will initalize the data of the form. In this case the method
         * will create a SQL Server connection that it will return based on the passed parameters.
         */
        public SqlConnection InitData(string server, string userid, string password, string database)
        {
            //Setup the connection string
            SqlConnection conn = null;
            string connectionString = @"server=" + server + ";userid=" + userid + ";password=" + password + ";database=" + database;

            //Try to connect to the database based on the connection string
            //Also fill the table list immediately since the database is currently selected
            try
            {
                //Open connection to database
                conn = new SqlConnection(connectionString);
                conn.Open();
            }
            catch (SqlException ex)
            {
                /*
                 * TODO: Handle exception.
                 */
            }

            //Return the new MySql Connection
            return conn;
        }

        //Method Name: CloseDBConnection
        //Parameters: None
        //Return: void
        //Description: The event handler for when the form is about to close. The method
        //  will close any lingering connections.
        public void CloseDBConnection()
        {
            //Close the connections before the application closes
            if (sourceConn != null &&
                sourceConn.State == ConnectionState.Open)
            {
                sourceConn.Close();
            }
            if (destinationConn != null &&
                destinationConn.State == ConnectionState.Open)
            {
                destinationConn.Close();
            }
        } 

        /*
         * Method Name: ExtractTables
         * Parameters: SqlConnection conn
         * Return: List<string>
         * Description: This will extract a list of table names from the MySql database from the connection.
         */
        private List<string> ExtractTables(SqlConnection conn)
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
    }
}