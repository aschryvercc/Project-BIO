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
            string tableName = "";
            char useMode = ' ';

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
            DataTable sourceData = sourceConnection.PullData(true, leftPair, rightPair, columns, conditions);

            /*
             * Insert/Update pulled into BioTrack.
             * TODO: Figure out what the 'char userMode' means.
             */
            destinationConnection.insertUpdate(tableName, sourceData, useMode);

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
            return 500; //Half a second
        }
    }
}
