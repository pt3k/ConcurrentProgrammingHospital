using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Szpital
{
    public class Lekarz
    {
        string nazwa;
        private object _lock = new object();
        Queue<Pacjent> kolejka = new Queue<Pacjent>();
        public List<Badanie> badania = new List<Badanie>();
        ListView obslugiwanyList, kolejkaList; 

        public Lekarz(string n, ListView ol, ListView kl, List<Badanie> b)
        {
            nazwa = n;
            obslugiwanyList = ol;
            kolejkaList = kl;
            badania = b;
        }

        public void Lecz()
        {
            Random random = new Random();

            while (true)
            {
                Pacjent pacjent = ObsluzPacjenta();

                Form1.RemoveFromListView(kolejkaList, 0);
                Form1.AddToListView(obslugiwanyList, pacjent.getName());
                Console.WriteLine($"[{nazwa}] Leczy {pacjent.getNumer()} z podejrzeniem {pacjent.getChoroba().nazwa}");

                Thread.Sleep(random.Next(3000));

                Form1.RemoveFromListView(obslugiwanyList, 0);
                if (random.NextDouble() < 0.3)
                {
                    int nrBadania = random.Next(badania.Count);
                    if (pacjent.ciezkiPrzypadek)
                    {
                        badania[nrBadania].CzekajWKolejcePozaKolejnoscia(pacjent);
                    }
                    else
                    {
                        badania[nrBadania].CzekajWKolejce(pacjent);
                    }
                    Console.WriteLine($"[{nazwa}] Wysłał {pacjent.getNumer()} do {badania[nrBadania].getNazwa()}");
                }
                else
                {
                    Console.WriteLine($"[{nazwa}] Wyleczył {pacjent.getNumer()} z {pacjent.getChoroba().nazwa}");
                }
            }
        }

        public void CzekajWKolejce(Pacjent pacjent)
        {
            lock (_lock)
            {
                kolejka.Enqueue(pacjent);
                Form1.AddToListView(kolejkaList, pacjent.getName());
                Monitor.Pulse(_lock);
            }
        }

        public void CzekajWKolejcePozaKolejnoscia(Pacjent pacjent)
        {
            lock (_lock)
            {
                var items = kolejka.ToArray();
                kolejka.Clear();
                Form1.ClearListView(kolejkaList);

                kolejka.Enqueue(pacjent);
                Form1.AddToListView(kolejkaList, pacjent.getName());

                foreach(var item in items)
                {
                    kolejka.Enqueue(item);
                    Form1.AddToListView(kolejkaList, item.getName());
                }    
                Monitor.Pulse(_lock);
            }
        }

        Pacjent ObsluzPacjenta()
        {
            lock(_lock)
            {
                while(!(kolejkaList.Items.Count > 0))
                {
                    Monitor.Wait(_lock);
                }

                return kolejka.Dequeue();
            }
        }

        public string getNazwa()
        {
            return nazwa;
        }
    }
}
