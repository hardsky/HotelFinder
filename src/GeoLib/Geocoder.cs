using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using log4net;
using Newtonsoft.Json.Linq;

namespace GeoLib
{
	public class Geocoder
	{
		readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public Coords GetCoords(string address)
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create(string.Format("http://geocode-maps.yandex.ru/1.x/?format=json&geocode={0}", HttpUtility.UrlEncode(address)));

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream dataStream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(dataStream, Encoding.UTF8))
				{
					var responceObj = JObject.Parse(reader.ReadToEnd());
					var pos = (string)responceObj["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"];
					var posArr = pos.Split(' ');
					return new Coords
					{
						Longitude = double.Parse(posArr[0], CultureInfo.InvariantCulture),
						Latitude = double.Parse(posArr[1], CultureInfo.InvariantCulture),
					};
				}
			}
			catch
			{
				logger.Error(string.Format("Coords error for adress: {0}", address));
			}

			return null;
		}
	}
}
