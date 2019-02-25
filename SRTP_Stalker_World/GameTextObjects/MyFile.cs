using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SRTP_Stalker_World.GameTextObjects
{
    public class MyFile : INotifyPropertyChanged
    {
        /// <summary>
        /// ПОлный путь к файлу
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
                OnPropertyChanged("FileName");
            }
        }
        private string _FileName;
        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime FileDate
        {
            get
            {
                return _FileDate;
            }
            set
            {
                _FileDate = value;
                OnPropertyChanged("FileDate");
            }
        }
        private DateTime _FileDate;
        public MyFile()
        {
            FileName = "";
            FileDate = new DateTime();
        }
        public MyFile(string PatchFile)
        {
            FileName = PatchFile;
            FileDate = File.GetLastWriteTime(PatchFile);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
