using System.Configuration;

namespace cep_challenge
{
    class Program
    {
        public const string appName = "cep_crawler";
        public static Model excel_model;
        public static Model CEP_informations_collected;
        public static Crawler crawler;
        public static Controller controller;
        
        static void Main(string[] args)
        {
            Logger.LogInitialStep();
            //excel_model = new Model(args[0]);
            excel_model = new Model(@"C:\Users\ecoltri2\OneDrive - DXC Production\Desktop\Lista_de_CEPs.xlsx");
            //CEP_informations_collected = new Model();
            CEP_informations_collected = new Model();
            crawler = new Crawler();
            controller = new Controller();
            //try
            //{
                CEP_informations_collected.Create_excel_header(ConfigurationManager.AppSettings["cep_excel_output_headers"].Split(';'));
                controller.Process_CEPs(ref excel_model, ref CEP_informations_collected, ref crawler);
                CEP_informations_collected.Save_excel(args[1]);
                Logger.LogEndStep();
                System.Environment.Exit(0);
            //}
            //catch(System.Exception err)
            //{
              //  Logger.LogErrorStep(err.ToString());
                if(CEP_informations_collected != null) CEP_informations_collected.Save_excel(args[1]);
                //System.Environment.Exit(1);
            //}
        }
    }
}
