using Excel = Microsoft.Office.Interop.Excel;

namespace cep_challenge
{
    class Model
    {
        Excel.Application excel_application;
        Excel.Workbook workbook;
        Excel.Worksheet worksheet;
        Excel.Range used_range;
        public Model(string excel_file_path)
        {
            Logger.LogAStep($"INITIALIZING MODEL FOR {excel_file_path} file");
            excel_application = new Excel.Application();
            workbook = excel_application.Workbooks.Open(excel_file_path);
            worksheet = (Excel.Worksheet)workbook.Sheets[1];
            used_range = worksheet.UsedRange;
            Logger.LogAStep($"STARTING TO REMOVE DUPLICATES");
            Remove_duplicate_CEP_ranges();
            Logger.LogAStep($"DUPLICATES REMOVED");

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
        }
    }
}
