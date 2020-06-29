using System;
using System.Collections.Generic;
using System.Text;

namespace Szpital
{
    public class Choroba
    {
        public string nazwa;
        public string typ;

        public Choroba(string nazwa, string typ)
        {
            this.nazwa = nazwa;
            this.typ = typ;
        }
    }
}
