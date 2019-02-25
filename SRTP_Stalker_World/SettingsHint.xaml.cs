using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;

namespace SRTP_Stalker_World
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void SettingsHint_Activated(object sender, EventArgs e)
        {
            //XDocument xdoc = new XDocument();
            //xdoc = XDocument.Load(@"settings\hint\hints.xml");
            //XElement root = xdoc.Element("objects");
            //ObservableCollection<XElement> list = new ObservableCollection<XElement>();
            //foreach (XElement item in root.Elements())
            //{
            //    list.Add(item);
            //}
            //listBox.ItemsSource = list;
        }
    }
}
