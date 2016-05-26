using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.CustomConfiguration
{
    public class TableSection : ConfigurationSection
    {
        [ConfigurationProperty("tables")]
        [ConfigurationCollection(typeof(TableCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public TableCollection Tables
        {
            get { return (TableCollection)base["tables"]; }
        }
    }
}
