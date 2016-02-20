using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading; //Thread

namespace DbScheduler
{
    public abstract class Job
    {
        /*
         * Method:  ExecuteJob()
         * 
         * Parameters: Void 
         * 
         * Returns: Void
         * 
         * Description: Execute the job.
         */
        public void ExecuteJob()
        {
            /*
             * Execute the job.
             * 
             * Enter if the job is to be repeatidly executed.
             */
            if (IsRepeatable())
            {
                /*
                 * Execute the job based on the determined interval time.
                 * TODO: Add exit condition... maybe.
                 */
                while (true)
                {
                    DoJob();
                    Thread.Sleep(GetRepeatableIntervalTime());
                }
            }
            /*
             * Else execute the job once.
             */
            else
            {
                DoJob();
            }
        }

        /*
         * Method:  GetParameters()
         * 
         * Parameters: Void 
         * 
         * Returns: Object
         * 
         * Description: Returns the parameter set of a job before it starts.
         */
        public virtual Object GetParameters()
        {
            return null;
        }

        /*
         * Method:  GetName()
         * 
         * Parameters: Void 
         * 
         * Returns: string
         * 
         * Description: Returns the name of the job.
         */
        public abstract string GetName();

        /*
         * Method:  DoJob()
         * 
         * Parameters: Void 
         * 
         * Returns: void
         * 
         * Description: The work executed for the job.
         */
        public abstract void DoJob();

        /*
         * Method:  IsRepeatable()
         * 
         * Parameters: Void 
         * 
         * Returns: bool
         * 
         * Description: Returns true if the job is to be repeated, false otherwise.
         */
        public abstract bool IsRepeatable();

        /*
         * Method:  GetRepeatableIntervalTime()
         * 
         * Parameters: Void 
         * 
         * Returns: int
         * 
         * Description: Returns the interval time the job's execution.
         */
        public abstract int GetRepeatableIntervalTime();
    }
}
