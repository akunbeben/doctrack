using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Common
{
    public class GlobalEnum
    {
        public enum ResponseStatus
        {
            Success,
            Failed
        }

        public enum DocumentStatus
        {
            Initiate,
            Progress,
            Done
        }

        public enum FlowStatus
        {
            New,
            SendTo,
            Received,
            SendBack
        }
    }
}
