﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Project.CrossCutting.Base
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {

        }
    }
}
