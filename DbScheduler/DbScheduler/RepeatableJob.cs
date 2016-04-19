using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using System.Data;
using DbConnector;

namespace DbScheduler
{
    class RepeatableJob : Job
    {
        private int counter = 0;
        private DbConnectorInfo sourceInfo;
        private DbConnectorInfo destinationInfo;
        private int i = 1;

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void DoJob()
        {
            const string ID_COLUMN_NAME = "EmployeeID";
            const string TABLE_NAME = "Employees";

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("JobType: {0}", GetName()).AppendLine();

            /*
             * TODO: Properly read in connection information here.
             */
            DbConnectorInfo sourceInfo = new DbConnectorInfo("localhost\\SQLEXPRESS", "NORTHWND", "sa", "Conestoga1");
            destinationInfo = new DbConnectorInfo("localhost\\SQLEXPRESS", "SimpleNorthwind", "sa", "Conestoga1");

            DbConnection sourceConnection = new DbConnection(sourceInfo, "MS");
            DbConnection destinationConnection = new DbConnection(destinationInfo, "MS");

            /*
             * Build the query for updating from the table.
             */
            //List<string> columns_to_pull = new List<string>();
            //columns_to_pull.Add(ROW_KEY);
            //columns_to_pull.Add(ROW_NAME);
            //columns_to_pull.Add(TABLE_NAME);
            List<string> where_clauses = new List<string>();
            /*
             * Some janky stuff to get this working.
             */
            where_clauses.Add(ID_COLUMN_NAME + " = " + i);
            ++i;

            sb.Append("Connecting to source...").AppendLine();
            sb.AppendFormat("DbConnectorInfo:").AppendLine();
            sb.AppendFormat("server: {0}", sourceInfo.server).AppendLine();
            sb.AppendFormat("database: {0}", sourceInfo.database).AppendLine();
            sb.AppendFormat("userid: {0}", sourceInfo.userid).AppendLine();

            sb.Append("Connecting to destination...").AppendLine();
            sb.AppendFormat("DbConnectorInfo:").AppendLine();
            sb.AppendFormat("server: {0}", destinationInfo.server).AppendLine();
            sb.AppendFormat("database: {0}", destinationInfo.database).AppendLine();
            sb.AppendFormat("userid: {0}", destinationInfo.userid).AppendLine();

            /*
             * Open the source and destination databases.
             */
            if (!sourceConnection.ConnectionOpen)
            {
                sourceConnection.OpenDBConnection();
            }
            if (!destinationConnection.ConnectionOpen)
            {
                destinationConnection.OpenDBConnection();
            }

            /*
             * Pull data from BioLinks.
             * TODO: Figure out wtf this method even takes.
             */
            DataTable sourceData = sourceConnection.pullData(TABLE_NAME, null, where_clauses);

            sb.AppendLine("Source Data: ");

            foreach (DataRow row in sourceData.Rows)
            {
                sb.AppendLine(sourceData.Rows.IndexOf(row).ToString());
            }

            /*
             * Insert/Update pulled into BioTrack.
             */
            destinationConnection.insert(TABLE_NAME, sourceData);

     
            destinationConnection.insert(TABLE_NAME, sourceData);

            Logger.logMessage(sb.ToString(), AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.RelativeSearchPath + "\\Events.txt");

            /*
             * Close the source and destination databases.
             */
            sourceConnection.CloseDBConnection();
            destinationConnection.CloseDBConnection();
        }

        public override bool IsRepeatable()
        {
            return true;
        }

        public override int GetRepeatableIntervalTime()
        {
            /*
             * Change this value if you would like to change the intervals this job is executed.
             */
            return 5000; //Half a second
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
            string connectionString = @"server=" + server + ";user id=" + userid + ";password=" + password + ";database=" + database;

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
                Logger.logException("Exceptions.txt", ex);
            }
            //Return the new MySql Connection
            return conn;
        }
    }
}
