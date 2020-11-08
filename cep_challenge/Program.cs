﻿using System.Configuration;

namespace cep_challenge
{
    class Program
    {
        public const string appName = "cep_crawler";
        static void Main(string[] args)
        {
            Logger.LogInitialStep();
            try
            {
                Model excel_model = new Model(args[0]);
                Model CEP_informations_collected = new Model();
                CEP_informations_collected.Create_excel_header(ConfigurationManager.AppSettings["cep_excel_output_headers"].Split(';'));
                Controller controller = new Controller();
                Crawler crawler = new Crawler();
                crawler.Go_to_correios();
                controller.Process_CEPs(ref excel_model, ref CEP_informations_collected, ref crawler);
                CEP_informations_collected.Save_excel(args[1]);
                Logger.LogEndStep();
                System.Environment.Exit(0);
            }
            catch(System.Exception err)
            {
                Logger.LogErrorStep(err.ToString());
                System.Environment.Exit(1);
            }
        }
    }
}
