using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Laboratory
{
    public static class FileHandler
    {
        private static readonly string remoteUri = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";

        public static string FileName { get; private set; } = "thrlist.xlsx";
        public static void GetFile()
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(remoteUri, FileName);
                        MessageBox.Show("Файл успешно скачан");
                    }
                }
                else
                {
                    MessageBox.Show("На вашем утройстве уже есть файл");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Неудалось скачать файл - {e.Message}");
            }
        }

        public static bool CheckFile()
        {
            if (!File.Exists(FileName))
            {
                MessageBoxResult result = MessageBox.Show("Файл не обнаружен, провести загрузку данных?", "Файл", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            GetFile();
                            return true;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Неудалось скачать файл");
                            return false;
                        }
                    case MessageBoxResult.No:
                        {
                            return false;
                        }
                    default:
                        return false;
                }
            }
            else
            {
                MessageBox.Show("На вашем утройстве уже обнаружен файл с данными");
                return true;
            }
        }

        public static void UpdateFile()
        {
            try
            {
                File.Delete(FileName);
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(remoteUri, FileName);
                    MessageBox.Show("Файл успешно обновлен");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Неудалось обновить файл - {e.Message}");
            }
        }
    }
}
