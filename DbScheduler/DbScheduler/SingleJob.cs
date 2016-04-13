using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using DbConnector;

namespace DbScheduler
{
    class SingleJob: Job
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
            StringBuilder sb = new StringBuilder(); // Result of the query.
            
            /*
             * TODO: Properly read in connection information here.
             */
            sourceInfo = new DbConnectorInfo();
            destinationInfo = new DbConnectorInfo();

            DbConnection sourceConnection = new DbConnection(sourceInfo);
            DbConnection destinationConnection = new DbConnection(destinationInfo);

            Dictionary<string, string> leftPair = new Dictionary<string,string>();
            Dictionary<string, string> rightPair = new Dictionary<string,string>();
            List<string> columns = new List<string>();
            List<string> conditions = new List<string>();

            sb.Append("Connecting to source...").AppendLine();
            sb.AppendFormat("DbConnectorInfo:").AppendLine();
            sb.AppendFormat("server: {0}", sourceInfo.server).AppendLine();
            sb.AppendFormat("database: {0}", sourceInfo.database).AppendLine();
            sb.AppendFormat("userid: {0}", sourceInfo.userid).AppendLine();

            /*
             * Open the source and destination databases.
             */
            if (!sourceConnection.ConnectionOpen)
            {                
                sourceConnection.OpenDBConnection();
            }

            /*
             * Pull data from BioLinks.
             * TODO: Figure out wtf this method even takes.
             */
            DataTable sourceData = sourceConnection.PullData(true, leftPair, rightPair, columns, conditions);

            sb.AppendFormat("Querying database..."/*with: {0} {1} {2} {3}", leftPair, rightPair, columns*/).AppendLine();

            /*
             * Log the query.
             */
            foreach (DataRow row in sourceData.Rows)
            {
                foreach (DataColumn col in sourceData.Columns)
                {
                    sb.AppendFormat("{0} ", row[col]);    
                }

                sb.AppendLine();
            }

            Logger.logMessage(sb.ToString());

            /*
             * Close the source and destination databases.
             */
            sourceConnection.CloseDBConnection();
        }

        public override bool IsRepeatable()
        {
            return false;
        }

        public override int GetRepeatableIntervalTime()
        {
            throw new NotImplementedException();
        }
    }
}
