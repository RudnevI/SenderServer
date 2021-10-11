using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenderServer
{
    class StatusPresentation
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public string Status { get; set; }

        public override string ToString()
        {
            return string.Format("{0:HH:mm:ss} - {1}", TimeStamp, Status);
        }
    }
}
