using System.Collections.Generic;
using System.Linq;

namespace HotelFinder
{
	public class ParetoHotels
	{
		private Hotel[] _hotels;

		public ParetoHotels(Hotel[] hotels)
		{
			_hotels = hotels;
		}

		public Hotel[] GetResult()
		{
			var result = new List<Hotel>();

			result.Add(_hotels.First());

			foreach (var currentHotel in _hotels)
			{
				var removeIdxes = new List<int>();
				bool needToAdd = false;
				for (int i = 0; i < result.Count; i++)
				{
					var selectedHotel = result[i];

					if(selectedHotel == currentHotel)
						continue;

					var compareResults = new[]
					{
						currentHotel.CompareDistance(selectedHotel),
						currentHotel.CompareMaxPrice(selectedHotel),
						currentHotel.CompareCategory(selectedHotel)
					};


					if (compareResults.All(t => t != Criterion.Worse) && compareResults.Any(t => t == Criterion.Better))
					{
						removeIdxes.Add(i);
					}

					if (compareResults.All(t => t != Criterion.Worse))
					{
						needToAdd = true;
					}
				}

				foreach (var removeIdx in removeIdxes)
				{
					result.RemoveAt(removeIdx);
				}

				if(needToAdd)
					result.Add(currentHotel);
			}

			return result.ToArray();
		}
	}
}
