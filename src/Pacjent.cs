using System;
using System.Collections.Generic;
using System.Text;

namespace Szpital
{
    public class Pacjent
    {
        public bool ciezkiPrzypadek;
        Choroba choroba;
        string name;
        int nrPacjenta;

        public Pacjent(int nr, Choroba ch, bool ciezkiPrzypadek)
        {
            this.ciezkiPrzypadek = ciezkiPrzypadek;
            nrPacjenta = nr;
            choroba = ch;

            if(ciezkiPrzypadek)
            {
                name = "#" + nrPacjenta + " CIEZKI PRZYPADEK";
            }
            else
            {
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
