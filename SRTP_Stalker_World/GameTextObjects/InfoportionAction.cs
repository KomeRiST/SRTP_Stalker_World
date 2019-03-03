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
    //
    //Набор свойств\условий для отображения Диалога или Фразы
    //
    public class InfoportionAction : INotifyPropertyChanged
    {

        public ObservableCollection<Infoportion> Has { get => _Has; set => _Has = value; }
        public ObservableCollection<Infoportion> DontHas { get => _DontHas; set => _DontHas = value; }
        public ObservableCollection<Infoportion> Give { get => _Give; set => _Give = value; }
        public ObservableCollection<Infoportion> Disable { get => _Disable; set => _Disable = value; }
        public ObservableCollection<string> Precondition { get => _Precondition; set => _Precondition = value; }
        public ObservableCollection<string> Action { get => _Action; set => _Action = value; }

        private ObservableCollection<Infoportion> _Has;
        private ObservableCollection<Infoportion> _DontHas;
        private ObservableCollection<Infoportion> _Give;
        private ObservableCollection<Infoportion> _Disable;

        private ObservableCollection<string> _Precondition;
        private ObservableCollection<string> _Action;

        public InfoportionAction()
        {
            this.Has = new ObservableCollection<Infoportion>();
            this.DontHas = new ObservableCollection<Infoportion>();
            this.Give = new ObservableCollection<Infoportion>();
            this.Disable = new ObservableCollection<Infoportion>();

            this.Precondition = new ObservableCollection<string>();
            this.Action = new ObservableCollection<string>();
        }

        void ClearFull()
        {
            this.Has.Clear();
            this.DontHas.Clear();
            this.Give.Clear();
            this.Disable.Clear();

            this.Precondition.Clear();
            this.Action.Clear();
        }

        public void Load(XmlNode xmlnode)
        {
            XmlNodeList has_info = xmlnode.SelectNodes("has_info");
            XmlNodeList dont_has_info = xmlnode.SelectNodes("dont_has_info");
            XmlNodeList give_info = xmlnode.SelectNodes("give_info");
            XmlNodeList disable_info = xmlnode.SelectNodes("disable_info");

            XmlNodeList action = xmlnode.SelectNodes("action");
            XmlNodeList precondition = xmlnode.SelectNodes("precondition");

            foreach (XmlNode item in has_info)
            {
                //Infoportion info;
                //info = MainWindow.GAME.GetInfoportionById(item.InnerText);
                Has.Add(MainWindow.VM.Game.GetInfoportionById(item.InnerText));
            }
            foreach (XmlNode item in dont_has_info)
            {
                //Infoportion info;
                //info = MainWindow.GAME.GetInfoportionById(item.InnerText);
                DontHas.Add(MainWindow.VM.Game.GetInfoportionById(item.InnerText));
            }
            foreach (XmlNode item in give_info)
            {
                //Infoportion info;
                //info = MainWindow.GAME.GetInfoportionById(item.InnerText);
                Give.Add(MainWindow.VM.Game.GetInfoportionById(item.InnerText));
            }
            foreach (XmlNode item in disable_info)
            {
                //Infoportion info;
                //info = MainWindow.GAME.GetInfoportionById(item.InnerText);
                Disable.Add(MainWindow.VM.Game.GetInfoportionById(item.InnerText));
            }

            foreach (XmlNode item in action)
            {
                Action.Add(item.InnerText);
            }
            foreach (XmlNode item in precondition)
            {
                Precondition.Add(item.InnerText);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
