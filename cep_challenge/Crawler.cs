using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace cep_challenge
{
    class Crawler
    {
        private ChromeDriver driver;
        private int wait_threshold;
        private By cep_field_at_correios;
        private By cep_informations_at_correios;
        private By dados_nao_encontrados_selector;
        public Crawler()
        {
            Logger.LogAStep($"STARTING DRIVER SERVICE");
            var driverService = ChromeDriverService.CreateDefaultService(ConfigurationManager.AppSettings["chromedriver_location"]);
            Logger.LogAStep($"DRIVER SERVICE SUCCESSFULY STARTED");
            ChromeOptions options = new ChromeOptions();
            options.AddArguments(
                                  "--headless",
                                  "--no-sandbox",
                                  "--lang=pt-br",
                                  "--incognito");
            Logger.LogAStep($"STARTING CHROME DRIVER");
            driver = new ChromeDriver(driverService, options, TimeSpan.FromSeconds(240));
            driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(120));
            Logger.LogAStep($"CHROME DRIVER SUCCESSFULY STARTED");
            wait_threshold = 14000;
            cep_field_at_correios = By.CssSelector("input[name='relaxation']");
            cep_informations_at_correios = By.CssSelector("tbody > tr:nth-child(2) > td");
            dados_nao_encontrados_selector = By.XPath("//p[text()='DADOS NAO ENCONTRADOS']");
        }

        private void wait_for_element(By by)
        {
            int cont = 0;
            while (cont < wait_threshold)
            {
                try
                {
                    driver.FindElement(by);
                    return;
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                    cont++;
                }
            }
        }

        public void Search_CEP(int cep)
        {
            Logger.LogAStep($"SEARCHING FOR CEP {cep}");
            driver.FindElement(cep_field_at_correios).SendKeys($"{cep}");
            driver.FindElement(By.CssSelector("input[value='Buscar']")).Click();
            Logger.LogAStep($"END OF SEARCHING FOR CEP {cep}");
        }

        public List<string> Get_CEP_informations()
        {
            wait_for_element(By.XPath("//h3[text()='Busca CEP - Endereço']"));
            if (driver.FindElements(dados_nao_encontrados_selector).Count == 1) return new List<string>();
            return driver.FindElements(cep_informations_at_correios).Select(element => element.Text).ToList();
        }

        public void Go_to_correios()
        {
            Logger.LogAStep($"VISITING CORREIOS");
            driver.Url = ConfigurationManager.AppSettings["cep_correios_home_page"];
            wait_for_element(cep_field_at_correios);
            Logger.LogAStep($"END OF VISITING CORREIOS");
        }

        ~Crawler()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
