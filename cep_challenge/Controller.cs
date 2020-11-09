namespace cep_challenge
{
    class Controller
    {
        public void Process_CEPs(ref Model excel_model, ref Model CEP_informations, ref Crawler crawler)
        {
            Logger.LogAStep("STARTING TO PROCESS CEPS");
            for (int i = 2; i < Program.excel_model.Get_last_row(); i++)
            {
                Logger.LogAStep($"STARTING TO PROCESS CEPS OF RANGE {i}");
                int initial_cep_of_range = Program.excel_model.Retrieve_CEP(i, 2);
                int last_cep_of_range = Program.excel_model.Retrieve_CEP(i, 3);
                for (int current_cep = initial_cep_of_range; current_cep <= last_cep_of_range; current_cep++)
                {
                    //pesquisa cep e guarda no model de CEPS
                    Program.crawler.Get_CEP_informations(current_cep);
                }
                Logger.LogAStep($"ENDING TO PROCESS CEPS OF RANGE {i}");
            }
            Logger.LogAStep("ENDING TO PROCESS CEPS");
        }
    }
}
