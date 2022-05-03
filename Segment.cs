using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeomComputTema6
{
    public class Segment
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public Segment(Point start, Point end)
        {
            Start = start;
            End = end;
        }
       

    }
}
