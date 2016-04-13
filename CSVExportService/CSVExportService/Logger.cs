using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
//using System.Web.Services.Protocols;
using System.Diagnostics;

/// <summary>
/// Summary description for Logger
/// </summary>
public class Logger
{
    public Logger()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool logException(Exception exception)
    {
        bool status = false;
        string logFilePathName = GetDefaultPath();
        if (!File.Exists(logFilePathName))
        {
            if (CheckForDirectory(logFilePathName))
            {
                FileStream fileStream = new FileStream(logFilePathName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fileStream.Close();
            }
        }
        try
        {
            StreamWriter streamWriter = new StreamWriter(logFilePathName, true);

            streamWriter.WriteLine("Date        : " + DateTime.Now.ToLongTimeString());
            streamWriter.WriteLine("Time        : " + DateTime.Now.ToShortDateString());
            streamWriter.WriteLine("Error       : " + exception.Message.ToString().Trim());
            streamWriter.WriteLine("Type        : " + exception.GetType().ToString());
            streamWriter.WriteLine("********************************************************");

            streamWriter.Flush();
            streamWriter.Close();

            status = true;
        }
        catch (Exception ex)
        {
            //There was an error return null.
            Console.WriteLine(ex.ToString());
        }

        return status;
    }

    public static bool logException(string logFilePathName, Exception exception)
    {
        bool status = false;

        if (logFilePathName == "" ||
            logFilePathName == null)
        {
            logFilePathName = GetDefaultPath();
        }
        else
        {
            if (!File.Exists(logFilePathName))
            {
                if (CheckForDirectory(logFilePathName))
                {
                    FileStream fileStream = new FileStream(logFilePathName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fileStream.Close();
                }
            }
        }
        try
        {
            StreamWriter streamWriter = new StreamWriter(logFilePathName, true);

            streamWriter.WriteLine("Date        : " + DateTime.Now.ToLongTimeString());
            streamWriter.WriteLine("Time        : " + DateTime.Now.ToShortDateString());
            streamWriter.WriteLine("Error       : " + exception.Message.ToString().Trim());
            streamWriter.WriteLine("Type        : " + exception.GetType().ToString());
            streamWriter.WriteLine("********************************************************");

            streamWriter.Flush();
            streamWriter.Close();

            status = true;
        }
        catch (Exception ex)
        {
            //There was an error return null.
            Console.WriteLine(ex.ToString());
        }

        return status;
    }

    public static bool logMessage(string message)
    {
        bool status = false;
        string logFilePathName = GetDefaultPath();
        if (!File.Exists(logFilePathName))
        {
            if (CheckForDirectory(logFilePathName))
            {
                FileStream fileStream = new FileStream(logFilePathName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fileStream.Close();
            }
        }
        try
        {
            StreamWriter streamWriter = new StreamWriter(logFilePathName, true);

            streamWriter.WriteLine("Date        : " + DateTime.Now.ToLongTimeString());
            streamWriter.WriteLine("Time        : " + DateTime.Now.ToShortDateString());
            streamWriter.WriteLine("Message     : " + message);
            streamWriter.WriteLine("********************************************************");

            streamWriter.Flush();
            streamWriter.Close();

            status = true;
        }
        catch (Exception ex)
        {
            //There was an error return null.
            Console.WriteLine(ex.ToString());
        }

        return status;
    }

    public static bool logMessage(string message, string logFilePathName)
    {
        bool status = false;

        if (logFilePathName == "" ||
            logFilePathName == null)
        {
            logFilePathName = GetDefaultPath();
        }
        else
        {
            if (!File.Exists(logFilePathName))
            {
                if (CheckForDirectory(logFilePathName))
                {
                    FileStream fileStream = new FileStream(logFilePathName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fileStream.Close();
                }
            }
        }
        try
        {
            StreamWriter streamWriter = new StreamWriter(logFilePathName, true);

            streamWriter.WriteLine("Date        : " + DateTime.Now.ToLongTimeString());
            streamWriter.WriteLine("Time        : " + DateTime.Now.ToShortDateString());
            streamWriter.WriteLine("Message     : " + message);
            streamWriter.WriteLine("********************************************************");

            streamWriter.Flush();
            streamWriter.Close();

            status = true;
        }
        catch (Exception ex)
        {
            //There was an error return null.
            Console.WriteLine(ex.ToString());
        }

        return status;
    }

    private static string GetDefaultPath()
    {
        try
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.RelativeSearchPath;
            string filePath = baseDirectory + "\\" + "DefaultLogFile.txt";

            //Create the default path to a log file if does not exist yet...
            if (!File.Exists(filePath))
            {
                if (CheckForDirectory(filePath))
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fileStream.Close();
                }
            }

            return filePath;
        }
        catch (Exception ex)
        {
            //There was an error return null.
            Console.WriteLine(ex.ToString());
            return null;
        }
    }

    private static bool CheckForDirectory(string pathName)
    {
        bool status = false;
        try
        {
            int slashPosition = pathName.Trim().LastIndexOf("\\");
            string directoryName = pathName.Trim().Substring(0, slashPosition);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            status = true;
            return status;
        }
        catch (Exception ex)
        {
            //There was an error return fasle.
            Console.WriteLine(ex.ToString());
            return status;
        }
    }
}