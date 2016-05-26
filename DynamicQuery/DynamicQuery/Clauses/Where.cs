using DynamicQuery.GLOBALS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.Clauses
{
    public class Where
    {
        private string _Column;
        public string Column
        {
            get { return this._Column; }
            set { this._Column = value; }
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
            set { _Value = value; }
        }
        
        private List<SubClause> _SubClauses;
        public List<SubClause> SubClauses
        {
            get { return this._SubClauses; }
        }

        public Where(string column, ComparisonOperators comparisonOperator, object value)
        {
            this.Column = column;
            this.ComparisonOperator = comparisonOperator;
            this.Value = value;
            this._SubClauses = new List<SubClause>();
        }

        public void AddSubClause(LogicalOperators logicalOperator, ComparisonOperators comparisonOperator, object value)
        {
            SubClause subClause = new SubClause(logicalOperator, comparisonOperator, value);
            _SubClauses.Add(subClause);
        }
    }
}
