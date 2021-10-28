using ProposeAppAPI.ResponseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProposeAppAPI.Models
{
    public class AddProposeBindingModel
    {
        [Required]
        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        public string name { get; set; }

        [StringLength(1000, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        public string description { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        public string customer { get; set; }

        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.")]
        public string company { get; set; }

        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.")]
        public string inCharge { get; set; }

        [StringLength(255, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.")]
        public string project { get; set; }

        [Required]
        public DateTime startDate { get; set; }

        [Required]
        public int currency { get; set; }
    }

    public class EditProposeBindingModel
    {
        public TakeSingleProposeModel propose { get; set; }
        public List<ProposeVersionJsonLine> lines { get; set; }
    }
}