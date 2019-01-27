using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System;

namespace KET4.klases
{
    public class Masina : Grid
    {
        /// <summary>
        /// klases kintamieji
        /// </summary>
        #region Kintamieji
        public Image masina_pav = new Image();
        public Image posukis_pav = new Image();
        public bool r_mas;                                                      // skirtas zinoti ar rodomi remai ar masina
        public string s_mas;                                                    // masinos spalva
        public char p_mas;                                                      // masinos posukio kryptis ('k' - kaire, 't' - tiesiai, 'd' - desine)
        public bool mirkt;                                                      // skirtas atpazinti sveisu mirksejima (on/off)
        public List<int> ch_zenklai;                                            // parinkti zenklai siam masinos objektui
        public int ch_pgr_kelias = -1;                                          // pasirinktas pirmenybes zenklo id
        public int ID;                                                          // sio objekto masinos indeksas zemelapyje
        private int[] wh = { 0, 0 };                                            // masinos ilgis ir plotis
        #endregion

        /// <summary>
        /// klases konstruktorius
        /// </summary>
        public Masina(int id)
        {
            ID = id;
            Viska_atstatyti();
            RenderTransformOrigin = new Point(0.5, 0.5);
        }
        
        private void AtnaujintiVaikus()
        {
            try
            {
                Children.Clear();
                Children.Add(masina_pav);
                Children.Add(posukis_pav);
            }
            catch
            {
                Children.Add(masina_pav);
                Children.Add(posukis_pav);
            }
        }

        /// <summary>
        /// Public Objekto funkcijos (tik veikiancios)
        /// </summary>
        #region Funkcijos
                    // nustatomos objekto koordinates
        public void NustatytiXY(double x, double y)
        {
            // irasomas -1 jei nenori nustatyti tos koordinates
            if (x != -1)
                Canvas.SetLeft(this, x);
            if (y != -1)
                Canvas.SetTop(this, Paveikslas.canvasY(y));
        }
                      // grazinama pasirinkta objekto koordinate
        public double GautiXY(char kord)
        {
            if (kord == 'X' || kord == 'x')
                return Canvas.GetLeft(this);
            else
                return Paveikslas.normalY(Canvas.GetTop(this));
        }
                    // nustato masinos paveiksleli
        public void MasinaSource(string vieta)
        {
            Masinos_wh(Paveikslas.Masinos_kryptis(rdl.juostu_kiekis, ID));
            masina_pav.Source = Paveikslas.Gauti_Source(vieta, wh);
            AtnaujintiVaikus();
        }
                    // nustatomas objekto matomumas
        public void Matomumas(bool ar_matomas)
        {
            if (ar_matomas)
            {
                masina_pav.Visibility = Visibility.Visible;
                posukis_pav.Visibility = Visibility.Visible;
            }
            else
            {
                masina_pav.Visibility = Visibility.Hidden;
                posukis_pav.Visibility = Visibility.Hidden;
            }
            AtnaujintiVaikus();
        }
                    // nustatomas masinos paveikslelio dydis
        public void NustatytiWH(int[] _wh)
        {
            wh = _wh;
            masina_pav.Width = wh[0];
            masina_pav.Height = wh[1];
            posukis_pav.Width = wh[0];
            posukis_pav.Height = wh[1];
            AtnaujintiVaikus();
        }
                    // atnaujina duomenis i pradinius
        public void Viska_atstatyti()
        {
            /// <summary>
            /// nustatomos pagrindines reiksmes
            /// </summary>
            try
            {
                masina_pav = new Image();
                posukis_pav = new Image();
                ch_zenklai = new List<int>();
                r_mas = false;
                s_mas = "nenustatyta";
                p_mas = 't';
                mirkt = false;
                ch_pgr_kelias = -1;
            }
            catch { MessageBox.Show("Klaida sukurian mašinas! Kreipkitės į kūrėjus!"); }
            AtnaujintiVaikus();
        }
        #endregion

        /// <summary>
        /// Pagalbines klases funkcijos
        /// </summary>
        #region Extra Funkcijos
                       // skaitini krypties indeksa keicia string formatu
        private string Masinos_kryptis(int kryp)
        {
            switch (kryp)
            {
                case 0: return "bottom";
                case 1: return "left";
                case 2: return "top";
                case 3: return "right";
                default: return "top";
            }
        }
                     // pagal krypti grazina remo matmenis
        private void Masinos_wh(int kryp)
        {
            switch (kryp)
            {
                case 0:
                    wh = rdl.wh_masina;
                    break;
                case 1:
                    wh = rdl.wh_masina_hor;
                    break;
                case 2:
                    wh = rdl.wh_masina;
                    break;
                case 3:
                    wh = rdl.wh_masina_hor;
                    break;
                default:
                    wh = rdl.wh_masina;
                    break;
            }
        }

        protected void Isrinkti_taska(double[] graza, int ch)
        {
            if (rdl.kelias == '+')                   // jei kelias '+'
            {
                switch (rdl.juostu_kiekis)
                {
                    case 1:
                        graza[0] = rdl.k_plius_car1[ch, 0];
                        graza[1] = rdl.k_plius_car1[ch, 1];
                        break;
                    case 2:
                        graza[0] = rdl.k_plius_car2[ch, 0];
                        graza[1] = rdl.k_plius_car2[ch, 1];
                        break;
                    case 3:
                        graza[0] = rdl.k_plius_car3[ch, 0];
                        graza[1] = rdl.k_plius_car3[ch, 1];
                        break;
                    default:
                        graza[0] = 0;
                        graza[1] = 0;
                        break;
                }
            }
            else
            {                                       // jei kelias 'T'
                switch (rdl.juostu_kiekis)
                {
                    case 1:
                        graza[0] = rdl.k_T_car1[ch, 0];
                        graza[1] = rdl.k_T_car1[ch, 1];
                        break;
                    case 2:
                        graza[0] = rdl.k_T_car2[ch, 0];
                        graza[1] = rdl.k_T_car2[ch, 1];
                        break;
                    case 3:
                        graza[0] = rdl.k_T_car3[ch, 0];
                        graza[1] = rdl.k_T_car3[ch, 1];
                        break;
                    default:
                        graza[0] = 0;
                        graza[1] = 0;
                        break;
                }
            }
        }
                    // masina rodo posuki i nurodyta puse
        public void NustatytiPosukioRema(ListBox lb)
        {
            double[] _taskas = { 0, 0 };
            int masi_index = Sarasas.GautiIndeksa(lb, lb.SelectedIndex);            // gaunamas pasirinktos masinos indeksas     
            int kryptis = Paveikslas.Masinos_kryptis(rdl.juostu_kiekis, masi_index);
            string sukti_i;
            Masinos_wh(Paveikslas.Masinos_kryptis(rdl.juostu_kiekis, masi_index));  // gaunamas 'wh'
            Isrinkti_taska(_taskas, masi_index);
            if (p_mas == 'k')
                sukti_i = "left";
            else
                sukti_i = "right";
            posukis_pav.Source = (Paveikslas.Gauti_Source("teksturos/masinos/1posukiai/" + sukti_i + "/" + Masinos_kryptis(kryptis) + ".png", wh));
        }
        #endregion
    }
}