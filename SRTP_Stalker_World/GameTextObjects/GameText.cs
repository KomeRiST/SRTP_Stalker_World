using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SRTP_Stalker_World.GameTextObjects
{
    /// <summary>
    /// Класс для хранения строк локализации из игры
    /// </summary>
    public class GameText : MainClass
    {
        /// <summary>
        /// Русский текст
        /// </summary>
        public string Text
        {
            get
            {
                if (_Text == "")
                {
                    return _Text;
                }
                else
                {
                    string buff;
                    buff = _Text.Replace("\n", Environment.NewLine);
                    return buff;
                }

            }
            set
            {
                if (value.IndexOf("\n") > 0)
                {
                    _Text = value.Replace(Environment.NewLine, "\n");
                }
                else
                {
                    _Text = value;
                }

                OnPropertyChanged("Text");
            }
        }
        private string _Text;


        public GameText() : base("", "")
        {
            Text = "";
        }
        // Constructor
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="xString">Узел XML документа</param>
        /// <param name="FileName">Полный путь к файлу</param>
        public GameText(XmlNode xString, string FileName) : base(xString, FileName)
        {
            //<string id="brodyaga_22">
            //    <text>Волк главный.Вон он, в сталкерском костюме.</text>
            //</string>
            if (xString.Attributes != null)
            {
                Text = xString.SelectSingleNode("text").InnerText;
            }
            else
            {
                //Вывод в консоль
            }
        }

        public override string ToString()
        {
            return $"{Id}: {Text}";
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }

}
