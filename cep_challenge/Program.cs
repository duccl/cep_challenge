using System;

namespace cep_challenge
{
    class Program
    {
        public const string appName = "cep_crawler";
        static Model model;
        static Crawler crawler;
        static void Process_CEPs()
        {
            for (int i = 2; i < model.Get_last_row(); i++)
            {
                int initial_cep_of_range = model.Retrieve_CEP(i, 2);
                int last_cep_of_range = model.Retrieve_CEP(i, 2);
                for (int current_cep = initial_cep_of_range; current_cep <= last_cep_of_range; current_cep++)
                {
                    //pesquisa cep e guarda no dict
                    crawler.Search_CEP(current_cep);
                    var response = crawler.Get_CEP_informations();
                    foreach (var item in response)
                    {
                        Console.WriteLine(item);
                    }
                    crawler.Go_to_correios();
                }
            }
        }

        static void Main(string[] args)
        {
            Logger.LogInitialStep();
            model = new Model(args[0]);
            crawler = new Crawler();
            crawler.Go_to_correios();
            Process_CEPs();
            Logger.LogEndStep();
        }
    }
}
