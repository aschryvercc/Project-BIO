using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.CustomConfiguration
{
    public class TableSectionCollection : ConfigurationElementCollection
    {
        public TableSection this[int index]
        {
            get { return (TableSection)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(TableSection column)
        {
            BaseAdd(column);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(TableSection column)
        {
            BaseRemove(column.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TableSection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TableSection)element).Name;
        }
    }
}
