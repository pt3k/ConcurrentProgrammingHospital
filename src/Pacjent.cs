using System;
using System.Collections.Generic;
using System.Text;

namespace Szpital
{
    public class Pacjent
    {
        public bool ciezkiPrzypadek;
        public Choroba choroba;
        string name;
        int nrPacjenta;

        public Pacjent(int nr, Choroba ch)
        {
            nrPacjenta = nr;
            choroba = ch;

            if((new Random()).NextDouble() < 0.2)
            {
                ciezkiPrzypadek = true;
                name = "#" + nrPacjenta + " CIEZKI PRZYPADEK";
            }
            else
            {
                ciezkiPrzypadek = false;
                name = "#" + nrPacjenta;
            }
        }

        public int getNumer()
        {
            return nrPacjenta;
        }

        public string getName()
        {
            return name;
        }

        public Choroba getChoroba()
        {
            return choroba;
        }
    }
}
