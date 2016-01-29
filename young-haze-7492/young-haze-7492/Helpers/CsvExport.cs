using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text; //String manipulation and formatting
using System.Reflection; //PropertyInfo class

namespace young_haze_7492.Helpers
{
    public class CsvExport<T> where T: class
    {
        private List<T> ObjectList; //List of objects to export to csv file friendly.

        /*
         * Constructor
         */
        public CsvExport(List<T> objectList)
        {
            ObjectList = objectList;
        }

        /*
         * Method:      Export()
         * Parameters:  header - Include the name of the property information of the object.
         * Description: Returns a .csv file friendly string. Headers are on by default; pass false to exclude them.
         */ 
        public string Export(bool header = true)
        {
            StringBuilder sb = new StringBuilder();

            IList<PropertyInfo> objectInfo = typeof(T).GetProperties();

            if(header)
            {
                foreach(PropertyInfo info in objectInfo)
                {
                    sb.Append(info.Name).Append(",");
                }

                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

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
                }

                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            return sb.ToString();
        }
    }
}