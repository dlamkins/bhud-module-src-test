using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Glide;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.Mistwar.Entities;
using Nekres.Mistwar.UI.Controls;
using Nekres.Mistwar.UI.Models;
using Nekres.Mistwar.UI.Views;

namespace Nekres.Mistwar
{
	[Export(typeof(Module))]
	public class MistwarModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<MistwarModule>();

		internal static MistwarModule ModuleInstance;

		internal SettingEntry<KeyBinding> ToggleKeySetting;

		internal SettingEntry<ColorType> ColorTypeSetting;

		internal SettingEntry<float> ColorIntensitySetting;

		internal SettingEntry<bool> DrawSectorsSetting;

		internal SettingEntry<bool> UseCustomIconsSetting;

		internal SettingEntry<float> ScaleRatioSetting;

		internal SettingEntry<bool> DrawObjectiveNamesSetting;

		internal SettingEntry<float> OpacitySetting;

		private MapImage _mapControl;

		private Dictionary<int, IEnumerable<WvwObjectiveEntity>> _wvwObjectiveCache;

		private bool _enabled;

		private DateTime? _prevApiRequestTime;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public int WorldId { get; private set; }

		[ImportingConstructor]
		public MistwarModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			ToggleKeySetting = settings.DefineSetting<KeyBinding>("ToggleKey", new KeyBinding((Keys)78), (Func<string>)(() => "Toggle Key"), (Func<string>)(() => "Key used to show and hide the tactical map overlay."));
			ColorTypeSetting = settings.DefineSetting<ColorType>("ColorType", ColorType.Normal, (Func<string>)(() => "Color Type"), (Func<string>)(() => "Select a different color type if you have a color deficiency."));
			ColorIntensitySetting = settings.DefineSetting<float>("ColorIntensity", 80f, (Func<string>)(() => "Color Intensity"), (Func<string>)(() => "Intensity of the background color."));
			OpacitySetting = settings.DefineSetting<float>("Opacity", 80f, (Func<string>)(() => "Opacity"), (Func<string>)(() => "Changes the opacity of the tactical map interface."));
			ScaleRatioSetting = settings.DefineSetting<float>("ScaleRatio", 80f, (Func<string>)(() => "Scale Ratio"), (Func<string>)(() => "Changes the size of the tactical map interface"));
			DrawSectorsSetting = settings.DefineSetting<bool>("DrawSectors", true, (Func<string>)(() => "Draw Sector Boundaries"), (Func<string>)(() => "Indicates if the sector boundaries should be drawn."));
			DrawObjectiveNamesSetting = settings.DefineSetting<bool>("DrawObjectiveNames", true, (Func<string>)(() => "Draw Objective Names"), (Func<string>)(() => "Indicates if the names of the objectives should be drawn."));
			UseCustomIconsSetting = settings.DefineSetting<bool>("UseCustomIcons", false, (Func<string>)(() => "Use Custom Icons"), (Func<string>)(() => "Indicates if icons provided by the module should be used for objective states."));
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
		}

		protected override void Initialize()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			_wvwObjectiveCache = new Dictionary<int, IEnumerable<WvwObjectiveEntity>>();
			MapImage mapImage = new MapImage();
			((Control)mapImage).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)mapImage).set_Size(new Point(0, 0));
			((Control)mapImage).set_Location(new Point(0, 0));
			_mapControl = mapImage;
			((Control)_mapControl).Hide();
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			ColorIntensitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			OnColorIntensitySettingChanged(null, new ValueChangedEventArgs<float>(0f, ColorIntensitySetting.get_Value()));
			OnOpacitySettingChanged(null, new ValueChangedEventArgs<float>(0f, OpacitySetting.get_Value()));
			ToggleKeySetting.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleKeyActivated);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			OpacitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnOpacitySettingChanged);
			ToggleKeySetting.get_Value().set_Enabled(true);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				ToggleMapControl(enabled: false, 0.1f, silent: true);
			}
		}

		private bool IsUiAvailable()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				ToggleMapControl(enabled: false, 0.1f, silent: true);
			}
		}

		private void OnOpacitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_mapControl.SetOpacity(MathHelper.Clamp(e.get_NewValue() / 100f, 0f, 1f));
		}

		private void OnToggleKeyActivated(object o, EventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (IsUiAvailable() && GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				ToggleMapControl(!_enabled, 0.1f);
			}
		}

		private void ToggleMapControl(bool enabled, float tDuration, bool silent = false)
		{
			_enabled = enabled;
			if (enabled)
			{
				((Control)_mapControl).Show();
				if (!silent)
				{
					GameService.Content.PlaySoundEffectByName("page-open-" + RandomUtil.GetRandom(1, 3));
					((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(_mapControl, (object)new
					{
						Opacity = 1f
					}, 0.35f, 0f, true);
				}
				return;
			}
			if (silent)
			{
				MapImage mapControl = _mapControl;
				if (mapControl != null)
				{
					((Control)mapControl).Hide();
				}
				return;
			}
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(_mapControl, (object)new
			{
				Opacity = 0f
			}, tDuration, 0f, true).OnComplete((Action)delegate
			{
				MapImage mapControl2 = _mapControl;
				if (mapControl2 != null)
				{
					((Control)mapControl2).Hide();
				}
			});
			GameService.Content.PlaySoundEffectByName("window-close");
		}

		private void OnColorIntensitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_mapControl.GrayscaleIntensity = (100f - e.get_NewValue()) / 100f;
		}

		private async void OnSubtokenUpdated(object o, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			if (!e.get_Value().Contains((TokenPermission)1))
			{
				return;
			}
			await ((IBlobClient<Account>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken)).ContinueWith(delegate(Task<Account> task)
			{
				if (!task.IsFaulted)
				{
					WorldId = task.Result.get_World();
					OnMapChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
				}
			});
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			if (!GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				ToggleKeySetting.get_Value().set_Enabled(false);
				ToggleMapControl(enabled: false, 0.1f, silent: true);
				return;
			}
			ToggleKeySetting.get_Value().set_Enabled(true);
			using (Stream imageStream = await MapUtil.DrawMapImage(e.get_Value(), removeBackground: true, string.Format("{0}/{1}.png", DirectoriesManager.GetFullDirectoryPath("mistwar"), e.get_Value())))
			{
				if (imageStream == null)
				{
					return;
				}
				Texture2D tex = Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), imageStream);
				_mapControl.Texture.SwapTexture(tex);
			}
			if (_wvwObjectiveCache.TryGetValue(e.get_Value(), out var currentObjectives))
			{
				_mapControl.WvwObjectives = currentObjectives;
				return;
			}
			await ((IAllExpandableClient<WvwObjective>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Objectives()).AllAsync(default(CancellationToken)).ContinueWith((Func<Task<IApiV2ObjectList<WvwObjective>>, Task>)async delegate(Task<IApiV2ObjectList<WvwObjective>> task)
			{
				if (!task.IsFaulted)
				{
					Map map = await MapUtil.RequestMap(e.get_Value());
					IEnumerable<ContinentFloorRegionMapSector> sectors = await MapUtil.RequestSectorsForFloor(map.get_ContinentId(), map.get_DefaultFloor(), map.get_RegionId(), e.get_Value());
					if (sectors != null)
					{
						List<WvwObjectiveEntity> newObjectives = new List<WvwObjectiveEntity>();
						foreach (ContinentFloorRegionMapSector sector in sectors)
						{
							WvwObjective obj = ((IEnumerable<WvwObjective>)task.Result).FirstOrDefault((WvwObjective x) => x.get_SectorId() == sector.get_Id());
							if (obj != null)
							{
								newObjectives.Add(new WvwObjectiveEntity(obj, map, sector));
							}
						}
						_mapControl.WvwObjectives = newObjectives;
						_wvwObjectiveCache.Add(map.get_Id(), _mapControl.WvwObjectives);
					}
				}
			});
		}

		protected override async void Update(GameTime gameTime)
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld() && _wvwObjectiveCache.ContainsKey(GameService.Gw2Mumble.get_CurrentMap().get_Id()) && (!_prevApiRequestTime.HasValue || !(DateTime.UtcNow.Subtract(_prevApiRequestTime.Value).TotalSeconds < 15.0)))
			{
				_prevApiRequestTime = DateTime.UtcNow;
				await Task.Run(() => UpdateObjectives(WorldId, GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			}
		}

		private async Task UpdateObjectives(int worldId, int mapId)
		{
			IReadOnlyList<WvwMatchMapObjective> objectives = await RequestObjectives(worldId, mapId);
			if (objectives == null || !_wvwObjectiveCache.TryGetValue(mapId, out var objEntities))
			{
				return;
			}
			foreach (WvwObjectiveEntity objEntity in objEntities)
			{
				WvwMatchMapObjective obj = objectives.First((WvwMatchMapObjective v) => v.get_Id().Equals(objEntity.Id, StringComparison.InvariantCultureIgnoreCase));
				objEntity.LastFlipped = obj.get_LastFlipped() ?? DateTime.MinValue;
				objEntity.Owner = obj.get_Owner().get_Value();
				objEntity.ClaimedBy = obj.get_ClaimedBy() ?? Guid.Empty;
				objEntity.GuildUpgrades = obj.get_GuildUpgrades();
				objEntity.YaksDelivered = obj.get_YaksDelivered().GetValueOrDefault();
			}
		}

		private async Task<IReadOnlyList<WvwMatchMapObjective>> RequestObjectives(int worldId, int mapId)
		{
			return await ((IBlobClient<WvwMatch>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Wvw()
				.get_Matches()
				.World(worldId)).GetAsync(default(CancellationToken)).ContinueWith(delegate(Task<WvwMatch> task)
			{
				if (task.IsFaulted)
				{
					return null;
				}
				WvwMatchMap obj = task.Result.get_Maps().FirstOrDefault((WvwMatchMap x) => x.get_Id() == mapId);
				return (obj == null) ? null : obj.get_Objectives();
			});
		}

		protected override void Unload()
		{
			ToggleKeySetting.get_Value().set_Enabled(false);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			ColorIntensitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			OpacitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnOpacitySettingChanged);
			MapImage mapControl = _mapControl;
			if (mapControl != null)
			{
				AsyncTexture2D texture = mapControl.Texture;
				if (texture != null)
				{
					texture.Dispose();
				}
			}
			MapImage mapControl2 = _mapControl;
			if (mapControl2 != null)
			{
				((Control)mapControl2).Dispose();
			}
			ModuleInstance = null;
		}
	}
}
