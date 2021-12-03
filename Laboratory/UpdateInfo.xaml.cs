using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Laboratory
{
    /// <summary>
    /// Interaction logic for UpdateInfo.xaml
    /// </summary>
    public partial class UpdateInfo : Window
    {
        private HashSet<Threat> oldThreats = new HashSet<Threat>();
        private HashSet<Threat> newThreats = new HashSet<Threat>();
        public UpdateInfo()
        {
            InitializeComponent();
        }

        public void UpdatedInfo(List<Threat> oldList, List<Threat> newList)
        {
            int different = 0;
            if(oldList.Count > newList.Count)
            {
                for (int i = 0; i < newList.Count; i++)
                {
                    var old = oldList[i].values;
                    var after = newList[i].values;

                    for (int j = 0; j < old.Count; j++)
                    {
                        if (old[j].ToString() != after[j].ToString())
                        {
                            newThreats.Add(newList[i]);
                            oldThreats.Add(oldList[i]);
                            different++;
                            break;
                        }
                    }
                }
                for (int i = newList.Count; i < oldList.Count; i++)
                {
                    oldThreats.Add(oldList[i]);
                    different++;
                }
            }
            else
            {
                for (int i = 0; i < oldList.Count; i++)
                {
                    var old = oldList[i].values;
                    var after = newList[i].values;

                    for (int j = 0; j < old.Count; j++)
                    {
                        if (old[j].ToString() != after[j].ToString())
                        {
                            newThreats.Add(newList[i]);
                            oldThreats.Add(oldList[i]);
                            different++;
                            break;
                        }
                    }
                }
                for(int i = oldList.Count; i < newList.Count; i++)
                {
                    newThreats.Add(newList[i]);
                    different++;
                }
            }

            MessageBoxResult result = MessageBox.Show($"Обновленных угроз: {different}, показать информации об обновлении?",
                    "Обновление", MessageBoxButton.YesNo);
            switch(result)
            {
                case MessageBoxResult.Yes:
                    Show();
                    oldTable.ItemsSource = oldThreats;
                    newTable.ItemsSource = newThreats;
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
