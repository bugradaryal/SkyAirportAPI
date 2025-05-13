using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DTO;

namespace Utilitys.CurrencyService
{
    public static class ForexManager
    {
        public static async Task<List<ForexDTO>> CurrencyGetter()
        {
            try
            {
                // TCMB döviz kuru verilerini çekeceğimiz URL
                  string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
                List<ForexDTO> list = new List<ForexDTO>();
                using (HttpClient client = new HttpClient())
                {
                    // XML verisini çekme
                    string xmlData = await client.GetStringAsync(url);

                    // XML verisini XDocument ile parse etme
                    XDocument xDoc = XDocument.Parse(xmlData);
                    List<string> types = ["USD", "EUR"];
                    foreach(string type in types)
                    {
                        var currency = xDoc.Descendants("Currency")
                              .Where(x => (string)x.Attribute("CurrencyCode") == type)
                              .Select(x => new
                              {
                                  CurrencyName = (string)x.Attribute("CurrencyCode"),
                                  ForexSelling = decimal.TryParse((string)x.Element("ForexSelling"), out var sellingRate) ? sellingRate : 0m
                              })
                              .FirstOrDefault();
                        if (currency != null)
                            list.Add(new ForexDTO { CurrencyName = currency.CurrencyName, ForexSelling = currency.ForexSelling });
                    }
                    return list;
                }
            }
            catch(Exception ex)
            {
                return null;
            }

        }
    }
}
