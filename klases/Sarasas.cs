using System.Windows.Controls;
using System.ComponentModel;

namespace KET4.klases
{
    public class Sarasas : INotifyPropertyChanged
    {
        public static bool ArPasirinkta(ListBox lb)
        {
            if (lb.SelectedItem != null)
                return true;
            else
                return false;
        }
                             // Angliska spalvos pavadinima pakeicia lietuvisku
        public static string IsverstiSpalva(string spal)
        {
            string isversta = "nerasta";
            switch (spal)
            {
                case "black":
                    isversta = "Juoda";
                    break;
                case "blue":
                    isversta = "Mėlina";
                    break;
                case "orange":
                    isversta = "Orandžinė";
                    break;
                case "purple":
                    isversta = "Purpurinė";
                    break;
                case "red":
                    isversta = "Raudona";
                    break;
                case "yellow":
                    isversta = "Geltona";
                    break;
                default:
                    isversta = "nerasta";
                    break;
            }
            return isversta;
        }
                          // Grazina masinos zemelapio indeksa naudojant ListBox indeksa
        public static int GautiIndeksa(ListBox lb, int id)
        {
            try
            {
                return (lb.Items[id] as Sarasas).masinos_id;
            }
            catch { return -1; }
        }

        public int masinos_id = -1;                                             // saugo masinos indeksa zemelapyje

        public string spalva;                                                   // angliskas spalvos pavadinimas

        private string antraste;
        public string Antraste
        {
            get { return antraste; }
            set
            {
                if (antraste != value)
                {
                    antraste = value;
                    Pasikeitimas("Antraste");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;               // nustato ListBox pasikeitima

        public void Pasikeitimas(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
