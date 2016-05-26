using DynamicQuery.GLOBALS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.Clauses
{
    public class SubClause
    {
        private LogicalOperators _LogicalOperator;
        public LogicalOperators LogicalOperator
        {
            get { return this._LogicalOperator; }
            set { this._LogicalOperator = value; }
        }

        private ComparisonOperators _ComparisonOperator;
        public ComparisonOperators ComparisonOperator
        {
            get { return this._ComparisonOperator; }
            set { this._ComparisonOperator = value; }
        }

        private object _Value;
        public object Value
        {
            get { return this._Value; }
            set { this._Value = value; }
        }

        public SubClause(LogicalOperators logicalOperator, ComparisonOperators comparisonOperator, object value)
        {
            this.LogicalOperator = logicalOperator;
            this.ComparisonOperator = comparisonOperator;
            this.Value = value;
        }

    }
}
