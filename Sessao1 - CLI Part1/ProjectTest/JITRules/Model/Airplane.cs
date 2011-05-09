
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Airplane
    {
        private int p;
        private string p_2;
        private int p_3;
        private int p_4;
        private int p_5;

        public int Seats { get; set; }
        public string Name { get; set; }
        public Tuple<int,int,int> Position { get; set; }

        public Airplane(int seats, string name, int x, int y, int z)
        {
            this.Seats = seats;
            this.Name = name;
            this.Position = new Tuple<int, int, int>(x, y, z);
        }


        public int Altitude()
        {
            return this.Position.Item3;
        }

        public string Flight(string destination)
        {
            return "Goa";
        }
    }
}
