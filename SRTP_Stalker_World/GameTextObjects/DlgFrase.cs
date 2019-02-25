using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SRTP_Stalker_World.GameTextObjects
{
    /// <summary>
    /// Фраза диалога
    /// </summary>
    public class DlgFrase : INotifyPropertyChanged
    {
        public GameDialog ParentDialog { get; set; }
        public DlgFrase ParenFrase { get; set; }
        public bool Cycle { get; set; }
        /// <summary>
        /// Уникальный ID фразы
        /// Обычно это число
        /// </summary>
        public string Id { get => _id; set => _id = value; }
        private string _id;
        /// <summary>
        /// Условия показа фразы
        /// </summary>
        public InfoportionAction Properties { get; set; }
        /// <summary>
        /// ID текста локализации
        /// </summary>
        public GameText Text { get; set; }
        //private string IdText;
        /// <summary>
        /// Массив вариантов ответов(следующих фраз)
        /// </summary>
        public ObservableCollection<DlgFrase> Next { get; set; }
        public List<string> NextText { get; set; }

        public DlgFrase(XmlNode xNode, GameDialog Parent)
        {
            ParentDialog = Parent;
            if ((xNode.NodeType != XmlNodeType.Comment) || (xNode.Attributes != null))
            {
                Id = xNode.Attributes.GetNamedItem("id").Value;
                Properties = new InfoportionAction();
                Properties.Load(xNode);
                XmlNode xN = xNode.SelectSingleNode("text");
                string t = "";
                if (xN != null)
                {
                    t = xN.InnerText;
                }
                Text = MainWindow.GAME.GetGameStringByID(t);
                Next = new ObservableCollection<DlgFrase>();
                NextText = new List<string>();
                XmlNodeList xNext = xNode.SelectNodes("next");
                foreach (XmlNode item in xNext)
                {
                    NextText.Add(item.InnerText);
                }
            }
        }

        public override string ToString()
        {

            return $"{Text.Text}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
