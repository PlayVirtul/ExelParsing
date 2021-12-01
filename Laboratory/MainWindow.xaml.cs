using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using OfficeOpenXml;

namespace Laboratory
{
    public partial class MainWindow : Window
    {
        private List<Threat> allThreats = new List<Threat>();

        public MainWindow()
        {
            InitializeComponent();
            FileHandler.CheckFile();
            allThreats = ReadExcelFile();
        }

        private void Download_Button(object sender, RoutedEventArgs e)
        {
            FileHandler.GetFile();
            allThreats = ReadExcelFile();
        }

        private void ShowTableButton_Click(object sender, RoutedEventArgs e)
        {
            mainTable.ItemsSource = allThreats;
        }

        private List<Threat> ReadExcelFile()
        {
            List<Threat> threats = new List<Threat>();
            FileInfo existingFile = new FileInfo(FileHandler.FileName);
            using (ExcelPackage excelPackage = new ExcelPackage(existingFile))
            {
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        int j = 1;
                        if (worksheet.Cells[i, j].Value != null)
                        {
                            Threat threat = new Threat(int.Parse(worksheet.Cells[i, j].Value.ToString()), worksheet.Cells[i, j + 1].Value.ToString(),
                                worksheet.Cells[i, j + 2].Value.ToString(), worksheet.Cells[i, j + 3].Value.ToString(),
                                worksheet.Cells[i, j + 4].Value.ToString(), worksheet.Cells[i, j + 5].Value.ToString(),
                                worksheet.Cells[i, j + 6].Value.ToString(), worksheet.Cells[i, j + 7].Value.ToString());
                            threats.Add(threat);
                        }
                    }
                }
            }
            return threats;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if(row == null)
            {
                return;
            }
            Info infoWindow = new Info();
            infoWindow.Show();
            infoWindow.ShowInfo(allThreats[row.GetIndex()]);
        }

        private void UpdateFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FileHandler.FileName))
            {
                var temp = allThreats;
                FileHandler.UpdateFile();
                allThreats = ReadExcelFile();

                MessageBox.Show(Convert.ToString(allThreats.Count));
                MessageBox.Show(Convert.ToString(temp.Count));
            }
            else
            {
                MessageBox.Show("Данные не были загружены.");
            }
        }

        private void SavingButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FileHandler.FileName))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel file |.xlsx;";
                saveFileDialog.DefaultExt = "xlsx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    byte[] bytes = File.ReadAllBytes(FileHandler.FileName);
                    File.WriteAllBytes(saveFileDialog.FileName, bytes);
                }
            }
            else
            {
                MessageBox.Show("Данные не были загружены.");
            }
        }
    }
}