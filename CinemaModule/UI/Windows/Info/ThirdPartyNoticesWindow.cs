using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Windows.Info
{
	public class ThirdPartyNoticesWindow : StandardWindow
	{
		private Panel _scrollPanel;

		private MultilineTextBox _noticesTextBox;

		public ThirdPartyNoticesWindow()
			: this(CinemaModule.Instance.TextureService.GetSmallWindowBackground(), new Rectangle(25, 26, 435, 480), new Rectangle(40, 10, 395, 440))
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Third-Party Notices");
			((Control)this).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)this).get_Width()) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)this).get_Height()) / 2));
			((WindowBase2)this).set_SavesPosition(true);
			BuildContent();
		}

		private void BuildContent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(((Container)this).get_ContentRegion().Width, ((Container)this).get_ContentRegion().Height));
			val.set_CanScroll(true);
			_scrollPanel = val;
			string noticesText = LoadThirdPartyNotices();
			string[] lines = noticesText.Split('\n');
			MultilineTextBox val2 = new MultilineTextBox();
			((Control)val2).set_Parent((Container)(object)_scrollPanel);
			((Control)val2).set_Location(new Point(5, 5));
			((Control)val2).set_Width(((Container)_scrollPanel).get_ContentRegion().Width);
			((Control)val2).set_Height(lines.Length * 14 + 50);
			((TextInputBase)val2).set_Text(noticesText);
			((TextInputBase)val2).set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)11, (FontStyle)0));
			_noticesTextBox = val2;
		}

		private string LoadThirdPartyNotices()
		{
			try
			{
				Stream stream = CinemaModule.Instance.ContentsManager.GetFileStream("THIRD-PARTY-NOTICES.txt");
				if (stream == null)
				{
					Logger.GetLogger<ThirdPartyNoticesWindow>().Warn("THIRD-PARTY-NOTICES.txt stream is null");
					return "Third-party-notices file not found.";
				}
				using (stream)
				{
					using StreamReader reader = new StreamReader(stream);
					return reader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				Logger.GetLogger<ThirdPartyNoticesWindow>().Warn("Failed to load third-party-notices: " + ex.Message);
				return "Third-party-notices could not be loaded.\nError: " + ex.Message;
			}
		}

		protected override void DisposeControl()
		{
			MultilineTextBox noticesTextBox = _noticesTextBox;
			if (noticesTextBox != null)
			{
				((Control)noticesTextBox).Dispose();
			}
			Panel scrollPanel = _scrollPanel;
			if (scrollPanel != null)
			{
				((Control)scrollPanel).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
