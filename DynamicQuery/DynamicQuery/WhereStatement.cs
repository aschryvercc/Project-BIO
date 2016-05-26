using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicQuery.Clauses;
using DynamicQuery.GLOBALS;
using System.Data.Common;

namespace DynamicQuery
{
    public class WhereStatement : List<List<Where>>
    {
        #region Utilities
        private void AssertLevelExistence(int level)
        {
            if (this.Count < (level - 1))
            {
                throw new Exception("Level " + level + " not allowed because level " + (level - 1) + " does not exist.");
            }
            else if (this.Count < level)
            {
                this.Add(new List<Where>());
            }
        }

        private void AddWhereClause(Where clause, int level)
        {
            AssertLevelExistence(level);
            this[level - 1].Add(clause);
        }
        
        private static string FormatValue(object value)
        {
            string result = "";

            if (value.Equals(null))
            {
                result = "NULL";
            }
            else
            {
                switch (value.GetType().Name)
                {
                    case "string": 
                        result = "'" + ((string)value).Replace("'", "''") + "'";
                        break;

                    case "DateTime": 
                        result = "'" + ((DateTime)value).ToString("yyyy/MM/dd hh:mm:ss") + "'"; 
                        break;

                    case "DBNull": 
                        result = "NULL"; 
                        break;

                    case "Boolean":
                        result = "0";
                        if ((bool)value)
                        {
                            result = "1";
                        }
                        break;

                    case "Literal": 
                        result = ((Literal)value).Query; 
                        break;

                    default: 
                        result = value.ToString(); 
                        break;
                }
            }

            return result;
        }

        internal static string BuildComparisonClause(string column, ComparisonOperators comparisonOperator, object value)
        {
            string result = "";

            if (value != null &&
                value != DBNull.Value)
            {
                result = column + " " + comparisonOperator.ToString() + " " + FormatValue(value);
            }
            else
            {
                if (!(comparisonOperator.Equals(ComparisonOperators.EQUAL)) &&
                    !(comparisonOperator.Equals(ComparisonOperators.NOTEQUAL)))
                {
                    throw new Exception("Comparison Operator: \"" + comparisonOperator.ToString() + "\" cannot be used on a NULL value.");
                }
                else
                {
                    if (comparisonOperator.Equals(ComparisonOperators.EQUAL))
                    {
                        result = column + " IS NULL";
                    }
                    else if (comparisonOperator.Equals(ComparisonOperators.NOTEQUAL))
                    {
                        result = "NOT " + column + " IS NULL";
                    }
                }
            }

            return result;
        }
        #endregion

        #region Methods
        public void Add(Where clause)
        {
            this.Add(clause, 1);
        }
        public void Add(Where clause, int level)
        {
            this.AddWhereClause(clause, level);
        }
        public Where Add(string field, ComparisonOperators comparisonOperator, object compareValue)
        {
            return this.Add(field, comparisonOperator, compareValue, 1);
        }
        public Where Add(Enum field, ComparisonOperators comparisonOperator, object compareValue)
        {
            return this.Add(field.ToString(), comparisonOperator, compareValue, 1);
        }
        public Where Add(string field, ComparisonOperators comparisonOperator, object compareValue, int level)
        {
            Where newWhereClause = new Where(field, comparisonOperator, compareValue);
            this.AddWhereClause(newWhereClause, level);
            return newWhereClause;
        }

        public string BuildWhereStatement()
        {
            DbCommand dummy = null;
            return BuildWhereStatement(false, ref dummy);
        }

        public string BuildWhereStatement(bool useDbCommand, ref DbCommand dbCommand)
        {
            string result = "";

            foreach (List<Where> WhereStatement in this)
            {

                string WhereLevel = "";
                foreach (Where clause in WhereStatement)
                {
                    string whereClause = "";

                    if (useDbCommand)
                    {
                        string parameterName = string.Format("@p{0}_{1}",
                                               dbCommand.Parameters.Count + 1,
                                               clause.Column.Replace('.', '_'));

                        DbParameter dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = parameterName;
                        dbParameter.Value = clause.Value;
                        dbCommand.Parameters.Add(dbParameter);

                        whereClause += BuildComparisonClause(clause.Column, clause.ComparisonOperator, new Literal(parameterName));
                    }
                    else
                    {
                        whereClause = BuildComparisonClause(clause.Column, clause.ComparisonOperator, clause.Value);
                    }

                    foreach (SubClause subWhereClause in clause.SubClauses)
                    {
                        whereClause += subWhereClause.LogicalOperator;

                        if (useDbCommand)
                        {
                            string parameterName = string.Format("@p{0}_{1}",
                                                   dbCommand.Parameters.Count + 1,
                                                   clause.Column.Replace('.', '_'));

                            DbParameter dbParameter = dbCommand.CreateParameter();
                            dbParameter.ParameterName = parameterName;
                            dbParameter.Value = subWhereClause.Value;
                            dbCommand.Parameters.Add(dbParameter);

                            whereClause += BuildComparisonClause(clause.Column, subWhereClause.ComparisonOperator, new Literal(parameterName));
                        }
                        else
                        {
                            whereClause = BuildComparisonClause(clause.Column, subWhereClause.ComparisonOperator, subWhereClause.Value);
                        }
                    }

                    WhereLevel += "(" + whereClause + ") " + LogicalOperators.AND.ToString() + " "; 
                }

                WhereLevel = WhereLevel.Substring(0, WhereLevel.Length - 5);

                if (WhereStatement.Count > 1)
                {
                    result += " (" + WhereLevel + ") ";
                }
                else
                {
                    result += " " + WhereLevel + " ";
                }

                result += " " + LogicalOperators.OR.ToString();
            }

            result = result.Substring(0, result.Length - 2);
            return result;
        }
        #endregion
    }
}
