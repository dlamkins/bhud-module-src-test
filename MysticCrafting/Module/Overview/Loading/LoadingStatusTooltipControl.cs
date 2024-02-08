using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Overview.Loading
{
	public class LoadingStatusTooltipControl : Container
	{
		private bool _loading;

		private string _name;

		private DateTime _lastLoaded;

		private DateTime _lastFailed;

		private LoadingSpinner _loadingSpinner;

		private Label _nameLabel;

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
				_nameLabel.Text = _name;
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
		{
			_loadingSpinner = new LoadingSpinner
			{
				Parent = this,
				Size = new Point(25, 25),
				Location = new Point(0, 0),
				Visible = false
			};
			_statusImage = new Image
			{
				Parent = this,
				Size = new Point(16, 16),
				Tint = GetStatusColor(),
				Texture = ServiceContainer.TextureRepository.GetRefTexture("157330-cantint.png"),
				Location = new Point(7, 4)
			};
			_nameLabel = new Label
			{
				Parent = this,
				Text = Name,
				Font = GameService.Content.DefaultFont14,
				Location = new Point(25, 2),
				AutoSizeWidth = true,
				TextColor = Color.White,
				ShowShadow = true
			};
			_dateLabel = new Label
			{
				Parent = this,
				Text = "Last updated: " + LastLoaded.ToString("t"),
				Location = new Point(25, 22),
				TextColor = Color.White * 0.5f,
				AutoSizeWidth = true,
				ShowShadow = true,
				Visible = false
			};
		}

		private void UpdateLoadingStatus()
		{
			if (Loading)
			{
				_loadingSpinner.Show();
				_statusImage.Hide();
			}
			else
			{
				_loadingSpinner.Hide();
				_statusImage.Show();
				_statusImage.Tint = GetStatusColor();
			}
		}

		private void UpdateLastLoadedDate()
		{
			string text = (HasFailed() ? Common.LoadingErrorMessage : $"{Common.LastLoaded}: {LastLoaded:t}");
			_dateLabel.Text = text;
			if (!HasFailed())
			{
				_ = LastLoaded;
				if (!(LastLoaded != DateTime.MinValue))
				{
					_dateLabel.Visible = false;
					goto IL_0071;
				}
			}
			_dateLabel.Visible = true;
			goto IL_0071;
			IL_0071:
			_statusImage.Tint = GetStatusColor();
		}

		private bool HasFailed()
		{
			_ = LastFailed;
			_ = LastLoaded;
			return LastFailed > LastLoaded;
		}

		private Color GetStatusColor()
		{
			Color color = Color.White;
			if (HasFailed())
			{
				color = Color.Red;
			}
			else
			{
				_ = LastLoaded;
				if (LastLoaded != DateTime.MinValue)
				{
					color = Color.LightGreen;
				}
			}
			return color;
		}
	}
}
