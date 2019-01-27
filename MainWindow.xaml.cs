using System;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using KET4.klases;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace KET4
{
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// kitos deklaracijos:
        /// </summary>
        private ObservableCollection<Sarasas> sara = new ObservableCollection<Sarasas>();
        private ObservableCollection<Sarasas> sara2 = new ObservableCollection<Sarasas>();
        private ObservableCollection<Zenklas> zenkl = new ObservableCollection<Zenklas>();
        private ObservableCollection<Zenklas2> zenkl2 = new ObservableCollection<Zenklas2>();
        private static Masina[] _masina = new Masina[rdl.CreatedImg];

        private int ch_pav_id = -1;                                     // paveiksliuko ant kurio laikome pele pavadinimas
        private int ch_masina = -1;                                     // pasirinkta masina zenklams
        
        /// <summary>
        /// Visa MainWindow.xaml lango logika
        /// </summary>
        #region Begining
        public MainWindow()
        {
            InitializeComponent();                                              // uzkraunamas xaml kodas

            DataContext = new                                                   // sukuriamas duomenu saltinis
            {
                Zzenklai = zenkl,
                Pzenklai = zenkl2,
                Mmasinos = _masina,
                Ssarasas = sara,
                Asarasas = sara2
            };

            temptiLabel.Visibility = Visibility.Hidden;                         // paslepiama etikete kol nieko nepasirinks
            rdl.langasHeight = Height;
            tema_combo.SelectedIndex = 0;                                       // nustatomas pasirinktas 1 daiktas sarasas
            list_of_cars.ItemsSource = sara;                                    // pribindinamas sarasas

            for (int i = 0; i < rdl.CreatedImg; i++)
                _masina[i] = new Masina(i);                                     // sukurimas nauji masinu objektai

            DispatcherTimer _taimeris = new DispatcherTimer();                  // laikrodzio funkcijos sukurimas
            _taimeris.Tick += Laikrodis;                                        // priskiriama funkcija kuria laikmatis vykdys
            _taimeris.Interval = new TimeSpan(0,0,1);                           // nustatomas laikmacio intervalas
            _taimeris.Start();

            Zenklai_init();                                                     // uzkraunami zenklai
        }
        #endregion

        /* ***************************************************************** */
        /* ***** ***** ***** MainWindow.xaml lango ivykiai ***** ***** ***** */
        /* ***************************************************************** */

        /// <summary>
        /// Nustatymu lango ivykiai ir informacijos langas
        /// </summary>
        #region Options bar
                     // ijungiamas nustatymu langas
        private void temos_click(object sender, RoutedEventArgs e)
        {
            temos_window.IsOpen = true;
        }
                     // isjungimas nustatymu langas
        private void uzdaryti_nustatymus(object sender, RoutedEventArgs e)
        {
            temos_window.IsOpen = false;
        }
                     // pasikeitus comboBox keisti tema
        private void tema_pasikeite(object sender, SelectionChangedEventArgs e)
        {
            int pasirinkta = tema_combo.SelectedIndex;
            switch(pasirinkta)
            {
                case 0:
                    Nustatyti_tema(palete.numatyta);
                    break;
                case 1:
                    Nustatyti_tema(palete.beach);
                    break;
                case 2:
                    Nustatyti_tema(palete.pastel);
                    break;
                case 3:
                    Nustatyti_tema(palete.blue);
                    break;
                case 4:
                    Nustatyti_tema(palete.muted);
                    break;
                case 5:
                    Nustatyti_tema(palete.candy);
                    break;
            }
        }
                     // nustato tam tikras lango spalvas
        private void Nustatyti_tema(string[] _spalvos)
        {
            pagrindinis.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_spalvos[0]));
            pasirinkimai_panel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_spalvos[1]));
            atsakymas_panel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_spalvos[2]));
            atsakymas_box.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_spalvos[3]));
            label1.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(_spalvos[4]));

            foreach (Zenklas znk in zenkl)
            {
                znk.Keisti_pavadinimo_spalva(_spalvos[0]);
            }
        }
        // paleidziamas pakeitus greiti nustatymuose
        private void greitis_changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rdl.greitis = greitis_slider.Value;
        }
                     // atstato lango dydi
        private void atstatyti_dydi_click(object sender, RoutedEventArgs e)
        {
            Width = 1456;
            Height = 1000;
        }
                     // uzdaromas info langas
        private void uzdaryti_info(object sender, RoutedEventArgs e)
        {
            info_window.IsOpen = false;
        }
                     // atidaro informacijos langa
        private void info_click(object sender, RoutedEventArgs e)
        {
            info_window.IsOpen = true;
        }
        #endregion

        /// <summary>
        /// Ivykiai pasirinkimui pasikeitus
        /// </summary>
        #region Selection events
                     // paveikslelis paruosiamas tempimui
        private void Tempimo_paruosimas()
        {
            for (int i = 0; i < rdl.CreatedImg; i++)
            {
                Tempimo_paruosimas(i);
            }
        }
        private void Tempimo_paruosimas(int id)
        {
            _masina[id].masina_pav.IsEnabled = true;
            _masina[id].masina_pav.AllowDrop = true;
            _masina[id].posukis_pav.AllowDrop = true;
            _masina[id].masina_pav.Drop += paveikslelis_Drop;
            _masina[id].masina_pav.MouseDown += Img_paspaudimas;
            _masina[id].masina_pav.MouseEnter += pav_MouseEnter;
            _masina[id].masina_pav.DragOver += pav_kelias_DragOver;
            _masina[id].masina_pav.MouseDown += pazymeti_mas_MouseDown;
            _masina[id].posukis_pav.MouseDown += pazymeti_mas_MouseDown;
            _masina[id].posukis_pav.MouseDown += Img_paspaudimas;
            _masina[id].posukis_pav.MouseEnter += pav_MouseEnter;
            _masina[id].posukis_pav.DragOver += pav_kelias_DragOver;

        }
        // Ivyksta pasirinkus kita masinos spalva
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Paveikslas.Ijungti_remus(_masina, false);               // oFF visu masinu remai
            temptiLabel.Visibility = Visibility.Visible;            // oN label matomumas (nes default - false)
            int sel_id = combo1.SelectedIndex + 1;
            string vieta = "teksturos/masinos/";                    // nustatoma masinos teksturos vieta diske
            switch (sel_id)
            {
                case 1: rdl.spalva = "black";                       // juoda
                    break;
                case 2: rdl.spalva = "blue";                        // melina
                    break;
                case 3: rdl.spalva = "orange";                      // orandzine
                    break;
                case 4: rdl.spalva = "purple";                      // purpurine
                    break;
                case 5: rdl.spalva = "red";                         // raudona
                    break;
                case 6: rdl.spalva = "yellow";                      // geltona
                    break;
                default: rdl.spalva = "black";                      // juoda
                    break;
            }
            vieta = vieta + rdl.spalva + "/top.png";
            pav_masina.Source = Paveikslas.Gauti_Source(vieta, rdl.wh_masina);  // parodoma pavyzdine masina (paskiau tempiama)
        }
        private bool p_kartas1 = true;
                     // Ivyksta pastumus slider reiksme (eismo juostas)
        private void slider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!p_kartas1)
            {
                rdl.juostu_kiekis = Convert.ToInt32(slider_juostos.Value);
                string failas = "teksturos/keliai/";
                rdl.NowImg = 4 * rdl.juostu_kiekis;
                /* isvalomas masinu sarasas */
                for (int i = 0; i < rdl.CreatedImg; i++)
                    try
                    {
                        _masina[i].Viska_atstatyti();
                    }
                    catch { ; }                                                                         // atstatomi duomenys i pradinius

                Paveikslas.Masinos_komandu_rinkimys(_masina, false);                                    // masinu paveiksleliu paruosimas
                Tempimo_paruosimas();                                                                   // paveikslelis vel paruosiamas tempimui
                try
                {
                    sara.Clear();
                    sara2.Clear();
                    combo1.SelectedIndex = -1;                                                          // nustatomas i nepasirinkta
                    temptiLabel.Visibility = Visibility.Hidden;                                         // paslepia teksta
                    pav_masina.Source = null;                                                           // paslepia pavyzdini paveiksleli
                    zenklu_panel.Visibility = Visibility.Hidden;                                        // neleidziama rinktis zenklu
                    atsakymas_label.Text = "Paspauskite mygtuka norėdami išspręsti situaciją!";         // matomas label aprasymas
                }
                catch { ; }                                                                             // isvalomas masinu sarasas

                if (rdl.kelias == '+')
                {
                    failas = failas + "plius_" + rdl.juostu_kiekis.ToString() + "juost.png";            // failas kuri uzkraudinesim
                    pav_kelias.Source = Paveikslas.Gauti_Source(failas, rdl.wh_kelias_plius);           // konvertuojamas failo linkas
                }
                else
                {
                    failas = failas + "T_" + rdl.juostu_kiekis.ToString() + "juost.png";                // failas kuri uzkraudinesim
                    pav_kelias.Source = Paveikslas.Gauti_Source(failas, rdl.wh_kelias_T);               // konvertuojamas failo linkas
                }
            }
            else
                p_kartas1 = false;
        }
        #endregion

        /// <summary>
        /// Radio mygtuku - zemelapio pasirinkimo ivykiai
        /// </summary>
        #region Radio events
                     // oFF radio panel
        private void IsjungtiRadio()
        {
            rb1.IsChecked = false;
            rb2.IsChecked = false;
            rb3.IsChecked = false;
            rbPanel.IsEnabled = false;
        }
                     // Pasikartojantys radio veiksmai
        private void Radio_veiksmai()
        {
            label1.Foreground = new SolidColorBrush(Colors.Black);              // nustatoma label spalva
            label1.Background = null;                                           // nuimamas background

            IsjungtiRadio();
            tab_item3.IsEnabled = false;                                        // off zenklu tabas
            tab_item2.IsEnabled = true;                                         // on masinu tabas
            slider_juostos.IsEnabled = true;                                    // on juostu pasirinkimas
            slider_juostos.Value = 1;                                           // pgr. reiksme - 1 juosta
            atsakymas_label.Text = "Paspauskite mygtuka norėdami išspręsti situaciją!"; // matomas label aprasymas
            rdl.NowImg = 4 * rdl.juostu_kiekis;                                 // nustomas naudojamu paveiksleliu skaicius
            try {
                sara.Clear();
                sara2.Clear();
                combo1.SelectedIndex = -1;                                      // nustatomas i nepasirinkta
                temptiLabel.Visibility = Visibility.Hidden;                     // paslepia teksta
                pav_masina.Source = null;                                       // paslepia pavyzdini paveiksleli
                zenklu_panel.Visibility = Visibility.Hidden;                    // neleidziama rinktis zenklu
            }
            catch { ; }                                                         // isvalomas masinu sarasas

            for (int i = 0; i < rdl.CreatedImg; i++)
                try
                {
                    _masina[i].Viska_atstatyti();
                } catch { Console.WriteLine("Klaida 3"); }                      // atstatomi duomenys i pradinius

            /// <summary>
            /// Masinu paveikslelio paruosimas
            /// </summary>
            Paveikslas.Masinos_komandu_rinkimys(_masina, false);
            Tempimo_paruosimas();                                               // paveikslelis vel paruosiamas tempimui
        }
                     // Isijungia izengus i radio button su pele
        private void radio1_enter(object sender, MouseEventArgs e)
        {
            pav_pavyzdys.Source = Paveikslas.Gauti_Source("teksturos/keliai/plius_formos.png", rdl.pavyzdys);
        }
                     // Isijungia izengus i radio button su pele
        private void radio2_enter(object sender, MouseEventArgs e)
        {
            pav_pavyzdys.Source = Paveikslas.Gauti_Source("teksturos/keliai/T_formos.png", rdl.pavyzdys);
        }
                     // Suveikia palikus radio button su pele
        private void radio1_leave(object sender, MouseEventArgs e)
        {
            pav_pavyzdys.Source = null;
        }
                     // Suveikia palikus radio button su pele
        private void radio2_leave(object sender, MouseEventArgs e)
        {
            pav_pavyzdys.Source = null;
        }
                     // Isijungia paspaudus ant radio button
        private void radio1_click(object sender, RoutedEventArgs e)
        {
            tempimas.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;        // scrollViewer control oN
            zem2.Height = 1000;
            rdl.kelias = '+';                                                       // parenkamas dabar naudojamas kelias
            pav_kelias.Source = Paveikslas.Gauti_Source("teksturos/keliai/plius_1juost.png", rdl.wh_kelias_plius);
            Radio_veiksmai();                                                       // atlieka veiksmu seka
        }
                     // Isijungia paspaudus ant radio button
        private void radio2_click(object sender, RoutedEventArgs e)
        {
            tempimas.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;        // scrollViewer control oN
            tempimas.ScrollToTop();                                                 // scrollViewer control pakelia i virsu
            zem2.Height = 795;
            rdl.kelias = 'T';                                                       // parenkamas dabar naudojamas kelias
            pav_kelias.Source = Paveikslas.Gauti_Source("teksturos/keliai/T_1juost.png", rdl.wh_kelias_T);
            Radio_veiksmai();                                                       // atlieka veiksmu seka
        }
        #endregion

        /// <summary>
        /// Paveiksleliu tempimo logika
        /// </summary>
        #region Drag & Drop
        // Paveikslelio tempimo pradzia
        private void pav_masina_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Paveikslas.Ijungti_remus(_masina, true);                                    // tempimo metu rodomi remai
            Image pav = e.Source as Image;                                              // nustatomas tempiamas paveikslelis
            DataObject data = new DataObject(typeof(ImageSource), pav.Source);
            DragDrop.DoDragDrop(pav, data, DragDropEffects.All);                        // tempimo efektai
        }
        // Suveikia kada tempdamas baigiasi netgi betkur
        private void pav_masina_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Paveikslas.Ijungti_remus(_masina, false);
        }
            // Fiksuojamas kada tempimo metu pele yra virs paveikslelio
        private void pav_kelias_DragOver(object sender, DragEventArgs e)
        {
            ch_pav_id = Paveikslas.Atpazinti_masina(sender, _masina);                   // nustato indeksa masinos ant kurios laikai pele
        }
            // Fiksuojamas kada uzvedi pele ant paveikslelio (netempiant)
        private void pav_MouseEnter(object sender, MouseEventArgs e)
        {
            ch_pav_id = Paveikslas.Atpazinti_masina(sender, _masina);                   // nustato indeksa masinos ant kurios laikai pele
        }
            // Fiksuojama kada tempiamas daiktas yra numetamas ant paveikslelio
        private void paveikslelis_Drop(object sender, DragEventArgs e)
        {
            string vieta = "teksturos/masinos/" + rdl.spalva + "/";
            _masina[ch_pav_id].r_mas = true;                                            // nusakai kad objektas saugo masinos paveiksleli
            _masina[ch_pav_id].s_mas = rdl.spalva;                                      // nusakai objekto paveikslelio masinos spalva
            
            bool radau = false;
            try
            {
                for (int i = 0; i < rdl.NowImg; i++)
                {
                    int b = Sarasas.GautiIndeksa(list_of_cars, i);
                    if (b == ch_pav_id)
                    {
                        sara[i].Antraste = " • " + (Sarasas.IsverstiSpalva(rdl.spalva));
                        radau = true;
                        break;
                    }
                    i++;
                }
            }
            catch
            {
            }
            if (!radau)
            {
                sara.Add(new Sarasas()
                {
                    Antraste = " • "
                    + (Sarasas.IsverstiSpalva(rdl.spalva)),
                    masinos_id = ch_pav_id,
                    spalva = rdl.spalva
                });
            }
            // papildai zenklu tab masinu pasirinkimu sarasa

            Image imageControl = (Image)sender;
            tab_item3.IsEnabled = true;

            if ((e.Data.GetData(typeof(ImageSource)) != null) && ch_pav_id != -1)
            {
                int[] _wh = new int[2];
                switch (Paveikslas.Masinos_kryptis(rdl.juostu_kiekis, ch_pav_id))
                {
                    case 0: vieta += "bottom.png";
                        _wh = rdl.wh_masina;
                        break;
                    case 1: vieta += "left.png";
                        _wh = rdl.wh_masina_hor;
                        break;
                    case 2: vieta += "top.png";
                        _wh = rdl.wh_masina;
                        break;
                    case 3: vieta += "right.png";
                        _wh = rdl.wh_masina_hor;
                        break;
                    default: vieta = "-";
                        break;
                }
                ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                if (vieta != "-")
                    _masina[ch_pav_id].masina_pav.Source = Paveikslas.Gauti_Source(vieta, _wh);
            }
        }
        #endregion

        /// <summary>
        /// Masinu trynimas
        /// </summary>
        #region Cars deleting
        private bool tr = false;                                                         // trynimas ijungtas ar isjungtas
                     // Funkcija isjungianti trynimo rezima
        private void IsjungtiTrynima()
        {
            trynti.IsChecked = false;
            trynti.Content = "Mašinų trynimas išjungtas!";
            Mouse.OverrideCursor = Cursors.Arrow;
            pav_masina.Visibility = Visibility.Visible;
            tr = false;
        }
                     // Aktyvuojas, kai pazymi, kad nori trinti masinas
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Paveikslas.ArYraMasinu(_masina))
            {
                trynti.Content = "Mašinų trynimas įjungtas!";
                Mouse.OverrideCursor = Cursors.No;
                pav_masina.Visibility = Visibility.Hidden;
                tr = true;
            }
            else
            {
                tr = false;
                trynti.IsChecked = false;

                MessageBox.Show("Deje, mašinų nėra, todėl trinti negalima!", "Apsirikote!", MessageBoxButton.OK, MessageBoxImage.Error);
                pav_masina.Visibility = Visibility.Visible;
            }
        }
                     // Aktyvuojas, kai pazymi, kad nebenori trinti masinu
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            IsjungtiTrynima();
        }
                     // Istrinama ta masina ant kurios paspaudi su pele
        private void Img_paspaudimas(object sender, MouseButtonEventArgs e)
        {
            if (tr)
            {
                Image pb = (Image)sender;
                int indeksas = 0;

                for (int i = 0; i < rdl.CreatedImg; i++)
                    if (pb == _masina[i].masina_pav)
                    {
                        indeksas = i;
                        break;
                    }

                int kryptis = Paveikslas.Masinos_kryptis(rdl.juostu_kiekis, ch_pav_id);
                if (kryptis == 0 || kryptis == 2)
                    _masina[indeksas].MasinaSource("teksturos/masinos/0remai/vertikalus.png");
                else
                    _masina[indeksas].MasinaSource("teksturos/masinos/0remai/horizontalus.png");

                _masina[ch_pav_id].Viska_atstatyti();
                Tempimo_paruosimas(indeksas);
                _masina[ch_pav_id].Matomumas(false);
                Paveikslas.Ikelti_mas_remus(_masina);

                for (int i = 0; i < sara.Count; i++)
                {
                    if (sara[i].masinos_id == ch_pav_id)
                    {
                        sara.RemoveAt(i);
                        break;
                    }
                }
                
                /* automatinis isjungimas kai nera masinu */
                if (!Paveikslas.ArYraMasinu(_masina))
                {
                    tab_item3.IsEnabled = false;
                    IsjungtiTrynima();
                }
            }
        }
        #endregion

        /// <summary>
        /// Posukio pasirinkimas
        /// </summary>
        #region Turns & lights and Selections
                     // pazymimimas radio button is pasirinkto posukio
        private void Pazymeti_pagal_posuki(char sukti)
        {
            switch (sukti)
            {
                case 'k':
                    rb1.IsChecked = true;
                    break;
                case 't':
                    rb2.IsChecked = true;
                    break;
                case 'd':
                    rb3.IsChecked = true;
                    break;
                default:
                    rb2.IsChecked = true;
                    break;
            }
        }
                     // Pasirinktai masinai uzdedamas remas aplink ja
        private void RemuFunkcija(int ch_masina, int[] dydis, string failo_vardas)
        {
            car_pasirinkimas.Visibility = Visibility.Visible;                                       // ijungiamas remo matomumas
            int kryptis = Paveikslas.Masinos_kryptis(rdl.juostu_kiekis, ch_masina);                 // nustatoma pasirintos masinos kryptis
            double[] taskas = { 0, 0 };

            if (rdl.kelias == 'T')                                                                  // 'T' formos kelio remai nustatomi auksciau nes neturi scrollView
            {//'T'
                double y = rdl.langasHeight - rdl.canvasHeight;
                taskas[0] = _masina[ch_masina].GautiXY('X') - 5;                                    // apskaicuojamos remo kordinates - x,
                taskas[1] = _masina[ch_masina].GautiXY('Y') + 5 - y;                                // y.
            }
            else
            {//'+'
                taskas[0] = _masina[ch_masina].GautiXY('X') - 5;                                    // apskaicuojamos remo kordinates - x,
                taskas[1] = _masina[ch_masina].GautiXY('Y') + 5;                                    // y.
            }

            if (kryptis == 0 || kryptis == 2)
            {
                dydis[0] = rdl.wh_masina[0] + 10;
                dydis[1] = rdl.wh_masina[1] + 10;
                
                car_pasirinkimas.Source = Paveikslas.Gauti_Source("teksturos/masinos/0remai/" + failo_vardas + "_vert.png", dydis);
            }
            else
            {
                dydis[0] = rdl.wh_masina_hor[0] + 10;
                dydis[1] = rdl.wh_masina_hor[1] + 10;

                car_pasirinkimas.Source = Paveikslas.Gauti_Source("teksturos/masinos/0remai/" + failo_vardas + "_hor.png", dydis);
            }
            Paveikslas.Taskas(car_pasirinkimas, taskas[0], taskas[1]);
        }
        private bool pasirinkta_mas = false;
                     // Aktyvuojasi, kai pasirenki kita meniu dalyka
        private void WrapPanel_Perziura(object sender, MouseButtonEventArgs e)
        {
            /// <summary>
            /// Pakeitus tab atstato pasirinkimo nustatymus
            /// </summary>
            pasirinkta_mas = false;
            list_of_cars.UnselectAll();
            /* *** zenklu pasirinkimo atstatymas *** */
            zenklu_panel.Visibility = Visibility.Hidden;                                            // neleidziama rinktis zenklu
            rbPanel.IsEnabled = false;                                                              // isjungiamas prieinamumas
            /* *** masinos pasirinkimo atstatymas *** */
            car_pasirinkimas.Visibility = Visibility.Hidden;
            car_pasirinkimas.Source = null;
            IsjungtiTrynima();
        }
        private bool list_pakeistas = false;                                                        // ar atnaujinti sarasa?
                     // Ivyksta pazymejus ListBox kita pasirinkima
        private void list_of_cars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!list_pakeistas)
            {
                rbPanel.IsEnabled = true;                                                           // radio buttons panelis
                int[] dydis = { 0, 0 };
                ch_masina = Sarasas.GautiIndeksa(list_of_cars, list_of_cars.SelectedIndex);         // konvertuoja i tikraji masinos indeksa (is listBox indekso)
                char posukis = ' ';
                try
                {
                    posukis = _masina[ch_masina].p_mas;                                             // nustatomas posukis
                    Pazymeti_pagal_posuki(posukis);                                                 // pazymi posuki
                    RemuFunkcija(ch_masina, dydis, "pasi");                                         // pasirinkus uzdedami remai aplink masina
                    zenklu_panel.Visibility = Visibility.Visible;
                    ZenklaiFunkcijos.Pazymeti_zenklus_pagal_id(_masina[ch_masina], zenkl, zenkl2);
                } catch { ; }
            }
            else
                list_pakeistas = false;
        }
                     // Laikmatis (1sekundes) sviesu mirksejimui
        private void Laikrodis(object sender, EventArgs e)
        {
            for (int i = 0; i < rdl.NowImg; i++)
            {
                if (_masina[i].r_mas && (_masina[i].p_mas == 'k' || _masina[i].p_mas == 'd'))
                {
                    if (_masina[i].mirkt == true)                                                   // jei ijungta, tai isjungi
                    {
                        _masina[i].posukis_pav.Visibility = Visibility.Hidden;
                        _masina[i].mirkt = false;
                    }
                    else
                    {
                        _masina[i].posukis_pav.Visibility = Visibility.Visible;
                        _masina[i].mirkt = true;
                    }
                }
            }
        }
                     // Pazymejus radio button - kaire
        private void rb1_Checked(object sender, RoutedEventArgs e)
        {
            if (ch_masina != -1 && Sarasas.ArPasirinkta(list_of_cars))
            {
                _masina[ch_masina].p_mas = 'k';
                _masina[ch_masina].NustatytiPosukioRema(list_of_cars);
            }
        }
                     // Pazymejus radio button - tiesiai
        private void rb2_Checked(object sender, RoutedEventArgs e)
        {
            if (ch_masina != -1)
            {
                _masina[ch_masina].p_mas = 't';
                _masina[ch_masina].posukis_pav.Source = null;
            }
        }
                     // Pazymejus radio button - desine
        private void rb3_Checked(object sender, RoutedEventArgs e)
        {
            if (ch_masina != -1 && Sarasas.ArPasirinkta(list_of_cars))
            {
                _masina[ch_masina].p_mas = 'd';
                _masina[ch_masina].NustatytiPosukioRema(list_of_cars);
            }
        }
                     // Zenklu rezime paspaudus ant masinos isivykdomas sitas kodas
        private void pazymeti_mas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int[] dydis = { 0, 0 };
            if (tabai.SelectedIndex == 2)
            {
                if (!pasirinkta_mas)                                                                    // remelis ijuntas
                {
                    ch_masina = ch_pav_id;
                    char posukis = _masina[ch_masina].p_mas;                                            // nustatomas posukis
                    Pazymeti_pagal_posuki(posukis);                                                     // pazymi posuki

                    pasirinkta_mas = true;
                    zenklu_panel.Visibility = Visibility.Visible;                                       // leidziama rinktis zenklus masinai
                    ZenklaiFunkcijos.Pazymeti_zenklus_pagal_id(_masina[ch_masina], zenkl, zenkl2);      // pazymi toks masinos pasirinktus zenklus
                    rbPanel.IsEnabled = true;
                    RemuFunkcija(ch_masina, dydis, "pasi");                                             // pasirinkus uzdedami remai aplink masina
                    /* Linkinimas nebind budu su ListBox */
                    int kiek = list_of_cars.Items.Count;
                    list_pakeistas = true;
                    for (int i = 0; i < kiek; i++)                                                      // surandamas pazymetos masinos indeksas
                        if (ch_masina == Sarasas.GautiIndeksa(list_of_cars, i))
                        {
                            list_of_cars.SelectedIndex = i;
                            break;
                        }
                    /* ********************************* */
                }
                else
                {                                                                                       // remelis isjungtas
                    IsjungtiRadio();                                                                    // oFF posukiu pasirinkimas
                    dydis[0] = 0;
                    dydis[1] = 0;
                    pasirinkta_mas = false;
                    zenklu_panel.Visibility = Visibility.Hidden;                                        // neleidziama rinktis zenklu
                    car_pasirinkimas.Visibility = Visibility.Hidden;                                    // isjungiamas remo matomumas
                    car_pasirinkimas.Source = null;                                                     // istrinamas remo paveikslelis
                    list_pakeistas = true;
                    list_of_cars.SelectedIndex = -1;                                                    // pasirinkimo nuemimas
                }
            }
        }
        #endregion
        
        /// <summary>
        /// Zenklu pasirinkimas
        /// </summary>
        #region Sign selection

        private void Zenklai_init()
        {
            const string path = "teksturos/zenklai/";
            /// <summary>
            /// Skaitomi duomenys apie zenkla
            /// </summary>
            using (StreamReader reader = new StreamReader(rdl.zenklai_file, Encoding.Default, true))
            {
                int z_id = 0;
                string vardas, komentaras, kelias, _kitas;
                while (true)                        // ciklas visiems zenklams skatyti
                {
                    /* *** Skaitomi duomenys *** */
                    vardas = reader.ReadLine();
                    komentaras = reader.ReadLine();
                    kelias = reader.ReadLine();
                    _kitas = reader.ReadLine();
                    kelias = path + kelias;
                    /* *** Tikrinamos failo klaidos ir kita *** */
                    if (_kitas != "</kitas>")
                    {
                        if (_kitas != "</pabaiga>")
                            MessageBox.Show("Ženklų failas yra blogai surašytas! Patikrinkite!");
                        else
                            zenkl.Add(new Zenklas(kelias, vardas, komentaras, z_id));
                        break;
                    }
                    /* *** Pridedamas zenklas i sarasa *** */
                    zenkl.Add(new Zenklas(kelias, vardas, komentaras, z_id));
                    z_id++;
                }
            }
            /// <summary>
            /// antra zenklu skaitymo dalis 
            /// </summary>
            using (StreamReader reader2 = new StreamReader(rdl.zenklai_file2, Encoding.Default, true))
            {
                int z_id = 0;
                string vardas, komentaras, kelias, _kitas;
                while (true)                        // ciklas visiems zenklams skatyti
                {
                    /* *** Skaitomi duomenys *** */
                    vardas = reader2.ReadLine();
                    komentaras = reader2.ReadLine();
                    kelias = reader2.ReadLine();
                    _kitas = reader2.ReadLine();
                    kelias = path + kelias;
                    /* *** Tikrinamos failo klaidos ir kita *** */
                    if (_kitas != "</kitas>")
                    {
                        if (_kitas != "</pabaiga>")
                            MessageBox.Show("Ženklų failas yra blogai surašytas! Patikrinkite!");
                        else
                            zenkl2.Add(new Zenklas2(kelias, vardas, komentaras, z_id));
                        break;
                    }
                    /* *** Pridedamas zenklas i sarasa *** */
                    zenkl2.Add(new Zenklas2(kelias, vardas, komentaras, z_id));
                    z_id++;
                }
            }
        }
        #endregion

        /// <summary>
        /// Masinos vazuovimas - situacijos sprendimas
        /// </summary>
        #region Driving
                     // nustato duomenis kuriu reikes sprendimo kurimui 
        private void Sudelioti_duomenis()
        {
            try
            {
                dm.masina = new List<m_duom>();                         // atnaujinamas sarasas
            }
            catch { }
            dm.kelias = rdl.kelias;                                     // pasirinktas kelias
            dm.juostos = rdl.juostu_kiekis;                             // pasirinktas juostu skaicius
            
            for (int i = 0; i < _masina.Length; i++)
            {
                if (_masina[i].r_mas) // jei masina pasirinta
                {
                    m_duom duom = new m_duom();     
                    duom.ch_zenklai = _masina[i].ch_zenklai;            // pasirinktu zenklu sarasas
                    duom.pos = _masina[i].p_mas;                        // masinos vaziavimo kryptis
                    duom.id = i;                                        // masinos indeksas zemelapyje
                    duom.pgr_kelio_zenklas = _masina[i].ch_pgr_kelias;  // pirmenybes kelio zenklo id
                    dm.masina.Add(duom);                                // pridedami i sarasa
                }
            }
        }
        
        // spresti situacija pagal pasirinkimus
        private void spresti_situacija_click(object sender, RoutedEventArgs e)
        {
            if (rdl.pasirinko_pirmenybe)                                            // jeigu pasirinkta kelio pirmenybe
            {
                Sudelioti_duomenis();                                               // siunciami duomenys situacijos sprendimui
                
                try {
                    Sprendimas sprend = new Sprendimas();                           // paleidzia skaiciavimus
                    sara2.Clear(); } catch {
                    MessageBox.Show("Įvyko klaida spręndžiant!"); }

                if (!dm.klaida)
                {
                    atsakymas_label.Text = "Mašinos važuos šia tvarka: (Pasirink mašiną ir ji pasižymės žaliu rėmeliu!)";
                    
                    int j = 1;
                    foreach (List<int> sublist in dm.vaziavimo_eile)      // eina per vaziavimo etapus
                    {
                        string ats = "Šios mašinos žemėlapyje nėra :)";             // standartine reiksme, jei nerastu atitikmens
                        int ats2 = -1;                                              // standartine reiksme
                        foreach (int reiksme in sublist)            // eina per masinas
                        {
                            for (int i = 0; i < sara.Count; i++)                        // ieskoma kitame sarase
                            {
                                if (reiksme == sara[i].masinos_id)
                                {
                                    ats = Sarasas.IsverstiSpalva(sara[i].spalva);
                                    ats2 = sara[i].masinos_id;
                                }
                            }
                            sara2.Add(new Sarasas()                                     // pridejimas i sarasa
                            {
                                Antraste = j.ToString() + ") " + ats,
                                masinos_id = ats2
                            });
                        }
                        j++;
                    }
                }
                else
                {
                    if (dm.klaidu_sarasas.Count == 1)
                        atsakymas_label.Text = "Klaida:";                // pakeicia label teksta
                    else
                        atsakymas_label.Text = "Klaidos:";               // pakeicia label teksta

                    foreach (string str in dm.klaidu_sarasas)
                    {
                        sara2.Add(new Sarasas()
                        {
                            Antraste = " ♦ " + str
                        });
                    }
                }
            }
            else
                MessageBox.Show("Ei! Pasirink kuriam keliui teiki pirmenybę :)", "Pamiršai vieną dalyką!", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
                     // paspaudus ant atsakymu dezutes
        private void atsakymas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ch_masina = Sarasas.GautiIndeksa(atsakymas_box, atsakymas_box.SelectedIndex);     // konvertuoja i tikraji masinos indeksa (is listBox indekso)
            if (!dm.klaida && ch_masina >= 0)
            {
                int[] dydis = { 0, 0 };
                car_pasirinkimas.Visibility = Visibility.Visible;
                RemuFunkcija(ch_masina, dydis, "zalias");                                     // pasirinkus uzdedami remai aplink masina
            }
            else
                car_pasirinkimas.Visibility = Visibility.Hidden;
        }
        #endregion

    }
}
