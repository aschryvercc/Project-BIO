using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuery
{
    public interface IExecuteBuilder
    {
        string BuildInsert();
        string BuildUpdate();
        string BuildInsertUpdate();
    }
}
