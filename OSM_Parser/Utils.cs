using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace OSM_Parser
{
	struct vec3
	{
		public double x, y, z;

		public vec3(double nx, double ny, double nz)
		{
			x = nx;
			y = ny;
			z = nz;
		}
		public vec3(float nx, float ny, float nz)
		{
			x = nx;
			y = ny;
			z = nz;
		}

		public static bool operator ==(vec3 a, vec3 b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}
		public static bool operator !=(vec3 a, vec3 b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}
		/*public override bool Equals(object o)
		{
			vec3 a = (vec3)o;
			return a.x == x && a.y == y && a.z == z;
		}*/
		public static vec3 operator *(vec3 v, double a)
		{
			return new vec3(v.x * a, v.y * a, v.z * a);
		}
		public static vec3 operator +(vec3 a, vec3 b)
		{
			return new vec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static vec3 operator -(vec3 a, vec3 b)
		{
			return new vec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static double Dot(vec3 a, vec3 b)
		{
			return a.x*b.x + a.y*b.y + a.z*b.z;
		}
		public vec3 Normalize()
		{
			double l = Math.Sqrt(x * x + y * y + z * z);
			this *= (1 / l);
			return this;
		}
		public vec3 Snap()
		{
			return new vec3(((int)(x*30))/30, ((int)(y*30)) / 30, ((int)(z*30)) / 30);
		}
		public override string ToString()
		{
			return x.ToString(NumberFormatInfo.InvariantInfo) + " " + y.ToString(NumberFormatInfo.InvariantInfo) + " " + z.ToString(NumberFormatInfo.InvariantInfo);
		}
	}

	struct Rect2D
	{
		public Rect2D(double nx, double ny, double nw, double nh)
		{
			x = nx;
			y = ny;
			w = nw;
			h = nh;
		}

		public bool Contains(Rect2D a)
		{
			if (x > a.x + a.w)
				return false;
			if (x + w < a.x)
				return false;
			if (y > a.y + a.h)
				return false;
			if (y + h < a.y)
				return false;

			return true;
		}

		public double x, y, w, h;
	}

	class Utils
	{
		public static vec3 LonLatToXYZ(double lon, double lat)
		{
			double R = 6371000;//m

			double sinLon = Math.Sin((lon / 180) * Math.PI);
			double cosLon = Math.Cos((lon / 180) * Math.PI);
			double sinLat = Math.Sin((lat / 180) * Math.PI);
			double cosLat = Math.Cos((lat / 180) * Math.PI);

			double tx = (cosLat * sinLon);
			double ty = sinLat;
			double tz = (cosLat * cosLon);


			/*x = lon / 180 * R;
			y = 0;
			z = -lat / 180 * R;*/

			return new vec3(tx, ty, tz) * R;
		}

		public static void RotateVec2(double a, ref double x, ref double y)
		{
			double sin = Math.Sin((a / 180) * Math.PI);
			double cos = Math.Cos((a / 180) * Math.PI);

			double nx = (x * cos - y * sin);
			double ny = (x * sin + y * cos);

			x = nx;
			y = ny;
		}

		public static double Area(vec3 a, vec3 b, vec3 c)
		{
			return (a.x * (b.z - c.z) + b.x * (c.z - a.z) + c.x * (a.z - b.z));
		}

		public static bool IsCollinear(vec3 a, vec3 b, vec3 c)
		{
			return Math.Abs(Area(a, b, c)) <= 0.000001;
		}

		public static bool Reflex(vec3 p, vec3 o, vec3 n)
		{
			//collinear check
			if (IsCollinear(p, o, n))
				return false;

			return (Area(p, o, n) < 0);//right
		}

		//From Mark Bayazit's convex decomposition algorithm
		public static vec3 LineIntersect(vec3 p1, vec3 p2, vec3 q1, vec3 q2)
		{
			vec3 i = new vec3(0, 0, 0);
			double a1 = p2.z - p1.z;
			double b1 = p1.x - p2.x;
			double c1 = a1 * p1.x + b1 * p1.z;
			double a2 = q2.z - q1.z;
			double b2 = q1.x - q2.x;
			double c2 = a2 * q1.x + b2 * q1.z;
			double det = a1 * b2 - a2 * b1;

			if (Math.Abs(det) > 0.0001)
			{
				// lines are not parallel
				i.x = (b2 * c1 - b1 * c2) / det;
				i.z = (a1 * c2 - a2 * c1) / det;
			}
			return i;
		}

		public static bool LineIntersect(ref vec3 point1, ref vec3 point2, ref vec3 point3, ref vec3 point4, bool firstIsSegment, bool secondIsSegment, out vec3 point)
		{
			point = new vec3();

			// these are reused later.
			// each lettered sub-calculation is used twice, except
			// for b and d, which are used 3 times
			double a = point4.z - point3.z;
			double b = point2.x - point1.x;
			double c = point4.x - point3.x;
			double d = point2.z - point1.z;

			// denominator to solution of linear system
			double denom = (a * b) - (c * d);

			// if denominator is 0, then lines are parallel
			if (!(denom >= -0.00001 && denom <= 0.00001))
			{
				double e = point1.z - point3.z;
				double f = point1.x - point3.x;
				double oneOverDenom = 1.0 / denom;

				// numerator of first equation
				double ua = (c * e) - (a * f);
				ua *= oneOverDenom;

				// check if intersection point of the two lines is on line segment 1
				if (!firstIsSegment || ua >= 0.0 && ua <= 1.0)
				{
					// numerator of second equation
					double ub = (b * e) - (d * f);
					ub *= oneOverDenom;

					// check if intersection point of the two lines is on line segment 2
					// means the line segments intersect, since we know it is on
					// segment 1 as well.
					if (!secondIsSegment || ub >= 0.0 && ub <= 1.0)
					{
						// check if they are coincident (no collision in this case)
						if (ua != 0f || ub != 0)
						{
							//There is an intersection
							point.x = point1.x + ua * b;
							point.z = point1.z + ua * d;
							return true;
						}
					}
				}
			}

			return false;
		}

		public static double SqDist(vec3 a, vec3 b)
		{
			double dx = b.x - a.x;
			double dy = b.z - a.z;
			return dx * dx + dy * dy;
		}

		public static bool CanSee(int i, int j, Polygon p)
		{
			vec3 prev = p.At(i - 1);
			vec3 on = p.At(i);
			vec3 next = p.At(i + 1);

			vec3 prevj = p.At(j - 1);
			vec3 onj = p.At(j);
			vec3 nextj = p.At(j + 1);

			if (Reflex(prev, on, next))
			{
				if ((Area(on, prev, onj) >= 0) && (Area(on, next, onj) <= 0))//LeftOn RightOn
					return false;
			}
			else
			{
				if ((Area(on, next, onj) <= 0) || (Area(on, prev, onj) >= 0))//RightOn LeftOn
					return false;
			}

			if (Reflex(prevj, onj, nextj))
			{
				if ((Area(onj, prevj, on) >= 0) && (Area(onj, nextj, on) <= 0))//LeftOn RightOn
					return false;
			}
			else
			{
				if ((Area(onj, nextj, on) <= 0) || (Area(onj, prevj, on) >= 0))//RightOn LeftOn
					return false;
			}

			for (int k = 0; k < p.verts.Length; k++)
			{
				vec3 intPoint = new vec3();
				vec3 p1 = p.At(i);
				vec3 p2 = p.At(j);
				vec3 q1 = p.At(k);
				vec3 q2 = p.At(k + 1);

				if (p1 == q1 || p1 == q2 || p2 == q1 || p2 == q2)
					continue;

				bool result = LineIntersect(ref p1, ref p2, ref q1, ref q2, true, true, out intPoint);

				if (result)
				{
					if ((intPoint != q1) || intPoint != q2)
						return false;
				}
			}

			return true;
		}
	}
}
