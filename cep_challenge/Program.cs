using System.Configuration;

namespace cep_challenge
{
    class Program
    {
        public const string appName = "cep_crawler";
        static void Main(string[] args)
        {
            Logger.LogInitialStep();
            //Model excel_model = new Model(args[0]);
            Model excel_model = new Model(@"C:\Users\ecoltri2\OneDrive - DXC Production\Desktop\Lista_de_CEPs.xlsx");
            Model CEP_informations_collected = new Model();
            CEP_informations_collected.Create_excel_header(ConfigurationManager.AppSettings["cep_excel_output_headers"].Split(';'));
            Controller controller = new Controller();
            Crawler crawler = new Crawler();
            crawler.Go_to_correios();
            controller.Process_CEPs(ref excel_model,ref CEP_informations_collected, ref crawler);
            CEP_informations_collected.Save_excel(".\\output.xlsx");
            Logger.LogEndStep();
        }
    }
}
