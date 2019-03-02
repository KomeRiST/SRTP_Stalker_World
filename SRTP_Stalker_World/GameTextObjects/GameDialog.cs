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
        /// <summary>
        /// Массив сказанных фраз
        /// Для редактора диалога
        /// </summary>
        public ObservableCollection<DlgFrase> Frase_voice { get; set; }
        /// <summary>
        /// Нулевая/стартовая фраза
        /// </summary>
        public DlgFrase Frases_0 { get; set; }

        /// <summary>
        /// Поиск фразы по его ИД
        /// </summary>
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
        /// <summary>
        /// Преверка фразы на на то,
        /// что она ссылается на саму себя в Next'ах своих Children.
        /// </summary>
        /// <param name="fr">Проверяемая фраза</param>
        /// <param name="fStep">Потомок проверяемой фразы</param>
        /// <returns>Наличие текущей фразы в её же потомках</returns>
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
        /// <summary>
        /// Создаёт иерархию фраз по Next'ам
        /// начиная с нулевой фразы.
        /// </summary>
        /// <param name="idfrase">ИД фразы от которой строить иерархию</param>
        /// <returns>Ссылку на фразу</returns>
        private DlgFrase NormalNext(string idfrase = "0")
        {
            // Ищем очередной некст фразу
            DlgFrase fr = GetFraseById(idfrase);
            if ((fr == null) || fr.Cycle)
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

        /// <summary>
        /// Читабельное представление объекта
        /// </summary>
        public override string ToString()
        {
            return Id;
        }

        /// <summary>
        /// Перезагрузка объекта из файла
        /// </summary>
        public override void Reload()
        {
            // Открыть файл
            // Найти объект по ИД
            // Создать новый диалог из XmlNode
            // Перезаписать поля текущего объекта из нового
            // Удалить новый объект.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохранение данных в файле
        /// </summary>
        public override void Save()
        {
            // 1. Открыть файл (f)
            // 2. Найти элемент по ИД (e = f.SelectSingleNode(ИД))
            // 3. Сформировать XmlNode (n = new XmlNode())
            // 4. e.OutText = n.Text
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаление объекта из файла
        /// </summary>
        public override void Delete()
        {
            // Либо пометить текущий объект на удаление и при сохранении изменений в проексте удалять
            // либо удалять сразу из файла
            // Но видимо метод удаления объектов придётся переместить во ViewModel,
            //      т.к. удаляемый объект может быть зависимым или иметь связи с другими объетами.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сброс сказанных фраз в редакторе диалога 
        /// кроме нулевой
        /// </summary>
        public void ClearFraseVoice()
        {
            Frase_voice.Clear();
            Frase_voice.Add(Frases_0);
        }
    }
}
