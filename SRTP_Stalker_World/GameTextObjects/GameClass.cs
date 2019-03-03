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
using System.Xml;

namespace SRTP_Stalker_World
{
    public class GameClass : INotifyPropertyChanged
    {
        // Константы
        public const string GAMEDATA = @"gamedata\";
        public const string CONFIG = @"gamedata\config\";
        public const string SYSTEM_FILE = @"gamedata\config\system.ltx";
        public const string LOCALIZATION_FILE = @"gamedata\config\localization.ltx";
        public const string GAMEPLAY = @"gamedata\config\gameplay\";
        public const string TEXT_RUS = @"gamedata\config\text\rus\";

        /// <summary>
        /// Путь к папке с игрой
        /// </summary>
        public string Folder_game { get; set; }
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
        /// <summary>
        /// Все квесты игры
        /// </summary>
        public ObservableCollection<GameTask> Tasks { get; set; }
        /// <summary>
        /// Все секции INI-файлов
        /// </summary>
        public ObservableCollection<GameIni> Inis { get; set; }

        // Поиск элементов в масивах
        /// <summary>
        /// Поиск ini секции
        /// </summary>
        /// <param name="vId">ID секции</param>
        /// <returns>Объект GameIni</returns>
        public GameIni GetIniById(string vId)
        {
            foreach (GameIni item in Inis)
            {
                if (item.Id == vId)
                {
                    return item;
                }
            }
            return null;
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
        /// Загрузка StringTable
        /// </summary>
        private void LoadStringTable()
        {
            // Считываем файл локализации
            ArrayList arrSTFiles = GetStringTableFiles();
            XmlDocument xDoc = new XmlDocument();
            foreach (string patch in arrSTFiles)
            {
                // Проверить каждый файл на существование
                if (File.Exists(Folder_game + TEXT_RUS + patch))
                {
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
        }
        /// <summary>
        /// Загрузка поршней
        /// </summary>
        private void LoadInfoport()
        {
            // Получили список файлов с поршнями
            ArrayList arrInfoFiles = GetInfoportionsFiles();
            foreach (string patch in arrInfoFiles)
            {
                // Проверить каждый файл на существование
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
                        Console.WriteLine($"Ошибка '{ex.Message}' в файле: '{patch}'");
                        continue;
                    }
                    finally
                    {
                        XmlNode xRoot = xDoc.DocumentElement;
                        if (xRoot != null)
                        {
                            // Ищем XML info_portion
                            XmlNodeList xInfos = xRoot.SelectNodes("info_portion");
                            // Передаём в конструктор поршня строку с XML
                            foreach (XmlNode n in xInfos)
                            {
                                Infoportions.Add(new Infoportion(n, Folder_game + GAMEPLAY + patch));
                            }
                        }
                    }
                }
                else
                {
                    //Вывод в лог
                }
            }
        }
        /// <summary>
        /// Загрузка диалогов
        /// </summary>
        private void LoadDialog()
        {
            // Получили список файлов с диалогами
            ArrayList arrDialogFiles = GetDialogFiles();
            XmlDocument xDoc = new XmlDocument();
            foreach (string patch in arrDialogFiles)
            {
                // Проверить каждый файл на существование
                if (File.Exists(Folder_game + GAMEPLAY + patch))
                {
                    // Открыть файл
                    xDoc.Load(Folder_game + GAMEPLAY + patch);
                    XmlNode xRoot = xDoc.DocumentElement;
                    // Ищем XML диалога
                    XmlNodeList xDialogs = xRoot.SelectNodes("dialog");
                    // Передаём в конструктор диалога строку с XML
                    foreach (XmlNode n in xDialogs)
                    {
                        Dialogs.Add(new GameDialog(n, Folder_game + GAMEPLAY + patch));
                    }
                }
                else
                {
                    //Вывод в лог
                }
            }
        }
        /// <summary>
        /// Чтение и загрузка ресурсов игры
        /// </summary>
        /// <param name="folder_game">Путь к папке с игрой</param>
        public void Load(string folder_game)
        {
            // Проверяем существование папки gamedata\
            if (Directory.Exists(folder_game + GAMEDATA))
            {
                Folder_game = folder_game;
                INI_Manager im = new INI_Manager(folder_game + CONFIG, "system.ltx");
                Inis = im.Inis;
                LoadStringTable();
                LoadInfoport();
                LoadDialog();
            }
            else
            {
                throw new Exception($"В папке с игрой не найдена папка '{GAMEDATA}' \n распакуйте *.db архивы и перезапустите программу.");
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
        public ArrayList GetIniFiles()
        {
            ArrayList arrIniFiles = new ArrayList();
            HashSet<string> uniqstr = new HashSet<string>();
            foreach (GameIni item in Inis)
            {
                uniqstr.Add(Path.GetFileName(item.thisFile.FileName));
            }
            foreach (string item in uniqstr)
            {
                arrIniFiles.Add(item);
            }
            return arrIniFiles;
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

            //if (File.Exists(Folder_game + LOCALIZATION_FILE))
            //{
            //Создание объекта, для работы с файлом
            //INIManager IniManager = new INIManager(Folder_game + LOCALIZATION_FILE);

            //Получить значение
            //    string files = IniManager.GetPrivateString("string_table", "files");
            GameIni i = GetIniById("string_table");
            string files = i.Properties["files"];
            string[] buff = files.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in buff)
            {
                arrDialogFiles.Add(str.Trim() + ".xml");
            }
            //}
            //else
            //{
            //    throw new Exception($"Отсутствует файл {LOCALIZATION_FILE}");
            //}
            return arrDialogFiles;
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public GameClass()
        {
            Folder_game = "";
            Strings = new ObservableCollection<GameText>();
            Dialogs = new ObservableCollection<GameDialog>();
            Infoportions = new ObservableCollection<Infoportion>();
            Tasks = new ObservableCollection<GameTask>();
        }
    }
}