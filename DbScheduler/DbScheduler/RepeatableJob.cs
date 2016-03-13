using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbScheduler
{
    class RepeatableJob : Job
    {
        private int counter = 0;
        //private DbConnector dbConnector() = new DbConnector();

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void DoJob()
        {
            /*
             * connect to biolinks
             * 
             * connect to biotrack
             * 
             * find new data in tables
             * 
             * pull data from tables
             * 
             * verify new data
             * 
             * push new data to database
             * 
             * disconnect from both databases
             * 
             */
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
