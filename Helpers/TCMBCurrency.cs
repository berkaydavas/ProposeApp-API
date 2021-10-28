using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace ProposeAppAPI.Helpers
{
    public class TCMBCurrency
    {
		public static List<TCMBCurrencyLine> GetCurrencies(DateTime date, bool allRecords = false)
        {
			try
			{
				string url = $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml";

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.ContentType = "application/xml";

				HttpWebResponse Response = (HttpWebResponse)request.GetResponse();

				string resultXML = new StreamReader(Response.GetResponseStream()).ReadToEnd();
				resultXML = resultXML.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("\\\"", "\"");

				XmlSerializer serializer = new XmlSerializer(typeof(TCMBCurrencies));

				using (StringReader reader = new StringReader(resultXML))
				{
					TCMBCurrencies currencies = (TCMBCurrencies)serializer.Deserialize(reader);
					return currencies.Currency;
				}
			}
			catch (WebException ex)
            {
				if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
				{
					HttpWebResponse resp = (HttpWebResponse)ex.Response;
					if (resp.StatusCode != HttpStatusCode.OK)
					{
						DateTime newDate = date.AddDays(-1);
						return GetCurrencies(newDate, allRecords);
					}
				}

				return new List<TCMBCurrencyLine>();
			}
        }
    }

	[XmlRoot(ElementName = "Currency")]
	public class TCMBCurrencyLine
	{

		[XmlElement(ElementName = "Unit")]
		public int Unit;

		[XmlElement(ElementName = "Isim")]
		public string Isim;

		[XmlElement(ElementName = "CurrencyName")]
		public string CurrencyName;

		[XmlElement(ElementName = "ForexBuying")]
		public string ForexBuying;

		[XmlElement(ElementName = "ForexSelling")]
		public string ForexSelling;

		[XmlElement(ElementName = "BanknoteBuying")]
		public string BanknoteBuying;

		[XmlElement(ElementName = "BanknoteSelling")]
		public string BanknoteSelling;

		[XmlElement(ElementName = "CrossRateUSD")]
		public string CrossRateUSD;

		[XmlElement(ElementName = "CrossRateOther")]
		public object CrossRateOther;

		[XmlAttribute(AttributeName = "CrossOrder")]
		public int CrossOrder;

		[XmlAttribute(AttributeName = "Kod")]
		public string Kod;

		[XmlAttribute(AttributeName = "CurrencyCode")]
		public string CurrencyCode;
	}

	[XmlRoot(ElementName = "Tarih_Date")]
	public class TCMBCurrencies
	{

		[XmlElement(ElementName = "Currency")]
		public List<TCMBCurrencyLine> Currency;

		[XmlAttribute(AttributeName = "Tarih")]
		public string Tarih;

		//[XmlAttribute(AttributeName = "Date")]
		//public DateTime Date;

		[XmlAttribute(AttributeName = "Bulten_No")]
		public string BultenNo;
	}
}