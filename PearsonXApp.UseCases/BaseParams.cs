using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonXApp.UseCases
{
    public class BaseParams
    {
        public required string UserName { get; set; }

        public DateTime DateTime { get; set; }
    }
}
