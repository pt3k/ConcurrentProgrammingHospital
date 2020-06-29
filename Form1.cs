using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szpital
{
    public partial class Form1 : Form
    {
        private delegate void AddToListViewDelegate(ListView list, string text);
        private delegate void RemoveFromListViewDelegate(ListView list, int index);
        private delegate void ClearListViewDelegate(ListView list);
        public static List<Lekarz> lekarze = new List<Lekarz>();
        List<Badanie> badania = new List<Badanie>();
        Rejestracja rejestracja;

        public Form1()
        {
            AllocConsole();
            InitializeComponent();

            InitOdzialRatunkowy();
        }

        private void InitOdzialRatunkowy()
        {
            Badanie rezonans = new Badanie("rezonans magnetyczny", rezonansObslugiwanyListView, rezonansKolejkaListView);
            badania.Add(rezonans);
            Thread rezonansThread = new Thread(rezonans.Dzialaj);

            Badanie USG = new Badanie("USG", USGObslugiwanyListView, USGKolejkaListView);
            badania.Add(USG);
            Thread USGThread = new Thread(USG.Dzialaj);

            Badanie krew = new Badanie("badanie krwi", krewObslugiwanyListView, krewKolejkaListView);
            badania.Add(krew);
            Thread krewThread = new Thread(krew.Dzialaj);


            Lekarz kardiolog = new Lekarz("Kardiolog", kardiologObslugiwanyListView, kardiologKolejkaListView, badania);
            lekarze.Add(kardiolog);
            Thread kardiologThread = new Thread(kardiolog.Lecz);

            Lekarz psychiatra = new Lekarz("Psychiatra", psychiatraObslugiwanyListView, psychiatraKolejkaListView, badania);
            lekarze.Add(psychiatra);
            Thread psychiatraThread = new Thread(psychiatra.Lecz);

            Lekarz ortopeda = new Lekarz("Ortopeda", ortopedaObslugiwanyListView, ortopedaKolejkaListView, badania);
            lekarze.Add(ortopeda);
            Thread ortopedaThread = new Thread(ortopeda.Lecz);


            rejestracja = new Rejestracja(lekarze, rejestracjaListView);
            Thread rejestracjaThread = new Thread(rejestracja.Obsluguj);

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
            for(int i = 0; i < 10; i++)
            {
                rejestracja.PrzyjmijPacjenta();
            }
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
    }
}
