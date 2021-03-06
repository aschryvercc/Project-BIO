﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;   //IPAddress

namespace DbConnector
{
    public class DbConnectorInfo
    {

        public DbConnectorInfo (string serv, string db, string uName, string pass, string dbType)
        {
            server = serv;
            database = db;
            userid = uName;
            password = pass;
            
            if (dbType.Equals(DbType.MYSQL.ToString()) ||
                dbType.Equals(DbType.SQLSERVER.ToString()))
            {
                dbtype = dbType;
            }
            else
            {
                throw new ArgumentException("Unknown Database Type", "dbType");
            }
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
        private string _dbtype;
        public string dbtype
        {
            set { this._dbtype = value; }
            get { return this._dbtype; }
        }
    }
}
