using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System;

namespace KET4.klases
{
    public class ZenklaiFunkcijos
    {
                           // pries pasirenkant kita masina atzymimi visi zenklai
        private static void Atzymeti_visus_zenklus(ObservableCollection<Zenklas> zenklai, ObservableCollection<Zenklas2> zenklai2)
        {
            foreach (Zenklas znk in zenklai)
            {
                znk.IsChecked = false;
            }
            foreach (Zenklas2 znk in zenklai2)
            {
                znk.Pazymeti(false);
            }
        }

        public static void Pazymeti_zenklus_pagal_id(Masina _masi, ObservableCollection<Zenklas> zenklai, ObservableCollection<Zenklas2> zenklai2)
        {
            Atzymeti_visus_zenklus(zenklai, zenklai2);                            // is pradziu nuzymimi visi zenklai
            foreach (int ii in _masi.ch_zenklai)
            {
				try
				{
					zenklai[ii].IsChecked = true;                           // po cia pazymimi visi reikiami
				}
				catch
				{
					// 
				}
            }
            rdl.pasirinkta__masi = _masi;                               // issaugoma pasirinkta masina globaliam sarase

            if (_masi.ch_pgr_kelias != -1)
                zenklai2[_masi.ch_pgr_kelias].Pazymeti(true);           // pazymimas pasirinktas pasirinkimas
        }

    }

    public class Zenklas : ToggleButton
    {
        /// <summary>
        /// klases kintamieji
        /// </summary>
        #region Variables
        private Image zenklas_pav;                                      // mygtuko paveikslelis
        private ImageSource vieta_pav;                                  // paveikslelio adresas
        private StackPanel stack_panel;
        private TextBlock text_blokas1;                                 // toolTip pavadinimas
        private int this_id;                                            // sio zenklo skaitinis pavadinimas

        public ImageSource paveikslelis_loc
        {
            get { return vieta_pav as ImageSource; }
            set
            {
                vieta_pav = value;
                zenklas_pav.Source = value;
            }
        }

        public string hintas
        {
            get { return ToolTip.ToString(); }
            set { ToolTip = value; }
        }
        #endregion

        public Zenklas()
        {
            Width = rdl.wh_zenklas[0];                                      // nustatomas control plotis
            Height = rdl.wh_zenklas[1];                                     // nustatomas control ilgis
        }

        public Zenklas(string vieta, string hint_name , string hint, int z_id)
        {
            Width = rdl.wh_zenklas[0];                                           // nustatomas control plotis
            Height = rdl.wh_zenklas[1];                                          // nustatomas control ilgis
            hintas = hint;                                                       // nustatomas paaiskinimas
            Content = null;                                                      // isimamas tekstas
            Margin = new Thickness(2);
            Background = Brushes.Gray;                                           // nustatomas fonas
            BorderBrush = Brushes.Coral;
            BorderThickness = new Thickness(2);
            Click += ToggleButton_Click;                                         // pridedamas naujas event paspaudus ant mygtuko
            this_id = z_id;

            /// <summary>
            /// kuriamas mygtuko paveikslelis
            /// </summary>
            zenklas_pav = new Image();
            paveikslelis_loc = Paveikslas.Gauti_Source(vieta, rdl.wh_zenklas);   // ikeliamas paveikslelis

            stack_panel = new StackPanel();
            stack_panel.Children.Add(zenklas_pav);
            Content = stack_panel;

            /// <summary>
            /// kuriamas mygtuko toolTip
            /// </summary>
            StackPanel stack_panel2 = new StackPanel();
            text_blokas1 = new TextBlock();
            TextBlock text_blokas2 = new TextBlock();

            stack_panel2.HorizontalAlignment = HorizontalAlignment.Center;
            text_blokas1.FontWeight = FontWeights.Bold;
            text_blokas1.Foreground = Brushes.BlueViolet;
            text_blokas1.Text = hint_name;
            text_blokas2.Text = hintas;

            stack_panel2.Children.Add(text_blokas1);
            stack_panel2.Children.Add(text_blokas2);
            ToolTip = stack_panel2;

            //_masina[i].posukis_pav.MouseDown += pazymeti_mas_MouseDown;
        }

        public void Keisti_pavadinimo_spalva(string hex_color)
        {
            text_blokas1.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(hex_color));
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsChecked == true)                                      // kai pazymi
            {
                rdl.pasirinkta__masi.ch_zenklai.Add(this_id);           // pridedi prie pasirinktu zenklu saraso tai masinai
            }
            else
            {                                                           // kai nuzymi
                for (int i = 0; i < rdl.pasirinkta__masi.ch_zenklai.Count; i++)
                {
                    if (this_id == rdl.pasirinkta__masi.ch_zenklai[i])  // surandamas sis zenklo indeksas tarp masinos pasirinktu zenklu kolekcijos ir istrinamas
                    {
                        rdl.pasirinkta__masi.ch_zenklai.RemoveAt(i);
                        break;                                          // radus ieskoti toliau nebereikia
                    }
                }
            }
        }
    }

    public class Zenklas2 : StackPanel
    {

        private RadioButton zym;
        private TextBlock tekstas;
        private Image zenklas;
        private int this_id;

        public Zenklas2(string vieta, string hint_name, string hint, int z_id)
        {
            Orientation = Orientation.Horizontal;
            zym = new RadioButton();
            tekstas = new TextBlock();
            zenklas = new Image();

            Height = rdl.wh_zenklas[1];
            tekstas.Text = hint_name;
            this.ToolTip = hint;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            zenklas.Source = Paveikslas.Gauti_Source(vieta, rdl.wh_zenklas);
            tekstas.FontSize = 15;
            tekstas.Margin = new Thickness(10);
            tekstas.Foreground = Brushes.Beige;

            zym.GroupName = "a";
            this_id = z_id;
            zym.Checked += Paspaudimas;

            Children.Add(zym);
            Children.Add(zenklas);
            Children.Add(tekstas);
        }

        public void Pazymeti(bool ar)
        {
            zym.IsChecked = ar;
        }

        private void Paspaudimas(object sender, RoutedEventArgs e)
        {
            rdl.pasirinkta__masi.ch_pgr_kelias = this_id;
            rdl.pasirinko_pirmenybe = true;
        }
    }
}