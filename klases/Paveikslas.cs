using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KET4.klases
{
    public class Paveikslas
    {
                           // tam tikra objekta pastato pagal duotas koordinates
        public static void Taskas(Image img, double naujasX, double naujasY)
        {
            Canvas.SetLeft(img, naujasX);
            Canvas.SetTop(img, canvasY(naujasY));
        }
                                  // grazina kompiuterini 'bitmap' paveikslelio adresa
        public static ImageSource Gauti_Source(string fileName, int[] wh)
        {
            byte[] buffer = File.ReadAllBytes(fileName);
            MemoryStream memoryStream = new MemoryStream(buffer);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.DecodePixelWidth = wh[0];
            bitmap.DecodePixelHeight = wh[1];
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
                           // tikrina bool masyva
        public static bool ArYraMasinu(Masina[] tikrinti)
        {
            for (int i = 0; i < rdl.NowImg; i++)
                if (tikrinti[i].r_mas == true)
                    return true;
            return false;
        }
                        // keicia i XY asiu logikos Y
        public static double normalY(double Y)
        {
            return rdl.langasHeight - Y;
        }
                        // keicia i canvas asiu logikos Y
        public static double canvasY(double Y)
        {
            if (rdl.kelias == 'T')
            {
                return rdl.canvasHeight - Y;
            }
            else
                return rdl.langasHeight - Y;
        }
                        // isvalomi masinu paveiksleliai
        public static void Isvalyti_mas(Masina[] masina) 
        {
            for (int i = 0; i < rdl.CreatedImg; i++)
                try
                {
                    masina[i].Viska_atstatyti();
                }
                catch
                {
                }
        }
                        // ikeliami masinu remai
        public static void Ikelti_mas_remus(Masina[] masina)
        {
            string kryptis;
            int[] my_wh = { 0, 0 };
            for (int i = 0; i < rdl.NowImg / rdl.juostu_kiekis; i++)             // pereina per visas kelio puses
            {
                switch(i + 1)                                                   // isrenka masinos krypti ir remu dydi
                {
                    case 1: kryptis = "vertikalus";
                    my_wh = rdl.wh_masina;
                    break;
                    case 2: kryptis = "horizontalus";
                    my_wh = rdl.wh_masina_hor;
                    break;
                    case 3: kryptis = "vertikalus";
                    my_wh = rdl.wh_masina;
                    break;
                    case 4: kryptis = "horizontalus";
                    my_wh = rdl.wh_masina_hor;
                    break;
                    default: kryptis = "vertikalus";
                    break;
                }
                if (rdl.kelias != 'T' || i != 0)
                    for (int j = 1; j <= rdl.juostu_kiekis; j++)
                    {
                        if (!masina[i * rdl.juostu_kiekis + j - 1].r_mas)
                        {
                            masina[i * rdl.juostu_kiekis + j - 1].NustatytiWH(my_wh);                                                                   // nustatoma X koordinate
                            masina[i * rdl.juostu_kiekis + j - 1].MasinaSource("teksturos/masinos/0remai/" + kryptis + ".png");                         // nustatomas remu paveikslelis
                        }
                    }
            }
        }
                        // su turimais duomenimis sudelioja paveikslelius zemelapyje
        public static void Sudelioti_mas(Masina[] masina)
        {
            for (int i = 0; i < rdl.NowImg; i++)
                if (rdl.kelias != 'T' || rdl.juostu_kiekis != i + 1)
                    if (rdl.kelias == '+')
                        switch(rdl.juostu_kiekis)
                        {
                            case 1:
                                masina[i].NustatytiXY(rdl.k_plius_car1[i, 0], rdl.k_plius_car1[i, 1]);
                                break;
                            case 2:
                                masina[i].NustatytiXY(rdl.k_plius_car2[i, 0], rdl.k_plius_car2[i, 1]);
                                break;
                            case 3:
                                masina[i].NustatytiXY(rdl.k_plius_car3[i, 0], rdl.k_plius_car3[i, 1]);
                                break;
                        }
                    else
                        switch (rdl.juostu_kiekis)
                        {
                            case 1:
                                masina[i].NustatytiXY(rdl.k_T_car1[i, 0], rdl.k_T_car1[i, 1]);
                                break;
                            case 2:
                                masina[i].NustatytiXY(rdl.k_T_car2[i, 0], rdl.k_T_car2[i, 1]);
                                break;
                            case 3:
                                masina[i].NustatytiXY(rdl.k_T_car3[i, 0], rdl.k_T_car3[i, 1]);
                                break;
                        }
        }
                        // reguluoja masinu matomuma
        public static void Ijungti_remus(Masina[] masina, bool ar)
        {
            for (int i = 0; i < rdl.NowImg; i++)
                if (ar)                                                         // ijungia visus remus
                {
                    try
                    {
                        masina[i].masina_pav.Visibility = System.Windows.Visibility.Visible;
                    }
                    catch { Console.WriteLine("Klaida 1"); }
                }
                else
                {
                    if (!masina[i].r_mas)                                          // isjungia visus remus isskyrus masinas
                    {
                        try
                        {
                            masina[i].masina_pav.Visibility = System.Windows.Visibility.Hidden;
                        }
                        catch { Console.WriteLine("Klaida 2"); }
                    }
                }
        }
                        // atstato masina i pradinius duomenis
        public static void Restartuoti_masinas(Masina[] masina)
        {
            for (int i = 0; i < rdl.CreatedImg; i++)
                masina[i].masina_pav.Source = null;
        }
                        // masinu tvarkymo komandu rinkinys
        public static void Masinos_komandu_rinkimys(Masina[] masina, bool ar)
        {
            /* masinu paveiksleliu paruosimas */
            Isvalyti_mas(masina);
            Ikelti_mas_remus(masina);
            Sudelioti_mas(masina);
            Ijungti_remus(masina, ar);
            /* ------------------------------ */
        }
                        // atpazina pasirinkta masina is zemelapio
        public static int Atpazinti_masina(object sender, Masina[] masina)
        {
            Image my_img = (Image)sender;
            for (int i = 0; i < rdl.NowImg; i++)
                if (my_img == masina[i].masina_pav || my_img == masina[i].posukis_pav)
                {
                    return i;
                }
            return -1;
        }
                        // atsako i kuria puse paveikslelis tures ziureti
        public static int Masinos_kryptis(int juostos, int ch_id)
        {
            if (juostos == 1)
            {
                return ch_id;
            }
            else if (juostos == 2)
            {
                if (ch_id == 0 || ch_id == 1)
                    return 0;
                else if (ch_id == 2 || ch_id == 3)
                    return 1;
                else if (ch_id == 4 || ch_id == 5)
                    return 2;
                else if (ch_id == 6 || ch_id == 7)
                    return 3;
            }
            else if (juostos == 3)
            {
                if (ch_id == 0 || ch_id == 1 || ch_id == 2)
                    return 0;
                else if (ch_id == 3 || ch_id == 4 || ch_id == 5)
                    return 1;
                else if (ch_id == 6 || ch_id == 7 || ch_id == 8)
                    return 2;
                else if (ch_id == 9 || ch_id == 10 || ch_id == 11)
                    return 3;
            }
            return 0;
        }
    }

    // programos temu spalvos
    public struct palete
    {
        public static readonly string[] numatyta = { "#FFA4C182", "#FF9961C7", "#FFC34A4A", "#BF1BC5C5", "#FFFF8989" };
        public static readonly string[] muted = { "#2e4045", "#83adb5", "#c7bbc9", "#5e3c58", "#bfb5b2" };
        public static readonly string[] beach = { "#96ceb4", "#ffeead", "#ff6f69", "#ffcc5c", "#88d8b0" };
        public static readonly string[] pastel = { "#1b85b8", "#5a5255", "#559e83", "#ae5a41", "#c3cb71" };
        public static readonly string[] blue = { " 	#77aaff", "#99ccff", "#bbeeff", "#5588ff", "#3366ff" };
        public static readonly string[] candy = { "#343467", "#964c65", "#ff6666", "#ffb399", "#ffffcd" };
    }
    // ReadOnly(kitaip tariant const) arba kiti globalus kintamieji
    public struct rdl
    {
        public const string zenklai_file = "teksturos/zenklai/init.txt";         // zenklu pradinis load failas
        public const string zenklai_file2 = "teksturos/zenklai/pirmenybes.txt";  // antras zenklu failas
        public const int CreatedImg = 12;                                        // maksimalus masinu kiekis zemelapyje
        public static int NowImg = CreatedImg;                                   // dabar naudojamas masinu kiekis zemelapyje
        public static double langasHeight;                                       // pagrindinio lango aukstis
        public const double canvasHeight = 794;                                  // zemelapio aukstis
        
        public static char kelias;                                               // pasirinkto kelio zenklas (+, T)
        public static string spalva;                                             // pasirinkta masinos spalva
        public static int juostu_kiekis = 1;                                     // pasirinkto kelio juostu kiekis   
        public static bool pasirinkta = false;                                   // ar pasirinktas remas (zenklu tabe)?
        public static bool list_pakeistas = false;                               // ar sarasas jau pakeistas?
        public static double greitis = 10;                                       // masinos judejimo greitis
        public static bool pasirinko_pirmenybe = false;                                  

        public static Masina pasirinkta__masi;                                   // pasirinktos masinos klase

        /* ***** ***** ***** kelio ***** ***** ***** */
        public static readonly int[] wh_kelias_plius = { 1098, 1000 };           // aukstis ir ilgis kelio sankryzos
        public static readonly int[] wh_kelias_T = { 1118, 795 };                // aukstis ir ilgis kelio T formos kelio
        /* ***** ***** ***** masinu **** ***** ***** */
        public static readonly int[] wh_masina = { 100, 200 };                   // aukstis ir ilgis vertikalios masinos
        public static readonly int[] wh_masina_hor = { 200, 100 };               // aukstis ir ilgis horizontalios masinos
        public static readonly int[] wh_zenklas = { 50, 48 };                    // aukstis ir ilgis zenklo
        public static readonly int[] pavyzdys = { 186, 186 };                    // aukstis ir ilgis pavyzdzio

        /// <summary>
        /// Masinu paveiksleliu koordinates
        /// </summary>
        #region Image coordinates
        public static readonly double[,] k_plius_car1 = { {450, 870},            // masinu kordinates plius formos sankryzoje
                                                          {724, 600},
                                                          {550, 330},
                                                          {165, 500} };
        public static readonly double[,] k_plius_car2 = { { 350, 960 },
                                                          { 450, 960 },
                                                          { 820, 700 },
                                                          { 820, 600 },
                                                          { 650, 235 },
                                                          { 550, 235 },
                                                          { 80, 400 },
                                                          { 80, 500 } };
        public static readonly double[,] k_plius_car3 = { { 250, 1065 },
                                                          { 350, 1065 },
                                                          { 450, 1065 },
                                                          { 930, 800 },
                                                          { 930, 700 },
                                                          { 930, 600 },
                                                          { 750, 140 },
                                                          { 650, 140 },
                                                          { 550, 140 },
                                                          { -25, 300 },
                                                          { -25, 400 },
                                                          { -25, 500 } };
        public static readonly double[,] k_T_car1 = { {0, 0},                    // masinu kordinates T formos sankryzoje
                                                      {735, 600},
                                                      {560, 340},
                                                      {190, 500} };
        public static readonly double[,] k_T_car2 = { { 0, 0 },
                                                      { 0, 0 },
                                                      { 830, 698 },
                                                      { 830, 598 },
                                                      { 662, 238 },
                                                      { 561, 238 },
                                                      { 75, 397 },
                                                      { 75, 497 } };
        public static readonly double[,] k_T_car3 = { { 0, 0 },
                                                      { 0, 0 },
                                                      { 0, 0 },
                                                      { 925, 792 },
                                                      { 925, 692 },
                                                      { 925, 592 },
                                                      { 764, 140 },
                                                      { 662, 140 },
                                                      { 560, 140 },
                                                      { -15, 292 },
                                                      { -15, 392 },
                                                      { -15, 492 } };
        #endregion
    }

}
