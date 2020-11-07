using System.IO;
using System;

namespace cep_challenge
{
    static public class Logger
    {
        static private string INFO_LEVEL = "INFO";
        static private string ERROR_LEVEL = "ERROR";
        static private string STATUSLOGNAME = "statusLog.log";
        static private string ERRORLOGNAME = "log.log";

        static private string GetStatusLogPath()
        {
            return $".\\{Program.appName}_{STATUSLOGNAME}";
        }

        static private string GetErrorLogPath()
        {
            return $".\\{Program.appName}_{DateTime.UtcNow.ToString("ddMMyyyy")}_{ERRORLOGNAME}";
        }

        static private void PrintStepToConsole(string message, string level)
        {
            Console.WriteLine($"[{DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss")} || {level}] {message}");
        }

        static public void LogInitialStep()
        {
            string message = $"START {Directory.GetCurrentDirectory()}";
            string path = GetStatusLogPath();
            PrintStepToConsole(message, INFO_LEVEL);
            File.AppendAllText(path, $"{message},{DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss")}\n");
        }

        static public void LogAStep(string message)
        {
            string path = GetStatusLogPath();
            PrintStepToConsole(message, INFO_LEVEL);
            File.AppendAllText(path, $"SUBSTEP,{DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss")},{message}\n");
        }

        static public void LogErrorStep(string message)
        {
            string path = GetErrorLogPath();
            PrintStepToConsole(message, ERROR_LEVEL);
            File.WriteAllText(path, $"SUBSTEP,{DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss")},{message}\n");
        }

        static public void LogEndStep()
        {
            string path = GetStatusLogPath();
            string message = "END";
            PrintStepToConsole(message, INFO_LEVEL);
            File.AppendAllText(path, $"{message},{DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss")}\n");
        }
    }
}
