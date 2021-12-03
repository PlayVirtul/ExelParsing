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
using PagedList;

namespace Laboratory
{
    public partial class MainWindow : Window
    {
        private List<Threat> allthreats;
        private IPagedList<Threat> threatsPage;
        private int pageNumber = 1;

        public MainWindow()
        {
            InitializeComponent();
            if (FileHandler.CheckFile())
            {
                allthreats = ReadExcelFile();
                threatsPage = GetPagedList();
                mainTable.ItemsSource = threatsPage;
                pageNumberText.Content = string.Format("Page {0}/{1}", pageNumber, threatsPage.PageCount);
            }
        }

        private void Download_Button(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(FileHandler.FileName))
            {
                FileHandler.GetFile();
                allthreats = ReadExcelFile();
                threatsPage = GetPagedList();
                mainTable.ItemsSource = threatsPage;
                pageNumberText.Content = string.Format("Page {0}/{1}", pageNumber, threatsPage.PageCount);
            }
            else
            {
                MessageBox.Show("На вашем утройстве уже есть файл");
            }
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
                        for (int k = 0; k < 8; k++)
                        {
                            if (worksheet.Cells[i, j + k].Value == null)
                            {
                                worksheet.Cells[i, j + k].Value = "";
                            }
                        }
                        Threat threat = new Threat(worksheet.Cells[i, j].Value.ToString(), worksheet.Cells[i, j + 1].Value.ToString(),
                                worksheet.Cells[i, j + 2].Value.ToString(), worksheet.Cells[i, j + 3].Value.ToString(),
                                worksheet.Cells[i, j + 4].Value.ToString(), worksheet.Cells[i, j + 5].Value.ToString(),
                                worksheet.Cells[i, j + 6].Value.ToString(), worksheet.Cells[i, j + 7].Value.ToString());

                        threats.Add(threat);
                    }
                }
            }
            return threats;
        }

        private IPagedList<Threat> GetPagedList(int pageNumber = 1, int pageSize = 15)
        {
            return allthreats.ToPagedList(pageNumber, pageSize);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if (row == null)
            {
                return;
            }
            Info infoWindow = new Info();
            infoWindow.Show();
            infoWindow.ShowInfo(threatsPage[row.GetIndex()]);
        }

        private void UpdateFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FileHandler.FileName))
            {
                var temp = allthreats;
                FileHandler.UpdateFile();
                allthreats = ReadExcelFile();
                UpdateInfo updateWindow = new UpdateInfo();
                updateWindow.UpdatedInfo(allthreats, temp);
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
                MessageBox.Show("Данные ещё не были загружены.");
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (threatsPage != null)
            {
                if (threatsPage.HasPreviousPage)
                {
                    threatsPage = GetPagedList(--pageNumber);
                    mainTable.ItemsSource = threatsPage;
                    pageNumberText.Content = string.Format("Page {0}/{1}", pageNumber, threatsPage.PageCount);
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (threatsPage != null)
            {
                if (threatsPage.HasNextPage)
                {
                    threatsPage = GetPagedList(++pageNumber);
                    mainTable.ItemsSource = threatsPage;
                    pageNumberText.Content = string.Format("Page {0}/{1}", pageNumber, threatsPage.PageCount);
                }
            }
        }
    }
}