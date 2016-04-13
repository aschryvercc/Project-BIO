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
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("JobType: {0}", GetName()).AppendLine();

            /*
             * TODO: Properly read in connection information here.
             */
            //sourceInfo = new DbConnectorInfo();
            //destinationInfo = new DbConnectorInfo();

            //DbConnection sourceConnection = new DbConnection(sourceInfo);
            //DbConnection destinationConnection = new DbConnection(destinationInfo);

            //Dictionary<string, string> leftPair = new Dictionary<string,string>();
            //Dictionary<string, string> rightPair = new Dictionary<string,string>();
            //List<string> columns = new List<string>();
            //List<string> conditions = new List<string>();
            //string tableName = "";
            //char useMode = ' ';

            //sb.Append("Connecting to source...").AppendLine();
            //sb.AppendFormat("DbConnectorInfo:").AppendLine();
            //sb.AppendFormat("server: {0}", sourceInfo.server).AppendLine();
            //sb.AppendFormat("database: {0}", sourceInfo.database).AppendLine();
            //sb.AppendFormat("userid: {0}", sourceInfo.userid).AppendLine();

            //sb.Append("Connecting to destination...").AppendLine();
            //sb.AppendFormat("DbConnectorInfo:").AppendLine();
            //sb.AppendFormat("server: {0}", destinationInfo.server).AppendLine();
            //sb.AppendFormat("database: {0}", destinationInfo.database).AppendLine();
            //sb.AppendFormat("userid: {0}", destinationInfo.userid).AppendLine();
            
            ///*
            // * Open the source and destination databases.
            // */
            //if (!sourceConnection.ConnectionOpen)
            //{
            //    sourceConnection.OpenDBConnection();
            //}
            //if (!destinationConnection.ConnectionOpen)
            //{
            //    destinationConnection.OpenDBConnection();
            //}

            /*
             * Pull data from BioLinks.
             * TODO: Figure out wtf this method even takes.
             */
            DataTable sourceData = new DataTable();//sourceConnection.PullData(true, leftPair, rightPair, columns, conditions);
            sourceData.Columns.AddRange(new DataColumn[] {
                new DataColumn("ID", typeof(Guid)),
                new DataColumn("Date", typeof(DateTime)),
                new DataColumn("StringValue", typeof(string)),
                new DataColumn("NumberValue", typeof(int)),
                new DataColumn("BooleanValue", typeof(bool))
            });
            sourceData.Rows.Add(Guid.NewGuid(), DateTime.Now, "String4", 400, false);
            sourceData.Rows.Add(Guid.NewGuid(), DateTime.Now, "String5", 500, true);
            sourceData.Rows.Add(Guid.NewGuid(), DateTime.Now, "String6", 600, false);
            
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

            //for (int i = sourceData.Rows.Count - 1; i >= 0; --i)
            //{
            //    DataRow dr = sourceData.Rows[i];
            //    if (dr)
            //    {
            //        dr.Delete();
            //    }
            //}

            /*
             * Insert/Update pulled into BioTrack.
             */
            //destinationConnection.insertUpdate(tableName, sourceData, useMode);

            /*
             * Close the source and destination databases.
             */
            //sourceConnection.CloseDBConnection();
            //destinationConnection.CloseDBConnection();
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
