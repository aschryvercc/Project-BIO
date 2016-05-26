using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicQuery.Clauses;
using DynamicQuery.GLOBALS;
using System.Data.Common;

namespace DynamicQuery
{
    public class ExecuteBuilder : IExecuteBuilder
    {
        #region Members
        #region Table
        private string _tablename;
        public string TableName
        {
            set { this._tablename = value; }
            get { return this._tablename; }
        }
        #endregion

        #region Data
        private DataTable _datatable;
        public DataTable Datatable
        {
            set { this._datatable = value; }
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

        #region DbProviderFactory
        private DbProviderFactory _dbProviderFactory;
        public DbProviderFactory dbProviderFactory
        {
            set { this._dbProviderFactory = value; }
        }
        #endregion
        #endregion

        #region Constructor
        public ExecuteBuilder()
        {
            _tablename = "";
            _datatable = new DataTable();
            _whereStatement = new WhereStatement();
            _dbProviderFactory = null;
        }
        #endregion

        #region Utility
        private object BuildInsert(bool buildCommand)
        {
            string result = "";
            DbCommand command = null;

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

            string columns = string.Join(",", _datatable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            string values = string.Join(",", _datatable.Columns.Cast<DataColumn>().Select(c => string.Format("@{0}", c.ColumnName)));

            result = string.Format("INSERT INTO " + _tablename + "({0}) VALUES({1})", columns, values);

            /*
             * Return build comand or query.
             */
            if (buildCommand)
            {
                command.CommandText = result;
                return command;
            }
            else
            {
                return result;
            }
        }

        private object BuildUpdate(bool buildCommand)
        {
            string result = "";
            DbCommand command = null;

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

            string set = string.Join(",", _datatable.Columns.Cast<DataColumn>().Select(c => string.Format("{0} = @{0}", c.ColumnName)));

            result = string.Format("UPDATE {0} SET {1}", _tablename, set);

            /*
             * Add given WHERE clauses.
             */
            if (_whereStatement.Count > 0)
            {
                result += " WHERE " + _whereStatement.BuildWhereStatement();
            }
            else
            {
                throw new Exception("Cannot build UPDATE without a WHERE statement.");
            }

            /*
             * Return build comand or query.
             */
            if (buildCommand)
            {
                command.CommandText = result;
                return command;
            }
            else
            {
                return result;
            }
        }

        private object BuildInsertUpdate(bool buildCommand)
        {
            string result = "";
            DbCommand command = null;

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

            string columns = string.Join(",", _datatable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            string values = string.Join(",", _datatable.Columns.Cast<DataColumn>().Select(c => string.Format("@{0}", c.ColumnName)));
            string set = string.Join(",", _datatable.Columns.Cast<DataColumn>().Select(c => string.Format("{0} = VALUES({0})", c.ColumnName)));

            result = string.Format("INSERT INTO " + _tablename + "({0}) VALUES({1})", columns, values);
            result += string.Format(" ON DUPLICATE KEY UPDATE {0}", set);

            /*
             * Return build comand or query.
             */
            if (buildCommand)
            {
                command.CommandText = result;
                return command;
            }
            else
            {
                return result;
            }
        }
        #endregion

        #region Methods
        public DbCommand BuildInsertCommand()
        {
            return (DbCommand)this.BuildInsert(true);
        }

        public string BuildInsert()
        {
            return (string)this.BuildInsert(false);
        }

        public DbCommand BuildUpdateCommand()
        {
            return (DbCommand)this.BuildUpdate(true);
        }

        public string BuildUpdate()
        {
            return (string)this.BuildUpdate(false);
        }

        public DbCommand BuildInsertUpdateCommand()
        {
            return (DbCommand)this.BuildInsertUpdate(true);
        }
        public string BuildInsertUpdate()
        {
            return (string)this.BuildInsertUpdate(false);
        }
        #endregion
    }
}
