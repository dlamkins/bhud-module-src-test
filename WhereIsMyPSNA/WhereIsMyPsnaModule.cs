using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhereIsMyPSNA
{
	[Export(typeof(Module))]
	public class WhereIsMyPsnaModule : Module
	{
		private ModuleParameters _moduleParameters;

		private CornerIcon _cornerIcon;

		private PsnaWindow _psnaWindow;

		private SettingEntry<bool> _hideKnownNpcs;

		[ImportingConstructor]
		public WhereIsMyPsnaModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			_moduleParameters = moduleParameters;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_hideKnownNpcs = settings.DefineSetting<bool>("HideKnownNpcs", false, (Func<string>)(() => "Hide NPC if recipe is already known"), (Func<string>)(() => "Hides the panel and copy button for NPCs whose today's recipe you already know."));
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			Texture2D icon = _moduleParameters.get_ContentsManager().GetTexture("psna.png");
			_psnaWindow = new PsnaWindow(_moduleParameters.get_ContentsManager(), _moduleParameters.get_Gw2ApiManager(), _hideKnownNpcs);
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(icon));
			((Control)val).set_BasicTooltipText("Where Is My PSNA");
			val.set_Priority(845201);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_psnaWindow.ToggleWindow();
			});
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			PsnaWindow psnaWindow = _psnaWindow;
			if (psnaWindow != null)
			{
				((Control)psnaWindow).Dispose();
			}
		}
	}
}
