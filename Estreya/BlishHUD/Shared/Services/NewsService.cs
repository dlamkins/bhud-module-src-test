using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Estreya.BlishHUD.Shared.Models;
using Flurl.Http;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Services
{
	public class NewsService : ManagedService
	{
		private const string FILE_NAME = "news.json";

		private readonly string _baseFilePath;

		private readonly IFlurlClient _flurlClient;

		public List<News> News { get; private set; }

		public NewsService(ServiceConfiguration configuration, IFlurlClient flurlClient, string baseFilePath)
			: base(configuration)
		{
			_flurlClient = flurlClient;
			_baseFilePath = baseFilePath;
		}

		protected override Task Initialize()
		{
			News = new List<News>();
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			News?.Clear();
			News = null;
		}

		protected override Task Clear()
		{
			News?.Clear();
			return Task.CompletedTask;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override async Task Load()
		{
			try
			{
				List<News> newsList = JsonConvert.DeserializeObject<List<News>>(await _flurlClient.Request(_baseFilePath, "news.json").GetStringAsync(default(CancellationToken), (HttpCompletionOption)0));
				News.AddRange(newsList);
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to load news:");
			}
		}
	}
}
