using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Models
{
    public class PowerShellResponse
    {
        public string StandartOutput { get; set; }

        public Exception Exception { get; set; }

        public PowerShellResponseStatus ResponseStatus { get; set; }
    }
}
