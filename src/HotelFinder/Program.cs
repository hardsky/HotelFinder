using System;
using System.Linq;
using System.Text;
using GeoLib;
using LINQtoCSV;

namespace HotelFinder
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;
			Console.InputEncoding = Encoding.GetEncoding(1251);

			while (true)
			{
				Console.WriteLine("Введите адрес от которого начинать поиск.");
				var address = Console.ReadLine();
				if (string.IsNullOrEmpty(address))
					return;

				var geocoder = new Geocoder();
				var coords = geocoder.GetCoords(address);
				CsvFileDescription inputFileDescription = new CsvFileDescription
				{
					SeparatorChar = ';',
					FirstLineHasColumnNames = true,
					IgnoreUnknownColumns = true,
					MaximumNbrExceptions = -1,
				};
				CsvContext cc = new CsvContext();
				var hotels = cc.Read<Hotel>("C:/work/HotelFinder/data/Гостиницы_с_координатами_и_категориями.csv",
					inputFileDescription);
				var hotelsWithDistances = hotels.Select(hotel =>
				{
					hotel.Distance = Calculations.GetDistance(coords,
						new Coords {Latitude = hotel.Latitude, Longitude = hotel.Longitude});
					return hotel;
				}).ToArray();

				var selectedHotels = new ParetoHotels(hotelsWithDistances).GetResult();
				Console.WriteLine("Подобранные гостиницы:");
				foreach (var selectedHotel in selectedHotels)
				{
					Console.WriteLine("Название: {0}, Расстояние: {1}, Телефон: {2}, Адрес: {3}, Макс. Цена: {4}, Категория: {5}",
						selectedHotel.Name, selectedHotel.Distance, selectedHotel.Phone, selectedHotel.Address, selectedHotel.MaxPrice,
						selectedHotel.Category);
				}
			}
		}
	}
}
