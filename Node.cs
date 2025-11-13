using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublePendulum
{
    internal class Node : MyPoint
    {
        private int Transparency = 255;
        private int Size;
        public Node(int x, int y, Color colour, int size) : base(x, y, colour)
        {

            this.Size = (int)Math.Ceiling((decimal)size / 2) * 2;
        }
        public int GetTransparency()
        {
            this.Transparency -= 1;
            return this.Transparency;
        }

        public override int GetSize()
        {
            return this.Size;
        }
        public void SetTransparency(int newTransparency)
        {
            this.Transparency = newTransparency;
        }
    }
}
