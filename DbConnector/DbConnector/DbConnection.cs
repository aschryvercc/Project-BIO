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
        //tracks connection info
        private DbConnectorInfo SourceDbConnectorInfoLeft; //The source connection information
        private DbConnectorInfo SourceDbConnectorInfoRight; //The source connection information
        private DbConnectorInfo mgmtConnInfo;
        private MySqlConnection MySourceConnLeft;
        private MySqlConnection MySourceConnRight;
        private SqlConnection MSSourceConnLeft = null;  //The source database connection
        private SqlConnection MSSourceConnRight = null;  //The source database connection
        private SqlConnection mgmtConn = null;
        private string connectionString = "";   //connection string to a database when connecting the source or destination
        private bool _ConnectionOpenLeft;
        private bool _ConnectionOpenRight;
        private bool _mgmtOpen;
        private string _leftSide;
        private string _rightSide;
        public bool ConnectionOpenLeft
        {
            get { return _ConnectionOpenLeft; }
        }
        public bool ConnectionOpenRight
        {
            get { return _ConnectionOpenRight; }
        }
        public bool mgmtOpen
        {
            get { return _mgmtOpen; }
        }
        public string leftSide
        {
            get { return _leftSide; }
        }
        public string rightSide
        {
            get { return _rightSide; }
        }

        /*
         * Constructor
         */
        public DbConnection(DbConnectorInfo connectionInfo1, DbConnectorInfo connectionInfo2, DbConnectorInfo mgmtInfo,  string left, string right)
        {
            SourceDbConnectorInfoLeft = connectionInfo1;
            SourceDbConnectorInfoRight = connectionInfo2;
            mgmtConnInfo = mgmtInfo;
            _ConnectionOpenLeft = false;
            _ConnectionOpenRight = false;
            _mgmtOpen = false;
            _leftSide = left;
            _rightSide = right;
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
        public DataTable MSPullData(Dictionary<string, string> tablePair, List<string> columns, List<string> conditions, char cond)
        {
            DataTable pulledContents = new DataTable();
            SqlCommand cmd;

            if (cond == 'L')
                cmd = new SqlCommand(buildSelect(tablePair, columns, conditions), MSSourceConnLeft);
            else if (cond == 'R')
                cmd = new SqlCommand(buildSelect(tablePair, columns, conditions), MSSourceConnRight);
            else
                return null;

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
        public DataTable MSPullData(string table, List<string> columns, List<string> conditions, char cond)
        {
            DataTable pulledContents = new DataTable();
            SqlCommand cmd;

            if (cond == 'L')
                cmd = new SqlCommand(buildSelect(table, columns, conditions), MSSourceConnLeft);
            else if (cond == 'R')
                cmd = new SqlCommand(buildSelect(table, columns, conditions), MSSourceConnRight);
            else
                return null;

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
         * Method Name: MsInsert
         * Parameters: 
         string table -- table to be inserted into
         DataTable data -- a datatable containing the data to be inserted.
         char useMode -- sets the buildInsertUpdate function to either create an insert or update statement.
         * Return: void
         * Description: This method executes either an insert or update statement to a database.
         */
        public void Msinsert(string table, DataTable data, char cond)
        {
            SqlCommand cmd;


            if (cond == 'l')
                cmd = new SqlCommand(buildInsert(table, data), MSSourceConnLeft);
            else if (cond == 'r')
                cmd = new SqlCommand(buildInsert(table, data), MSSourceConnRight);
            else
                cmd = null;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }


        public void MsUpdate(string table, string idRow, DataTable data, char cond)
        {
            SqlCommand cmd;
            List<string> cols = new List<string>();

            foreach (DataColumn col in data.Columns)
            {
                cols.Add(col.ColumnName);
            }

            foreach (DataRow row in data.Rows)
            {
                if (cond == 'l')
                    cmd = new SqlCommand(buildUpdate(table, row.Field<string>(idRow), idRow, cols, row), MSSourceConnLeft);
                else if (cond == 'r')
                    cmd = new SqlCommand(buildUpdate(table, row.Field<string>(idRow), idRow, cols, row), MSSourceConnRight);
                else
                    cmd = null;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
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
                    query += conditions[i] + " ";
                }
                query = query.Remove(query.Length - 3);
            }

            query += ";";

            return query;
        }

        /*
         * Method Name: buildInsert
         * Parameters: 
         string table -- table to be inserted into
         DataTable data -- a datatable containing the data to be inserted.
         char useMode -- sets the buildInsertUpdate function to either create an insert or update statement.
         * Return: void
         * Description: This method builds an insert statement based off a Datatable.
         */
        private string buildInsert(string table, DataTable data)
        {
            string query = "insert into " + table + " (";

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
         * Method Name: buildUpdate
         * Parameters: 
         string table -- table to be inserted into
         DataTable data -- a datatable containing the data to be inserted.
         char useMode -- sets the buildInsertUpdate function to either create an insert or update statement.
         * Return: void
         * Description: This method builds an update statement based off a Datatable.
         */
        private string buildUpdate(string table, string rowKey, string idRow, List<string> columnNames, DataRow data)
        {
            string query = "update " + table + " set ";

            foreach (string s in columnNames)
            {
                if (s != idRow)
                {
                    query += s + " = '" + data.Field<string>(s) + "', ";
                }
            }
            query = query.Remove(query.Length - 3);

            query += " where " + rowKey + " = " + idRow + ";";

            return query;
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
            connectionString = @"server=" + server + ";user id=" + userid + ";password=" + password + ";database=" + database;

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
        public void OpenDBConnectionLeft()
        {
            //If logging into the source database connect to it
            if (leftSide == "MS")
            MSSourceConnLeft = MsInitData(SourceDbConnectorInfoLeft.server, SourceDbConnectorInfoLeft.userid, 
                                  SourceDbConnectorInfoLeft.password, SourceDbConnectorInfoLeft.database);
            else  if (leftSide == "MY")
            MySourceConnLeft = MyInitData(SourceDbConnectorInfoLeft.server, SourceDbConnectorInfoLeft.userid,
                                  SourceDbConnectorInfoLeft.password, SourceDbConnectorInfoLeft.database);

            _ConnectionOpenLeft = true;
            //sourceTables = ExtractTables(sourceConn);
            //sourceDBName = sourceDbConnectorInfo.database;
        }

        public void OpenDBConnectionRight()
        {
            //If logging into the source database connect to it
            if (leftSide == "MS")
                MSSourceConnRight = MsInitData(SourceDbConnectorInfoRight.server, SourceDbConnectorInfoRight.userid,
                                      SourceDbConnectorInfoRight.password, SourceDbConnectorInfoRight.database);
            else if (leftSide == "MY")
                MySourceConnRight = MyInitData(SourceDbConnectorInfoRight.server, SourceDbConnectorInfoRight.userid,
                                      SourceDbConnectorInfoRight.password, SourceDbConnectorInfoRight.database);

            _ConnectionOpenRight = true;
            //sourceTables = ExtractTables(sourceConn);
            //sourceDBName = sourceDbConnectorInfo.database;
        }

        public void OpenMGMTConnection()
        {
            mgmtConn = MsInitData(mgmtConnInfo.server, mgmtConnInfo.userid,
                                      mgmtConnInfo.password, mgmtConnInfo.database);

            _mgmtOpen = true;
            //sourceTables = ExtractTables(sourceConn);
            //sourceDBName = sourceDbConnectorInfo.database;
        }

        /*
         * Method Name: CloseDBConnectionLeft/CloseDBConnectionRight
         * Parameters: void
         * Return: void
         * Description: The method will close any lingering connections.
         */
        public void CloseDBConnectionLeft()
        {
            //Close the connections before the application closes
            if (MSSourceConnLeft != null &&
                MSSourceConnLeft.State == ConnectionState.Open && leftSide == "MS")
            {
                MSSourceConnLeft.Close();
            }
            else if (MySourceConnLeft != null &&
                MySourceConnLeft.State == ConnectionState.Open && leftSide == "MY")
            {
                MySourceConnLeft.Close();
            }

            _ConnectionOpenLeft = false;
        }

        public void CloseDBConnectionRight()
        {
            //Close the connections before the application closes
            if (MSSourceConnRight != null &&
                MSSourceConnRight.State == ConnectionState.Open && rightSide == "MS")
            {
                MSSourceConnRight.Close();
            }
            else if (MySourceConnRight != null &&
                MySourceConnRight.State == ConnectionState.Open && rightSide == "MY")
            {
                MySourceConnRight.Close();
            }

            _ConnectionOpenRight = false;
        }

        public void CloseMGMTConnection()
        {
            //Close the connections before the application closes
            if (mgmtConn != null &&
                mgmtConn.State == ConnectionState.Open)
            {
                MSSourceConnRight.Close();
            }

            _mgmtOpen = false;
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
         *  string table - 
         * Return: unsynccedRows a list of strings containing the rows to be sent over.
         * Description: This will compare two tables in two databases and find any missing rows.
         */
        public List<string> findMissingRows(string[] conditions)
        {
            DataTable pulledContents = new DataTable();
            List<string> unsyncedRows = new List<string>();
            List<string> conditionsToAdd = new List<string>();
            conditionsToAdd.Add("tableName = " + conditions[0]);
            conditionsToAdd.Add("&&");
            conditionsToAdd.Add("dbInstance = " + conditions[1]);
            conditionsToAdd.Add("&&");
            conditionsToAdd.Add("new = " + conditions[2]);
            conditionsToAdd.Add("&&");
            conditionsToAdd.Add("synced = 0");


            SqlCommand cmd = new SqlCommand(buildSelect("updateLog", null, conditionsToAdd), mgmtConn);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(pulledContents);
                da.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (DataRow i in pulledContents.Rows)
            {
                unsyncedRows.Add(i.Field<string>("rowName") + " = " + i.Field<string>("rowKey") + " ||");
            }

            return unsyncedRows;
        }

        public void syncNewLeft(string leftTable, string rightTable)
        {
            List<string> unsyncedRows = new List<string>();
            DataTable contentsToAdd = new DataTable();
            string[] cond = { leftTable, SourceDbConnectorInfoLeft.database, "1" };

            OpenMGMTConnection();
            unsyncedRows = findMissingRows(cond);
            CloseMGMTConnection();

            OpenDBConnectionLeft();
            if (leftSide == "MS")
                contentsToAdd = MSPullData(leftTable, null, unsyncedRows, 'l');
            else if (leftSide == "MY")
                contentsToAdd = MSPullData(leftTable, null, unsyncedRows, 'l');
            CloseDBConnectionLeft();

            OpenDBConnectionRight();
            if (rightSide == "MS")
                Msinsert(rightTable, contentsToAdd, 'r');
            else if (leftSide == "MY")
                Msinsert(rightTable, contentsToAdd, 'r');
            CloseDBConnectionLeft();

        }

        public void syncNewRight(string leftTable, string rightTable)
        {
            List<string> unsyncedRows = new List<string>();
            DataTable contentsToAdd = new DataTable();
            string[] cond = { leftTable, SourceDbConnectorInfoLeft.database, "1" };

            OpenMGMTConnection();
            unsyncedRows = findMissingRows(cond);
            CloseMGMTConnection();

            OpenDBConnectionRight();
            if (leftSide == "MS")
                contentsToAdd = MSPullData(rightTable, null, unsyncedRows, 'r');
            else if (leftSide == "MY")
                contentsToAdd = MSPullData(rightTable, null, unsyncedRows, 'r');
            CloseDBConnectionRight();

            OpenDBConnectionLeft();
            if (rightSide == "MS")
                Msinsert(leftTable, contentsToAdd, 'l');
            else if (leftSide == "MY")
                Msinsert(leftTable, contentsToAdd, 'l');
            CloseDBConnectionRight();

        }

        public void syncUpdateLeft(string leftTable, string rightTable, string idRow)
        {
            List<string> unsyncedRows = new List<string>();
            DataTable contentsToAdd = new DataTable();
            string[] cond = { leftTable, SourceDbConnectorInfoLeft.database, "0" };

            OpenMGMTConnection();
            unsyncedRows = findMissingRows(cond);
            CloseMGMTConnection();

            OpenDBConnectionLeft();
            if (leftSide == "MS")
                contentsToAdd = MSPullData(leftTable, null, unsyncedRows, 'l');
            else if (leftSide == "MY")
                contentsToAdd = MSPullData(leftTable, null, unsyncedRows, 'l');
            CloseDBConnectionLeft();

            OpenDBConnectionRight();
            if (rightSide == "MS")
                MsUpdate(rightTable, idRow, contentsToAdd, 'r');
            else if (leftSide == "MY")
                MsUpdate(rightTable, idRow, contentsToAdd, 'r');
            CloseDBConnectionLeft();
        }


        public void syncUpdateRight(string leftTable, string rightTable, string idRow)
        {
            List<string> unsyncedRows = new List<string>();
            DataTable contentsToAdd = new DataTable();
            string[] cond = { leftTable, SourceDbConnectorInfoLeft.database, "0" };

            OpenMGMTConnection();
            unsyncedRows = findMissingRows(cond);
            CloseMGMTConnection();

            OpenDBConnectionLeft();
            if (leftSide == "MS")
                contentsToAdd = MSPullData(rightTable, null, unsyncedRows, 'r');
            else if (leftSide == "MY")
                contentsToAdd = MSPullData(rightTable, null, unsyncedRows, 'r');
            CloseDBConnectionLeft();

            OpenDBConnectionRight();
            if (rightSide == "MS")
                MsUpdate(rightTable, idRow, contentsToAdd, 'l');
            else if (leftSide == "MY")
                MsUpdate(rightTable, idRow, contentsToAdd, 'l');
            CloseDBConnectionLeft();
        }
    }
}