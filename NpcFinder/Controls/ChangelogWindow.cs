using System;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace NpcFinder.Controls
{
	public sealed class ChangelogWindow : StandardWindow
	{
		private Panel _root;

		private Panel _viewport;

		private Label _text;

		public ChangelogWindow(AsyncTexture2D background, string changelogText)
			: this(background, new Rectangle(5, 60, 580, 590), new Rectangle(30, 20, 580, 550))
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("NpcFinder - Changelog");
			((WindowBase2)this).set_CanResize(false);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesPosition(true);
			BuildUi(changelogText ?? "");
		}

		public void SetText(string changelogText)
		{
			if (_text != null)
			{
				_text.set_Text(changelogText ?? "");
			}
		}

		private void BuildUi(string changelogText)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected O, but got Unknown
			Rectangle cr = ((Container)this).get_ContentRegion();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(cr.X, cr.Y));
			((Control)val).set_Size(new Point(cr.Width, cr.Height));
			((Control)val).set_ClipsBounds(true);
			_root = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_root);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(((Control)_root).get_Size());
			((Control)val2).set_ClipsBounds(true);
			_viewport = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_viewport);
			((Control)val3).set_Location(new Point(8, 8));
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Width(((Control)_viewport).get_Width() - 16);
			val3.set_WrapText(true);
			val3.set_Text(changelogText);
			_text = val3;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)_root).set_Location(new Point(contentRegion.X, contentRegion.Y));
				((Control)_root).set_Size(new Point(contentRegion.Width, contentRegion.Height));
				((Control)_viewport).set_Size(((Control)_root).get_Size());
				if (_text != null)
				{
					((Control)_text).set_Width(((Control)_viewport).get_Width() - 16);
				}
			});
		}
	}
}
