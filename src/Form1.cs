using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Szpital
{
    public partial class Form1 : Form
    {
        private delegate void AddToListViewDelegate(ListView list, string text);
        private delegate void RemoveFromListViewDelegate(ListView list, int index);
        private delegate void ClearListViewDelegate(ListView list);
        public static List<Lekarz> lekarze = new List<Lekarz>();
        List<Choroba> choroby = new List<Choroba>();
        List<Badanie> badania = new List<Badanie>();
        Rejestracja rejestracja;

        Thread rezonansThread;
        Thread USGThread;
        Thread krewThread;
        Thread kardiologThread;
        Thread psychiatraThread;
        Thread ortopedaThread;
        Thread rejestracjaThread;

        public Form1()
        {
            AllocConsole();
            InitializeComponent();

            InitOdzialRatunkowy();

            foreach(Choroba c in choroby)
            {
                chorobaComboBox.Items.Add(c);
            }
        }

        private void InitOdzialRatunkowy()
        {
            choroby.Add(new Choroba("zlamanie", "ortopedyczna"));
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

            Badanie rezonans = new Badanie("rezonans magnetyczny", rezonansObslugiwanyListView, rezonansKolejkaListView);
            badania.Add(rezonans);
            rezonansThread = new Thread(rezonans.Dzialaj);

            Badanie USG = new Badanie("USG", USGObslugiwanyListView, USGKolejkaListView);
            badania.Add(USG);
            USGThread = new Thread(USG.Dzialaj);

            Badanie krew = new Badanie("badanie krwi", krewObslugiwanyListView, krewKolejkaListView);
            badania.Add(krew);
            krewThread = new Thread(krew.Dzialaj);


            Lekarz kardiolog = new Lekarz("Kardiolog", kardiologObslugiwanyListView, kardiologKolejkaListView, badania);
            lekarze.Add(kardiolog);
            kardiologThread = new Thread(kardiolog.Lecz);

            Lekarz psychiatra = new Lekarz("Psychiatra", psychiatraObslugiwanyListView, psychiatraKolejkaListView, badania);
            lekarze.Add(psychiatra);
            psychiatraThread = new Thread(psychiatra.Lecz);

            Lekarz ortopeda = new Lekarz("Ortopeda", ortopedaObslugiwanyListView, ortopedaKolejkaListView, badania);
            lekarze.Add(ortopeda);
            ortopedaThread = new Thread(ortopeda.Lecz);


            rejestracja = new Rejestracja(lekarze, rejestracjaListView, choroby);
            rejestracjaThread = new Thread(rejestracja.Obsluguj);

            rezonansThread.Start();
            USGThread.Start();
            krewThread.Start();
            kardiologThread.Start();
            psychiatraThread.Start();
            ortopedaThread.Start();
            rejestracjaThread.Start();
        }

        //konsola
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void dodajLosowegoPacjentaButton_Click(object sender, EventArgs e)
        {
            rejestracja.PrzyjmijPacjenta();
        }

        private void dodaj10LosowychPacjentowButton_Click(object sender, EventArgs e)
        {
            int ilePacjentow;
            if(!int.TryParse(wieluPacjentowTextBox.Text, out ilePacjentow))
            {
                ilePacjentow = 1;
            }

            for(int i = 0; i < ilePacjentow; i++)
            {
                rejestracja.PrzyjmijPacjenta();
            }
        }

        private void dodajKonkretnegoPacjentaButton_Click(object sender, EventArgs e)
        {
            Choroba choroba = (Choroba)chorobaComboBox.SelectedItem;
            bool ciezkiPrzypadek = ciezkiPrzypadekCheckBox.Checked;

            rejestracja.PrzyjmijPacjenta(choroba, ciezkiPrzypadek);
        }

        public static void AddToListView(ListView list,string text)
        {
            if (list.InvokeRequired)
            {
                var d = new AddToListViewDelegate(AddToListView);
                list.Invoke(d, new object[] {list ,text });
            }
            else
            {
                list.Items.Add(text);
            }
        }

        public static void RemoveFromListView(ListView list, int index)
        {
            if (list.InvokeRequired)
            {
                var d = new RemoveFromListViewDelegate(RemoveFromListView);
                list.Invoke(d, new object[] { list, index });
            }
            else
            {
                list.Items.RemoveAt(index);
            }
        }

        public static void ClearListView(ListView list)
        {
            if (list.InvokeRequired)
            {
                var d = new ClearListViewDelegate(ClearListView);
                list.Invoke(d, new object[] { list });
            }
            else
            {
                list.Items.Clear();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            rezonansThread.Abort();
            USGThread.Abort();
            krewThread.Abort();
            kardiologThread.Abort();
            psychiatraThread.Abort();
            ortopedaThread.Abort();
            rejestracjaThread.Abort();
        }
    }
}
