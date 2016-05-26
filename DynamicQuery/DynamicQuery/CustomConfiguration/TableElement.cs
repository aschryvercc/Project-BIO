using DynamicQuery.CustomConfiguration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.CustomConfiguration
{
    public class TableElement : ConfigurationSection
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("columns")]
        [ConfigurationCollection(typeof(ColumnCollection), 
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ColumnCollection Columns
        {
            get { return (ColumnCollection)base["columns"]; }
        }
    }
}
