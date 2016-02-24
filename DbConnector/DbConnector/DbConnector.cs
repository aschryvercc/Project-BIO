using System;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace DbConnector
{

    public class DbConnector
    {
        //tracks connection info
        private DbConnectorInfo cnnInfo = new DbConnectorInfo();

        //build select statement.
        private string buildSelect(bool hasJoins, Dictionary<string,string> leftPair, Dictionary<string, string> rightPair, List<string> columns, List<string> conditions)
        {
            string query = "select  ";

            foreach (string column in columns)
            {
                query += column + ", ";
            }
            query = query.Remove(query.Length - 3);

            query += " from " + leftPair.Keys.ElementAt(0) + " ";

            if (hasJoins)
            {
                for (int i = 0; i <= leftPair.Count; i++)
                {
                    query += " joins " + rightPair.Keys.ElementAt(i) + " on " +
                             leftPair.Values.ElementAt(i) + " = " + rightPair.Values.ElementAt(i);
                }
            }

            query += "where ";

            for (int i = 0; i <= conditions.Count; i++)
            {
                query += conditions[i] + ", ";
            }

            query += ";";

            return query;
        }

        //build insert statement
        private string buildInsertUpdate(string table, DataTable data, char useMode)
        {
            string query = "";
            if (useMode == 'i')
            {
                query = "insert into " + table + " (";
            }
            else if (useMode == 'u')
            {
                query = "update " + table + " (";
            }

            //insert columns to be inserted into
            foreach (DataColumn column in data.Columns)
            {
                query += column.ColumnName + ", ";
            }
            query = query.Remove(query.Length - 3);

            query += " values ";
            //insert data
            foreach (DataRow row in data.Rows)
            {
                query += "(";
                foreach (String val in row.ItemArray)
                {
                    query += val + ", ";
                }
                query = query.Remove(query.Length - 3);
                query += "), ";
            }
            query = query.Remove(query.Length - 3);
            query += ";";

            return query;
        }
        
        //execute pull into datatable
        public DataTable PullData(bool hasJoins, Dictionary<string, string> leftPair, Dictionary<string, string> rightPair, List<string> columns, List<string> conditions)
        {
            DataTable pulledContents = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = makeCnnString();
            SqlCommand cmd = new SqlCommand(buildSelect(hasJoins, leftPair, rightPair, columns, conditions), cnn);
           
            try
            {
                cnn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(pulledContents);
                cnn.Close();
                da.Dispose();
            }
            catch (Exception ex)
            {
                //Add exception handling...
            }

            return pulledContents;
        }

        public void insertUpdate(string table, DataTable data, char useMode)
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = makeCnnString();
            SqlCommand cmd = new SqlCommand(buildInsertUpdate(table, data, useMode), cnn);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch
            {
                //Add exception handling...
            }
        }

        //creates connection string
        private string makeCnnString()
        {
            return
                "Data Source = " + cnnInfo.serverName + ";" +
                "Initial Catalog = " + cnnInfo.serverName + "; " +
                "User ID = " + cnnInfo.userName + ";" +
                "Password = " + cnnInfo.password;
        }
    }
}