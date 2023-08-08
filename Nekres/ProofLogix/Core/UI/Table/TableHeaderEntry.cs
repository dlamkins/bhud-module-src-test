using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ProofLogix.Core.UI.Table
{
	internal class TableHeaderEntry : TableEntryBase
	{
		private const string TIMESTAMP_TITLE = "#";

		private readonly AsyncTexture2D _classIcon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(517179);

		private const string CHAR_TITLE = "Character";

		private const string ACCOUNT_TITLE = "Account";

		protected override string Timestamp => "#";

		protected override AsyncTexture2D ClassIcon => _classIcon;

		protected override string CharacterName => "Character";

		protected override string AccountName => "Account";

		protected override void PaintToken(SpriteBatch spriteBatch, Rectangle bounds, int tokenId)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			Rectangle centered = default(Rectangle);
			((Rectangle)(ref centered))._002Ector(bounds.X + (bounds.Width - bounds.Height) / 2, bounds.Y, bounds.Height, bounds.Height);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ProofLogix.Instance.Resources.GetApiIcon(tokenId).Result), centered);
		}
	}
}
