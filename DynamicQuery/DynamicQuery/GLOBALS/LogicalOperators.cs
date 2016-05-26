using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.GLOBALS
{
    public sealed class LogicalOperators
    {
        private readonly string name;
        private readonly int value;

        public static readonly LogicalOperators ALL = new LogicalOperators("ALL", 1);
        public static readonly LogicalOperators AND = new LogicalOperators("AND", 2);
        public static readonly LogicalOperators ANY = new LogicalOperators("ANY", 3);
        public static readonly LogicalOperators BETWEEN = new LogicalOperators("BETWEEN", 4);
        public static readonly LogicalOperators EXISTS = new LogicalOperators("EXISTS", 5);
        public static readonly LogicalOperators IN = new LogicalOperators("IN", 6);
        public static readonly LogicalOperators LIKE = new LogicalOperators("LIKE", 7);
        public static readonly LogicalOperators NOT = new LogicalOperators("NOT", 8);
        public static readonly LogicalOperators OR = new LogicalOperators("OR", 9);
        public static readonly LogicalOperators ISNULL = new LogicalOperators("IS NULL", 10);
        public static readonly LogicalOperators UNIQUE = new LogicalOperators("UNIQUE", 11);

        private LogicalOperators(string name, int value)
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
