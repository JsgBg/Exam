﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SoftUniRestaurant.Models.Tables.ChildClasses
{
    public class OutsideTable : Table
    {
        private const decimal InitialPricePerPerson = 3.5m;
        public OutsideTable(int tableNumber, int capacity) 
            : base(tableNumber, capacity, InitialPricePerPerson)
        {
        }
    }
}
