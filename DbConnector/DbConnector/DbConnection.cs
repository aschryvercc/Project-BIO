using System;
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
        private string connectionString = "";   //connection string to a database when connecting the source or destination
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

        /*
         * Method Name: PullData
         * Parameters: 
         bool hasJoins -- indicates whether the select statement has joins
         Dictionary<string,string> tablePair -- a dictionary of tables with their identifying column for use in creating joins. (Table, tableID)
         List<string> columns -- list of columns to select.
         List<string> conditions -- the where conditions of the statement.
         * Return: DataTable PullData -- a datatable containing the pulled data
         * Description: The method executes a query to a database based off a dynamically created select statement from a group of joined tables.
         */
        public DataTable PullData(Dictionary<string, string> tablePair, List<string> columns, List<string> conditions)
        {
            DataTable pulledContents = new DataTable();
            SqlCommand cmd = new SqlCommand(buildSelect(tablePair, columns, conditions), sourceConn);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(pulledContents);
                da.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex); 
            }

            return pulledContents;
        }

        /*
         * Method Name: PullData
         * Parameters: 
         bool hasJoins -- indicates whether the select statement has joins
        String Table -- The table to be pulled from.
         List<string> columns -- list of columns to select.
         List<string> conditions -- the where conditions of the statement.
         * Return: DataTable PullData -- a datatable containing the pulled data
         * Description: The method executes a query to a database based off a dynamically created select statement from a single table.
         */
        public DataTable PullData(string table, List<string> columns, List<string> conditions)
        {
            DataTable pulledContents = new DataTable();
            SqlCommand cmd = new SqlCommand(buildSelect(table, columns, conditions), sourceConn);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(pulledContents);
                da.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return pulledContents;
        }

        /*
         * Method Name: insertUpdate
         * Parameters: 
         string table -- table to be inserted into
         DataTable data -- a datatable containing the data to be inserted.
         char useMode -- sets the buildInsertUpdate function to either create an insert or update statement.
         * Return: void
         * Description: This method executes either an insert or update statement to a database.
         */
        public void insertUpdate(string table, DataTable data, char useMode)
        {
            SqlCommand cmd = new SqlCommand(buildInsertUpdate(table, data, useMode), sourceConn);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }

        /*
         * Method Name: buildSelect
         * Parameters: 
         Dictionary<string,string> tablePair -- a dictionary of tables with their identifying column for use in creating joins. (Table, tableID)
         List<string> columns -- list of columns to select.
         List<string> conditions -- the where conditions of the statement.
         * Return: void
         * Description: The method dynamically creates a select statement based off the parameters it is passed for multiple joined tables.
         */
        private string buildSelect(Dictionary<string,string> tablePair, List<string> columns, List<string> conditions)
        {
            //begin query
            string query = "select  ";

            //add columns to be selected
            if (columns != null)
            {
                foreach (string column in columns)
                {
                    query += column + ", ";
                }
                query = query.Remove(query.Length - 3);
            }
            //if the list is null, assume select all columns
            else
            {
                query += "* ";
            }

            //add first table to join
            query += " from " + tablePair.Keys.ElementAt(0) + " ";
            
            //incrememntally add remaining tables and comparer.
            for (int i = 0; i < tablePair.Count; i++)
            {
                query += " joins " + tablePair.Keys.ElementAt(i + 1) + " on " +
                            tablePair.Values.ElementAt(i) + " = " + tablePair.Values.ElementAt(i+1);
            }

            //add conditions if applicable.
            if (conditions != null)
            {
                query += " where ";

                for (int i = 0; i <= conditions.Count; i++)
                {
                    query += conditions[i] + ", ";
                }
            }

            query += ";";

            return query;
        }

        /*
         * Method Name: buildSelect
         * Parameters: 
         Dictionary<string,string> tablePair -- a dictionary of tables with their identifying column for use in creating joins. (Table, tableID)
         List<string> columns -- list of columns to select.
         List<string> conditions -- the where conditions of the statement.
         * Return: void
         * Description: The method dynamically creates a select statement based off the parameters it is passed for multiple joined tables.
         */
        private string buildSelect(string table, List<string> columns, List<string> conditions)
        {
            //begin query
            string query = "select  ";

            //add columns to be selected
            if (columns != null)
            {
                foreach (string column in columns)
                {
                    query += column + ", ";
                }
                query = query.Remove(query.Length - 3);
            }

            //if the list is null, assume select all columns
            else
            {
                query += "* ";
            }

            query += " from " + table;

            //add conditions if applicable.
            if (conditions != null)
            {
                query += " where ";
                for (int i = 0; i <= conditions.Count; i++)
                {
                    query += conditions[i] + ", ";
                }
                query = query.Remove(query.Length - 3);
            }

            query += ";";

            return query;
        }

        /*
         * Method Name: buildInsertUpdate
         * Parameters: 
         string table -- table to be inserted into
         DataTable data -- a datatable containing the data to be inserted.
         char useMode -- sets the buildInsertUpdate function to either create an insert or update statement.
         * Return: void
         * Description: This method builds an insert or update statement based off a Datatable.
         */
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
        public DataTable checkMissingRows(DbConnectorInfo destinationConnInfo, string leftT, string leftID, string rightT, string rightID)
        {
            DataTable leftContents = new DataTable();
            DataTable rightContents = new DataTable();
            DataTable missingContents = new DataTable();
            bool rowPresent = false;

            try
            {

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
                cmd = new SqlCommand("Select * from " + rightT, InitData(destinationConnInfo.server, 
                    destinationConnInfo.userid,destinationConnInfo.password, destinationConnInfo.database));

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
            }
            catch (Exception ex)
            {
                throw (ex);
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

        /*
         * Method Name: updateLeft
         * Parameters:
         * Description: Pulls updated row information from mgmt database, pulls those changes from the left database and pushes them to the right.
         */
        public DataTable updateLeft(DbConnectorInfo destinationConn, DbConnectorInfo mgmtDB, List<string> whereClauses, string leftTable, string rightTable)
        {
            DataTable markedRows = new DataTable();
            DataTable missingContents = new DataTable();
            List<string> rowsToFind = new List<string>();
            SqlCommand cmd = new SqlCommand(buildSelect("updateLog", null, whereClauses), InitData(mgmtDB.server, mgmtDB.userid, mgmtDB.password, mgmtDB.database));
            
            //get data from mgmt database
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(markedRows);
                da.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //translate into where clauses.
            foreach (DataRow i in markedRows.Rows)
            {
                string clauseToAdd = "";
                if (i == markedRows.Rows[0])
                {
                    clauseToAdd = i.Field<string>("rowName") + " = " + i.Field<string>("rowKey");
                }
                else
                {
                    clauseToAdd = i.Field<string>("rowKey");
                }
                if (i != markedRows.Rows[markedRows.Rows.Count])
                {
                    clauseToAdd += " || ";
                }
                rowsToFind.Add(clauseToAdd);
            }

            //pull missing data
            cmd = new SqlCommand(buildSelect(leftTable, null, rowsToFind), sourceConn);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(missingContents);
                da.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //push new data
            cmd = new SqlCommand(buildInsertUpdate(rightTable, missingContents, 'u'), 
                InitData(destinationConn.server, destinationConn.userid, destinationConn.password, destinationConn.database));

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return missingContents;
        }

    }
}