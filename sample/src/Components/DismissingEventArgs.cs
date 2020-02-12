using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunit.SampleApp.Components
{
    public class DismissingEventArgs
    {
        public Alert Sender { get; }

        public bool Cancel { get; set; }

        public DismissingEventArgs(Alert sender)
        {
            Sender = sender;
        }
    }
}
