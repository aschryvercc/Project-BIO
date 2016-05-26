using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.CustomConfiguration
{
    public class TableCollection : ConfigurationElementCollection
    {
        public TableElement this[int index]
        {
            get { return (TableElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(TableElement column)
        {
            BaseAdd(column);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(TableElement column)
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
            return new TableElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TableElement)element).Name;
        }
    }
}
