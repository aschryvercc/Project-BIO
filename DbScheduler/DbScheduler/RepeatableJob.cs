using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using DbConnector;

namespace DbScheduler
{
    class RepeatableJob : Job
    {
        private int counter = 0;
        private DbConnectorInfo sourceInfo;
        private DbConnectorInfo destinationInfo;

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void DoJob()
        {
            const string ROW_NAME = "rowName";
            const string ROW_KEY = "rowKey";
            const string TABLE_NAME = "tableName";

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("JobType: {0}", GetName()).AppendLine();

            /*
             * TODO: Properly read in connection information here.
             */
            sourceInfo = new DbConnectorInfo();
            destinationInfo = new DbConnectorInfo();

            DbConnection sourceConnection = new DbConnection(sourceInfo, "MS");
            DbConnection destinationConnection = new DbConnection(destinationInfo, "MS");

            /*
             * Build the query for updating from the table.
             */
            List<string> columns_to_pull = new List<string>();
            columns_to_pull.Add(ROW_KEY);
            columns_to_pull.Add(ROW_NAME);
            columns_to_pull.Add(TABLE_NAME);
            List<string> where_clauses = new List<string>();
            where_clauses.Add("synced = 0");

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
            DataTable sourceData = sourceConnection.pullData("updateLog", columns_to_pull, where_clauses);
            
            /*
             * Log the query.
             */
            foreach (DataRow row in sourceData.Rows)
            {
                where_clauses = new List<string>();
                where_clauses.Add(row.Field<string>(ROW_NAME) + "=" + row.Field<string>(ROW_KEY) + " ||");
                
                /*
                 * Pull data from BioLinks.
                 */
                sourceData = sourceConnection.pullData(row.Field<string>(TABLE_NAME), null, where_clauses);

                /*
                 * Insert/Update pulled into BioTrack.
                 */
                destinationConnection.update(row.Field<string>(TABLE_NAME), row.Field<string>(ROW_NAME), sourceData);
            }

            Logger.logMessage(sb.ToString());

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
    }
}
