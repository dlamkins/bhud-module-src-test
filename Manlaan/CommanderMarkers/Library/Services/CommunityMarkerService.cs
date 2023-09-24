using System;
using System.Net;
using Manlaan.CommanderMarkers.Library.Models;
using Newtonsoft.Json;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public class CommunityMarkerService
	{
		protected const string FILE_URL = "https://bhm.blishhud.com/Manlaan.CommanderMarkers/Community/Markers.json";

		protected CommunitySets? _communitySets;

		public CommunitySets CommunitySets
		{
			get
			{
				if (_communitySets == null)
				{
					return FetchListing();
				}
				return _communitySets;
			}
		}

		public event EventHandler<CommunitySets>? CommunitySetsUpdated;

		public CommunitySets FetchListing()
		{
			try
			{
				using WebClient webClient = new WebClient();
				CommunitySets sets = JsonConvert.DeserializeObject<CommunitySets>(webClient.DownloadString("https://bhm.blishhud.com/Manlaan.CommanderMarkers/Community/Markers.json"));
				if (sets == null)
				{
					return new CommunitySets();
				}
				_communitySets = sets;
				this.CommunitySetsUpdated?.Invoke(this, _communitySets);
				return _communitySets;
			}
			catch (Exception)
			{
				return new CommunitySets();
			}
		}
	}
}
