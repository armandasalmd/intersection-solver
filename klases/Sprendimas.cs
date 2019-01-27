using System;
using System.Collections.Generic;

namespace KET4.klases
{
    public struct m_duom
    {
        public int id;                      // masinos id zemelapyje
        public char pos;                    // posukis (k,t,d)
        public List<int> ch_zenklai;        // pasirinkti masinos zenklai
        public int pgr_kelio_zenklas;       // pasirinktas pagrindinio kelio zenklas
    }

    public struct dm // duomenys
    {
        /// <summary>
        /// Gaunami duomenys
        /// </summary>
      
        public static char kelias;
        public static int juostos;
        public static List<m_duom> masina;                    // masinos objektas

        /// <summary>
        /// Atsakymo duomenys
        /// </summary>

        public static bool klaida;                            // ar pavyko sukompiluoti?
        public static List<string> klaidu_sarasas;            // tekstas ivedamas i ekrana - 'erroras'
        public static List<List<int>> vaziavimo_eile;         // masinu vaziavimo eile
        
    }

    public class Sprendimas
    {
        public static void Ziureti_atsakyma()
        {
            foreach (var ex in dm.vaziavimo_eile)
            {
                foreach (var eb in ex)
                {
                    Console.Write(eb + " ");
                }
                Console.WriteLine();
            }
            //Console.WriteLine(dm.vaziavimo_eile[0][1]);      
        }

        public Sprendimas()
        {
            List<int> sub = new List<int>();
            dm.vaziavimo_eile = new List<List<int>>();
            

            sub.Add(5);             // pridedi pirmo ejimo reiksmes
            sub.Add(3);             // pridedi pirmo ejimo reiksmes

            dm.vaziavimo_eile.Add(sub); // kai jau PIRMA ejima baigi ji pridedi i pagrindini sarasa
            sub = new List<int>();  // tada atnaujini pagalbini sarasa

            sub.Add(1);             // pridedi antro ejimo reiksmes

            dm.vaziavimo_eile.Add(sub); // kai jau ANTRA ejima baigi ji pridedi i pagrindini sarasa
            sub = new List<int>();  // tada atnaujini pagalbini sarasa
            
        }
    }
}

