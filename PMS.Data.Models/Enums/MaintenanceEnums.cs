using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Data.Models.Enums
{
    
    public enum ConditionAfter
    {
        Good,
        Improved,
        Degraded,
        NotOperational
    }

    public enum Satus
    {
        Completed,
        Pending,
        WaitingSpares,
        InProgeress
    }
}
