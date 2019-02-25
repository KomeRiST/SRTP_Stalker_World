using SRTP_Stalker_World.GameTextObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRTP_Stalker_World
{
    /// <summary>
    /// Логика взаимодействия для PhraseControl.xaml
    /// </summary>
    [ContentProperty("Phrase")]
    public partial class PhraseControl : UserControl
    {
        public DlgFrase Phrase
        {
            get
            {
                return (DlgFrase)this.GetValue(StateProperty);
                //return _phrase;
            }
            set
            {
                this.SetValue(StateProperty, value);
                //_phrase = value;
                lblIdFrase.Content = value.Id;
                lblTextRus.Text = value.Text.Text;
            }
        }

        public PhraseControl()
        {
            InitializeComponent();
        }

        //private DlgFrase _phrase;

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("Phrase", typeof(DlgFrase), typeof(PhraseControl));

    }
}
