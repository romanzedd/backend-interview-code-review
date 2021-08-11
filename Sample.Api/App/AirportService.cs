using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Sample.Api.Model;

namespace Sample.Api.App
{
	public class AirportService : HttpClient
	{
		public string ApiHost;
		public AirportService(string apiHost)
		{
			ApiHost = apiHost;
		}

		public List<AirportPair> CalculateDistance(List<Airport> airports)
		{
			List<AirportPair> pairs = new List<AirportPair>(); // No need for implicit declaration for pairs and queue, use var
			Queue<Airport> queue = new Queue<Airport>(airports); // Variables names should make more sense, could've named them airportPairs and airportsQueue f.e.
			Airport currentPort;
			// Let's be consistend with curly brackets and have opening bracket on a new line.
			while (queue.TryDequeue(out currentPort)) {
				foreach (Airport airport in queue) {
					if (!currentPort.City.Equals(airport.City)) {
						var distanceHelper = new DistanceCalculator();
						pairs.Add(new AirportPair() {
							First = currentPort,
							Second = airport,
							Distance = distanceHelper.DistanceBetweenPlaces(currentPort, airport)
						});
					}
				}
			}

			return pairs;
		}

		public async Task<List<Airport>> GetAirports(string[] cities)
		{
			cities = cities.Select(a => a.ToLower()).Distinct().ToArray();
			List<Airport> ports = new List<Airport>();

			foreach (string city in cities) {
				var cityPorts = await GetAirport(city);
				ports.AddRange(cityPorts.Where(port => port.City.ToLower().Equals(city)));
			}
			return ports;
		}

// If you're not using this commented code, it is better to remove it.
//		public async Task<Airport[]> GetAirports(string[] cities)
//		{
//			string url = "Airport/search";
//			Uri requestUri = new Uri(ApiHost + $"/Airport/search?pattern={string.Join(",", cities)}");
//			try {
//				var response = await GetAsync(requestUri);
//
//				Airport[] result = null;
//				if (response.IsSuccessStatusCode) {
//					result = await response.Content.ReadAsAsync<Airport[]>();
//					return result;
//				} else {
//					return null;
//				}
//			} catch (Exception ex) {
//				System.Diagnostics.Trace.TraceError($"Something happen during web service call: {ex.Message}");
//				throw ex;
//			}
//
//			return null;
//		}

		public async Task<Airport[]> GetAirport(string city)
		{
			string url = "Airport/search";
			Uri requestUri = new Uri(ApiHost + $"/Airport/search?pattern={city}");
			// Did you mean to use url variable here?
			// var url = "/Airport/search";
			// var requestUri = new Uri(ApiHost + url + $"?pattern={city}");
			try
			{
				var response = await GetAsync(requestUri);

				Airport[] result = null;
				if (response.IsSuccessStatusCode) {
					result = await response.Content.ReadAsAsync<Airport[]>();
					return result;
				} else {
					return null;
				}
			} catch (Exception ex) {
				System.Diagnostics.Trace.TraceError($"Something happen during web service call: {ex.Message}");
				throw ex;
			}

			// This part of the code will never be reached, consider deleting it.
			return null;
		}

	}
}