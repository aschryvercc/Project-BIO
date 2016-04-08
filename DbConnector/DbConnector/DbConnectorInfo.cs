using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;   //IPAddress

namespace DbConnector
{
    public class DbConnectorInfo
    {

        public DbConnectorInfo (string serv, string db, string uName, string pass)
        {
            server = serv;
            database = db;
            userid = uName;
            password = pass;
        }

        private string _server;
        public string server
        {
            set { this._server = value; }
            get { return this._server; }
        }
        
        private string _database;
        public string database
        {
            set { this._database = value; }
            get { return this._database; }
        }

        private string _userid;
        public string userid
        {
            set { this._userid = value; }
            get { return this._userid; }
        }


        private string _password;
        public string password
        {
            set { this._password = value; }
            get { return this._password; }
        }
    }
}
