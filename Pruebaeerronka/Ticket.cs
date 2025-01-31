using System;
using TiketBAI;

namespace TiketBAI
{
    public class Tiketa
    {
        public string Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Time { get; set; }
        public Produktu Produktu { get; set; }

        public Tiketa(string id, DateOnly data, TimeOnly time, Produktu produktu)
        {
            Id = id;
            Data = data;
            Time = time;
            Produktu = produktu;
        }
    }
}