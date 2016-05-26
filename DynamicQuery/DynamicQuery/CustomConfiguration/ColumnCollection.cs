using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.CustomConfiguration
{
    public class ColumnCollection : ConfigurationElementCollection
    {
        public ColumnElement this[int index]
        {
            get { return (ColumnElement)BaseGet(index); }
            set 
            { 
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(ColumnElement column)
        {
            BaseAdd(column);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(ColumnElement column)
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
            return new ColumnElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ColumnElement) element).Name;
        }
    }
}
