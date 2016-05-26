using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.GLOBALS
{
    public sealed class SortingOperators
    {
        private readonly string name;
        private readonly int value;

        public static readonly SortingOperators ASCENDING = new SortingOperators("ASCENDING", 1);
        public static readonly SortingOperators DESCENDING = new SortingOperators("DESCENDING", 2);

        private SortingOperators(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
