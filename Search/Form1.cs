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
            data.ColumnCount = 5;
            data.Columns[0].Name = "Filename";
            data.Columns[1].Name = "Path";
            data.Columns[2].Name = "Size";
            data.Columns[3].Name = "Cell"; //worksheet and cell name
            data.Columns[4].Name = "Text"; //found text
            data.AllowUserToAddRows = false;
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

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

        private void btn_search_Click(object sender, EventArgs e)
        {
            string folderPath = this.f_path.Text;
            string keyword = this.txt_keyword.Text;

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

            try
            {
                string[] dirs = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                foreach (string dir in dirs)
                {
                    string filename = Path.GetFileName(dir);
                    string length = getSize(dir);
                    string ext = Path.GetExtension(dir);
                    string cell = "";
                    string text = null;

                    if (ext.Equals(".txt")) text = getLine(dir, keyword); else 
                    if (ext.Equals(".xls") || ext.Equals(".xlsx")) getCell(dir, keyword, ext, filename, length); else
                    if (System.Text.RegularExpressions.Regex.IsMatch(filename, keyword)) text = "";
                    else continue;

                    if (text != null)
                    {
                        string[] row = new string[] { filename, dir, length, cell, text };
                        data.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
            }
        }

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

        private string getLine(string dir, string key)
        {
            using (var reader = new StreamReader(dir))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (System.Text.RegularExpressions.Regex.IsMatch(line, key)) return line;
                }
                return null;
            }
        }

        [STAThread]
        private void getCell(string dir, string key, string ext, 
                             string filename, string length)
        {
            SpreadsheetInfo.SetLicense("EIKU-1Y5V-Y7R4-ASNB");

            ExcelFile ef = new ExcelFile();
            if (ext.Equals(".xlsx")) ef.LoadXlsx(dir, XlsxOptions.None);
            else ef.LoadXls(dir);

            
            string cell;
            string text;

            foreach (ExcelWorksheet sheet in ef.Worksheets)
            {
                foreach (var currnetRow in sheet.Rows)
                {
                    int row;
                    int col;
                    if (currnetRow.Cells.FindText(key, false, false, out row, out col))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(sheet.Cells[row, col].Value);
                        text = sb.ToString();
                        cell = sheet.Name+"/"+CellRange.RowColumnToPosition(row, col);
                        string[] nrow = new string[] { filename, dir, length, cell, text };
                        data.Rows.Add(nrow);
                        break;
                    }
                }
            }
        }

        private void data_results_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string dir = data.Rows[e.RowIndex].Cells[1].Value.ToString();
            Console.WriteLine("{0}", dir);
            System.Diagnostics.Process.Start(dir);
        }

        [STAThread]
        private void btn_exp_Click(object sender, EventArgs e)
        {
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
            
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "XLS files (*.xls)|*.xls|XLT files (*.xlt)|*.xlt|XLSX files (*.xlsx)|*.xlsx|XLSM files (*.xlsm)|*.xlsm|XLTX (*.xltx)|*.xltx|XLTM (*.xltm)|*.xltm|ODS (*.ods)|*.ods|OTS (*.ots)|*.ots|CSV (*.csv)|*.csv|TSV (*.tsv)|*.tsv|HTML (*.html)|*.html|MHTML (.mhtml)|*.mhtml|PDF (*.pdf)|*.pdf|XPS (*.xps)|*.xps|BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|TIFF (*.tif)|*.tif|WMP (*.wdp)|*.wdp";
            saveFileDialog.FilterIndex = 3;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                    ExcelFile ef = new ExcelFile();
                    ExcelWorksheet ws = ef.Worksheets.Add("Sheet1");
                    ws.InsertDataTable(dt, 0, 0, true);
                    ef.SaveXlsx(saveFileDialog.FileName);
            }
            
        }
    }
}
