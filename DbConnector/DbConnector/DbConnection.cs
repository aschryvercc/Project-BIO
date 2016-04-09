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
        private DbConnectorInfo SourceDbConnectorInfo; //The source connection information
        private DbConnectorInfo mgmtConnInfo;
        private MySqlConnection MySourceConn;
        private SqlConnection MsSourceConn = null;  //The source database connection
        private SqlConnection mgmtConn = null;
        private string connectionString = "";   //connection string to a database when connecting the source or destination
        private bool _ConnectionOpen;
        private bool _mgmtOpen;
        private string _sourceType;
        public bool ConnectionOpen
        {
            get { return _ConnectionOpen; }
        }
        public bool mgmtOpen
        {
            get { return _mgmtOpen; }
        }
        public string sourceType
        {
            get { return _sourceType; }
        }

        /*
         * Constructor
         */
        public DbConnection(DbConnectorInfo connectionInfo, DbConnectorInfo mgmtInfo,  string type)
        {
            SourceDbConnectorInfo = connectionInfo;
            mgmtConnInfo = mgmtInfo;
            _ConnectionOpen = false;
            _mgmtOpen = false;
            _sourceType = type;
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
        public DataTable pullData(Dictionary<string, string> tablePair, List<string> columns, List<string> conditions)
        {
            DataTable pulledContents = new DataTable();

            if (sourceType == "MS")
            {
                SqlCommand cmd = new SqlCommand(buildSelect(tablePair, columns, conditions), MsSourceConn);

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
            }
            else if (sourceType == "MY")
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(buildSelect(tablePair, columns, conditions), MySourceConn);
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(pulledContents);
                    da.Dispose();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
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
        public DataTable pullData(string table, List<string> columns, List<string> conditions)
        {
            DataTable pulledContents = new DataTable();

            if (sourceType == "MS")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(buildSelect(table, columns, conditions), MsSourceConn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(pulledContents);
                    da.Dispose();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else if (sourceType == "MY")
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(buildSelect(table, columns, conditions), MySourceConn);
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(pulledContents);
                    da.Dispose();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
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
        public void insert(string table, DataTable data)
        {
            if (sourceType == "MS")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(buildInsert(table, data), MsSourceConn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else if (sourceType == "MY")
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(buildInsert(table, data), MySourceConn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        /*
         * Method Name: MsUpdate
         * Parameters: 
         * string table -- table to update
         * string idRow -- name of column containing primary key.
         * DataTable data -- datatable containing data to use.
         * Return: void
         * Description: This method executes either an insert or update statement to a database.
         */
        public void update(string table, string idRow, DataTable data)
        {
            List<string> cols = new List<string>();

            foreach (DataColumn col in data.Columns)
            {
                cols.Add(col.ColumnName);
            }

            if (sourceType == "MS")
            {
                foreach (DataRow row in data.Rows)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand(buildUpdate(table, Convert.ToString(row.Field<object>(idRow)), idRow, cols, row), MsSourceConn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }
            }
            else if (sourceType == "MY")
            {
                foreach (DataRow row in data.Rows)
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(buildUpdate(table, row.Field<string>(idRow), idRow, cols, row), MySourceConn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
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
                query = query.Remove(query.Length - 2);
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
                query = query.Remove(query.Length - 2);
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
                query = query.Remove(query.Length - 2);
            }

            query += ";";

            return query;
        }

        /*
         * Method Name: buildInsert
         * Parameters: 
         * string table -- table to be inserted into
         * DataTable data -- a datatable containing the data to be inserted.
         * char useMode -- sets the buildInsertUpdate function to either create an insert or update statement.
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
            query = query.Remove(query.Length - 2);

            query += ") values ";
            //insert data
            foreach (DataRow row in data.Rows)
            {
                query += "(";
                foreach (var val in row.ItemArray)
                {
                    if (val.ToString() == "")
                        query += "null, ";
                    else if (val.GetType().Equals(typeof(string)))
                        query += "'" + val.ToString() + "', ";
                    else if (val != null)
                        query += val.ToString() + ", ";

                }
                query = query.Remove(query.Length - 2);
                query += "), ";
            }
            query = query.Remove(query.Length - 2);
            query += ";";

            return query;
        }

        /*
         * Method Name: buildUpdate
         * Parameters: 
         * string table -- table to be inserted into
         * DataTable data -- a datatable containing the data to be inserted.
         * char useMode -- sets the buildInsertUpdate function to either create an insert or update statement.
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
                    if (data.Field<object>(s) != null)
                    {
                        if (data.Field<object>(s).GetType().Equals(typeof(string)))
                            query += s + " = '" + Convert.ToString(data.Field<object>(s)) + "1', ";
                        else
                            query += s + " = " + Convert.ToString(data.Field<object>(s)) + "1, ";
                    }
                }
            }
            query = query.Remove(query.Length - 2);

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
        public void OpenDBConnection()
        {
            //If logging into the source database connect to it
            if (sourceType == "MS")
            MsSourceConn = MsInitData(SourceDbConnectorInfo.server, SourceDbConnectorInfo.userid,
                                  SourceDbConnectorInfo.password, SourceDbConnectorInfo.database);
            else  if (sourceType == "MY")
            MySourceConn = MyInitData(SourceDbConnectorInfo.server, SourceDbConnectorInfo.userid,
                                  SourceDbConnectorInfo.password, SourceDbConnectorInfo.database);

            _ConnectionOpen = true;
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
        public void CloseDBConnection()
        {
            //Close the connections before the application closes
            if (MsSourceConn != null &&
                MsSourceConn.State == ConnectionState.Open && sourceType == "MS")
            {
                MsSourceConn.Close();
            }
            else if (MySourceConn != null &&
                MySourceConn.State == ConnectionState.Open && sourceType == "MY")
            {
                MySourceConn.Close();
            }

            _ConnectionOpen = false;
        }

        public void CloseMGMTConnection()
        {
            //Close the connections before the application closes
            if (mgmtConn != null &&
                mgmtConn.State == ConnectionState.Open)
            {
                MsSourceConn.Close();
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
        public DataTable findMissingRows(string table, bool isNew)
        {
            DataTable pulledContents = new DataTable();
            List<string> unsyncedRows = new List<string>();
            List<string> conditionsToAdd = new List<string>();
            conditionsToAdd.Add("tableName = '" + table + "'");
            conditionsToAdd.Add("&&");
            conditionsToAdd.Add("new = " + isNew);
            conditionsToAdd.Add("&&");
            conditionsToAdd.Add("synced = 0");

            if (sourceType == "MS")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(buildSelect("updateLog", null, conditionsToAdd), MsSourceConn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(pulledContents);
                    da.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (sourceType == "MS")
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(buildSelect("updateLog", null, conditionsToAdd), MySourceConn);
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(pulledContents);
                    da.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
          
            return pulledContents;
        }
    }
}