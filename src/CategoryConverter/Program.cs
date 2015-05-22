using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using LINQtoCSV;

[assembly: XmlConfigurator(Watch = true)]
namespace CategoryConverter
{
	public class Hotel
	{
		public string Name { get; set; }

		public string Address { get; set; }

		public string Category { get; set; }

		public double MaxPrice { get; set; }

		public string Phone { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }
	}


	public class CategorySpider
	{
		public Dictionary<string, int> Categories { get; set; }

		public CategorySpider()
		{
			Categories = new Dictionary<string, int>(new CategoryComparer());
		}

		public int ToInt(string category)
		{
			int intCategory;
			if (Categories.TryGetValue(category, out intCategory))
				return intCategory;

			intCategory = Categories.Count + 1;
			Categories[category] = intCategory;

			Program.Log.InfoFormat("{0} = {1},", category, intCategory);

			return intCategory;
		}
	}

	public class CategoryComparer : EqualityComparer<string>
	{
		public override bool Equals(string category1, string category2)
		{
			if (category1 == null || category2 == null)
				return category1 == category2;

			category1 = category1.ToLower().Trim();
			category2 = category2.ToLower().Trim();

			return category1 == category2;
		}

		public override int GetHashCode(string category)
		{
			if (string.IsNullOrEmpty(category))
				return 0;

			return category.GetHashCode();
		}
	}

	class Program
	{
		public static readonly ILog Log = LogManager.GetLogger("CategoryConverter");

		static void Main(string[] args)
		{
			CsvFileDescription inputFileDescription = new CsvFileDescription
			{
				SeparatorChar = ';',
				FirstLineHasColumnNames = true,
				IgnoreUnknownColumns = true,
				MaximumNbrExceptions = -1,
			};
			CsvContext cc = new CsvContext();
			var hotels = cc.Read<Hotel>("C:/work/HotelFinder/data/Гостиницы_с_координатами.csv", inputFileDescription);
			var categorySpider = new CategorySpider();
			var hotelsWithCategories = from h in hotels
								   select
									   new
									   {
										   h.Name,
										   h.Address,
										   h.Category,
										   CategoryInt = categorySpider.ToInt(h.Category),
										   h.MaxPrice,
										   h.Phone,
										   h.Longitude,
										   h.Latitude
									   };

			Log.Info("Start conversion.");

			CsvFileDescription outputFileDescription = new CsvFileDescription
			{
				SeparatorChar = ';',
				FirstLineHasColumnNames = true,
				TextEncoding = Encoding.UTF8,
				FileCultureInfo = CultureInfo.InvariantCulture
			};
			cc.Write(hotelsWithCategories, "C:/work/HotelFinder/data/Гостиницы_с_координатами_и_категориями.csv", outputFileDescription);

			Log.Info("Complete conversion.");

		}
	}
}
