using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SRTP_Stalker_World.GameTextObjects
{
    class INI_Manager
    {
        public ObservableCollection<GameIni> Inis;

        public INI_Manager(string path, string file)
        { // INI_Manager im = new INI_Manager(@"C:\SoC\gamedata\config\system.ltx");
            Inis = new ObservableCollection<GameIni>();
            LoadFiles(path, file);
            Console.WriteLine(Inis);
        }

        private void LoadFiles(string path, string file)
        {
            string[] lines = File.ReadAllLines(path + file); // Читаем файл построчно
            string include = "";
            string section = "";
            string key = "";
            string value = "";
            string[] res = null;
            GameIni buff = null;
            Regex rg_property = new Regex(@"(.+)[=]{1}(.+)[;]{1}(.+)");  // создаём паттерн "файл.тип"
            Regex rg_section = new Regex(@"\[(.+)\]");  // создаём паттерн "файл.тип"
            Regex rg_include = new Regex(@"#include(\s*)""(.+)""");  // создаём паттерн "файл.тип"
            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }
                //Console.WriteLine(lines[i]);
                res = lines[i].Trim().Split(';');
                if (res[0].Trim() == "")
                {
                    continue;
                }
                res = res[0].Trim().Split('=');
                if (res.Length > 1)
                {
                    key = res[0].Trim(); // получаем KEY
                    value = res[1].Trim(); // получаем VALUE
                    if (buff.Properties.ContainsKey(key))
                    {
                        //Console.WriteLine($"Попытка повторного ввода ключа: {key} = {value}");
                        continue;
                    }
                    buff.Properties.Add(key, value);
                    continue;
                }
                section = rg_section.Match(lines[i]).Groups[1].Value; // получаем SECTION
                if (!string.IsNullOrEmpty(section))
                {
                    if (buff != null)
                    {
                        Inis.Add(buff);
                    }
                    // Название секции
                    //Console.WriteLine(section);
                    GameIni gi = new GameIni
                    {
                        Id = section,
                        Parent = new ObservableCollection<GameIni>(),
                        Podpis = new Podpis(),
                        Properties = new Dictionary<string, string>(),
                        thisFile = new MyFile(path+file)
                    };
                    buff = gi;
                    continue;
                }
                include = rg_include.Match(lines[i]).Groups[2].Value; // получаем значение #include
                if (!string.IsNullOrEmpty(include))
                {
                    // Путь к подгружаемым файлам (то, что в #include)
                    //Console.WriteLine(include);
                    string pth = Path.GetDirectoryName(path + file) + "\\";
                    LoadFiles(pth, include);
                }
            }
        }
    }
}
