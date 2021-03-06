﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading; //Thread

namespace DbScheduler
{
    public class JobManager
    {

        public JobManager()
        {
        }

        /*
         * Method:  GetAllTypesImplementingInterface()
         * 
         * Parameters: Type interfaceType
         * 
         * Returns: IEnumerable<Type>
         * 
         * Description: Return all types implenmented or interfaced by the given type.
         */
        private IEnumerable<Type> GetAllTypesImplementingInterface(Type interfaceType)
        {
            IEnumerable<Type> returnValue = AppDomain
                                            .CurrentDomain
                                            .GetAssemblies()
                                            .SelectMany(assembly => assembly.GetTypes())
                                            .Where(type => interfaceType.IsAssignableFrom(type));

            return returnValue;
        }

        /*
         * Method:  ExecuteAllJobs()
         * 
         * Parameters: Void 
         * 
         * Returns: Void
         * 
         * Description: Execute the job.
         */
        public void ExecuteAllJobs()
        {
            try
            {
                Thread thread = null;
                Job jobObject = null;

                /*
                 * Get all job implementations.
                 */
                IEnumerable<Type> jobs = GetAllTypesImplementingInterface(typeof(Job));

                /*
                 * Execute the job.
                 */
                if (jobs != null &&
                    jobs.Count() > 0)
                {
                    foreach (Type job in jobs)
                    {
                        if (IsRealClass(job))
                        {
                            try
                            {
                                /*
                                 * Start a new job.
                                 */
                                jobObject = (Job)Activator.CreateInstance(job);
                                thread = new Thread(new ThreadStart(jobObject.ExecuteJob));

                                thread.Start();
                            }
                            catch (Exception ex)
                            {
                                /*
                                 * Do something here with the exception...
                                 */
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                /*
                 * Do something here with the exception..
                 */
            }
        }

        /*
         * Method:  IsRealClass(Type objectType)
         * 
         * Parameters: Type objectType 
         * 
         * Returns: returnValue
         * 
         * Description: Returns true if a given object is "real"; not abstract, or generic, or an interface. 
         */
        public bool IsRealClass(Type objectType)
        {
            bool returnValue = false;

            if (objectType.IsAbstract == false &&
                objectType.IsGenericType == false &&
                objectType.IsInterface == false)
            {
                returnValue = true;
            }

            return returnValue;
        }
    }
}
