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
        // Константы
        public const string GAMEDATA = @"gamedata\";
        public const string CONFIG = @"gamedata\config\";
        public const string SYSTEM_FILE = @"gamedata\config\system.ltx";
        public const string LOCALIZATION_FILE = @"gamedata\config\localization.ltx";
        public const string GAMEPLAY = @"gamedata\config\gameplay\";
        public const string TEXT_RUS = @"gamedata\config\text\rus\";

        // Публичные поля
        /// <summary>
        /// Путь к папке с игрой
        /// </summary>
        public string Folder_game
        {
            get { return folder_game; }
            set
            {
                folder_game = value;
            }
        }
        /// <summary>
        /// Все диалоги игры
        /// </summary>
        public ObservableCollection<GameDialog> Dialogs { get; set; }
        /// <summary>
        /// Русские тексты
        /// </summary>
        public ObservableCollection<GameText> Strings { get; set; }
        /// <summary>
        /// Все инфопоршни игры
        /// </summary>
        public ObservableCollection<Infoportion> Infoportions { get; set; }

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
                    searchString = "";
                }
            }
        }


        public IEnumerable<Object> TreeDialogs
        {
            get
            {
                //Dictionary<string, List<GameDialog>> Dict = new Dictionary<string, List<GameDialog>>();
                List<VMTreeObjs> res = new List<VMTreeObjs>();
                ArrayList dlgfiles = GetDialogFiles();
                foreach (string f in dlgfiles)
                {
                    VMTreeObjs Dict = new VMTreeObjs();
                    List<GameDialog> DialogList = new List<GameDialog>();
                    foreach (GameDialog item in Dialogs)
                    {
                        if (item.thisFile.FileName == Folder_game + GAMEPLAY + f)
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
                //return Dict;
                return res;
            }
        }
        public IEnumerable<Object> TreeStrings
        {
            get
            {
                List<VMTreeObjs> res = new List<VMTreeObjs>();
                ArrayList dlgfiles = GetStringTableFiles(); // Массив имён файлов
                foreach (string f in dlgfiles) // цикл по именам файлов
                {
                    VMTreeObjs Dict = new VMTreeObjs(); // Словарь: Файл -> Строки
                    List<GameText> StringList = new List<GameText>(); // Массив отфильтрованных строк
                    foreach (GameText item in Strings) // Цикл по всем строкам
                    {
                        if (item.thisFile.FileName == Folder_game + TEXT_RUS + f) // Имена файлов совпадают
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

                return res;
            }
        }

        public ArrayList GetInfoportionsFiles()
        {
            ArrayList res = new ArrayList();
            if (File.Exists(Folder_game + SYSTEM_FILE))
            {
                //Создание объекта, для работы с файлом
                INIManager IniManager = new INIManager(Folder_game + SYSTEM_FILE);

                //Получить значение
                string files = IniManager.GetPrivateString("info_portions", "files");
                string[] buff = files.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in buff)
                {
                    res.Add(str.Trim() + ".xml");
                }
            }
            else
            {
                throw new Exception($"Отсутствует файл {SYSTEM_FILE}");
            }
            return res;
        }
        public ArrayList GetTasksFiles()
        {
            ArrayList res = new ArrayList();
            if (File.Exists(Folder_game + SYSTEM_FILE))
            {
                //Создание объекта, для работы с файлом
                INIManager IniManager = new INIManager(Folder_game + SYSTEM_FILE);

                //Получить значение
                string files = IniManager.GetPrivateString("game_tasks", "files");
                string[] buff = files.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in buff)
                {
                    res.Add(str.Trim() + ".xml");
                }
            }
            else
            {
                throw new Exception($"Отсутствует файл {SYSTEM_FILE}");
            }
            return res;
        }

        /// <summary>
        /// Текущий выбранный для редактирования диалог
        /// </summary>
        private GameDialog selectedDialog;
        private string folder_game;
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
                      fn = Folder_game + GAMEPLAY + fn;
                      XmlDocument xDoc = new XmlDocument();
                      xDoc.Load("EmptyDialog.xml");
                      GameDialog d = new GameDialog(xDoc, fn);
                      Dialogs.Insert(0, d);
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
                      Load(fn);
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

        // Поиск элементов в масивах
        /// <summary>
        /// Возвращает объект GameString
        /// </summary>
        /// <param name="Id_string">Уникальное имя текста</param>
        /// <returns>GameString или null</returns>
        public GameText GetGameStringByID(string Id_string)
        {
            foreach (GameText str in Strings)
            {
                if (str.Id == Id_string)
                {
                    return str;
                }
            }
            return new GameText();
        }
        /// <summary>
        /// Поиск текста в файлах локализации по ID текста
        /// </summary>
        /// <param name="Id_string">Уникальное имя текста</param>
        /// <returns>Русский текст или пустая строка</returns>
        public string GetTextRusByID(string Id_string)
        {
            return GetGameStringByID(Id_string).Text;
        }
        /// <summary>
        /// Возвращает полный путь к файлу с искомым game_string
        /// </summary>
        /// <param name="Id_string">id game_string</param>
        /// <returns>Путь к файлу или пустая строка</returns>
        public string GetFileNameByStringID(string Id_string)
        {
            return GetGameStringByID(Id_string).thisFile.FileName;
        }

        /// <summary>
        /// Поиск поршня
        /// </summary>
        /// <param name="vId">ID поршня</param>
        /// <returns>Объект Infoportion</returns>
        public Infoportion GetInfoportionById(string vId)
        {
            foreach (Infoportion item in Infoportions)
            {
                if (item.Id == vId)
                {
                    return item;
                }
            }
            return null;
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

        /// <summary>
        /// Чтение и загрузка ресурсов игры
        /// </summary>
        /// <param name="Folder_game">Путь к папке с игрой</param>
        public void Load(string Folder_game)
        {
            // Проверяем существование папки gamedata\
            if (Directory.Exists(Folder_game + GAMEDATA))
            {
                // Считываем файл локализации
                ArrayList arrSTFiles = GetStringTableFiles();
                // Проверить каждый файл на существование
                foreach (string patch in arrSTFiles)
                {
                    if (File.Exists(Folder_game + TEXT_RUS + patch))
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(Folder_game + TEXT_RUS + patch);
                        XmlNode xStringTable = xDoc.DocumentElement;
                        XmlNodeList xStringList = xStringTable.SelectNodes("string");
                        foreach (XmlNode xString in xStringList)
                        {
                            Strings.Add(new GameText(xString, Folder_game + TEXT_RUS + patch));
                        }
                    }
                    else
                    {
                        // Вывод в консоль
                    }
                }
                // Получили список файлов с поршнями
                ArrayList arrInfoFiles = GetInfoportionsFiles();
                // Проверить каждый файл на существование
                foreach (string patch in arrInfoFiles)
                {
                    if (File.Exists(Folder_game + GAMEPLAY + patch))
                    {
                        // Открыть файл
                        XmlDocument xDoc = new XmlDocument();
                        try
                        {
                            xDoc.Load(Folder_game + GAMEPLAY + patch);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка {ex.Message} в файле: {patch}");
                            continue;
                        }
                        finally
                        {
                            XmlNode xRoot = xDoc.DocumentElement;
                            if (xRoot != null)
                            {
                                // Ищем XML диалога
                                XmlNodeList xInfos = xRoot.SelectNodes("info_portion");
                                // Передаём в конструктор диалога строку с XML
                                foreach (XmlNode n in xInfos)
                                {
                                    Infoportion Inf = new Infoportion(n, Folder_game + GAMEPLAY + patch);
                                    //string dlgid = n.Attributes.GetNamedItem("id").Value;
                                    Infoportions.Add(Inf);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Вывод в лог
                    }
                }
                // Получили список файлов с диалогами
                ArrayList arrDialogFiles = GetDialogFiles();
                // Проверить каждый файл на существование
                foreach (string patch in arrDialogFiles)
                {
                    if (File.Exists(Folder_game + GAMEPLAY + patch))
                    {
                        // Открыть файл
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(Folder_game + GAMEPLAY + patch);
                        XmlNode xRoot = xDoc.DocumentElement;
                        // Ищем XML диалога
                        XmlNodeList xDialogs = xRoot.SelectNodes("dialog");
                        // Передаём в конструктор диалога строку с XML
                        foreach (XmlNode n in xDialogs)
                        {
                            GameDialog Dlg = new GameDialog(n, Folder_game + GAMEPLAY + patch);
                            //string dlgid = n.Attributes.GetNamedItem("id").Value;
                            Dialogs.Add(Dlg);
                        }
                    }
                    else
                    {
                        //Вывод в лог
                    }
                }
            }
            else
            {
                throw new Exception("В папке с игрой не найдена папка 'gamedata' \n распакуйте *.db архивы и перезапустите программу.");
            }
        }
        /// <summary>
        /// Нормализация XML файла путём удаления комментариев
        /// </summary>
        /// <param name="v">Полный путь к файлу</param>
        /// <returns>Строка XML данных</returns>
        public string NormalFile(string v)
        {
            string s = ""; // Строка - результат
            string[] lines = File.ReadAllLines(v); // Читаем файл построчно
            for (int i = lines.Length - 1; i > 0; i--)
            {
                string r = lines[i]; // буфер текущей строки
                string pattern = @"<!--(.*)-->"; // определяем строку паттерна для поиска комментов в стрке
                string target = ""; // определяем строку на которую будем производить замену
                Regex regex = new Regex(pattern); // создаём паттерн
                lines[i] = regex.Replace(r, target); // производим замену

                if (lines[i].IndexOf("#include") >= 0) // проверяем тек.строку на наличие подключения др. файлов в текущий файл
                {
                    // <!-- textextext -->
                    Regex rg = new Regex(@"""(.*)""");  // создаём паттерн "файл.тип"
                    string result = rg.Match(lines[i]).Groups[1].Value; // получаем текст между кавычками
                    Console.WriteLine(result);
                    string fil = File.ReadAllText(Folder_game + CONFIG + result); // читаем полученный файл
                    lines[i] = regex.Replace(fil, target); // вставляем текст файла вместо #include
                }
                s = lines[i] + s;
            }
            return s;
        }
        /// <summary>
        /// Получаем строковый массив с именами диалоговых файлов 
        /// </summary>
        /// <returns>ArrayList с именами xml-файлов</returns>
        public ArrayList GetDialogFiles()
        {
            ArrayList arrDialogFiles = new ArrayList();
            if (File.Exists(Folder_game + SYSTEM_FILE))
            {
                //Создание объекта, для работы с файлом
                INIManager IniManager = new INIManager(Folder_game + SYSTEM_FILE);

                //Получить значение
                string files = IniManager.GetPrivateString("dialogs", "files");
                string[] buff = files.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in buff)
                {
                    arrDialogFiles.Add(str.Trim() + ".xml");
                }
            }
            else
            {
                throw new Exception($"Отсутствует файл {SYSTEM_FILE}");
            }
            return arrDialogFiles;
        }
        /// <summary>
        /// Получаем список файлов локализации
        /// </summary>
        /// <returns>Массив строк с файлами локализации</returns>
        public ArrayList GetStringTableFiles()
        {
            ArrayList arrDialogFiles = new ArrayList();
            if (File.Exists(Folder_game + LOCALIZATION_FILE))
            {
                //Создание объекта, для работы с файлом
                INIManager IniManager = new INIManager(Folder_game + LOCALIZATION_FILE);

                //Получить значение
                string files = IniManager.GetPrivateString("string_table", "files");
                string[] buff = files.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in buff)
                {
                    arrDialogFiles.Add(str.Trim() + ".xml");
                }
            }
            else
            {
                throw new Exception($"Отсутствует файл {LOCALIZATION_FILE}");
            }
            return arrDialogFiles;
        }

        // Constructor
        public ViewModel()
        {
            Folder_game = "";
            SearchString = "";
            //SelectedString = new GameText();
            Strings = new ObservableCollection<GameText>();
            Dialogs = new ObservableCollection<GameDialog>();
            Infoportions = new ObservableCollection<Infoportion>();
            ReactorObjects = new ObservableCollection<object>();
        }
        public ViewModel(string Patch) : base()
        {
            if (Folder_game == "")
            {
                throw new Exception("Не задан путь к папке с игрой!");
            }
            else
            {
                Folder_game = "";
                Load(Patch);
            }
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
