﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SoftUniRestaurant.Models.Foods.ChildClasses
{
    public class MainCourse : Food
    {
        private const int InitialServingSize = 500;
        public MainCourse(string name, decimal price) : base(name, InitialServingSize, price)
        {
        }
    }
}
