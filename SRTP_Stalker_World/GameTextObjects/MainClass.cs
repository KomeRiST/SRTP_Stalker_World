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
    public abstract class MainClass : INotifyPropertyChanged
    {

        /// <summary>
        /// Уникальное имя объекта
        /// </summary>
        string _id;
        /// <summary>
        /// Файл в котором храниться инфа(обьект)
        /// </summary>
        MyFile _thisFile;

        /// <summary>
        /// Авторство
        /// </summary>
        public Podpis Podpis { get; set; }
        /// <summary>
        /// Файл в котором храниться инфа(обьект)
        /// </summary>
        public MyFile thisFile
        {
            get
            {
                return _thisFile;
            }
            set
            {
                _thisFile = value;
                OnPropertyChanged("thisFile");
            }
        }

        /// <summary>
        /// Уникальное имя объекта
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private MainClass(string id)
        {
            _id = Id;
        }
        public MainClass(string id, string FileName) : this(id)
        {
            if (FileName != "")
            {
                thisFile = new MyFile(FileName);
            }
            else
            {
                thisFile = new MyFile();
            }

        }
        public MainClass(XmlNode xNode, string FileName)
        {
            // Добавить обработку данных перед внесением в поля
            // https://metanit.com/sharp/tutorial/2.14.php - try .. catch .. finally
            // https://metanit.com/sharp/tutorial/2.28.php - конкретно про catch
            // https://metanit.com/sharp/tutorial/2.29.php - типы исключений
            // https://metanit.com/sharp/tutorial/3.17.php - свои классы исключений
            // https://metanit.com/sharp/tutorial/2.31.php - оператор throw
            _id = xNode.Attributes.GetNamedItem("id").Value;
            thisFile = new MyFile(FileName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Перезагрузка объекта из файла
        /// </summary>
        public abstract void Reload();

        /// <summary>
        /// Сохранение объекта
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Удаление объекта
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Редактирование объекта
        /// </summary>
        public abstract void Edit();
    }
}
