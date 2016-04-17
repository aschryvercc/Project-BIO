using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

/*
 * Based on the blog post found here:
 * http://web.archive.org/web/20140312141611/http://hyperthink.net/blog/a-simple-ish-approach-to-custom-context-in-wcf/
 */

namespace qbExportService
{
    public class Context : IExtension<OperationContext>
    {
        public Context Current
        {
            get { return OperationContext.Current.Extensions.Find<Context>(); }
        }

        #region IExtension<OperationContext> Members

        public void Attach(OperationContext owner)
        {
            /*
             * Nothing
             */
        }

        public void Detach(OperationContext owner)
        {
            /*
             * Nothing
             */
        }

        #endregion

        #region Session Variables / Context Variables

        private int counter;
        public int Counter
        {
            get { return this.counter; }
            set { this.counter = value; }
        }

        private int ce_counter;
        public int Ce_counter
        {
            get { return this.ce_counter; }
            set { this.ce_counter = value; }
        }

        #endregion
    }
}