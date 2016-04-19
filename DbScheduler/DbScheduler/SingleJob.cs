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

            sb.AppendFormat("JobType: {0}", GetName()).AppendLine();
            
            /*
             * TODO: Properly read in connection information here.
             */
            DbConnectorInfo sourceInfo = new DbConnectorInfo("localhost\\SQLEXPRESS", "NORTHWND", "sa", "Conestoga1");
            DbConnection sourceConnection = new DbConnection(sourceInfo, "MS");

            Dictionary<string, string> keys = new Dictionary<string, string>();
            keys.Add("Customers", "CustomerID");
            keys.Add("Orders", "OrderID");

            List<string> joins = new List<string>();
            joins.Add("full");

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
            DataTable sourceData = sourceConnection.pullData(keys, joins, null, null);

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

            Logger.logMessage(sb.ToString(), AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.RelativeSearchPath + "\\Events2.txt");

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
