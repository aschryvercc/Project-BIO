using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery.GLOBALS
{
    public sealed class JoinTypes
    {
        private readonly string name;
        private readonly int value;

        public static readonly JoinTypes INNERJOIN = new JoinTypes("INNER JOIN", 1);
        public static readonly JoinTypes OUTERJOIN = new JoinTypes("OUTER JOIN", 2);
        public static readonly JoinTypes LEFTJOIN = new JoinTypes("LEFT JOIN", 3);
        public static readonly JoinTypes RIGHTJOIN = new JoinTypes("RIGHT JOIN", 4);

        private JoinTypes(string name, int value)
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
