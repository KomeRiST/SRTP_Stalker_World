using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTP_Stalker_World
{
    /// <summary>
    /// Класс отфильтрованных обьектов
    /// </summary>
    class SearchResult
    {
        /// <summary>
        /// Название хранимых обьектов
        /// (для фильтрации/выбора шаблонов в XAML разметке)
        /// </summary>
        public string NameObject { get; set; }
        /// <summary>
        /// Количество элементов в Objs
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Массив обьектов
        /// </summary>
        public List<Object> Objs { get; set; }

        public SearchResult()
        {
            Objs = new List<object>();
        }

    }
}
