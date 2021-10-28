using ProposeAppAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ProposeAppAPI.DataModels
{
    public class Product
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 1)]
        public string brand { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 1)]
        [Index(IsUnique = true)]
        public string model { get; set; }

        [StringLength(30, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 1)]
        [Index(IsUnique = true)]
        public string stockCode { get; set; }

        public string description { get; set; }

        [Required]
        public int pricePer { get; set; }

        [Required]
        public string unit { get; set; }

        [Required]
        public double price { get; set; }

        [Required]
        [ForeignKey("currency")]
        public int currencyId { get; set; }
        public virtual Currency currency { get; set; }

        [Required]
        public bool isActive { get; set; }

        [Required]
        public DateTime createdDate { get; set; }

        [Required]
        [ForeignKey("createdBy")]
        public string createdById { get; set; }
        public virtual ApplicationUser createdBy { get; set; }

        public Product()
        {
            this.createdDate = DateTime.Now;
            this.createdById = HttpContext.Current.User.Identity.GetUserId();
        }
    }
}