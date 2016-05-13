using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConnector
{
    public sealed class DbType
    {
        private readonly String name;
        private readonly int value;

        public static readonly DbType MYSQL = new DbType(1, "MySql");
        public static readonly DbType SQLSERVER = new DbType(2, "SQL Server");

        public DbType(int value, String name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
