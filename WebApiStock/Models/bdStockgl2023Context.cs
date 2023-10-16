using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApiStock.Models
{
    public class bdStockgl2023Context : DbContext
    {
        public bdStockgl2023Context():base("connBdStock") 
        {

        }

        public DbSet<Produit> Produits { get; set;}
    }
}