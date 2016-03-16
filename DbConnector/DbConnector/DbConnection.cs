﻿using System;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace DbConnector
{

    public class DbConnection
    {
        //tracks connection info
        private DbConnectorInfo sourceDbConnectorInfo; //The source connection information
        private SqlConnection sourceConn = null;  //The source database connection
        private SqlDataAdapter dbDataAdapter = null;  //A data adapter used to bridge the data to a data set object
        private string connectionString = "";   //connection string to a database when connecting the source or destination
        //private string sourceDBName = "";   //Name of the source database
        //private string destinationDBName = "";  //Name of the destination database
        //private List<string> sourceTables = new List<string>(); //List of the names of the source database's tables
        //private List<string> destinationTables = new List<string>();    //List of the names of the destination database's tables

        private bool _ConnectionOpen;
        public bool ConnectionOpen
        {
            get { return _ConnectionOpen; }
        }

        /*
         * Constructor
         */
        public DbConnection(DbConnectorInfo connectionInfo)
        {
            sourceDbConnectorInfo = connectionInfo;
            _ConnectionOpen = false;
        }

        //execute pull into datatable
        public DataTable PullData(bool hasJoins, Dictionary<string, string> leftPair, Dictionary<string, string> rightPair, List<string> columns, List<string> conditions)
        {
            DataTable pulledContents = new DataTable();
            SqlCommand cmd = new SqlCommand(buildSelect(hasJoins, leftPair, rightPair, columns, conditions), sourceConn);

            try
            {
                //OpenDBConnection();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(pulledContents);
                //CloseDBConnection();
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
            SqlCommand cmd = new SqlCommand(buildInsertUpdate(table, data, useMode), sourceConn);

            try
            {
                //OpenDBConnection();
                cmd.ExecuteNonQuery();
                //CloseDBConnection();
            }
            catch
            {
                //Add exception handling...
            }
        }

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

        /*
         * Method Name: InitData
         * Parameters: string server, string userid, string password, string database
         * Return: SqlConnection
         * Description: The method will create a SQL Server connection that it will return based on the passed parameters.
         */
        private SqlConnection InitData(string server, string userid, string password, string database)
        {
            //Setup the connection string
            SqlConnection conn = null;
            connectionString = @"server=" + server + ";userid=" + userid + ";password=" + password + ";database=" + database;

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

        /*
         * Method Name: OpenDBConnection
         * Parameters: void
         * Return: void
         * Description: The method will open a connection to a database and get the 
         * database information used to create the connection.
         */
        public void OpenDBConnection()
        {
            //If logging into the source database connect to it
            sourceConn = InitData(sourceDbConnectorInfo.server, sourceDbConnectorInfo.userid, 
                                  sourceDbConnectorInfo.password, sourceDbConnectorInfo.database);

            _ConnectionOpen = true;
            //sourceTables = ExtractTables(sourceConn);
            //sourceDBName = sourceDbConnectorInfo.database;
        }

        /*
         * Method Name: CloseDBConnection
         * Parameters: void
         * Return: void
         * Description: The method will close any lingering connections.
         */
        public void CloseDBConnection()
        {
            //Close the connections before the application closes
            if (sourceConn != null &&
                sourceConn.State == ConnectionState.Open)
            {
                sourceConn.Close();
            }

            _ConnectionOpen = false;
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
         * Method Name: checkMissingRows
         * Parameters: 
         *  DbConnectorInfo compareDB -- Connection info for second database
         *  string leftT -- Left table to be compared against
         *  string leftID -- right table to be compared against
         *  string rightT -- table ID
         *  string rightID -- table ID
         * Return: DataTable missingContents -- contents to be synced over
         * Description: This will compare two tables in two databases and find any missing rows.
         */
        private DataTable checkMissingRows(SqlConnection destinationConn, string leftT, string leftID, string rightT, string rightID)
        {
            DataTable leftContents = new DataTable();
            DataTable rightContents = new DataTable();
            DataTable missingContents = new DataTable();
            bool rowPresent = false;

            try
            {
                /*
                * Open the database for sql commands.
                */
                //OpenDBConnection();

                #region select statement for table 1
                /*
                 * Select statement for table 1
                 */
                SqlCommand cmd = new SqlCommand("Select * from " + leftT, sourceConn);

                /*
                 * Run the command and get the results.
                 */
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(leftContents);

                /*
                 * Dispose the sql adapter.
                 */
                da.Dispose();
                #endregion

                #region select statement for table 2
                /*
                 * Select statement for table 2
                 */
                cmd = new SqlCommand("Select * from " + rightT, destinationConn);

                /*
                 * Run the command and get the results.
                 */
                da = new SqlDataAdapter(cmd);
                da.Fill(rightContents);

                /*
                 * Dispose the sql adapter.
                 */
                da.Dispose();
                #endregion

                /*
                 * Close the database connection.
                 */
                //CloseDBConnection();
            }
            catch (Exception ex)
            {
                /*
                 * Do something with the exception.
                 */
            }

            //iterate through each row and check if every instance of the left side exists in the right. If not, add to the missing contents.

            //iterate left side.
            foreach (DataRow leftRow in leftContents.Rows)
            {
                //iterate right side.
                foreach (DataRow rightRow in rightContents.Rows)
                {
                    //if row is found, mark so, break and continue.
                    if (leftRow.Field<Type>(leftID) == rightRow.Field<Type>(rightID))
                    {
                        rowPresent = true;
                        break;
                    }
                }
                //if row is not found, add to list to be synced.
                if (rowPresent != true)
                {
                    missingContents.Rows.Add(leftRow);
                }
                rowPresent = false;
            }

            return missingContents;
        }

        private DataTable checkUpdateRows(SqlConnection destinationConn, string leftT, string leftID, string rightT, string rightID)
        {
            DataTable leftContents = new DataTable();
            DataTable rightContents = new DataTable();
            DataTable missingContents = new DataTable();
            bool rowPresent = false;

            try
            {
                /*
                 * Open the database for sql commands.
                 */
                //OpenDBConnection();

                #region select statement for table 1
                /*
                 * Select statement for table 1
                 */
                SqlCommand cmd = new SqlCommand("Select * from " + leftT, sourceConn);

                /*
                 * Run the command and get the results.
                 */
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(leftContents);

                /*
                 * Dispose the sql adapter.
                 */
                da.Dispose();
                #endregion

                #region select statement for table 2
                /*
                 * Select statement for table 2
                 */
                cmd = new SqlCommand("Select * from " + rightT, destinationConn);

                /*
                 * Run the command and get the results.
                 */
                da = new SqlDataAdapter(cmd);
                da.Fill(leftContents);

                /*
                 * Dispose the sql adapter.
                 */
                da.Dispose();
                #endregion

                /*
                 * Close the database connection.
                 */
                //CloseDBConnection();
            }
            catch (Exception ex)
            {
                /*
                 * Do something with the exception.
                 */
            }

            //iterate through each row and check if every instance of the left side exists in the right. If not, add to the missing contents.

            //iterate left side.
            foreach (DataRow leftRow in leftContents.Rows)
            {
                //iterate right side.
                foreach (DataRow rightRow in rightContents.Rows)
                {
                    //iterate through fields
                    List<string> leftVals = new List<string>();
                    List<string> rightVals = new List<string>();

                    //add all fields to seperate lists for comparison.
                    foreach (string value in leftRow.ItemArray)
                    {
                        leftVals.Add(value);
                    }
                    foreach (string value in rightRow.ItemArray)
                    {
                        rightVals.Add(value);
                    }
                    //iterate through list, if they do not match, an update is required and the data should be added to be carried over.
                    for (int i = 0; i <= leftVals.Count; i++)
                    {
                        if (leftVals[i] != rightVals[i])
                        {
                            missingContents.Rows.Add(leftRow);
                        }
                    }
                }
            }

            return missingContents;
        }

    }
}