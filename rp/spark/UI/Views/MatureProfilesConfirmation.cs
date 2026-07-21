using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	internal sealed class MatureProfilesConfirmation : IDisposable
	{
		private readonly SparkSettings _settings;

		private readonly Action<bool> _maturePreferenceChanged;

		private Panel _confirmationPanel;

		public string ButtonText
		{
			get
			{
				if (!_settings.ShowMatureProfiles.get_Value())
				{
					return "Mature Profiles Hidden";
				}
				return "Mature Profiles Visible";
			}
		}

		public MatureProfilesConfirmation(SparkSettings settings, Action<bool> maturePreferenceChanged)
		{
			_settings = settings;
			_maturePreferenceChanged = maturePreferenceChanged;
		}

		public void Toggle(Container popupParent)
		{
			if (_settings.ShowMatureProfiles.get_Value())
			{
				SetEnabled(enabled: false);
			}
			else
			{
				OpenConfirmation(popupParent);
			}
		}

		public void CloseConfirmation()
		{
			Panel confirmationPanel = _confirmationPanel;
			if (confirmationPanel != null)
			{
				((Control)confirmationPanel).Dispose();
			}
			_confirmationPanel = null;
		}

		private void SetEnabled(bool enabled)
		{
			if (_settings.ShowMatureProfiles.get_Value() != enabled)
			{
				_settings.ShowMatureProfiles.set_Value(enabled);
				_maturePreferenceChanged?.Invoke(enabled);
			}
		}

		private void OpenConfirmation(Container popupParent)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			CloseConfirmation();
			Container parent = (Container)(((object)popupParent) ?? ((object)GameService.Graphics.get_SpriteScreen()));
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Enable Mature Profiles?");
			((Control)val).set_Size(new Point(500, 190));
			((Control)val).set_Location(GetCenteredPopupLocation(parent, 500, 190));
			((Control)val).set_Parent(parent);
			((Control)val).set_BackgroundColor(new Color(38, 35, 32));
			((Control)val).set_ClipsBounds(false);
			((Control)val).set_ZIndex(100);
			_confirmationPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("X");
			((Control)val2).set_Location(new Point(468, -28));
			((Control)val2).set_Size(new Point(24, 24));
			((Control)val2).set_Parent((Container)(object)_confirmationPanel);
			((Control)val2).set_ClipsBounds(false);
			((Control)val2).set_ZIndex(10011);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseConfirmation();
			});
			Label val3 = new Label();
			val3.set_Text("Enabling this will allow you to view profiles marked as mature/18+. These profiles may contain explicit details not suitable for minors." + Environment.NewLine + Environment.NewLine + "Are you sure you want to continue?");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(Color.get_White());
			val3.set_WrapText(true);
			((Control)val3).set_Location(new Point(16, 6));
			((Control)val3).set_Size(new Point(468, 92));
			((Control)val3).set_Parent((Container)(object)_confirmationPanel);
			StandardButton val4 = new StandardButton();
			val4.set_Text("Show Mature Profiles");
			((Control)val4).set_Location(new Point(200, 108));
			((Control)val4).set_Size(new Point(165, 32));
			((Control)val4).set_Parent((Container)(object)_confirmationPanel);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetEnabled(enabled: true);
				CloseConfirmation();
			});
			StandardButton val5 = new StandardButton();
			val5.set_Text("No");
			((Control)val5).set_Location(new Point(379, 108));
			((Control)val5).set_Size(new Point(105, 32));
			((Control)val5).set_Parent((Container)(object)_confirmationPanel);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseConfirmation();
			});
		}

		private static Point GetCenteredPopupLocation(Container parent, int width, int height)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Point size;
			if (parent == null)
			{
				size = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			}
			else
			{
				Rectangle contentRegion = parent.get_ContentRegion();
				size = ((Rectangle)(ref contentRegion)).get_Size();
			}
			Point parentSize = size;
			return new Point(Math.Max(8, (parentSize.X - width) / 2), Math.Max(8, (parentSize.Y - height) / 2));
		}

		public void Dispose()
		{
			CloseConfirmation();
		}
	}
}
