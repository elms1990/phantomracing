using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public struct Event
    {
        // Event category
        public String EventName { get; set; }

        // Object that sent this event
        public GameObject Sender { get; set; }

        // Object that will receive this event
        public GameObject Receiver { get; set; }

        // Possible event data
        public Object Data { get; set; }
    }
}
