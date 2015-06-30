using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PointFHelp
{
    public class PointF
    {
        float _x, _y;

        public float X
        {
            get
            { return _x; }
            set
            { _x = value; }
        }
        public float Y
        {
            get
            { return _y; }
            set
            { _y = value; }
        }

        public PointF(float x, float y)
        {
            _x = x;
            _y = y;
        }

        #region operators
        public static PointF operator +(PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }
        public static PointF operator -(PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }
        public static PointF operator *(PointF a, float b)
        {
            return new PointF(a.X * b, a.Y * b);
        }
        public static PointF operator /(PointF a, float b)
        {
            return new PointF(a.X / b, a.Y / b);
        }
        public static implicit operator PointF(System.Drawing.PointF p)
        {
            return new PointF(p.X, p.Y);
        }
        public static implicit operator System.Drawing.PointF(PointF p)
        {
            return new System.Drawing.PointF(p.X, p.Y);
        }
        public static implicit operator PointF(Point p)
        {
            return new PointF(p.X, p.Y);
        }
        public static implicit operator System.Drawing.Point(PointF p)
        {
            return new System.Drawing.Point((int)p.X, (int)p.Y);
        }
        #endregion
    }
}
