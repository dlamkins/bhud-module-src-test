using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Controls.Favorites
{
	internal class FavoriteRowControl : Panel
	{
		private readonly Func<int> _getCurrentMapId;

		private readonly StandardButton _upButton;

		private readonly StandardButton _downButton;

		private readonly Label _nameLabel;

		private readonly Label _cooldownLabel;

		private readonly StandardButton _useButton;

		private readonly StandardButton _removeButton;

		public SpamFavoriteDto Favorite { get; private set; }

		public event EventHandler<SpamFavoriteDto> UseClicked;

		public event EventHandler<Guid> RemoveClicked;

		public event EventHandler MoveUpClicked;

		public event EventHandler MoveDownClicked;

		public FavoriteRowControl(SpamFavoriteDto favorite, Func<int> getCurrentMapId)
			: this()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Expected O, but got Unknown
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Expected O, but got Unknown
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Expected O, but got Unknown
			Favorite = favorite ?? throw new ArgumentNullException("favorite");
			_getCurrentMapId = getCurrentMapId ?? throw new ArgumentNullException("getCurrentMapId");
			((Control)this).set_Height(30);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("^");
			((Control)val).set_BasicTooltipText("Move up");
			((Control)val).set_Left(5);
			((Control)val).set_Top(2);
			((Control)val).set_Width(22);
			((Control)val).set_Height(24);
			((Control)val).set_Visible(false);
			_upButton = val;
			((Control)_upButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.MoveUpClicked?.Invoke(this, EventArgs.Empty);
			});
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("v");
			((Control)val2).set_BasicTooltipText("Move down");
			((Control)val2).set_Left(29);
			((Control)val2).set_Top(2);
			((Control)val2).set_Width(22);
			((Control)val2).set_Height(24);
			((Control)val2).set_Visible(false);
			_downButton = val2;
			((Control)_downButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.MoveDownClicked?.Invoke(this, EventArgs.Empty);
			});
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(TruncateName(favorite.SpamName, 20));
			((Control)val3).set_BasicTooltipText(favorite.SpamName + (string.IsNullOrEmpty(favorite.SpamDescription) ? "" : ("\n" + favorite.SpamDescription)));
			((Control)val3).set_Left(5);
			((Control)val3).set_Top(5);
			((Control)val3).set_Width(150);
			((Control)val3).set_Height(20);
			_nameLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(GetCooldownText());
			((Control)val4).set_Left(160);
			((Control)val4).set_Top(5);
			((Control)val4).set_Width(60);
			((Control)val4).set_Height(20);
			_cooldownLabel = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Use");
			((Control)val5).set_Left(255);
			((Control)val5).set_Top(2);
			((Control)val5).set_Width(45);
			((Control)val5).set_Height(24);
			_useButton = val5;
			((Control)_useButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.UseClicked?.Invoke(this, Favorite);
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("X");
			((Control)val6).set_BasicTooltipText("Remove from favorites");
			((Control)val6).set_Left(271);
			((Control)val6).set_Top(2);
			((Control)val6).set_Width(25);
			((Control)val6).set_Height(24);
			((Control)val6).set_Visible(false);
			_removeButton = val6;
			((Control)_removeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.RemoveClicked?.Invoke(this, Favorite.Id);
			});
			UpdateCooldownDisplay();
		}

		public void SetEditMode(bool editMode)
		{
			((Control)_upButton).set_Visible(editMode);
			((Control)_downButton).set_Visible(editMode);
			((Control)_removeButton).set_Visible(editMode);
			if (editMode)
			{
				((Control)_nameLabel).set_Left(56);
				((Control)_nameLabel).set_Width(100);
				((Control)_cooldownLabel).set_Left(160);
				((Control)_useButton).set_Left(223);
			}
			else
			{
				((Control)_nameLabel).set_Left(5);
				((Control)_nameLabel).set_Width(150);
				((Control)_cooldownLabel).set_Left(160);
				((Control)_useButton).set_Left(255);
			}
		}

		public void UpdateFavorite(SpamFavoriteDto favorite)
		{
			Favorite = favorite;
			_nameLabel.set_Text(TruncateName(favorite.SpamName, 15));
			((Control)_nameLabel).set_BasicTooltipText(favorite.SpamName + (string.IsNullOrEmpty(favorite.SpamDescription) ? "" : ("\n" + favorite.SpamDescription)));
			UpdateCooldownDisplay();
		}

		public void UpdateCooldownDisplay()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			string text = GetCooldownText();
			bool canUse = CanUseFavorite();
			_cooldownLabel.set_Text(text);
			_cooldownLabel.set_TextColor(canUse ? Color.get_LightGreen() : Color.get_Red());
			((Control)_useButton).set_Enabled(!(Module.Instance?.SpamOrchestrator?.IsSpamming).GetValueOrDefault());
		}

		private DateTimeOffset? GetEffectiveLastSpammed()
		{
			if (!Favorite.HasMapLines)
			{
				return Favorite.LastSpammed;
			}
			int currentMapId = _getCurrentMapId();
			return (Favorite.MapCooldowns?.FirstOrDefault((SpamMapHistoryDto mc) => mc.MapId == currentMapId))?.LastSpammed;
		}

		private string GetCooldownText()
		{
			DateTimeOffset? lastSpammed = GetEffectiveLastSpammed();
			if (!lastSpammed.HasValue)
			{
				return "Ready";
			}
			TimeSpan elapsed = DateTimeOffset.UtcNow - lastSpammed.Value;
			TimeSpan remaining = TimeSpan.FromSeconds(Favorite.CooldownSeconds) - elapsed;
			if (remaining.TotalSeconds <= 0.0)
			{
				return "Ready";
			}
			if (remaining.TotalMinutes < 1.0)
			{
				return $"{(int)remaining.TotalSeconds}s";
			}
			if (remaining.TotalHours < 1.0)
			{
				return $"{(int)remaining.TotalMinutes}m";
			}
			return $"{(int)remaining.TotalHours}h";
		}

		private bool CanUseFavorite()
		{
			DateTimeOffset? lastSpammed = GetEffectiveLastSpammed();
			if (!lastSpammed.HasValue)
			{
				return true;
			}
			TimeSpan timeSpan = DateTimeOffset.UtcNow - lastSpammed.Value;
			TimeSpan cooldownDuration = TimeSpan.FromSeconds(Favorite.CooldownSeconds);
			return timeSpan >= cooldownDuration;
		}

		private string TruncateName(string name, int maxLength)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "";
			}
			if (name.Length > maxLength)
			{
				return name.Substring(0, maxLength - 3) + "...";
			}
			return name;
		}
	}
}
