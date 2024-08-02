using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.Loading
{
	public class LoadingStatusTooltipControl : Container
	{
		private bool _loading;

		private string _name;

		private DateTime _lastLoaded;

		private DateTime _lastFailed;

		private LoadingSpinner _loadingSpinner;

		private Label _nameLabel;

		private Label _versionLabel;

		private Label _dateLabel;

		private Image _statusImage;

		public bool Loading
		{
			get
			{
				return _loading;
			}
			set
			{
				_loading = value;
				UpdateLoadingStatus();
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				_nameLabel.set_Text(_name);
			}
		}

		public int LoadingIntervalMin
		{
			set
			{
				_versionLabel.set_Text(string.Format(Common.LoadingMinuteInterval, value));
			}
		}

		public DateTime LastLoaded
		{
			get
			{
				return _lastLoaded;
			}
			set
			{
				_lastLoaded = value;
				UpdateLastLoadedDate();
			}
		}

		public DateTime LastFailed
		{
			get
			{
				return _lastFailed;
			}
			set
			{
				_lastFailed = value;
				UpdateLastLoadedDate();
			}
		}

		public LoadingStatusTooltipControl()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Expected O, but got Unknown
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(25, 25));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Visible(false);
			_loadingSpinner = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(new Point(16, 16));
			val2.set_Tint(GetStatusColor());
			val2.set_Texture(ServiceContainer.TextureRepository.GetRefTexture("157330-cantint.png"));
			((Control)val2).set_Location(new Point(7, 4));
			_statusImage = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Name);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val3).set_Location(new Point(25, 2));
			val3.set_AutoSizeWidth(true);
			val3.set_TextColor(Color.get_White());
			val3.set_ShowShadow(true);
			_nameLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(string.Empty);
			val4.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val4).set_Location(new Point(25, 22));
			val4.set_AutoSizeWidth(true);
			val4.set_TextColor(Color.get_White() * 0.7f);
			val4.set_ShowShadow(true);
			_versionLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text($"{Common.LastLoaded}: {LastLoaded:t}");
			val5.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val5).set_Location(new Point(25, 40));
			val5.set_TextColor(Color.get_LightYellow());
			val5.set_AutoSizeWidth(true);
			val5.set_ShowShadow(true);
			((Control)val5).set_Visible(false);
			_dateLabel = val5;
		}

		private void UpdateLoadingStatus()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (Loading)
			{
				((Control)_loadingSpinner).Show();
				((Control)_statusImage).Hide();
			}
			else
			{
				((Control)_loadingSpinner).Hide();
				((Control)_statusImage).Show();
				_statusImage.set_Tint(GetStatusColor());
			}
		}

		private void UpdateLastLoadedDate()
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			string text = (HasFailed() ? Common.LoadingErrorMessage : $"{Common.LastLoaded}: {LastLoaded:t}");
			_dateLabel.set_Text(text);
			if (!HasFailed())
			{
				_ = LastLoaded;
				if (!(LastLoaded != DateTime.MinValue))
				{
					((Control)_dateLabel).set_Visible(false);
					goto IL_0071;
				}
			}
			((Control)_dateLabel).set_Visible(true);
			goto IL_0071;
			IL_0071:
			_statusImage.set_Tint(GetStatusColor());
		}

		private bool HasFailed()
		{
			_ = LastFailed;
			_ = LastLoaded;
			return LastFailed > LastLoaded;
		}

		private Color GetStatusColor()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Color color = Color.get_White();
			if (HasFailed())
			{
				color = Color.get_Red();
			}
			else
			{
				_ = LastLoaded;
				if (LastLoaded != DateTime.MinValue)
				{
					color = Color.get_LightGreen();
				}
			}
			return color;
		}
	}
}
