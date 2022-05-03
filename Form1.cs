using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeomComputTema6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            List<Point> pointsList = new List<Point>();
            Graphics g = e.Graphics;
            Pen redPen = new Pen(Color.Red, 3 );
            Pen bluePen = new Pen(Color.Blue, 2);
            Pen greenPen = new Pen(Color.LimeGreen, 2);
            using(StreamReader sr = new StreamReader(@"..\..\TextFile1.txt"))
            {
                int cornersNr = int.Parse(sr.ReadLine());
                for (int i = 1; i <= cornersNr; i++)
                {
                    string[] coord = sr.ReadLine().Split(' ');
                    Point P = new Point();
                    P.X = int.Parse(coord[0]);
                    P.Y = int.Parse(coord[1]);
                    pointsList.Add(P);
                    g.DrawEllipse(redPen, P.X, P.Y, 2, 2);

                }
            
            for(int i = 0; i < pointsList.Count - 1; i++)
            {
                g.DrawLine(bluePen, pointsList[i], pointsList[i + 1]);

            }
            g.DrawLine(bluePen, pointsList[pointsList.Count - 1], pointsList[0]);

            List<Segment> diagonalsList = new List<Segment>();
            bool broke = false;
            for(int i = 0; i < cornersNr - 3; i++)
            {
                for(int j = i + 2; j < cornersNr - 1; j++)
                {
                        Segment ij = new Segment(pointsList[i], pointsList[j]);
                        if (i == 0 && j == cornersNr - 1)
                            break;
                        if (!IntersectsSides(pointsList, pointsList[i], pointsList[j]) &&
                            !IntersectsDiagonals(diagonalsList, pointsList[i], pointsList[j]) &&
                            IsInside(pointsList, ij))
                        {
                            diagonalsList.Add(ij);
                        }
                        
                        if (diagonalsList.Count == cornersNr - 3)
                        {
                            broke = true;
                            break;
                        }
                }
                    if (broke)
                        break;
            }
            foreach(Segment s in diagonalsList)
                {
                    g.DrawLine(greenPen, s.Start, s.End);
                }
            }
        }
        /// <summary>
        /// Checks if three points turn left or right
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns>0 for collinear, 1 for rigth turn, 2 for left turn</returns>
        private static int Orientation(Point A, Point B, Point C)
        {
            int value = (B.Y - A.Y) * (C.X - B.X) - (C.Y - B.Y) * (B.X - A.X);
            if (value == 0)
            {
                return 0;  //collinear points
            }
            else
            {
                return value > 0 ? 1 : 2;  //clockwise(right) / counterclockwise(left) orientation
            }
        }
        /// <summary>
        /// Checks if two line segments intersect
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="Q1"></param>
        /// <param name="P2"></param>
        /// <param name="Q2"></param>
        /// <returns>true / false</returns>
        private static bool DoIntersect(Point P1, Point Q1, Point P2, Point Q2)
        {
            int o1 = Orientation(P1, Q1, P2);
            int o2 = Orientation(P1, Q1, Q2);
            int o3 = Orientation(P2, Q2, P1);
            int o4 = Orientation(P2, Q2, Q1);

            if (o1 != o2 && o3 != o4)
                return true;
            else return false;
        }
        /// <summary>
        /// Checks if a line segment P1P2 intersects the sides of a polygon(list)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns>true / false</returns>
        private static bool IntersectsSides(List<Point> l, Point P1, Point P2)
        {
            for (int i = 0; i < l.Count - 1; i++)
            {
                if(l[i] != P1 && l[i] != P2 && l[i + 1] != P1 && l[i + 1] != P2)
                {
                    if (DoIntersect(P1, P2, l[i], l[i + 1]))
                        return true;
                        
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a line segment P1P2 intersects other line segments from a list
        /// </summary>
        /// <param name="l"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns>true / false</returns>
        private static bool IntersectsDiagonals(List<Segment> l, Point P1, Point P2)
        {
            foreach(Segment s in l)
            {
                if (P1 != s.Start &&
                    P2 != s.Start&&
                    P1 != s.End &&
                    P2 != s.End &&
                    DoIntersect(P1, P2, s.Start, s.End))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a line segment given by two points on the specified index position is inside a polygon
        /// </summary>
        /// <param name="l"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsInside(List<Point> l, int i, int j)
        {
            int beforei = i - 1;
            if(i == 0)
            {
                beforei = l.Count - 1;
            }
            if (Orientation(l[beforei], l[i], l[i + 1]) == 1 &&
                Orientation(l[i], l[j], l[i + 1]) == 2 &&
                Orientation(l[i], l[beforei], l[j]) == 2)
                return true;
            else if(Orientation(l[beforei], l[i], l[i + 1]) == 2 &&
                (Orientation(l[i], l[j], l[i + 1]) == 2 && Orientation(l[i], l[beforei], l[j]) == 2) ||
                Orientation(l[i], l[j], l[i + 1]) != Orientation(l[i], l[beforei], l[j]))

                 return true;
            return false;
        }
        /// <summary>
        /// checks if a line segment is inside a polygon(list)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="s"></param>
        /// <returns>true / false</returns>
        private static bool IsInside(List<Point> l, Segment s)
        {
            //if segment doesn't intersect sides of the polygon  =>it's enough to check
            //if the middle is inside 
            Point middle = new Point();
            middle.X = (s.End.X + s.Start.X) / 2;
            middle.Y = (s.End.Y + s.Start.Y) / 2;   
            Point extreme = new Point(800, middle.Y);
            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % l.Count;
                if (DoIntersect(l[i], l[next], middle, extreme))
                {
                    count++;
                }
                i = next;
            } while (i != 0);
            return count % 2 == 1;

        }
       

    }
}
