using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class TestPanel : Panel, IPanelOverride
	{
		private readonly StandardButton _backButton;

		public Panel Panel => (Panel)(object)this;

		public Texture2D Icon { get; } = RacingModule.ContentsManager.GetTexture("EditIcon.png");


		public string Caption => Strings.TestingRace;

		public TestPanel()
			: this()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.BackToEditing);
			_backButton = val;
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RacingModule.Racer.EditMode = true;
			});
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (_backButton != null)
			{
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				Point size = ((Rectangle)(ref contentRegion)).get_Size();
				Vector2 val = ((Point)(ref size)).ToVector2() / 2f;
				size = ((Control)_backButton).get_Size();
				Vector2 center = val - ((Point)(ref size)).ToVector2() / 2f;
				((Control)_backButton).set_Location(new Point((int)center.X, (int)center.Y));
			}
		}

		protected override void DisposeControl()
		{
			Texture2D icon = Icon;
			if (icon != null)
			{
				((GraphicsResource)icon).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
