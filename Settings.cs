using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublePendulum
{
    internal class Settings
    {
        private double Length1;
        private double Length2;
        private double Angle1;
        private double Angle2;
        private bool multi;
        private int pendulumNumber;

        public double mass1;
        public double mass2;
        public double gravitationalConstant;




        public Settings(double length1, double length2, double angle1, double angle2, bool multi, int pendulumNumber, double mass1, double mass2, double gravitationalConstant)
        {
            Length1 = length1;
            Length2 = length2;
            Angle1 = angle1;
            Angle2 = angle2;
            this.multi = multi;
            this.pendulumNumber = pendulumNumber;
            this.mass1 = mass1;
            this.mass2 = mass2;
            this.gravitationalConstant = gravitationalConstant;
        }

        public double GetLength1()
        {
            return this.Length1;
        }

        public double GetLength2()
        {
            return this.Length2;
        }
        public double GetAngle1()
        {
            return this.Angle1;
        }

        public double GetAngle2()
        {
            return this.Angle2;

        }
        public bool GetMulti()
        {
            return this.multi;
        }
        public int getPenNum()
        {
            return this.pendulumNumber;
        }
        public double GetMass1()
        {
            return this.mass1;
        }
        public double GetMass2()
        {
            return this.mass2;
        }
        public double GetGravitationalConstant()
        {
            return this.gravitationalConstant;
        }


    }
}
