﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WcfGestionStock.Model
{
    public class examenContext:DbContext
    {
        public examenContext():base("connExamen") 
        {

        }
    }
}