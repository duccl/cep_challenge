using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;

namespace cep_challenge
{
    class Crawler
    {
        HttpClient client;
        HtmlDocument document;

        public Crawler()
        {
            document = new HtmlDocument();
            client = new HttpClient();
        }

        private FormUrlEncodedContent Create_CEP_payload(int cep)
        {
            return new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "relaxation",$"{cep}"},
                    { "semelhante","N"},
                    { "tipoCEP", "ALL"},
                }
            );
        }
        public async Task Retrive_CEP_informations_request_and_load_it(int cep)
        {
            var payload = Create_CEP_payload(cep);
            var response = await client.PostAsync(ConfigurationManager.AppSettings["cep_correios_post_url"],payload);
            var content = await response.Content.ReadAsStringAsync();
            document.LoadHtml(content);
        }

        public async void Get_CEP_informations(int cep)
        {
            Logger.LogAStep($"SEARCHING FOR CEP {cep}");
            await Retrive_CEP_informations_request_and_load_it(cep);
            var cep_informations = document.DocumentNode.SelectNodes(ConfigurationManager.AppSettings["cep_informations_at_correios_selector"])
                                            .Cast<HtmlNode>()
                                            .Select(element => element.InnerHtml.Trim())
                                            .ToList();
            Logger.LogAStep($"END OF SEARCHING FOR CEP {cep}");
            Logger.LogAStep($"STARTING TO SAVE {cep}");
            Program.CEP_informations_collected.Add_CEP_informations(cep_informations,cep);
            Logger.LogAStep($"ENDING TO SAVE {cep}");
            System.Threading.Thread.Sleep(100);
        }
    }
}
