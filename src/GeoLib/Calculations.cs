using System;

namespace GeoLib
{
	public static class Calculations
	{
		/// <summary>
		/// Расчитывает расстояние в метрах между 2 географическими точками
		/// </summary>
		/// <param name="p1">в градусах</param>
		/// <param name="p2">в градусах</param>
		/// <returns></returns>
		public static double GetDistance(Coords p1, Coords p2)
		{
			double f = p2.Latitude - p1.Latitude;
			double ldt = p2.Longitude - p1.Longitude;


			double k1 = 111.13209 - 0.56605*Math.Cos(2*f) + 0.0012*Math.Cos(4*f);
			double k2 = 111.41513*Math.Cos(f) - 0.09455*Math.Cos(3*f) + 0.00012*Math.Cos(5*f);

			return Math.Sqrt(Math.Pow(k1*f, 2) + Math.Pow(k2*ldt, 2)) * 1000;
		}
	}
}
