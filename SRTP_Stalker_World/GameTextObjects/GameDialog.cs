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
    /// Описание диалога
    /// </summary>
    public class GameDialog : MainClass
    {
        /// <summary>
        /// Приоритет диалога
        /// </summary>
        public string Prioritet { get; set; }
        /// <summary>
        /// Условия запуска диалога
        /// </summary>
        public InfoportionAction Properties { get; set; }
        /// <summary>
        /// Массив фраз диалога
        /// </summary>
        public ObservableCollection<DlgFrase> Frases { get; set; }
        public ObservableCollection<DlgFrase> Frase_voice { get; set; }
        public DlgFrase Frases_0 { get; set; }

        public DlgFrase GetFraseById(string fId)
        {
            foreach (DlgFrase item in Frases)
            {
                if (item.Id == fId)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Конструктор дилога
        /// </summary>
        /// <param name="xNode">Корень элемента dialog с аттрибутом id</param>
        /// <param name="FileName">Полный путь к файлу</param>
        public GameDialog(XmlNode xNode, string FileName) : base(xNode, FileName)
        {
            Podpis = new Podpis();
            Properties = new InfoportionAction();
            Properties.Load(xNode);
            Frases = new ObservableCollection<DlgFrase>();
            Frase_voice = new ObservableCollection<DlgFrase>();
            foreach (XmlNode xItem in xNode.ChildNodes)
            {
                // Если попали на список фраз
                if (xItem.Name == "phrase_list")
                {
                    // Проходим по всем фразам диалога
                    foreach (XmlNode xFrase in xItem.ChildNodes)
                    {
                        //if (xFrase.Attributes["id"].Value == "0")
                        //{
                        //    Frases.Add(new DlgFrase(xFrase, this));
                        //    LoadNext(xFrase);
                        //}
                        // Извлекаем узел и передаём в конструктор фразы
                        DlgFrase Frase = new DlgFrase(xFrase, this);
                        //string idfrase = xFrase.Attributes.GetNamedItem("id").Value;
                        Frases.Add(Frase);
                    }
                }
                Frases_0 = GetFraseById("0");
                NormalNext("0");
                Frase_voice.Add(Frases_0);
            }
        }
        private bool IsCycleFrase(DlgFrase fr, DlgFrase fStep)
        {
            if (fr == fStep)
            {
                return true;
            }
            else
            {
                foreach (DlgFrase item in fr.Next)
                {
                    return IsCycleFrase(fr, item);
                }
            }
            return false;
        }
        private DlgFrase NormalNext(string idfrase = "0")
        {
            // Ищем очередной некст фразу
            DlgFrase fr = GetFraseById(idfrase);
            if ((fr == null) || (fr.Cycle))
            {
                return null;
            }
            fr.Cycle = true;
            // Цикл по некстам найденной фразы
            foreach (string idnext in fr.NextText)
            {
                // рекурсивный вызов по поиску фразы по его ИД
                DlgFrase fn = NormalNext(idnext);
                if (fn != null)
                {
                    fr.Next.Add(fn);
                    fn.ParenFrase = fr;
                }
            }
            return fr;
        }

        public override string ToString()
        {
            return Id;
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
