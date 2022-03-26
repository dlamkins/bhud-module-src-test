using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Manlaan.HPGrid.Controls;
using Manlaan.HPGrid.Models;
using Microsoft.Xna.Framework;

namespace Manlaan.HPGrid
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static Module ModuleInstance;

		private SettingEntry<bool> _settingHPGridShowDesc;

		private List<Grid> _hpgrid_items;

		private DirectoryReader _directoryReader;

		private JsonSerializerOptions _jsonOptions;

		private DrawGrid _gridImg;

		private string[] _jsonfiles = new string[28]
		{
			"sample.txt", "Fractal_AquaticRuins.json", "Fractal_CaptainMaiTrin.json", "Fractal_Chaos.json", "Fractal_Cliffside.json", "Fractal_Deepstone.json", "Fractal_MoltenBoss.json", "Fractal_MoltenFurnace.json", "Fractal_Nightmare.json", "Fractal_Shattered.json",
			"Fractal_SirensReef.json", "Fractal_Snowblind.json", "Fractal_Sunqua.json", "Fractal_Swampland.json", "Fractal_Thaumanova.json", "Fractal_TwilightOasis.json", "Fractal_Volcanic.json", "Raid_BastionPenitent.json", "Raid_HallChains.json", "Raid_KeyAhdashim.json",
			"Raid_MythwrightGambit.json", "Raid_SalvationPass.json", "Raid_SpiritVale.json", "Raid_StrongholdFaithful.json", "Strike_Boneskinner.json", "Strike_FraenerJormag.json", "Strike_ShiverpeakPass.json", "Strike_WhisperJormag.json"
		};

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingHPGridShowDesc = settings.DefineSetting<bool>("HPGridShowDesc", true, "Show Description", "", (SettingTypeRendererDelegate)null);
			_settingHPGridShowDesc.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateShowDesc);
		}

		protected override void Initialize()
		{
			_hpgrid_items = new List<Grid>();
			_gridImg = new DrawGrid();
			((Control)_gridImg).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_gridImg.ShowDesc = _settingHPGridShowDesc.get_Value();
		}

		protected override async Task LoadAsync()
		{
			string[] jsonfiles = _jsonfiles;
			foreach (string s in jsonfiles)
			{
				ExtractFile(s);
			}
			string hpgridDirectory = DirectoriesManager.GetFullDirectoryPath("hpgrid");
			_directoryReader = new DirectoryReader(hpgridDirectory);
			Module module = this;
			JsonSerializerOptions val = new JsonSerializerOptions();
			val.set_ReadCommentHandling((JsonCommentHandling)1);
			val.set_AllowTrailingCommas(true);
			val.set_IgnoreNullValues(true);
			module._jsonOptions = val;
			_directoryReader.LoadOnFileType((Action<Stream, IDataReader>)delegate(Stream fileStream, IDataReader dataReader)
			{
				readJson(fileStream);
			}, ".json", (IProgress<string>)null);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		private void UpdateShowDesc(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			_gridImg.ShowDesc = _settingHPGridShowDesc.get_Value();
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			((Control)_gridImg).set_Visible(false);
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return;
			}
			foreach (Grid _grid in _hpgrid_items)
			{
				if (_grid.Map != GameService.Gw2Mumble.get_CurrentMap().get_Id())
				{
					continue;
				}
				foreach (GridFight _fight in _grid.Fights)
				{
					if (_fight.InRadius(GameService.Gw2Mumble.get_PlayerCharacter().get_Position()))
					{
						_gridImg.SetSize();
						((Control)_gridImg).set_Visible(true);
						_gridImg.Phases = _fight.Phase;
					}
				}
			}
		}

		protected override void Unload()
		{
			ModuleInstance = null;
			((Control)_gridImg).Dispose();
		}

		private void readJson(Stream fileStream)
		{
			string jsonContent;
			using (StreamReader jsonReader = new StreamReader(fileStream))
			{
				jsonContent = jsonReader.ReadToEnd();
			}
			Grid g = null;
			try
			{
				g = JsonSerializer.Deserialize<Grid>(jsonContent, _jsonOptions);
				_hpgrid_items.Add(g);
			}
			catch (Exception ex)
			{
				Logger.Error("HPGrid deserialization failure: " + ex.Message);
			}
		}

		private void ExtractFile(string filePath)
		{
			string fullPath = Path.Combine(DirectoriesManager.GetFullDirectoryPath("hpgrid"), filePath);
			using Stream fs = ContentsManager.GetFileStream(filePath);
			fs.Position = 0L;
			byte[] buffer = new byte[fs.Length];
			int content = fs.Read(buffer, 0, (int)fs.Length);
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			File.WriteAllBytes(fullPath, buffer);
		}
	}
}
