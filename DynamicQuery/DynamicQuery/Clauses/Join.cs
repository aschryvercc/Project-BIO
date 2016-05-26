using DynamicQuery.GLOBALS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.Clauses
{
    public class Join
    {
        private JoinTypes _JoinType;
        public JoinTypes JoinType
        {
            set { this._JoinType = value; }
            get { return this._JoinType; }
        }

        private string _FromTable;
        public string FromTable
        {
            set { this._FromTable = value; }
            get { return this._FromTable; }
        }

        private string _FromColumn;
        public string FromColumn
        {
            set { this._FromColumn = value; }
            get { return this._FromColumn; }
        }
        private ComparisonOperators _ComparisonOperator;
        public ComparisonOperators ComparisonOperator
        {
            set { this._ComparisonOperator = value; }
            get { return this._ComparisonOperator; }
        }

        private string _ToTable;
        public string ToTable
        {
            set { this._ToTable = value; }
            get { return this._ToTable; }
        }

        private string _ToColumn;
        public string ToColumn
        {
            set { this._ToColumn = value; }
            get { return this._ToColumn; }
        }

        public Join(JoinTypes JoinType, string FromTable, string FromColumn, ComparisonOperators ComparisonOperator, string ToTable, string ToColumn)
        {
            this.JoinType = JoinType;
            this.FromTable = FromTable;
            this.FromColumn = FromColumn;
            this.ComparisonOperator = ComparisonOperator;
            this.ToTable = ToTable;
            this.ToColumn = ToColumn;
        }
    }
}
