﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Models.ProviderTwo
{
    public class ProviderTwoRoute
    {
        // Mandatory
        // Start point of route
        public ProviderTwoPoint Departure { get; set; }

        // Mandatory
        // End point of route
        public ProviderTwoPoint Arrival { get; set; }

        // Mandatory
        // Price of route
        public decimal Price { get; set; }

        // Mandatory
        // Timelimit. After it expires, route became not actual
        public DateTime TimeLimit { get; set; }
    }
}
