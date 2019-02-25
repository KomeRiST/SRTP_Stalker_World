using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/*
 * Этот класс создавался для отображения 
 * древовидной инфы в виде:
 *  - Файл
 *  |- Обьект
 *   - Обьект
 */
namespace SRTP_Stalker_World.GameTextObjects
{
    class VMTreeObjs : INotifyPropertyChanged
    {
        private string _FileName;
        public string FileName
        {
            get => _FileName;
            set
            {
                _FileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public IEnumerable<Object> EnumObjcs { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
