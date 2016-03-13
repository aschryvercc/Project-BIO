using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;   //IPAddress

namespace DbConnector
{
    class DbConnectorInfo
    {
        private IPAddress connectIP;
        private string server;
        private string userid;
        private string password;
        private string database;

        //Method Name: GetUserID
        //Parameters: void
        //Return: string
        //Description: Read only method allowing access to the user id.
        public string GetUserID()
        {
            return userid;
        }

        //Method Name: GetServer
        //Parameters: void
        //Return: string
        //Description: Read only method allowing access to the server.
        public string GetServer()
        {
            return server;
        }

        //Method Name: GetPassword
        //Parameters: void
        //Return: string
        //Description: Read only method allowing access to the password.
        public string GetPassword()
        {
            return password;
        }

        //Method Name: GetDatabase
        //Parameters: void
        //Return: string
        //Description: Read only method allowing access to the database.
        public string GetDatabase()
        {
            return database;
        }
    }
}
