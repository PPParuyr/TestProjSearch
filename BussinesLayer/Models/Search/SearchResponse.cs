﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Models.Search
{
    public class SearchResponse
    {
        // Mandatory
        // Array of routes
        public Route[] Routes { get; set; }

        // Mandatory
        // The cheapest route
        public decimal MinPrice { get; set; }

        // Mandatory
        // Most expensive route
        public decimal MaxPrice { get; set; }

        // Mandatory
        // The fastest route
        public int MinMinutesRoute { get; set; }

        // Mandatory
        // The longest route
        public int MaxMinutesRoute { get; set; }
    }
}
