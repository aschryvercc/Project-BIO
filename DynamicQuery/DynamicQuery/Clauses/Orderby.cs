using DynamicQuery.GLOBALS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.Clauses
{
    public class Orderby
    {
        private string _columnName;
        public string ColumnName
        {
            get { return this._columnName; }
            set { this._columnName = value; }
        }
        
        private SortingOperators _sortingOperator;
        public SortingOperators SortingOperator
        {
            get { return this._sortingOperator; }
            set { this._sortingOperator = value; }
        }

        public Orderby(string column, SortingOperators sortingOperator)
        {
            this.ColumnName = column;
            this.SortingOperator = sortingOperator;
        }
    }
}
