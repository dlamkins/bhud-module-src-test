using System;
using System.IO;
using System.Threading.Tasks;
using BhModule.Community.Pathing.State.UserResources;
using BhModule.Community.Pathing.State.UserResources.Population.Converters;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BhModule.Community.Pathing.State
{
	public class UserResourceStates : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<UserResourceStates>();

		public PopulationDefaults Population { get; set; }

		public IgnoreDefaults Ignore { get; set; }

		public StaticValues Static { get; set; }

		public UserResourceStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		protected override async Task<bool> Initialize()
		{
			await ExportAllDefault();
			await LoadAllStates();
			return true;
		}

		private async Task ExportDefaultState<T>(string statePath, ISerializer yamlSerializer, T defaultExport)
		{
			if (File.Exists(statePath))
			{
				return;
			}
			try
			{
				using StreamWriter stateWriter = File.CreateText(statePath);
				await stateWriter.WriteAsync(yamlSerializer.Serialize(defaultExport));
			}
			catch (Exception e)
			{
				Logger.Error(e, "Failed to write state defaults to " + statePath);
			}
		}

		private async Task ExportAllDefault()
		{
			string userResourceDir = DataDirUtil.GetSafeDataDir("user");
			ISerializer yamlSerializer = new SerializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).WithTypeConverter(new ColorConverter()).Build();
			await ExportDefaultState(Path.Combine(userResourceDir, "populate.yaml"), yamlSerializer, new PopulationDefaults());
			await ExportDefaultState(Path.Combine(userResourceDir, "ignore.yaml"), yamlSerializer, new IgnoreDefaults());
			await ExportDefaultState(Path.Combine(userResourceDir, "static.yaml"), yamlSerializer, new StaticValues());
		}

		private async Task<T> LoadState<T>(string statePath, IDeserializer yamlDeserializer, Func<T> returnOnError) where T : class
		{
			T result = null;
			try
			{
				using StreamReader stateReader = File.OpenText(statePath);
				result = yamlDeserializer.Deserialize<T>(await stateReader.ReadToEndAsync());
			}
			catch (Exception e)
			{
				Logger.Error(e, "Failed to read or parse " + statePath + ".");
				Logger.Warn("Since " + statePath + " failed to load, internal defaults will be used instead.  Delete it to have it rebuilt.");
			}
			return result ?? returnOnError();
		}

		private async Task LoadAllStates()
		{
			string userResourceDir = DataDirUtil.GetSafeDataDir("user");
			IDeserializer yamlDeserializer = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).WithTypeConverter(new ColorConverter()).IgnoreUnmatchedProperties()
				.Build();
			Population = await LoadState(Path.Combine(userResourceDir, "populate.yaml"), yamlDeserializer, () => new PopulationDefaults());
			Ignore = await LoadState(Path.Combine(userResourceDir, "ignore.yaml"), yamlDeserializer, () => new IgnoreDefaults());
			Static = await LoadState(Path.Combine(userResourceDir, "static.yaml"), yamlDeserializer, () => new StaticValues());
		}

		public override Task Unload()
		{
			Population = null;
			Ignore = null;
			Static = null;
			return Task.CompletedTask;
		}

		public override async Task Reload()
		{
			await LoadAllStates();
		}

		public override void Update(GameTime gameTime)
		{
		}
	}
}
