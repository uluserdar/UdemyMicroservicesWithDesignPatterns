using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IOrderRequestFailedEvent
    {
        int OrderId { get; set; }
        string Reason { get; set; }
    }
    
}
