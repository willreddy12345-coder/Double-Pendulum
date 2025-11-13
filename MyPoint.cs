using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublePendulum
{
    internal class MyPoint
    {
        protected int X, Y;
        private int size = 1;
        protected Color Colour;
        public MyPoint(int x, int y, Color colour)
        {
            this.X = x;
            this.Y = y;
            this.Colour = colour;
        }
        public int GetX()
        {
            return this.X;
        }
        public int GetY()
        {
            return this.Y;
        }
        public void SetX(int newX)
        {
            this.X = newX;
        }
        public void SetY(int newY)
        {
            this.Y = newY;
        }
        public virtual int GetSize()
        {
            return this.size;
        }

    }
}

