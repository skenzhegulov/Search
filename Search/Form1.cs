using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GemBox.Spreadsheet;

namespace Exam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        DataGridView data;
        private void InitializeDataGridView()
        {
            data = this.data_results;
            //Initialize DataGridView
            data.ColumnCount = 5;
            data.Columns[0].Name = "Filename";  //file name
            data.Columns[1].Name = "Path";      //file path
            data.Columns[2].Name = "Size";      //filesize
            data.Columns[3].Name = "Cell";      //worksheet and cell name
            data.Columns[4].Name = "Text";      //found text
            data.AllowUserToAddRows = false;
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        //Event listener for 'Browse' button to get folder path for search
        private void btn_result_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
            }
            this.f_path.Text = folderPath;
        }

        //Event listener for 'Search' button, search files or txt/excel with given keyword
        private void btn_search_Click(object sender, EventArgs e)
        {
            //get folder path and keyword
            string folderPath = this.f_path.Text;
            string keyword = this.txt_keyword.Text;
            //Clear DataGridView
            do
            {
                foreach (DataGridViewRow row in data.Rows)
                {
                    try
                    {
                        data.Rows.Remove(row);
                    }
                    catch (Exception) { }
                }
            } while (data.Rows.Count > 0);
            //Look through all files in directory and subdirectories
            try
            {
                string[] dirs = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                //string[] dirs = getFiles(folderPath, keyword+"|*.txt|*.xls", SearchOption.AllDirectories);
                foreach (string dir in dirs)
                {
                    string filename = Path.GetFileName(dir);    //Get file name
                    string length = getSize(dir);               //Get Size of the file
                    string ext = Path.GetExtension(dir);        //Get extension of the file
                    string text = null;                         //To store text line or cell text with a keyword

                    /* 
                        First, if current file is txt then check its contents for the keyword
                        Otherwise, if it's excel file then check its contents for the keyword
                    */
                    if (ext.Equals(".txt")) text = getLine(dir, keyword);
                    else
                    if (ext.Equals(".xls") || ext.Equals(".xlsx"))
                    {
                        //Skip to the next file if we found cell or cells with a keyword
                        if (getCell(dir, keyword, ext, filename, length)) continue;
                    }

                    //We keyword was found in the text or in the filename, we add it to the DataGridView
                    if (text != null ||
                        System.Text.RegularExpressions.Regex.IsMatch(filename, WildcardToRegex(keyword)) ||
                        filename.Contains(keyword))
                    {
                        string[] row = new string[] { filename, dir, length, "", text };
                        data.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
            }
        }

        //Function to convert wildcard specifiers into regular expression
        private static string WildcardToRegex(string pattern)
        {
            return "^" + System.Text.RegularExpressions.Regex.Escape(pattern)
                              .Replace("\\*", ".*")
                              .Replace("\\?", ".")
                       + "$";
        }

        //Function to get files with a given search pattern
        public string[] getFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption)
        {
            // ArrayList will hold all file names
            List<string> alFiles = new List<string>();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            // returns string array of relevant file names
            return alFiles.ToArray();
        }

        //Function to get readable file size
        string[] sizes = { "B", "KB", "MB", "GB" };
        private string getSize(string dir)
        {
            long len = new System.IO.FileInfo(dir).Length;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        //Function to get line with some keyword in txt, if there is
        private string getLine(string dir, string key)
        {
            using (var reader = new StreamReader(dir))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains(key) || System.Text.RegularExpressions.Regex.IsMatch(line, WildcardToRegex(key)))
                        return line;
                }
                return null;
            }
        }

        //Function to look through excel to find cells with a given keyword
        [STAThread]
        private bool getCell(string dir, string key, string ext, string filename, string length)
        {
            SpreadsheetInfo.SetLicense("License_Key");
            //Load excel file
            ExcelFile ef = new ExcelFile();
            if (ext.Equals(".xlsx")) ef.LoadXlsx(dir, XlsxOptions.None);
                                else ef.LoadXls(dir);

            string cell;        //To store cell information
            string text;        //To store text of the cell
            bool flag = false;  //Becomes True when we find a cell with a given keyword

            //Look through whole excel for a given keyword
            foreach (ExcelWorksheet sheet in ef.Worksheets)
            {
                foreach (var currnetRow in sheet.Rows)
                {
                    int row;
                    int col;
                    if (currnetRow.Cells.FindText(key, false, false, out row, out col))
                    {
                        //Get text from the cell
                        StringBuilder sb = new StringBuilder();
                        sb.Append(sheet.Cells[row, col].Value);
                        text = sb.ToString();

                        //Get cell information
                        cell = sheet.Name+"/"+CellRange.RowColumnToPosition(row, col);

                        //Add to the DataGridView
                        string[] nrow = new string[] { filename, dir, length, cell, text };
                        data.Rows.Add(nrow);

                        //Indicate that cell with a given keyword was found
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        //Event listener to run selected file from the DataGridView
        private void data_results_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string dir = data.Rows[e.RowIndex].Cells[1].Value.ToString();
            Console.WriteLine("{0}", dir); //For debug
            System.Diagnostics.Process.Start(dir);
        }

        //Event listere for 'Expand' button, to save DataGridView as an excel file
        [STAThread]
        private void btn_exp_Click(object sender, EventArgs e)
        {
            //Convert DataGridView to a DataTable
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in data.Columns)
            {
                dt.Columns.Add(col.HeaderText);
            }

            foreach (DataGridViewRow row in data.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            
            //Create saveFileDialog window
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            //Save DataTable as an excel file
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExcelFile ef = new ExcelFile();
                    ExcelWorksheet ws = ef.Worksheets.Add("Sheet1");
                    ws.InsertDataTable(dt, 0, 0, true);
                    ef.SaveXlsx(saveFileDialog.FileName);
                }
                catch (Exception) { }
            }
            
        }
    }
}
