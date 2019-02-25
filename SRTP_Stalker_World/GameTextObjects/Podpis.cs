using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTP_Stalker_World.GameTextObjects
{
    /// <summary>
    /// Хранит инфу об авторстве объекта
    /// </summary>
    public class Podpis
    {
        /// <summary>
        /// Ник создателя/редактора
        /// </summary>
        public string Autor;
        /// <summary>
        /// Описание объекта
        /// </summary>
        public string Description;
        /// <summary>
        /// Дата создания/изменения
        /// </summary>
        public string CreateFileDate
        {
            set
            {
                this.Data = DateTime.Parse(this.CreateFileDate);
            }
            get
            {
                return this.Data.ToString();
            }
        }
        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime LastChangeInfo;
        /// <summary>
        /// Показывает, есть ли не сохранённые изменения
        /// </summary>
        public bool IsChange = false;

        private DateTime Data;

        public Podpis() { Autor = "Default"; Description = "None"; CreateFileDate = DateTime.Now.ToString(); }
    }
}
