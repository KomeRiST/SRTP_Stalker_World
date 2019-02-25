using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTP_Stalker_World.GameTextObjects
{
    /// <summary>
    /// Класс для чтения/записи INI-файлов
    /// </summary>
    public class INIManager
    {
        /// <summary>
        /// Конструктор, принимающий путь к INI-файлу
        /// </summary>
        /// <param name="aPath">Абсолютный путь к INI-файлу</param>
        public INIManager(string aPath)
        {
            path = aPath;
        }

        //Конструктор без аргументов (путь к INI-файлу нужно будет задать отдельно)
        public INIManager() : this("") { }
        
        /// <summary>
        /// Возвращает значение из INI-файла (по указанным секции и ключу) 
        /// </summary>
        /// <param name="aSection"> Название секции</param>
        /// <param name="aKey">Название ключа</param>
        /// <returns></returns>
        public string GetPrivateString(string aSection, string aKey)
        {
            //Для получения значения
            StringBuilder buffer = new StringBuilder(SIZE);

            //Получить значение в buffer
            GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

            //Вернуть полученное значение
            return buffer.ToString();
        }
        
        /// <summary>
        /// Пишет значение в INI-файл (по указанным секции и ключу) 
        /// </summary>
        /// <param name="aSection">Имя секции</param>
        /// <param name="aKey">Ключ</param>
        /// <param name="aValue">Значение</param>
        public void WritePrivateString(string aSection, string aKey, string aValue)
        {
            //Записать значение в INI-файл
            WritePrivateString(aSection, aKey, aValue, path);
        }
        
        /// <summary>
        /// Возвращает или устанавливает путь к INI файлу
        /// </summary>
        public string Path { get { return path; } set { path = value; } }

        //Поля класса
        private const int SIZE = 1024; //Максимальный размер (для чтения значения из файла)
        private string path = null; //Для хранения пути к INI-файлу
        
        /// <summary>
        /// Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);
        
        /// <summary>
        /// Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="str"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);
    }
}
