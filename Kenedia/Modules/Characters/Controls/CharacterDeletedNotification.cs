using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterDeletedNotification : BaseNotification
	{
		private Rectangle _textRectangle;

		private DetailedTexture _delete = new DetailedTexture(358366, 358367);

		private DetailedTexture _dismiss = new DetailedTexture(156012, 156011)
		{
			TextureRegion = new Rectangle(4, 4, 24, 24)
		};

		private Character_Model _markedCharacter;

		public Character_Model MarkedCharacter
		{
			get
			{
				return _markedCharacter;
			}
			set
			{
				Common.SetProperty(ref _markedCharacter, value, ((Control)this).RecalculateLayout);
			}
		}

		public CharacterDeletedNotification()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			base.NotificationType = NotificationType.CharacterDeleted;
		}

		public override void RecalculateLayout()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			int height = GameService.Content.get_DefaultFont14().get_LineHeight() + 4;
			_dismiss.Bounds = new Rectangle(0, 0, height, height);
			DetailedTexture delete = _delete;
			Rectangle bounds = _dismiss.Bounds;
			delete.Bounds = new Rectangle(((Rectangle)(ref bounds)).get_Right() + 2, 0, height, height);
			if (MarkedCharacter != null)
			{
				int width2 = ((Control)this).get_Width();
				bounds = _delete.Bounds;
				int width = width2 - ((Rectangle)(ref bounds)).get_Right() - 6;
				string wrappedText = TextUtil.WrapText(GameService.Content.get_DefaultFont14(), string.Format(strings.DeletedCharacterNotification, MarkedCharacter.Name, MarkedCharacter.Created.ToString("d")), width);
				RectangleF rect = GameService.Content.get_DefaultFont14().GetStringRectangle(wrappedText);
				bounds = _delete.Bounds;
				_textRectangle = new Rectangle(((Rectangle)(ref bounds)).get_Right() + 6, 0, width, (height > (int)rect.Height) ? height : ((int)rect.Height));
				((Control)this).set_Height(Math.Max(height, _textRectangle.Height));
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			string txt = string.Empty;
			if (MarkedCharacter != null)
			{
				_delete.Draw((Control)(object)this, spriteBatch, ((Control)this).get_RelativeMousePosition());
				if (_delete.Hovered)
				{
					txt = string.Format(strings.DeletedCharacterNotification_DeleteTooltip, MarkedCharacter.Name);
				}
				_dismiss.Draw((Control)(object)this, spriteBatch, ((Control)this).get_RelativeMousePosition());
				if (_dismiss.Hovered)
				{
					txt = string.Format(strings.DeletedCharacterNotification_DismissTooltip, MarkedCharacter.Name);
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, string.Format(strings.DeletedCharacterNotification, MarkedCharacter.Name, MarkedCharacter.Created.ToString("d")), GameService.Content.get_DefaultFont14(), _textRectangle, Color.get_White(), true, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			((Control)this).set_BasicTooltipText(txt);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			if (MarkedCharacter == null)
			{
				return;
			}
			if (_delete.Hovered)
			{
				MarkedCharacter.Delete();
				((Control)this).Dispose();
			}
			if (_dismiss.Hovered)
			{
				Container parent = ((Control)this).get_Parent();
				((Control)this).Dispose();
				if (parent != null)
				{
					((Control)parent).Invalidate();
				}
			}
		}
	}
}
