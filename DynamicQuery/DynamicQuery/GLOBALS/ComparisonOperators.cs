using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.GLOBALS
{
    public sealed class ComparisonOperators
    {
        private readonly string name;
        private readonly int value;

        public static readonly ComparisonOperators EQUAL = new ComparisonOperators("=", 1);
        public static readonly ComparisonOperators NOTEQUAL = new ComparisonOperators("<>", 2);
        public static readonly ComparisonOperators GREATERTHAN = new ComparisonOperators(">", 3);
        public static readonly ComparisonOperators LESSTHAN = new ComparisonOperators("<", 4);
        public static readonly ComparisonOperators GREATERTHANEQUAL = new ComparisonOperators(">=", 5);
        public static readonly ComparisonOperators LESSTHANEQUAL = new ComparisonOperators("<=", 6);
        public static readonly ComparisonOperators NOTLESSTHAN = new ComparisonOperators("!<", 7);
        public static readonly ComparisonOperators NOTGREATERTHAN = new ComparisonOperators("!>", 8);

        private ComparisonOperators(string name, int value)
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
