using ProposeAppAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.ComponentModel;

namespace ProposeAppAPI.DataModels
{
    public class Propose
    {
        [Key]
        public int id { get; set; }

        public int reviseNumber { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        public string keyName { get; set; }

        [StringLength(1000, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        public string description { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        public string customer { get; set; }

        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.")]
        public string company { get; set; }

        [StringLength(150, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.")]
        public string inCharge { get; set; }

        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.")]
        public string project { get; set; }

        [Required]
        public DateTime startDate { get; set; }

        [ForeignKey("currency")]
        public int currencyId { get; set; }
        public virtual Currency currency { get; set; }

        [Required]
        public DateTime exrateDate { get; set; }

        [Required]
        public DateTime createdDate { get; set; }

        [Required]
        [ForeignKey("createdBy")]
        public string createdById { get; set; }
        public virtual ApplicationUser createdBy { get; set; }


        public virtual ICollection<ProposeVersion> versions { get; set; }

        public Propose()
        {
            this.reviseNumber = 1;
            this.exrateDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.createdById = HttpContext.Current.User.Identity.GetUserId();
        }
    }
}