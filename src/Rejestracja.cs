using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Szpital
{
    class Rejestracja
    {
        ListView rejestracjaView;
        public List<Lekarz> lekarze;
        List<Choroba> choroby;
        static private object _lock = new object();
        static Queue<Pacjent> kolejkaRejestracja = new Queue<Pacjent>();
        int pacjenci = 0;

        public Rejestracja(List<Lekarz> l, ListView rv, List<Choroba> ch)
        {
            lekarze = l;
            rejestracjaView = rv;
            choroby = ch;
        }

        public void Obsluguj()
        {
            while (true)
            {
                Pacjent pacjent = KierujPacjentow();

                int nrLekarz = 0;
                if (pacjent.getChoroba().typ == "kardiologiczna")
                {
                    nrLekarz = 0;
                } else if(pacjent.getChoroba().typ == "psychologiczna")
                {
                    nrLekarz = 1;
                } else if(pacjent.getChoroba().typ == "ortopedyczna")
                {
                    nrLekarz = 2;
                }
                Console.WriteLine($"[Rejestracja] {pacjent.getNumer()} -> {lekarze[nrLekarz].getNazwa()}");

                if (pacjent.ciezkiPrzypadek)
                {
                    lekarze[nrLekarz].CzekajWKolejcePozaKolejnoscia(pacjent);
                }
                else
                {
                    lekarze[nrLekarz].CzekajWKolejce(pacjent);
                }
                
                Thread.Sleep(400);
            }
        }

        public Pacjent KierujPacjentow()
        {
            lock (_lock)
            {
                while (kolejkaRejestracja.Count < 1)
                {
                    Monitor.Wait(_lock);
                }
                Form1.RemoveFromListView(rejestracjaView, 0);
                return kolejkaRejestracja.Dequeue();
            }
        }

        public void PrzyjmijPacjenta()
        {
            lock (_lock)
            {
                Choroba choroba = choroby[new Random().Next(choroby.Count)];
                bool ciezkiPrzypadek;
                if ((new Random()).NextDouble() < 0.2)
                {
                    ciezkiPrzypadek = true;
                }
                else
                {
                    ciezkiPrzypadek = false;
                }

                PrzyjmijPacjenta(choroba, ciezkiPrzypadek);
            }
        }

        public void PrzyjmijPacjenta(Choroba choroba, bool ciezkiPrzypadek)
        {
            lock (_lock)
            {
                Pacjent pacjent = new Pacjent(pacjenci, choroba, ciezkiPrzypadek);
                Console.WriteLine($"[Rejestracja] Przyjeto pacjenta {pacjenci} z podejrzeniem {pacjent.getChoroba().nazwa}");
                kolejkaRejestracja.Enqueue(pacjent);
                Form1.AddToListView(rejestracjaView, pacjent.getName());
                pacjenci++;
                Monitor.Pulse(_lock);
            }
        }
    }
}
