using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.CustomConfiguration
{
    public class DbTableSections : ConfigurationSection
    {
        [ConfigurationProperty("tableSections")]
        [ConfigurationCollection(typeof(TableSectionCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public TableSectionCollection TableSections
        {
            get { return (TableSectionCollection)base["tableSections"]; }
        }

    }
}
