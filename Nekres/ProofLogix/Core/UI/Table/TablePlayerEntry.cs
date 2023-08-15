using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.Services.PartySync.Models;

namespace Nekres.ProofLogix.Core.UI.Table
{
	public class TablePlayerEntry : TableEntryBase
	{
		private Player _player;

		private bool _remember;

		private const int STATUS_ICON_SIZE = 8;

		public Player Player
		{
			get
			{
				return _player;
			}
			set
			{
				((Control)this).SetProperty<Player>(ref _player, value, false, "Player");
			}
		}

		public bool Remember
		{
			get
			{
				return _remember;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _remember, value, false, "Remember");
				SetBackgroundColor();
			}
		}

		protected override string Timestamp => Player.Created.ToLocalTime().AsTimeAgo();

		protected override AsyncTexture2D ClassIcon => Player.Icon;

		protected override string CharacterName => Player.CharacterName;

		protected override string AccountName => Player.AccountName;

		public TablePlayerEntry(Player player)
		{
			_player = player;
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			base.OnMouseLeft(e);
			SetBackgroundColor();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			base.OnMouseEntered(e);
			SetBackgroundColor();
		}

		private void SetBackgroundColor()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (base.IsHovering)
			{
				((Control)this).set_BackgroundColor((Remember ? Color.get_LightCyan() : Color.get_LightBlue()) * 0.2f);
			}
			else
			{
				((Control)this).set_BackgroundColor(Remember ? (Color.get_LightGreen() * 0.2f) : Color.get_Transparent());
			}
		}

		protected override string GetTimestampTooltip()
		{
			return Player.Created.ToLocalTime().AsRelativeTime();
		}

		protected override string GetClassTooltip()
		{
			return Player.Class;
		}

		protected override string GetCharacterTooltip()
		{
			return Player.CharacterName;
		}

		protected override string GetAccountTooltip()
		{
			return Player.AccountName;
		}

		protected override string GetTokenTooltip(int tokenId)
		{
			Token token = Player.KpProfile.GetToken(tokenId);
			return AssetUtil.GetItemDisplayName(token.Name, token.Amount, brackets: false);
		}

		protected override void PaintStatus(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Rectangle centered = default(Rectangle);
			((Rectangle)(ref centered))._002Ector(bounds.X + (bounds.Width - 8) / 2, bounds.Y + (bounds.Height - 8) / 2, 8, 8);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), centered, ProofLogix.Instance.PartySync.GetStatusColor(Player.Status));
		}

		protected override void PaintToken(SpriteBatch spriteBatch, Rectangle bounds, int tokenId)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			Token token = Player.KpProfile.GetToken(tokenId);
			Color color = ProofLogix.Instance.PartySync.GetTokenAmountColor(tokenId, token.Amount, ProofLogix.Instance.TableConfig.get_Value().ColorGradingMode);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, AssetUtil.Truncate(token.Amount.ToString(), bounds.Width, base.Font), base.Font, bounds, color, false, true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
		}

		protected override string GetStatusTooltip()
		{
			return Player.Status.ToString();
		}
	}
}
