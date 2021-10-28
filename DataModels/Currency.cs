using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProposeAppAPI.DataModels
{
    public class Currency
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 1)]
        [Index(IsUnique = true)]
        public string code { get; set; }

        [StringLength(10, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 1)]
        public string symbol { get; set; }

        public bool isPrimary { get; set; }

        public virtual ICollection<Propose> Proposes { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}