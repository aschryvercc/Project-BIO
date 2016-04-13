﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            bool test = false;
            int count = 0;

            Console.WriteLine("Starting DbScheduler Test Harness...");
            
            JobManager jm = new JobManager();

            Console.WriteLine("Checking for Job types...");
            IEnumerable<Type> jobs = jm.GetAllTypesImplementingInterface(typeof(Job));
            foreach (Type t in jobs)
            {
                Console.WriteLine(t.ToString());
                Console.WriteLine("Checking Object type " + t.ToString());
                test = jm.IsRealClass(t);
                Console.WriteLine("Result = " + test.ToString());

                ++count;
            }

            if (count > 0)
            {
                Console.WriteLine("Starting all jobs...");

                jm.ExecuteAllJobs();
            }
            else
            {
                Console.WriteLine("There are no jobs to execute...");
                Console.WriteLine("Stopping process...");                
            }

            return;
        }
    }
}
