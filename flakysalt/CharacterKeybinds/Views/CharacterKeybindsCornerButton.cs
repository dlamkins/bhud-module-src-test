using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Resources;
using flakysalt.CharacterKeybinds.Services;

namespace flakysalt.CharacterKeybinds.Views
{
	public class CharacterKeybindsCornerButton : IDisposable
	{
		private Texture2D _cornerTexture;

		private ContentService contentService;

		private CornerIcon cornerIcon;

		public Action OnCornerButtonClicked = delegate
		{
		};

		private readonly CharacterKeybindsSettings _settingsModel;

		public CharacterKeybindsCornerButton(ContentService contentService, CharacterKeybindsSettings settings)
		{
			this.contentService = contentService;
			_settingsModel = settings;
			((SettingEntry)_settingsModel.displayCornerIcon).add_PropertyChanged((PropertyChangedEventHandler)EnableOrCreateCornerIcon);
			if (_settingsModel.displayCornerIcon.get_Value())
			{
				EnableOrCreateCornerIcon(null, null);
			}
		}

		private void EnableOrCreateCornerIcon(object sender, PropertyChangedEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			if (cornerIcon == null)
			{
				_cornerTexture = contentService.GetTexture("images/logo_small.png");
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(_cornerTexture));
				((Control)val).set_BasicTooltipText(Loca.moduleName);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				cornerIcon = val;
				((Control)cornerIcon).add_Click((EventHandler<MouseEventArgs>)CornerIconClicked);
			}
			((Control)cornerIcon).set_Visible(_settingsModel.displayCornerIcon.get_Value());
			((Control)cornerIcon).set_Enabled(_settingsModel.displayCornerIcon.get_Value());
		}

		private void CornerIconClicked(object sender, MouseEventArgs e)
		{
			OnCornerButtonClicked();
		}

		public void Dispose()
		{
			Texture2D cornerTexture = _cornerTexture;
			if (cornerTexture != null)
			{
				((GraphicsResource)cornerTexture).Dispose();
			}
			if (cornerIcon != null)
			{
				((Control)cornerIcon).remove_Click((EventHandler<MouseEventArgs>)CornerIconClicked);
			}
			((SettingEntry)_settingsModel.displayCornerIcon).remove_PropertyChanged((PropertyChangedEventHandler)EnableOrCreateCornerIcon);
			CornerIcon obj = cornerIcon;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			cornerIcon = null;
		}
	}
}
