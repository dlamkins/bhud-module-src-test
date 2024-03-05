using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Estreya.BlishHUD.PortalDistance.Controls;
using Estreya.BlishHUD.PortalDistance.Models;
using Estreya.BlishHUD.PortalDistance.UI.Views;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.ArcDPS;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.UI.Views;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.PortalDistance
{
	[Export(typeof(Module))]
	public class PortalDistanceModule : BaseModule<PortalDistanceModule, ModuleSettings>
	{
		private Vector3 _portalPosition;

		private PortalDefinition _activePortal;

		private DistanceMessageControl _messageControl;

		private List<PortalDefinition> _portals = new List<PortalDefinition>
		{
			new PortalDefinition(10198, delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Invalid comparison between Unknown and I4
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Invalid comparison between Unknown and I4
				MapType type = GameService.Gw2Mumble.get_CurrentMap().get_Type();
				bool flag = (((int)type == 2 || (int)type == 6) ? true : false);
				return flag ? 6000 : 5000;
			}),
			new PortalDefinition(16437, () => 5000f),
			new PortalDefinition(34978, () => 5000f)
		};

		protected override string UrlModuleName => "portal-distance";

		protected override string API_VERSION_NO => "1";

		protected override bool NeedsBackend => false;

		protected override int CornerIconPriority => 1289351269;

		[ImportingConstructor]
		public PortalDistanceModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
			_messageControl = new DistanceMessageControl();
			base.ModuleSettings.ManualKeyBinding.get_Value().add_Activated((EventHandler<EventArgs>)ManualKeyBinding_Activated);
		}

		private void ManualKeyBinding_Activated(object sender, EventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			UpdatePortalPosition((_portalPosition == Vector3.get_Zero()) ? GameService.Gw2Mumble.get_PlayerCharacter().get_Position() : Vector3.get_Zero());
		}

		private void ArcDPSService_AreaCombatEvent(object sender, CombatEvent e)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			if (e.Type != CombatEventType.BUFF)
			{
				return;
			}
			IEnumerable<PortalDefinition> portals = _portals.Where((PortalDefinition x) => x.SkillID == e.SkillId);
			if (portals.Any())
			{
				PortalDefinition portal = portals.First();
				Vector3 position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				if (e.State == CombatEventState.BUFFAPPLY)
				{
					base.Logger.Debug($"Portal skill ({e.SkillId} - {e.RawCombatEvent.get_SkillName()}) activated at {position}");
					_activePortal = portal;
				}
				else
				{
					base.Logger.Debug($"Portal skill ({e.SkillId} - {e.RawCombatEvent.get_SkillName()}) deactivated at {position}");
					_activePortal = null;
				}
				UpdatePortalPosition((_activePortal != null) ? position : Vector3.get_Zero());
			}
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			UpdatePortalDistance();
		}

		private void UpdatePortalDistance()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			if (_portalPosition == Vector3.get_Zero())
			{
				DistanceMessageControl messageControl = _messageControl;
				if (messageControl != null && ((Control)messageControl).get_Visible())
				{
					((Control)_messageControl).Hide();
				}
				return;
			}
			DistanceMessageControl messageControl2 = _messageControl;
			if (messageControl2 != null && !((Control)messageControl2).get_Visible())
			{
				((Control)_messageControl).Show();
			}
			float distance = Vector3.Distance(GameService.Gw2Mumble.get_PlayerCharacter().get_Position(), _portalPosition).ToInches();
			_messageControl?.UpdateDistance(distance);
			if (_activePortal != null)
			{
				_messageControl?.UpdateColor((distance > _activePortal.GetMaxDistance()) ? Color.get_Red() : Color.get_Green());
			}
			else
			{
				_messageControl?.UpdateColor(Color.get_Yellow());
			}
		}

		private void UpdatePortalPosition(Vector3 position)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_portalPosition = (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() ? Vector3.get_Zero() : position);
		}

		protected override void Unload()
		{
			base.ModuleSettings.ManualKeyBinding.get_Value().remove_Activated((EventHandler<EventArgs>)ManualKeyBinding_Activated);
			if (base.ArcDPSService != null)
			{
				base.ArcDPSService.AreaCombatEvent -= ArcDPSService_AreaCombatEvent;
			}
			_activePortal = null;
			DistanceMessageControl messageControl = _messageControl;
			if (messageControl != null)
			{
				((Control)messageControl).Dispose();
			}
			_messageControl = null;
			base.Unload();
		}

		protected override void OnSettingWindowBuild(TabbedWindow settingWindow)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			settingWindow.SavesSize = true;
			settingWindow.CanResize = true;
			settingWindow.RebuildViewAfterResize = true;
			settingWindow.UnloadOnRebuild = false;
			settingWindow.MinSize = settingWindow.Size;
			settingWindow.MaxSize = new Point(((Control)settingWindow).get_Width() * 2, ((Control)settingWindow).get_Height() * 3);
			settingWindow.RebuildDelay = 500;
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156736.png"), (Func<IView>)(() => (IView)(object)new GeneralSettingsView(base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, base.ModuleSettings)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("generalSettingsView-title", "General"), (int?)null));
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView(base.IconService, base.TranslationService);
		}

		protected override BaseModuleSettings DefineModuleSettings(SettingCollection settings)
		{
			return new ModuleSettings(settings);
		}

		protected override void ConfigureServices(ServiceConfigurations configurations)
		{
			bool useArcDPS = base.ModuleSettings.UseArcDPS.get_Value();
			configurations.Skills.Enabled = useArcDPS;
			configurations.Skills.AwaitLoading = false;
			configurations.ArcDPS.Enabled = useArcDPS;
		}

		protected override void OnBeforeServicesStarted()
		{
			if (base.ArcDPSService != null)
			{
				base.ArcDPSService.AreaCombatEvent += ArcDPSService_AreaCombatEvent;
			}
		}

		protected override string GetDirectoryName()
		{
			return "portal_distance";
		}

		protected override AsyncTexture2D GetEmblem()
		{
			return base.IconService.GetIcon("740204.png");
		}

		protected override AsyncTexture2D GetCornerIcon()
		{
			return base.IconService.GetIcon("textures/102338-grey.png");
		}

		protected override AsyncTexture2D GetErrorCornerIcon()
		{
			return base.IconService.GetIcon("textures/102338-grey-error.png");
		}
	}
}
