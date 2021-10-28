using ProposeAppAPI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProposeAppAPI.ResponseModels
{
    public class TakeProposesResponseModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string customer { get; set; }
        public string company { get; set; }
        public string inCharge { get; set; }
        public string project { get; set; }
        public DateTime startDate { get; set; }
        public double totalPrice { get; set; }
        public TakeSingleProposeCurrency currency { get; set; }
        public DateTime exrateDate { get; set; }
        public DateTime createdDate { get; set; }
        public TakeSingleProposeCreated createdBy { get; set; }
    }

    public class TakeSingleProposeResponseModel
    {
        public TakeSingleProposeModel propose { get; set; }
        public List<TakeSingleProposeCurrency> currencies { get; set; }
        public List<string> brands { get; set; }
    }

    public class TakeSingleProposeModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string customer { get; set; }
        public string company { get; set; }
        public string inCharge { get; set; }
        public string project { get; set; }
        public DateTime startDate { get; set; }
        public TakeSingleProposeCurrency currency { get; set; }
        public DateTime exrateDate { get; set; }
        public DateTime createdDate { get; set; }
        public TakeSingleProposeCreated createdBy { get; set; }
        public List<ProposeVersionJsonLine> lines { get; set; }
    }

    public class TakeSingleProposeCurrency
    {
        public int id { get; set; }
        public string symbol { get; set; }
        public string code { get; set; }
        public decimal value { get; set; }
    }

    public class TakeSingleProposeCreated
    {
        public string id { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
    }

    public class ProposeVersionJson
    {
        public List<ProposeVersionJsonLine> lines { get; set; }
    }

    public class ProposeVersionJsonLine
    {
        public ProposeVersionJsonLineType type { get; set; }
        public string key { get; set; }
        public int productId { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string description { get; set; }
        public int qty { get; set; }
        public string unit { get; set; }
        public double unitPrice { get; set; }
        public double productPrice { get; set; }
        public TakeSingleProposeCurrency priceCurrency { get; set; }
        public bool? productIsActive { get; set; }
        public decimal discountRate1 { get; set; }
        public double discountPrice1 { get; set; }
        public decimal discountRate2 { get; set; }
        public double discountPrice2 { get; set; }
        public bool isBrandDiscounted { get; set; }
        public string color { get; set; }
        public string visualId { get; set; }
        public DateTime createdDate { get; set; }
        public string createdById { get; set; }
        public DateTime updatedDate { get; set; }
        public string updatedById { get; set; }
        public List<ProposeVersionJsonLine> children { get; set; }
    }

    public enum ProposeVersionJsonLineType
    {
        productLine = 0,
        mainTitle = 1,
        subTitle = 2,
        mainCategoryTitle = 3,
        subCategoryLine = 4
    }
}