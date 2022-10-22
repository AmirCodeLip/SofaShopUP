﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModel.Form
{
    public interface IInputRequired
    {
        bool Required { get; set; }
        string ErrorMessage { get; set; }
    }
}