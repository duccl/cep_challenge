using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace cep_challenge
{
    class Model
    {
        Excel.Application excel_application;
        Excel.Workbook workbook;
        Excel.Worksheet worksheet;
        Excel.Range used_range;
        int last_row;
        public void Start_Excel(string file_path="")
        {
            last_row = 1;
            excel_application = new Excel.Application();
            excel_application.Visible = true;
            workbook = file_path.Length != 0 ? excel_application.Workbooks.Open(file_path) : excel_application.Workbooks.Add() ;
            worksheet = (Excel.Worksheet)workbook.ActiveSheet;
            used_range = worksheet.UsedRange;
        }

        public Model(string excel_file_path)
        {
            Logger.LogAStep($"INITIALIZING MODEL FOR {excel_file_path} file");
            Start_Excel(excel_file_path);
            Logger.LogAStep($"STARTING TO REMOVE DUPLICATES");
            Remove_duplicate_CEP_ranges();
            Logger.LogAStep($"DUPLICATES REMOVED");
        }

        public Model()
        {
            Logger.LogAStep($"INITIALIZING MODEL");
            Start_Excel();
        }

        public void Create_excel_header(string[] header)
        {
            for (int i = 0; i < header.Length; i++)
            {
                worksheet.Cells[1, i+1].Value2 = header[i];
            }
            last_row++;
        }
        public void Set_CEP_informations(CEP current_cep)
        {
            worksheet.Cells[last_row, 1] = current_cep.CEP_number;
            worksheet.Cells[last_row, 2] = current_cep.Bairro;
            worksheet.Cells[last_row, 3] = current_cep.Localidade;
            worksheet.Cells[last_row, 4] = current_cep.Logradouro;
            worksheet.Cells[last_row, 5] = current_cep.Momento_de_processamento;
            last_row += 1;
        }

        public string Get_current_moment()
        {
            return System.DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
        }

        public async void Add_CEP_informations(List<string> CEP_informations, int cep_number)
        {
            CEP current_cep = new CEP();
            current_cep.Momento_de_processamento = Get_current_moment();
            if (CEP_informations.Count != 0)
            {
                current_cep.Logradouro = CEP_informations[0];
                current_cep.Bairro = CEP_informations[1];
                current_cep.Localidade = CEP_informations[2];
                current_cep.CEP_number = CEP_informations[3];
            }
            else
            {
                current_cep.CEP_number = cep_number.ToString();
            }
            Set_CEP_informations(current_cep);
        } 
        
        public void Save_excel(string file_path)
        {
            workbook.SaveAs(file_path);
        }

        public int Get_last_row()
        {
            return used_range.Rows.Count;
        }

        private void Remove_duplicate_CEP_ranges()
        {
            used_range.RemoveDuplicates(1);
        }

        public int Retrieve_CEP(int row, int column)
        {
            return (int)worksheet.Cells[row, column].Value2;
        }

        ~Model()
        {           
            workbook.Close();
            excel_application.Quit();
            Marshal.ReleaseComObject(excel_application);
        }
    }
}
