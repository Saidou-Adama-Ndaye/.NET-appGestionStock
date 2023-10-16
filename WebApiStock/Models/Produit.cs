using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiStock.Models
{
    public class Produit
    {
        [Key]
        public int IdProduit { get; set; }

        [Required, MaxLength(80)]
        public string LibelleProduit { get; set; }

        [Required, MaxLength(500)]
        public string DescriptionProduit { get; set; }

        [Required]
        public float QteProduit { get; set; }

        [Required]
        public float PuProduit { get; set; }
    }
}