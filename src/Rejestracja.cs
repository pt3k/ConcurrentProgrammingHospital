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
        List<Choroba> choroby = new List<Choroba>();
        static private object _lock = new object();
        static Queue<Pacjent> kolejkaRejestracja = new Queue<Pacjent>();
        int pacjenci = 0;

        public Rejestracja(List<Lekarz> l, ListView rv)
        {
            lekarze = l;
            rejestracjaView = rv;

            choroby.Add(new Choroba("zlamanie","ortopedyczna"));
            choroby.Add(new Choroba("zwichniecie", "ortopedyczna"));
            choroby.Add(new Choroba("zwyrodnienie kregoslupa", "ortopedyczna"));
            choroby.Add(new Choroba("lordoza", "ortopedyczna"));
            choroby.Add(new Choroba("zapalenie kosci", "ortopedyczna"));

            choroby.Add(new Choroba("nadcisnienie tetnicze", "kardiologiczna"));
            choroby.Add(new Choroba("zawał", "kardiologiczna"));
            choroby.Add(new Choroba("miażdżyca", "kardiologiczna"));
            choroby.Add(new Choroba("zapalenie mięśnia sercowego", "kardiologiczna"));

            choroby.Add(new Choroba("depresja", "psychologiczna"));
            choroby.Add(new Choroba("lęki", "psychologiczna"));
            choroby.Add(new Choroba("drazliwosc", "psychologiczna"));
            choroby.Add(new Choroba("bezsennosc", "psychologiczna"));
        }

        public void Obsluguj()
        {
            Random random = new Random();

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
                
                Thread.Sleep(200);
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
                Pacjent pacjent = new Pacjent(pacjenci, choroby[new Random().Next(choroby.Count)]);
                Console.WriteLine($"[Rejestracja] Przyjeto pacjenta {pacjenci} z podejrzeniem {pacjent.getChoroba().nazwa}");
                kolejkaRejestracja.Enqueue(pacjent);
                Form1.AddToListView(rejestracjaView, pacjent.getName());
                pacjenci++;
                Monitor.Pulse(_lock);
            }
        }
    }
}
