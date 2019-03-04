using SRTP_Stalker_World.GameTextObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SRTP_Stalker_World
{
    public class GameIni : MainClass
    {
        public GameIni() { }

        public GameIni(string _id)
        {
            Id = _id;
            Properties = new Dictionary<string, string>();
            Parent = new ObservableCollection<GameIni>();
        }
        /// <summary>
        /// Список от которых наследуется данный объект
        /// </summary>
        public ObservableCollection<GameIni> Parent { get; set; }

        /// <summary>
        /// Список свойств объекта
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Id;
        }
    }
}