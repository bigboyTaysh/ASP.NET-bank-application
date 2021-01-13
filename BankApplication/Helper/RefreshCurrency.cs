using BankApplication.DAL;
using BankApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BankApplication.Helper
{
    public static class RefreshCurrency
    {
        public static async Task RefreshCurrenciesAsync()
        {
            ExchangeRatesSeries currency;

            using (var db = new BankContext())
            {
                foreach (var item in db.Currencies.ToList().Where(c => !c.Code.Contains("PLN")))
                {
                    currency = Task.Run(() => GetCurrencyExchangeRatesAsync(item.Code)).Result;

                    item.EffectiveDate = currency.Rates[0].EffectiveDate;
                    item.Bid = currency.Rates[0].Bid;
                    item.Ask = currency.Rates[0].Ask;

                    db.Entry(item).State = EntityState.Modified;
                }

                await db.SaveChangesAsync();
            }
        }

        public static async Task<ExchangeRatesSeries> GetCurrencyExchangeRatesAsync(string currency)
        {
            ExchangeRatesSeries res = new ExchangeRatesSeries();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client
                    .GetAsync($"http://api.nbp.pl/api/exchangerates/rates/c/{currency}/?format=json")
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode == true)
                {
                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    res = JsonConvert.DeserializeObject<ExchangeRatesSeries>(apiResponse);
                }
            }
               
            return res;
        }
    }
}