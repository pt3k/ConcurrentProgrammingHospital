using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Szpital
{
    public class Badanie
    {
        string nazwa;
        private object _lock = new object();
        Queue<Pacjent> kolejka = new Queue<Pacjent>();
        ListView obslugiwanyList, kolejkaList;

        public Badanie(string n, ListView ol, ListView kl)
        {
            nazwa = n;
            obslugiwanyList = ol;
            kolejkaList = kl;
        }

        public void Dzialaj()
        {
            Random random = new Random();

            while (true)
            {
                Pacjent pacjent = BadajPacjenta();
                Form1.RemoveFromListView(kolejkaList, 0);
                Form1.AddToListView(obslugiwanyList, pacjent.getName());
                Console.WriteLine($"[{nazwa}] Badanie {pacjent.getNumer()}");
                Thread.Sleep(random.Next(6000));

                int nrLekarz = 0;
                if (pacjent.getChoroba().typ == "kardiologiczna")
                {
                    nrLekarz = 0;
                }
                else if (pacjent.getChoroba().typ == "psychologiczna")
                {
                    nrLekarz = 1;
                }
                else if (pacjent.getChoroba().typ == "ortopedyczna")
                {
                    nrLekarz = 2;
                }

                if (pacjent.ciezkiPrzypadek)
                {
                    Form1.lekarze[nrLekarz].CzekajWKolejcePozaKolejnoscia(pacjent);
                }
                else
                {
                    Form1.lekarze[nrLekarz].CzekajWKolejce(pacjent);
                }
                Form1.RemoveFromListView(obslugiwanyList, 0);
                Console.WriteLine($"[{nazwa}] {pacjent.getNumer()} -> {Form1.lekarze[nrLekarz].getNazwa()}");
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

                foreach (var item in items)
                {
                    kolejka.Enqueue(item);
                    Form1.AddToListView(kolejkaList, item.getName());
                }
                Monitor.Pulse(_lock);
            }
        }

        Pacjent BadajPacjenta()
        {
            lock (_lock)
            {
                while (kolejka.Count < 1)
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
