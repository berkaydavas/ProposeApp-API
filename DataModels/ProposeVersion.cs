using Microsoft.AspNet.Identity;
using ProposeAppAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProposeAppAPI.DataModels
{
    public class ProposeVersion
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("propose")]
        public int proposeId { get; set; }
        public virtual Propose propose { get; set; }

        public string json { get; set; }

        [StringLength(10, ErrorMessage = "{0}, en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 1)]
        public string type { get; set; }

        public int reviseNumber { get; set; }

        [Required]
        public DateTime createdDate { get; set; }

        [Required]
        [ForeignKey("createdBy")]
        public string createdById { get; set; }
        public virtual ApplicationUser createdBy { get; set; }

        public ProposeVersion()
        {
            this.json = "{}";
            this.createdDate = DateTime.Now;
            this.createdById = HttpContext.Current.User.Identity.GetUserId();
        }
    }
}