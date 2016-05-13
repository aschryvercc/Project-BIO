using System;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using System.Linq;
using System.Web;

using System.Data.Common;

namespace DbConnector
{

    public class DbConnector
    {
        private DbConnectorInfo sourceDbConnectorInfo;

        /*
         * Constructor
         */
        public DbConnector(DbConnectorInfo connectionInfo)
        {
            sourceDbConnectorInfo = connectionInfo;
        }
    }
}