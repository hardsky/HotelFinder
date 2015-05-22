using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using GeoLib;
using log4net;
using log4net.Config;
using LINQtoCSV;

namespace GeoConverter
{
	public class Hotel
	{
		[CsvColumn(Name = "Название гостиницы")]
		public string Name { get; set; }

		[CsvColumn(Name = "Адрес")]
		public string Address { get; set; }

		[CsvColumn(Name = "Категория гостиницы")]
		public string Category { get; set; }

		[CsvColumn(Name = "Максимальная стоимость")]
		public string MaxPrice { get; set; }

		[CsvColumn(Name = "Телефон")]
		public string Phone { get; set; }

		public override string ToString()
		{
			return string.Format("{{{0}, {1}, {2}, {3}, {4}}}", Name, Address, Category, MaxPrice, Phone);
		}

		public static double ParseDouble(string number)
		{
			double result;
			if (double.TryParse(number, out result))
				return result;

			return 0;
		}
	}


	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.Unicode;

			BasicConfigurator.Configure();

			ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

			CsvFileDescription inputFileDescription = new CsvFileDescription
			{
				SeparatorChar = ';',
				FirstLineHasColumnNames = true,
				IgnoreUnknownColumns = true,
				MaximumNbrExceptions = -1,
			};
			CsvContext cc = new CsvContext();
			var hotels = cc.Read<Hotel>("C:/work/HotelFinder/data/Гостиницы.csv", inputFileDescription);
			var geocoder = new Geocoder();
			var hotelsWithCoords = from h in hotels
								   let coords = geocoder.GetCoords(h.Address)
								   where coords != null
								   select
									   new
									   {
										   h.Name,
										   h.Address,
										   h.Category,
										   MaxPrice = Hotel.ParseDouble(h.MaxPrice),
										   h.Phone,
										   coords.Longitude,
										   coords.Latitude
									   };

			logger.Info("Start conversion.");

			CsvFileDescription outputFileDescription = new CsvFileDescription
			{
				SeparatorChar = ';',
				FirstLineHasColumnNames = true,
				TextEncoding = Encoding.UTF8,
				FileCultureInfo = CultureInfo.InvariantCulture
			};
			cc.Write(hotelsWithCoords, "C:/work/HotelFinder/data/Гостиницы_с_координатами.csv", outputFileDescription);

			logger.Info("Complete conversion.");
		}
	}
}
