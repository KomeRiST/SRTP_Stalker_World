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
        /// <summary>
        /// Уникальный ID фразы
        /// Обычно это число
        /// </summary>
        public string Id { get => _id; set => _id = value; }
        public GameDialog ParentDialog { get => _parentDialog; set => _parentDialog = value; }
        public DlgFrase ParenFrase { get => _parenFrase; set => _parenFrase = value; }
        public bool Cycle { get => _cycle; set => _cycle = value; }
        /// <summary>
        /// Условия показа фразы
        /// </summary>
        public InfoportionAction Properties { get => _properties; set => _properties = value; }
        /// <summary>
        /// ID текста локализации
        /// </summary>
        public GameText Text { get => _text; set => _text = value; }
        /// <summary>
        /// Массив вариантов ответов(следующих фраз)
        /// </summary>
        public ObservableCollection<DlgFrase> Next { get => _next; set => _next = value; }
        public List<string> NextText { get => _nextText; set => _nextText = value; }
        private string _id;
        private GameDialog _parentDialog;
        private DlgFrase _parenFrase;
        private bool _cycle;
        private InfoportionAction _properties;
        private GameText _text;
        private ObservableCollection<DlgFrase> _next;
        private List<string> _nextText;

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
