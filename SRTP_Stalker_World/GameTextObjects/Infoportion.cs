using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

// формат поршня в файле

//  <infoportion id = "" >
//    < location >
//      < level />
//      < object />
//      < icon name="" x="" y="" width="" height="" marker="" r="" g="" b="" />
//      <text />
//      <x />
//      <y />
//    </location>
//    <disable />
//    <task />
//    <dialog />
//    <text />
//    <article />
//    <action />
//  </infoportion>


namespace SRTP_Stalker_World.GameTextObjects
{
    public class Infoportion : MainClass
    {
        public string Text { get => _text; set => _text = value; }
        public ObservableCollection<Infoportion> Disable { get => _disable; set => _disable = value; }
        public ObservableCollection<GameTask> Task { get => _task; set => _task = value; }
        public ObservableCollection<GameDialog> Dialog { get => _dialog; set => _dialog = value; }
        public ObservableCollection<GameArticle> Article { get => _article; set => _article = value; }
        public ObservableCollection<string> Action { get => _action; set => _action = value; }
        
        string _text;
        ObservableCollection<Infoportion> _disable;
        ObservableCollection<GameTask> _task;
        ObservableCollection<GameDialog> _dialog;
        ObservableCollection<GameArticle> _article;
        ObservableCollection<string> _action;
        
        public Infoportion(XmlNode xVal, string FileName) : base(xVal, FileName)
        {
            Disable = new ObservableCollection<Infoportion>();
            Task = new ObservableCollection<GameTask>();
            Dialog = new ObservableCollection<GameDialog>();
            Article = new ObservableCollection<GameArticle>();
            Action = new ObservableCollection<string>();
            _text = "";
            // Где обработка исключений???
            XmlNode xv = xVal.SelectSingleNode("text");
            if (xv != null)
            {
                _text = xv.InnerText;
            }
        }

        public override string ToString()
        {
            return $"{Id}";
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

        public override void Edit()
        {
            throw new NotImplementedException();
        }
    }
}
