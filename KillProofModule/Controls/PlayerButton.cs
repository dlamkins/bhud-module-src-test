using System;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using KillProofModule.Controls.Views;
using KillProofModule.Models;
using KillProofModule.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace KillProofModule.Controls
{
	public class PlayerButton : DetailsButton
	{
		private const int DEFAULT_WIDTH = 327;

		private const int DEFAULT_HEIGHT = 100;

		private const int DEFAULT_BOTTOMSECTION_HEIGHT = 35;

		private readonly Texture2D BORDER_SPRITE;

		private readonly Texture2D PIXEL;

		private readonly Texture2D SEPARATOR;

		private BitmapFont _font;

		private PlayerProfile _playerProfile;

		private bool _isNew;

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (_font != value)
				{
					_font = value;
					((Control)this).OnPropertyChanged("Font");
				}
			}
		}

		public PlayerProfile PlayerProfile
		{
			get
			{
				return _playerProfile;
			}
			private set
			{
				_playerProfile = value;
				((Control)this).OnPropertyChanged("PlayerProfile");
			}
		}

		public bool IsNew
		{
			get
			{
				return _isNew;
			}
			set
			{
				if (_isNew != value)
				{
					_isNew = value;
					((Control)this).OnPropertyChanged("IsNew");
				}
			}
		}

		public PlayerButton(PlayerProfile playerProfile)
			: this()
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			PlayerProfile = playerProfile;
			BORDER_SPRITE = BORDER_SPRITE ?? Control.get_Content().GetTexture("controls/detailsbutton/605003");
			SEPARATOR = SEPARATOR ?? Control.get_Content().GetTexture("157218");
			PIXEL = PIXEL ?? Textures.get_Pixel();
			((Control)this).set_Size(new Point(327, 100));
			((DetailsButton)this).set_Icon(KillProofModule.ModuleInstance.GetProfessionRender(playerProfile.Player));
			Font = Control.get_Content().GetFont((FontFace)0, (FontSize)16, (FontStyle)0);
			playerProfile.PlayerChanged += OnPlayerChanged;
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnClick);
		}

		private void OnClick(object o, MouseEventArgs e)
		{
			IsNew = false;
			MainView.LoadProfileView(PlayerProfile.AccountName);
		}

		private void OnPlayerChanged(object o, ValueEventArgs<Player> e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((DetailsButton)this).set_Icon(KillProofModule.ModuleInstance.GetProfessionRender(e.get_Value()));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Invalid comparison between Unknown and I4
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Invalid comparison between Unknown and I4
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PIXEL, bounds, Color.get_Black() * 0.25f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, PIXEL, ((Container)this).get_ContentRegion(), Color.get_Black() * 0.1f);
			int iconSize = (((int)((DetailsButton)this).get_IconSize() == 1) ? 100 : 65);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, PlayerProfile.AccountName, Control.get_Content().get_DefaultFont14(), new Rectangle(iconSize + 20, iconSize - 35, 287, 35), Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (((DetailsButton)this).get_Icon() != null && ((DetailsButton)this).get_Icon().get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((DetailsButton)this).get_Icon()), new Rectangle(iconSize / 2 - 32 + (((int)((DetailsButton)this).get_IconSize() == 0) ? 10 : 0), iconSize / 2 - 32, 64, 64), Color.get_White());
				if ((int)((DetailsButton)this).get_IconSize() == 1)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, BORDER_SPRITE, new Rectangle(0, 0, iconSize, iconSize), Color.get_White());
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, SEPARATOR, new Rectangle(((Container)this).get_ContentRegion().X, bounds.Height - 40, bounds.Width, 8), Color.get_White());
			if (PlayerProfile.CharacterName != null && Font != null)
			{
				string wrappedText = DrawUtil.WrapText(Font, PlayerProfile.CharacterName, (float)(287 - iconSize - 20));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, wrappedText, Font, new Rectangle(iconSize + 20, 0, 327 - iconSize - 20, ((Control)this).get_Height() - 35), Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (IsNew)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, global::KillProofModule.Properties.Resources.New, Control.get_Content().get_DefaultFont14(), new Rectangle(iconSize + 18, 2, 327 - iconSize - 20, ((Control)this).get_Height() - 35), Color.get_Gold(), false, true, 2, (HorizontalAlignment)2, (VerticalAlignment)0);
			}
		}
	}
}
