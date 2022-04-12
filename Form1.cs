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
            }
            for(int i = 0; i < pointsList.Count - 1; i++)
            {
                g.DrawLine(bluePen, pointsList[i], pointsList[i + 1]);

            }
            g.DrawLine(bluePen, pointsList[pointsList.Count - 1], pointsList[0]);
        }
    }
}
