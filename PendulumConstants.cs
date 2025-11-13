using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublePendulum
{
    internal class PendulumConstants
    {
        double Acceleration = 0;
        int M1 = 0;
        int M2 = 0;
        int L1 = 0;
        int L2 = 0;
        int tickInterval = 0;
        Color rootColour = Color.Black;
        Color middleColour = Color.Black;
        Color tailColour = Color.Black;
        Color lineColour = Color.Red;
        Color TrailColour;

        public PendulumConstants(double acceleration, int m1, int m2, int l1, int l2, Color RootColour, Color MiddleColour, Color TailColour, Color LineColour, Color TrailColour, int TickInterval)
        {
            this.Acceleration = acceleration * 1.5;
            this.L1 = l1;
            this.L2 = l2;
            this.M1 = m1;
            this.M2 = m2;
            this.rootColour = RootColour;
            this.middleColour = MiddleColour;
            this.tailColour = TailColour;
            this.lineColour = LineColour;
            this.tickInterval = TickInterval;
            this.TrailColour = TrailColour;
        }
        public double GetAcceleration()
        {
            return this.Acceleration;
        }
        public int GetM1()
        {
            return this.M1;
        }
        public int GetM2()
        {
            return this.M2;
        }
        public int GetL1()
        {
            return this.L1;
        }
        public int GetL2()
        {
            return this.L2;
        }
        public Color[] GetColours()
        {
            Color[] colourlist = { this.rootColour, this.middleColour, this.tailColour, this.lineColour, this.TrailColour };
            return colourlist;
        }
        public void ChangeTrailColour(Color Trail)
        {
            TrailColour = Trail;
        }
        public double Getdt()
        {
            return (double)tickInterval / 1000;
        }
        public void setTrailColour(Color colour)
        {
            TrailColour = colour;
        }
    }
}
