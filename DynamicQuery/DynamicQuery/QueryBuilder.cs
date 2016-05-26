using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicQuery.Clauses;
using DynamicQuery.GLOBALS;

namespace DynamicQuery
{
    public class QueryBuilder : IQueryBuilder
    {
        #region Memebers
        #region Distinct
        private bool _distinct;
        public bool Distinct
        {
            set { this._distinct = value; }
            get { return this._distinct; }
        }
        #endregion

        #region Selected Columns
        private List<string> _selectedColumns;
        public string[] SelectedColumns
        {
            get
            {
                if (_selectedColumns.Count > 0)
                {
                    return this._selectedColumns.ToArray();
                }
                else
                {
                    return new string[1] { "*" };
                }
            }
        }
        public void SelectAllColumns()
        {
            this._selectedColumns.Clear();
        }
        public void SelectColumn(string columnName)
        {
            this._selectedColumns.Clear();
            this._selectedColumns.Add(columnName);
        }
        public void SelectColumns(params string[] columnNames)
        {
            this._selectedColumns.Clear();
            foreach(string columnName in columnNames)
            {
                this._selectedColumns.Add(columnName);
            }
        }
        #endregion

        #region Selected Tables
        private List<string> _selectedTables;
        public string[] SelectedTables
        {
            get { return this._selectedTables.ToArray(); }
        }
        public void SelectFromTable(string tableName)
        {
            _selectedTables.Clear();
            _selectedTables.Add(tableName);
        }
        public void SelectFromTables(params string[] tableNames)
        {
            _selectedTables.Clear();
            foreach (string tableName in tableNames)
            {
                _selectedTables.Add(tableName);
            }
        }
        #endregion

        #region Joins
        private List<Join> _joins;
        public void AddJoin(Join newJoin)
        {
            this._joins.Add(newJoin);
        }
        public void AddJoin(JoinTypes joinType, string toTablename, string toColumn, string fromTablename, string fromColumn, ComparisonOperators comparisonOperator)
        {
            Join newJoin = new Join(joinType, toTablename, toColumn, comparisonOperator, fromTablename, fromColumn);
            this._joins.Add(newJoin);
        }
        #endregion

        #region Where Statement
        private WhereStatement _whereStatement;
        public WhereStatement WhereStatement
        {
            get { return this._whereStatement; }
            set { this._whereStatement = value; }
        }
        public Where AddWhere(string column, ComparisonOperators comparisonOperator, object value, int level)
        {
            Where newWhere = new Where(column, comparisonOperator, value);
            this._whereStatement.Add(newWhere, level);
            return newWhere;
        }
        public Where AddWhere(string column, ComparisonOperators comparisonOperator, object value)
        {
            return this.AddWhere(column, comparisonOperator, value, 1);
        }
        public Where AddWhere(Enum column, ComparisonOperators comparisonOperator, object value)
        {
            return this.AddWhere(column.ToString(), comparisonOperator, value, 1);
        }
        #endregion

        #region Group By
        private List<string> _groupBy;
        public void AddGroupBy(params string[] columns)
        {
            foreach (string column in columns)
            {
                _groupBy.Add(column);
            }
        }
        #endregion

        #region Having Statement
        private WhereStatement _having;
        public WhereStatement Having
        {
            get { return this._having; }
            set { this._having = value; }
        }
        #endregion

        #region Order By
        private List<Orderby> _orderBy;
        public void AddOrderBy(Orderby clause)
        {
            this._orderBy.Add(clause);
        }
        public void AddOrderBy(string column, SortingOperators sortingOperator)
        {
            Orderby newOrderBy = new Orderby(column, sortingOperator);
            this._orderBy.Add(newOrderBy);
        }
        public void AddOrderBy(Enum column, SortingOperators sortingOperator)
        {
            this.AddOrderBy(column.ToString(), sortingOperator);
        }
        #endregion

        #region DbProviderFactory
        private DbProviderFactory _dbProviderFactory;
        public DbProviderFactory dbProviderFactory
        {
            set { this._dbProviderFactory = value; }
        }
        #endregion
        #endregion

        #region Constructor
        public QueryBuilder()
        {
            _distinct = new bool();
            _selectedColumns = new List<string>();
            _selectedTables = new List<string>();
            _joins = new List<Join>();
            _whereStatement = new WhereStatement();
            _groupBy = new List<string>();
            _having = new WhereStatement();
            _orderBy = new List<Orderby>();
            _dbProviderFactory = null;
        }
        #endregion

        #region Utility
        private object BuildQuery(bool buildCommand)
        {
            DbCommand command = null;
            string query = "SELECT ";

            /*
             * Initiliaze the DbCommand based on the database factory provided.
             */
            if (buildCommand &&
               _dbProviderFactory == null)
            {
                throw new Exception("Cannot build object without a specified DbProviderFactory");
            }
            else if (buildCommand)
            {
                command = _dbProviderFactory.CreateCommand();
            }

            /*
             * If set, add DISTINCT to the query statement.
             */
            if (_distinct)
            {
                query += "DISTINCT ";
            }

            /*
             * If no columns has been added to the query, default to all columns.
             */
            if (_selectedColumns.Count == 0)
            {
                if (_selectedTables.Count == 1)
                {
                    query += _selectedTables[0] + ".";
                }
                query += "*";
            }            
            else
            {
                foreach (string column in _selectedColumns)
                {
                    query += column + ",";
                }

                query = query.TrimEnd(',');
                query += ' ';
            }

            /*
             * Add specified tables, if a table(s) has been added to the query.
             */
            if (_selectedTables.Count > 0)
            {
                query += " FROM ";
                foreach (string table in _selectedTables)
                {
                    query += table + ",";
                }

                query = query.TrimEnd(',');
                query += " ";
            }

            /*
             * Add given JOIN(s).
             */
            if (_joins.Count > 0)
            {
                foreach (Join join in _joins)
                {
                    query += join.JoinType.ToString() + " " + join.ToTable + " ON ";
                    query += WhereStatement.BuildComparisonClause(join.FromTable + "." + join.FromColumn, 
                                                                  join.ComparisonOperator, 
                                                                  new Literal(join.ToTable + "." + join.ToColumn));
                    query += " ";
                }
            }

            /*
             * Add given WHERE clauses.
             */
            if (_whereStatement.Count > 0)
            {
                query += " WHERE ";
                if (buildCommand)
                {
                    query += _whereStatement.BuildWhereStatement(true, ref command);
                }
                else
                {
                    query += _whereStatement.BuildWhereStatement();
                }
            }

            /*
             * Add GROUP BY clauses.
             */
            if (_groupBy.Count > 0)
            {
                query += " GROUP BY ";

                foreach (string column in _groupBy)
                {
                    query += column + ",";
                }
                query = query.TrimEnd(',');
                query += " ";
                                
                /*
                 * Add HAVING clauses.
                 */
                if (_having.Count > 0)
                {
                    query += " HAVING ";

                    if (buildCommand)
                    {
                        query += _having.BuildWhereStatement(true, ref command);
                    }
                    else
                    {
                        query += _having.BuildWhereStatement();
                    }
                }
            }

            /*
             * Add ORDER BY clauses.
             */
            if (_orderBy.Count > 0)
            {
                query += " ORDER BY ";

                foreach (Orderby clause in _orderBy)
                {
                    query += clause.ColumnName + clause.SortingOperator + ",";
                }
                query = query.TrimEnd(',');
                query += " ";
            }
            
            /*
             * Return build comand or query.
             */
            if (buildCommand)
            {
                command.CommandText = query;
                return command;
            }
            else
            {
                return query;
            }
        }
        #endregion

        #region Methods
        public DbCommand BuildCommand()
        {
            return (DbCommand)this.BuildQuery(true);
        }
        public string BuildQuery()
        {
            return (string)this.BuildQuery(false);
        }
        #endregion
    }
}
