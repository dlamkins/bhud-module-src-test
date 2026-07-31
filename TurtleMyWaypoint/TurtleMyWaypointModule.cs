using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurtleMyWaypoint
{
	[Export(typeof(Module))]
	public class TurtleMyWaypointModule : Module
	{
		private ModuleParameters _moduleParameters;

		private CornerIcon _cornerIcon;

		private TurtleMyWaypointWindow _window;

		[ImportingConstructor]
		public TurtleMyWaypointModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			_moduleParameters = moduleParameters;
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			Texture2D icon = _moduleParameters.get_ContentsManager().GetTexture("corner.png");
			_window = new TurtleMyWaypointWindow(_moduleParameters.get_ContentsManager());
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(icon));
			((Control)val).set_BasicTooltipText("Turtle My Waypoint");
			val.set_Priority(845202);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)_window).ToggleWindow();
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
			TurtleMyWaypointWindow window = _window;
			if (window != null)
			{
				((Control)window).Dispose();
			}
		}
	}
}
