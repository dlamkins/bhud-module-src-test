using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class PuzzleLikeControl : Panel
	{
		private readonly Puzzle _puzzle;

		private readonly string _accountName;

		private readonly Image _heartIcon;

		private readonly Label _likeCount;

		private readonly StandardButton _likeButton;

		private bool _hasUpvoted;

		private EventHandler<MouseEventArgs>? _likeButtonClickHandler;

		public PuzzleLikeControl(Puzzle puzzle, string accountName)
			: this()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			_puzzle = puzzle;
			_accountName = accountName;
			_hasUpvoted = puzzle.HasUserUpvoted(accountName);
			Image val = new Image();
			val.set_Texture(Service.Textures.DatAsset(156127));
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Parent((Container)(object)this);
			_heartIcon = val;
			Label val2 = new Label();
			val2.set_Text(_puzzle.Votes.Count + " Upvotes");
			val2.set_Font(Control.get_Content().get_DefaultFont18());
			val2.set_TextColor(Color.get_White());
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Height(48);
			val2.set_AutoSizeWidth(true);
			_likeCount = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Icon(Service.Textures.DatAsset(_hasUpvoted ? 156331 : 156127));
			val3.set_Text(_hasUpvoted ? "You upvoted this puzzle" : "Upvote this puzzle");
			((Control)val3).set_Width(200);
			((Control)val3).set_Height(50);
			((Control)val3).set_Parent((Container)(object)this);
			_likeButton = val3;
			_likeButtonClickHandler = async delegate
			{
				((Control)_likeButton).set_Enabled(false);
				await Service.GeoServerWrapper.SubmitPuzzleUpvote(_puzzle.Id.ToString(), _accountName);
				_hasUpvoted = true;
				_likeCount.set_Text((_puzzle.Votes.Count + 1).ToString());
				_likeButton.set_Icon(Service.Textures.DatAsset(156331));
				_likeButton.set_Text("You upvoted this puzzle");
			};
			((Control)_likeButton).add_Click(_likeButtonClickHandler);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (_hasUpvoted)
			{
				((Control)_likeButton).set_Visible(false);
				((Control)_heartIcon).set_Location(new Point(0, 0));
				((Control)_likeCount).set_Location(new Point(((Control)_heartIcon).get_Right() + 5, 0));
				((Control)this).set_Height(((Control)_heartIcon).get_Height());
				((Control)this).set_Width(((Control)_heartIcon).get_Width() + ((Control)_likeCount).get_Width() + 5);
			}
			else
			{
				((Control)_likeButton).set_Visible(true);
				((Control)_heartIcon).set_Visible(false);
				((Control)_likeCount).set_Visible(false);
				((Control)this).set_Height(((Control)_likeButton).get_Height());
				((Control)this).set_Width(((Control)_likeButton).get_Width());
			}
		}

		protected override void DisposeControl()
		{
			if (_likeButton != null)
			{
				((Control)_likeButton).remove_Click(_likeButtonClickHandler);
			}
			Image heartIcon = _heartIcon;
			if (heartIcon != null)
			{
				((Control)heartIcon).Dispose();
			}
			Label likeCount = _likeCount;
			if (likeCount != null)
			{
				((Control)likeCount).Dispose();
			}
			StandardButton likeButton = _likeButton;
			if (likeButton != null)
			{
				((Control)likeButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
