using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Models.ProviderTwo
{
    public class ProviderTwoPoint
    {
        // Mandatory
        // Name of point, e.g. Moscow\Sochi
        public string Point { get; set; }

        // Mandatory
        // Date for point in Route, e.g. Point = Moscow, Date = 2023-01-01 15-00-00
        public DateTime Date { get; set; }
    }
}
