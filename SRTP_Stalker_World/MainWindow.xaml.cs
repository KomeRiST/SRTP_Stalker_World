using SRTP_Stalker_World.GameTextObjects;
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Xml;
using System.Xml.Linq;

namespace SRTP_Stalker_World
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static GameClass GAME;
        public static ViewModel VM;
        public MainWindow()
        {
            InitializeComponent();
            VM = new ViewModel(); // Создаём вьюшку
            XmlDocument xDoc = new XmlDocument(); // Готовимся к работе с XML
            xDoc.Load(@"settings\hint\Hints.xml"); // грузим файл с настройками
            XmlElement xRoot = xDoc.DocumentElement; // Получаем корневой элемент
            XmlNode xNode = xRoot.SelectSingleNode("obj[folder_game]"); // ищем нужный НОДЕ с настройкаим проги
            string fn = xNode.SelectSingleNode("folder_game").InnerText; // Получаем путь к папке с игрой
            VM.Game.Load(fn); // Грузим файлы игры
            GAME = VM.Game;
            DataContext = VM; // Передаём нашу вьюшку в DataContext окна.
        }

        private void MainMenuFilesItem_Dialogs_Click(object sender, RoutedEventArgs e)
        {
            // 
            //TreeViewMainObjects.Items.Clear();
            //ArrayList dlgfiles = ViewModel.GetDialogFiles();
            //foreach (string f in dlgfiles)
            //{
            //    TreeViewItem tvFile = new TreeViewItem();
            //    tvFile.Header = f;
            //    TreeViewMainObjects.Items.Add(tvFile);
            //    List<string> DialogList = new List<string>();
            //    foreach (GameDialog item in ViewModel.Dialogs)
            //    {
            //        if (item.thisFile.FileName == ViewModel.Folder_game + ViewModel.GAMEPLAY + f)
            //        {
            //            DialogList.Add(item.Id);
            //        }
            //    }
            //    DialogList.Sort();
            //    foreach (string dlg in DialogList)
            //    {
            //        GameDialog dialog;
            //        ViewModel.Dialogs.TryGetValue(dlg, out dialog);
            //        TreeViewItem tvItem = new TreeViewItem();
            //        tvItem.Header = dlg;
            //        ToolTip mainTT = new ToolTip();
            //        StackPanel Panel = new StackPanel();
            //        Panel.Height = 100;
            //        Panel.Width = 150;
            //        Panel.Children.Add(new TextBlock { Text = $"Автор: {dialog.Podpis.Autor}" });
            //        Panel.Children.Add(new TextBlock { Text = $"Описание: {dialog.Podpis.Description}" });
            //        Panel.Children.Add(new TextBlock { Text = "----------------------" });
            //        Panel.Children.Add(new TextBlock { Text = $"Последнее изменение:\n {dialog.Podpis.LastChangeInfo.ToString()}" });
            //        mainTT.Content = Panel;
            //        tvItem.ToolTip = mainTT;
            //        tvFile.Items.Add(tvItem);
            //    }
            //}
        }

        private void TreeView_Strings_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView TV = (sender as TreeView);
            if (TV.SelectedItem is GameText)
            {
                VM.SelectedString = TV.SelectedItem as GameText;
            }
            if (TV.SelectedItem is GameDialog)
            {
                VM.SelectedDialog = TV.SelectedItem as GameDialog;
            }
            if (TV.SelectedItem is DlgFrase)
            {
                VM.SelectedDialog = (TV.SelectedItem as DlgFrase).ParentDialog;
            }
        }
        /// <summary>
        /// Использовал для анализа ХМЛ файлов.
        /// На выходе формировался ХМЛ файл с максимальновозможной структурой.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XDocument xNewDoc = new XDocument();
            XElement xResult = new XElement("Infoportions");
            XElement xR = new XElement("infoportion");
            ArrayList FileListInfoportions = GAME.GetInfoportionsFiles();
            foreach (string patch in FileListInfoportions)
            {
                if (File.Exists(GAME.Folder_game + GameClass.GAMEPLAY + patch))
                {
                    // нормализуем файл (удаляем комментарии)
                    string NFile = GAME.NormalFile(GAME.Folder_game + GameClass.GAMEPLAY + patch);
                    File.WriteAllText("buff_info.xml", NFile);

                    XDocument xdoc = new XDocument();
                    try
                    {
                        xdoc = XDocument.Load("buff_info.xml");
                        XElement root = xdoc.Element("game_information_portions");
                        foreach (XElement xInfo in root.Elements())
                        {
                            AnalizXml(xInfo, xR);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Проблема в файле ({patch})");
                        Console.WriteLine(value: $"ОштбкаЖ {e}");
                        continue;
                    }
                }
                else
                {
                    // Вывод в консоль
                }
            }
            xResult.Add(xR);
            xNewDoc.Add(xResult);
            xNewDoc.Save("xResult_Infoportion.xml");

            /// <summary>
            /// Создаёт полный XML элемент 
            /// (со всевозможными атрибутами и тегами)
            /// на основе чтения одинаковых XmlNode.
            /// </summary>
            void AnalizXml(XElement xTeg, XElement xEtalon)
            {
                foreach (XAttribute atr in xTeg.Attributes())
                {
                    try
                    {
                        xEtalon.Add(new XAttribute(atr.Name, ""));
                    }
                    catch
                    {
                        continue;
                    }
                }
                foreach (XElement xE in xTeg.Elements())
                {
                    if (xEtalon.HasElements)
                    {
                        bool b = true;
                        foreach (XElement xER in xEtalon.Elements())
                        {
                            if (xER.Name == xE.Name)
                            {
                                b = false;
                                AnalizXml(xE, xER);
                            }
                        }
                        if (b)
                        {
                            XElement xBuff = new XElement(xE.Name);
                            xEtalon.Add(xBuff);
                            AnalizXml(xE, xBuff);
                        }
                    }
                    else
                    {
                        XElement xBuff = new XElement(xE.Name);
                        xEtalon.Add(xBuff);
                        AnalizXml(xE, xBuff);
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 SettingWindow = new Window1();
            SettingWindow.Show();
        }

        private void TreeViewSearchResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeView TV = (sender as TreeView);
            if ((TV.SelectedValue is SearchResult) || (TV.SelectedItem is VMTreeObjs))
            {

            }
            else
            {
                VM.OpenObjToRedactor(TV.SelectedItem);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            VM.RemoveObjToRedactor(TabControlRedactors.SelectedItem);
        }

        private void TabControlRedactors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Main MainWin = new Main();
            MainWin.DataContext = DataContext;
            MainWin.Show();
        }
    }
}
