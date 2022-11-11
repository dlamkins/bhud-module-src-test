using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Compass_Module
{
	[Export(typeof(Module))]
	public class CompassModule : Module
	{
		internal static CompassModule ModuleInstance;

		private SettingEntry<float> _settingCompassSize;

		private SettingEntry<float> _settingCompassRadius;

		private SettingEntry<float> _settingVerticalOffset;

		private SettingEntry<bool> _settingFadeForwardDirection;

		private CompassBillboard _northBb;

		private CompassBillboard _eastBb;

		private CompassBillboard _southBb;

		private CompassBillboard _westBb;

		private const float VERTICALOFFSET_MIDDLE = 2.5f;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		[ImportingConstructor]
		public CompassModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingCompassSize = settings.DefineSetting<float>("CompassSize", 0.5f, (Func<string>)(() => "Compass Size"), (Func<string>)(() => "Size of the compass elements."));
			_settingCompassRadius = settings.DefineSetting<float>("CompassRadius", 0f, (Func<string>)(() => "Compass Radius"), (Func<string>)(() => "Radius of the compass."));
			_settingVerticalOffset = settings.DefineSetting<float>("VerticalOffset", 2.5f, (Func<string>)(() => "Vertical Offset"), (Func<string>)(() => "How high to offset the compass off the ground."));
			_settingFadeForwardDirection = settings.DefineSetting<bool>("FadeForwardDirection", true, (Func<string>)(() => "Fade Forward Direction"), (Func<string>)(() => "If enabled, the direction in front of the character is faded out."));
			SettingComplianceExtensions.SetRange(_settingCompassSize, 0.1f, 2f);
			SettingComplianceExtensions.SetRange(_settingCompassRadius, 0f, 4f);
			SettingComplianceExtensions.SetRange(_settingVerticalOffset, 0f, 5f);
		}

		protected override Task LoadAsync()
		{
			return Task.CompletedTask;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_northBb = new CompassBillboard(ContentsManager.GetTexture("north.png"));
			_eastBb = new CompassBillboard(ContentsManager.GetTexture("east.png"));
			_southBb = new CompassBillboard(ContentsManager.GetTexture("south.png"));
			_westBb = new CompassBillboard(ContentsManager.GetTexture("west.png"));
			GameService.Graphics.get_World().AddEntity((IEntity)(object)_northBb);
			GameService.Graphics.get_World().AddEntity((IEntity)(object)_eastBb);
			GameService.Graphics.get_World().AddEntity((IEntity)(object)_southBb);
			GameService.Graphics.get_World().AddEntity((IEntity)(object)_westBb);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateBillboardSize();
			UpdateBillboardOffset();
			UpdateBillboardOpacity();
		}

		private void UpdateBillboardSize()
		{
			_northBb.Scale = _settingCompassSize.get_Value();
			_eastBb.Scale = _settingCompassSize.get_Value();
			_southBb.Scale = _settingCompassSize.get_Value();
			_westBb.Scale = _settingCompassSize.get_Value();
		}

		private void UpdateBillboardOffset()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			_northBb.Offset = new Vector3(0f, 1f + _settingCompassRadius.get_Value(), _settingVerticalOffset.get_Value() - 2.5f);
			_eastBb.Offset = new Vector3(1f + _settingCompassRadius.get_Value(), 0f, _settingVerticalOffset.get_Value() - 2.5f);
			_southBb.Offset = new Vector3(0f, -1f - _settingCompassRadius.get_Value(), _settingVerticalOffset.get_Value() - 2.5f);
			_westBb.Offset = new Vector3(-1f - _settingCompassRadius.get_Value(), 0f, _settingVerticalOffset.get_Value() - 2.5f);
		}

		private void UpdateBillboardOpacity()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			if (_settingFadeForwardDirection.get_Value())
			{
				_northBb.Opacity = Math.Min(1f - GameService.Gw2Mumble.get_PlayerCamera().get_Forward().Y, 1f);
				_eastBb.Opacity = Math.Min(1f - GameService.Gw2Mumble.get_PlayerCamera().get_Forward().X, 1f);
				_southBb.Opacity = Math.Min(1f + GameService.Gw2Mumble.get_PlayerCamera().get_Forward().Y, 1f);
				_westBb.Opacity = Math.Min(1f + GameService.Gw2Mumble.get_PlayerCamera().get_Forward().X, 1f);
			}
			else
			{
				_northBb.Opacity = 1f;
				_eastBb.Opacity = 1f;
				_southBb.Opacity = 1f;
				_westBb.Opacity = 1f;
			}
		}

		protected override void Unload()
		{
			ModuleInstance = null;
			GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_northBb);
			GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_eastBb);
			GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_southBb);
			GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_westBb);
		}
	}
}
