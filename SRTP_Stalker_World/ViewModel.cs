using SRTP_Stalker_World.GameTextObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace SRTP_Stalker_World
{
    public class ViewModel : INotifyPropertyChanged
    {

        // Публичные поля
        public GameClass Game { get; set; }
        public ObservableCollection<Object> ReactorObjects { get; set; }


        /// <summary>
        /// Текущий выбранный для редактирования диалог
        /// </summary>
        public GameDialog SelectedDialog
        {
            get { return selectedDialog; }
            set
            {
                selectedDialog = value;
                OnPropertyChanged("SelectedDialog");
            }
        }
        /// <summary>
        /// Текущий выбранный GameString для отображения и редактирования
        /// </summary>
        public GameText SelectedString
        {
            get
            {
                return selectedString;
            }
            set
            {
                selectedString = value;
                OnPropertyChanged("SelectedString");

            }
        }
        /// <summary>
        /// Строка для общего фильтра объектов
        /// </summary>
        public string SearchString
        {
            get
            {
                return searchString;
            }
            set
            {
                if (value.Length >= 3)
                {
                    searchString = value;
                    OnPropertyChanged("SearchString");
                    OnPropertyChanged("SearchObjs");
                }
                else
                {
                    searchString = value;
                }
            }
        }


        public IEnumerable<Object> TreeInis
        {
            get
            {
                List<VMTreeObjs> res = new List<VMTreeObjs>();
                ArrayList inifiles = Game.GetIniFiles();
                foreach (string f in inifiles)
                {
                    VMTreeObjs Dict = new VMTreeObjs();
                    List<GameIni> IniList = new List<GameIni>();
                    foreach (GameIni item in Game.Inis)
                    {
                        if (Path.GetFileName(item.thisFile.FileName) == f)
                        {
                            if (item.Id.IndexOf(SearchString) >= 0)
                            {
                                IniList.Add(item);
                            }
                        }
                    }

                    if (IniList.Count > 0)
                    {
                        Dict.FileName = f;
                        Dict.EnumObjcs = IniList;
                        res.Add(Dict);
                    }
                }
                return res;
            }
        }
        public IEnumerable<Object> TreeDialogs
        {
            get
            {
                List<VMTreeObjs> res = new List<VMTreeObjs>();
                ArrayList dlgfiles = Game.GetDialogFiles();
                foreach (string f in dlgfiles)
                {
                    VMTreeObjs Dict = new VMTreeObjs();
                    List<GameDialog> DialogList = new List<GameDialog>();
                    foreach (GameDialog item in Game.Dialogs)
                    {
                        if (item.thisFile.FileName == Game.Folder_game + GameClass.GAMEPLAY + f)
                        {
                            if (item.Id.IndexOf(SearchString) >= 0)
                            {
                                DialogList.Add(item);
                            }
                        }
                    }

                    if (DialogList.Count > 0)
                    {
                        Dict.FileName = f;
                        Dict.EnumObjcs = DialogList;
                        res.Add(Dict);
                    }
                }
                return res;
            }
        }
        public IEnumerable<Object> TreeStrings
        {
            get
            {
                List<VMTreeObjs> res = new List<VMTreeObjs>();
                ArrayList dlgfiles = Game.GetStringTableFiles(); // Массив имён файлов
                foreach (string f in dlgfiles) // цикл по именам файлов
                {
                    VMTreeObjs Dict = new VMTreeObjs(); // Словарь: Файл -> Строки
                    List<GameText> StringList = new List<GameText>(); // Массив отфильтрованных строк
                    foreach (GameText item in Game.Strings) // Цикл по всем строкам
                    {
                        if (item.thisFile.FileName == Game.Folder_game + GameClass.TEXT_RUS + f) // Имена файлов совпадают
                        {
                            if (item.Id.IndexOf(SearchString) >= 0)
                            {
                                StringList.Add(item);
                            }
                        }
                    }
                    if (StringList.Count > 0)
                    {
                        Dict.FileName = f;
                        Dict.EnumObjcs = StringList;
                        res.Add(Dict);
                    }
                }
                return res;
            }
        }
        public IEnumerable<Object> TreeInfos
        {
            get
            {
                List<VMTreeObjs> res = new List<VMTreeObjs>();
                ArrayList infofiles = Game.GetInfoportionsFiles(); // Массив имён файлов
                foreach (string f in infofiles) // цикл по именам файлов
                {
                    VMTreeObjs Dict = new VMTreeObjs(); // Словарь: Файл -> Строки
                    List<Infoportion> StringList = new List<Infoportion>(); // Массив отфильтрованных строк
                    foreach (Infoportion item in Game.Infoportions) // Цикл по всем строкам
                    {
                        if (item.thisFile.FileName == Game.Folder_game + GameClass.GAMEPLAY + f) // Имена файлов совпадают
                        {
                            if (item.Id.IndexOf(SearchString) >= 0)
                            {
                                StringList.Add(item);
                            }
                        }
                    }
                    if (StringList.Count > 0)
                    {
                        Dict.FileName = f;
                        Dict.EnumObjcs = StringList;
                        res.Add(Dict);
                    }
                }
                return res;
            }
        }
        public IEnumerable SearchObjs
        {
            get
            {
                List<SearchResult> res = new List<SearchResult>();

                SearchResult SRes_str = new SearchResult();
                SRes_str.NameObject = "Strings";
                SRes_str.Objs = TreeStrings.ToList();
                SRes_str.Count = SRes_str.Objs.Count;
                res.Add(SRes_str);

                SearchResult SRes_dlg = new SearchResult();
                SRes_dlg.NameObject = "Dialogs";
                SRes_dlg.Objs = TreeDialogs.ToList();
                SRes_dlg.Count = SRes_dlg.Objs.Count;
                res.Add(SRes_dlg);

                SearchResult SRes_inf = new SearchResult();
                SRes_inf.NameObject = "Infoportions";
                SRes_inf.Objs = TreeInfos.ToList();
                SRes_inf.Count = SRes_inf.Objs.Count;
                res.Add(SRes_inf);

                SearchResult SRes_ini = new SearchResult();
                SRes_ini.NameObject = "Ini_sections";
                SRes_ini.Objs = TreeInis.ToList();
                SRes_ini.Count = SRes_ini.Objs.Count;
                res.Add(SRes_ini);

                return res;
            }
        }


        /// <summary>
        /// Текущий выбранный для редактирования диалог
        /// </summary>
        private GameDialog selectedDialog;
        private string searchString;
        private GameText selectedString;

        // Команды
        private RelayCommand addDialogCommand;
        private RelayCommand addFraseVoiced;

        /// <summary>
        /// создание нового диалога
        /// </summary>
        public RelayCommand AddDialogCommand
        {
            get
            {
                return addDialogCommand ??
                  (addDialogCommand = new RelayCommand(obj =>
                  {
                      string fn = obj as string;
                      fn = Game.Folder_game + GameClass.GAMEPLAY + fn;
                      XmlDocument xDoc = new XmlDocument();
                      xDoc.Load("EmptyDialog.xml");
                      GameDialog d = new GameDialog(xDoc, fn);
                      Game.Dialogs.Insert(0, d);
                      SelectedDialog = d;
                  }));
            }
        }
        public RelayCommand ChangeText
        {
            get
            {
                return addDialogCommand ??
                  (addDialogCommand = new RelayCommand(obj =>
                  {
                      GameText newtext = obj as GameText;
                      SelectedString = newtext;
                  },
                  (obj) => SelectedString != null));
            }
        }
        public RelayCommand LoadGame
        {
            get
            {
                return addDialogCommand ??
                  (addDialogCommand = new RelayCommand(obj =>
                  {
                      string fn = obj as string;
                      Game.Load(fn);
                  }));
            }
        }
        public RelayCommand AddFraseVoiced
        {
            get
            {
                return addFraseVoiced ??
                  (addFraseVoiced = new RelayCommand(obj =>
                  {
                      MessageBoxResult result = MessageBox.Show("Окрасить кнопку в красный цвет?");
                  }));
            }
        }

        //Методы
        /// <summary>
        /// Метод добавляет элемент в список редактируемых объектов
        /// </summary>
        /// <param name="value">Передоваемый объект для редактирования</param>
        public void OpenObjToRedactor(Object value)
        {
            if (ReactorObjects.Contains(value))
            {
                // Выбрать этот TabItem
            }
            else
            {
                ReactorObjects.Add(value);
            }
        }
        /// <summary>
        /// Метод убирает элемент из списока редактируемых объектов
        /// </summary>
        /// <param name="value">Убираемый объект для редактирования</param>
        public void RemoveObjToRedactor(Object value)
        {
            ReactorObjects.Remove(value);
        }


        // Constructor
        public ViewModel()
        {
            SearchString = "";
            Game = new GameClass();
            ReactorObjects = new ObservableCollection<object>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    /// <summary>
    /// Менеджер выбора шаблона редакторов (Header)
    /// </summary>
    public class ManagerTemplateSelector_Header : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            { // Блок проверки элемента из массива редактируемых объектов
                if (item is GameTask)
                {
                    //GameTask taskitem = item as GameTask; // Приводим текущий Object к конкретному объекту
                    return element.FindResource("TabItemRTask_Header") as DataTemplate;

                    //if (taskitem.Prio == "1")
                    //    return
                    //        element.FindResource("importantTaskTemplate") as DataTemplate; // Собственно выбираем нужный шаблон для отображения инфы
                    //else
                    //    return
                    //        element.FindResource("myTaskTemplate") as DataTemplate;
                }
                if (item is GameDialog)
                {
                    return element.FindResource("TabItemRDialog_Header") as DataTemplate;
                }
                if (item is Infoportion)
                {
                    return element.FindResource("TabItemRInfoport_Header") as DataTemplate;
                }
                if (item is GameText)
                {
                    return element.FindResource("TabItemRString_Header") as DataTemplate;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Менеджер выбора шаблона редакторов (Content)
    /// </summary>
    public class ManagerTemplateSelector_Content : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            { // Блок проверки элемента из массива редактируемых объектов
                if (item is GameTask)
                {
                    //GameTask taskitem = item as GameTask; // Приводим текущий Object к конкретному объекту
                    return element.FindResource("TabItemRTask_Content") as DataTemplate;

                    //if (taskitem.Prio == "1")
                    //    return
                    //        element.FindResource("importantTaskTemplate") as DataTemplate; // Собственно выбираем нужный шаблон для отображения инфы
                    //else
                    //    return
                    //        element.FindResource("myTaskTemplate") as DataTemplate;
                }
                if (item is GameDialog)
                {
                    return element.FindResource("TabItemRDialog_Content") as DataTemplate;
                }
                if (item is Infoportion)
                {
                    return element.FindResource("TabItemRInfoport_Content") as DataTemplate;
                }
                if (item is GameText)
                {
                    return element.FindResource("TabItemRString_Content") as DataTemplate;
                }
            }
            return null;
        }
    }
}
