using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublePendulum
{
    internal class Pendulum
    {
        //define class variables
        Node Root, Middle, Tail;
        Bitmap bitmap;
        Graphics graphics;
        SolidBrush rootBrush;
        SolidBrush middleBrush;
        SolidBrush tailBrush;
        Pen linePen;
        double L1, L2;
        double angularVelocity1;
        double angularVelocity2;
        PendulumConstants constants;
        double angularAcceleration1;
        double angularAcceleration2;
        double angle1;
        double angle2;
        MyQueue trailQueue = new MyQueue();
        Color trailColor;
        Brush trailBrush;
        Color backColor;
        public Pendulum(int width, int height, int size, PendulumConstants p, double angle1, double angle2, Color backColour)
        {
            // Initialize class variables with provided values and create necessary objects.
            constants = p;
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            Root = new Node(width / 2, height / 2, constants.GetColours()[0], size);
            this.L1 = (double)constants.GetL1();
            this.L2 = (double)constants.GetL2();
            this.angle1 = angle1;
            this.angle2 = angle2;
            int[] coords = calculateNewCoords(angle1, angle2);
            Middle = new Node(coords[0], coords[1], constants.GetColours()[1], size);
            Tail = new Node(coords[2], coords[3], constants.GetColours()[2], size);
            rootBrush = new SolidBrush(constants.GetColours()[0]);
            middleBrush = new SolidBrush(constants.GetColours()[1]);
            tailBrush = new SolidBrush(constants.GetColours()[2]);
            trailColor = constants.GetColours()[4];
            linePen = new Pen(constants.GetColours()[3], 4);
            this.backColor = backColour;

        }

        public int[] calculateNewCoords(double angle1, double angle2)
        {
            // Calculate the coordinates of pendulum nodes based on angles and lengths.
            int x1 = (int)(Root.GetX() + this.L1 * Math.Sin(angle1));
            int y1 = (int)(Root.GetY() + this.L1 * Math.Cos(angle1));
            int x2 = (int)(x1 + this.L2 * Math.Sin(angle2));
            int y2 = (int)(y1 + this.L2 * Math.Cos(angle2));
            int[] coords = { x1, y1, x2, y2 };
            return coords;
        }

        public Bitmap GetImage(bool showTrailQueue, bool showTrailFade, bool first, bool showPendulums, Image image)
        {
            // Generate an image of the pendulum's current state.
            if (first)
            {
                graphics.Clear(backColor);
            }
            else
            {
                bitmap = (Bitmap)image;
            }
            graphics = Graphics.FromImage(bitmap);
            int size = Root.GetSize();
            int halfSize = size / 2;
            int x0 = (int)(Root.GetX());
            int y0 = (int)(Root.GetY());
            int x1 = (int)(Middle.GetX());
            int y1 = (int)(Middle.GetY());
            int x2 = (int)(Tail.GetX());
            int y2 = (int)(Tail.GetY());


            //creates the trail 
            if (showTrailQueue == true)
            {
                Node newValue = new Node(Tail.GetX(), Tail.GetY(), Color.Blue, 2);
                trailQueue.enqueue(newValue);
                int queueSize = trailQueue.GetSize();
                for (int i = 0; i < queueSize; i++)
                {
                    Node current = trailQueue.dequeue();
                    int transparency = current.GetTransparency();
                    if (transparency > 0 || showTrailFade == false)
                    {
                        if (showTrailFade == false)
                        {
                            transparency = 255;
                        }
                        trailBrush = new SolidBrush(Color.FromArgb(transparency, trailColor.R, trailColor.G, trailColor.B));
                        graphics.FillEllipse(trailBrush, current.GetX() - 2, current.GetY() - 2, 4, 4);
                        trailQueue.enqueue(current);
                    }
                }
            }


            else
            {
                if (trailQueue.checkEmpty() == false)
                {
                    trailQueue.clear();
                }
            }
            if (showPendulums)
            {
                rootBrush = new SolidBrush(constants.GetColours()[0]);
                graphics.DrawLine(linePen, x0, y0, x1, y1);
                graphics.DrawLine(linePen, x1, y1, x2, y2);

                graphics.FillEllipse(rootBrush, x0 - halfSize, y0 - halfSize, size, size);
                graphics.FillEllipse(middleBrush, x1 - halfSize, y1 - halfSize, size, size);
                graphics.FillEllipse(tailBrush, x2 - halfSize, y2 - halfSize, size, size);

            }

            return bitmap;
        }

        public void calculateNewAngle()
        {
            // Calculate new angles and angular velocities for the pendulum.
            double term1 = (-constants.GetAcceleration() * (2 * constants.GetM1() + constants.GetM2()) * Math.Sin(angle1)); // Gravitational torque acting on the pendulum due to m1 and m2, proportional to sine of angle1
            double term2 = constants.GetM2() * constants.GetAcceleration() * Math.Sin(angle1 - 2 * angle2); // Torque caused by m2 at a distance of L2 away from the pivot, dependent on the difference in angles between angle1 and angle2
            double term3 = (Math.Sin(angle1 - angle2) * constants.GetM2() * (angularVelocity2 * angularVelocity2 * constants.GetL2() + angularVelocity1 * angularVelocity1 * constants.GetL1() * Math.Cos(angle1 - angle2))); // Torque caused by angularVelocity1 and angularVelocity2, related to the kinetic energy of the pendulum
            double term4 = (constants.GetL1() * (2 * constants.GetM1() + constants.GetM2() - constants.GetM2() * Math.Cos(2 * angle1 - 2 * angle2))); // Moment of inertia of the pendulum
            angularAcceleration1 = ((term1 - term2 - 2 * term3) / term4);

            double term5 = (angularVelocity1 * angularVelocity1 * constants.GetL1() * (constants.GetM1() + constants.GetM2())); // Torque caused by the relative motion of m1 and m2

            double term6 = constants.GetAcceleration() * (constants.GetM1() + constants.GetM2()) * Math.Cos(angle1); // Gravitational torque acting on the pendulum due to m1 and m2, proportional to cosine of angle1
            double term7 = angularVelocity2 * angularVelocity2 * constants.GetL2() * constants.GetM2() * Math.Cos(angle1 - angle2); // Torque caused by m2 at a distance L2 away from the pivot, dependent on the difference between angle1 and angle2
            double term8 = constants.GetL2() * (2 * constants.GetM1() + constants.GetM2() - constants.GetM2() * Math.Cos(2 * angle1 - 2 * angle2)); // Moment of inertia of the pendulum arm connected to m2

            angularAcceleration2 = ((2 * Math.Sin(angle1 - angle2) * (term5 + term6 + term7)) / term8); // All proportional to sine of angle1-angle2
            angularVelocity1 += (angularAcceleration1 * constants.Getdt());
            angularVelocity2 += (angularAcceleration2 * constants.Getdt());
            angle1 += (angularVelocity1 * constants.Getdt());
            angle2 += (angularVelocity2 * constants.Getdt());
        }

        public void SetNewCoords()
        {
            // Update the coordinates of pendulum nodes based on current angles.
            int[] coords = calculateNewCoords(angle1, angle2);
            Middle.SetX(coords[0]);
            Middle.SetY(coords[1]);
            Tail.SetX(coords[2]);
            Tail.SetY(coords[3]);
        }

        public Node GetTail()
        {
            return this.Tail;
        }

        public Node GetMiddle()
        {
            return this.Middle;
        }

        public Node GetRoot()
        {
            return this.Root;
        }

        public double GetL1()
        {
            return this.L1;
        }

        public double GetL2()
        {
            return this.L2;
        }

        public void SetTail(Node tail)
        {
            this.Tail = (tail);
        }

        public void SetMiddle(Node middle)
        {
            this.Middle = (middle);
        }

        public double GetAngV1()
        {
            return this.angularVelocity1;
        }

        public double GetAngV2()
        {
            return this.angularVelocity2;
        }

        public double getAngle2()
        {
            return this.angle2;
        }
        public void Setangle1(double angle1)
        {
            this.angle1 = (angle1);
        }
        public void Setangle2(double angle2)
        {
            this.angle2 = (angle2);
        }
        public void setTrailColor(Color trailColor)
        {
            this.trailColor = trailColor;
        }
        public void setBackColour(Color backColor)
        {
            this.backColor = backColor;
        }
    }
}
