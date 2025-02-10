//garapen aldaketa

using System;
using TiketBAI;

namespace TiketBAI
{
    public class Produktu
    {
        public string Name { get; set; }
        public Saltzaile Saltzaile { get; set; }
        public double Preziokiloko { get; set; }
        public double Pisua { get; set; }
        public double Totala { get; set; }
        public string Department { get; set; }

        public Produktu(string name, Saltzaile saltzaile, double preziokiloko, double pisua, double totala, string department)
        {
            Name = name;
            Saltzaile = saltzaile;
            Preziokiloko = preziokiloko;
            Pisua = pisua;
            Totala = totala;
            Department = department;
        }
    }
}