using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text; //String manipulation and formatting
using System.Reflection; //PropertyInfo class

namespace CSVExportService
{
    public class CsvExport<T> where T: class
    {
        private List<T> ObjectList; //List of objects to export to csv file friendly.

        /*
         * Constructor
         */
        public CsvExport(List<T> objectList)
        {
            ObjectList = objectList; //Set the private list of objects to export.
        }

        /*
         * Method:  ConvertToCsv()
         * Parameters:  obj - value to be converted to Csv friendly format.
         * Description: Converts a object value to a Csv friendly formatted string and returns it.
         */
        private string ConvertToCsv(object obj)
        {
            string value = obj.ToString();

            if (obj == null)
            {
                value = "";
            }
            else if (obj is DateTime)
            {
                if(((DateTime)obj).TimeOfDay.TotalSeconds == 0)
                {
                    value = ((DateTime)obj).ToString("yyyy-MM-dd");
                }
                else
                {
                    value = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
                }                
            }
            else
            {
                if (value.Contains(",") ||
                    value.Contains("\""))
                {
                    value = '"' +
                            value.Replace("\"", "\"\"") +
                            '"';
                }
            }

            return value;
        }

        /*
         * Method:      Export()
         * Parameters:  header - Include the name of the property information of the object.
         * Description: Returns a .csv file friendly string. Headers are on by default; pass false to exclude them.
         */ 
        public string Export(bool header = true)
        {
            StringBuilder sb = new StringBuilder(); //Csv friendly string to build.

            IList<PropertyInfo> objectInfo = typeof(T).GetProperties(); //The property information list of the objects.

            /*
             * Enter if header information is to be included.
             */ 
            if(header)
            {
                /*
                 * For each object information in the object information list, append the header information to the string.
                 */ 
                foreach(PropertyInfo info in objectInfo)
                {
                    sb.Append(info.Name).Append(",");
                }

                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            /*
             * For each object in the object list, get the values of its properties and append it to the string.
             */ 
            foreach(T obj in ObjectList)
            {
                foreach(PropertyInfo info in objectInfo)
                {
                    /*
                     * Append value of the objects property here. Make sure it is in friendly to .csv formatting...
                     * This means calling a seperate method and returning the result here...
                     * 
                     * The prototype could be something like this:
                     * 
                     * string methodName(object val);
                     */
                    sb.Append(ConvertToCsv(info.GetValue(obj))).Append(",");
                }

                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            return sb.ToString();
        }
    }
}