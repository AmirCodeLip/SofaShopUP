﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public interface IDefaultCreatorRepository
    {
        Task FastCreate();
    }
}