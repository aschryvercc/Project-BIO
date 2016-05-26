using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery
{
    public class Literal
    {
        private const string ROWS_AFFECTED = "SELECT @@ROWCOUNT";

        private string _query;
        public string Query
        {
            get { return this._query; }
            set { this._query = value; }
        }

        public Literal(string query)
        {
            this._query = query;
        }
    }
}
