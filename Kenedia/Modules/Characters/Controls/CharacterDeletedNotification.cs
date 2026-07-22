using System;
using System.Runtime.CompilerServices;
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

		public Character_Model MarkedCharacter
		{
			[CompilerGenerated]
			get
			{
				return _003CMarkedCharacter_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CMarkedCharacter_003Ek__BackingField, value, delegate(Character_Model v)
				{
					_003CMarkedCharacter_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public CharacterDeletedNotification()
		{
			base.NotificationType = NotificationType.CharacterDeleted;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int height = GameService.Content.DefaultFont14.LineHeight + 4;
			_dismiss.Bounds = new Rectangle(0, 0, height, height);
			_delete.Bounds = new Rectangle(_dismiss.Bounds.Right + 2, 0, height, height);
			if (MarkedCharacter != null)
			{
				int width = base.Width - _delete.Bounds.Right - 6;
				string wrappedText = TextUtil.WrapText(GameService.Content.DefaultFont14, string.Format(strings.DeletedCharacterNotification, MarkedCharacter.Name, MarkedCharacter.Created.ToString("d")), width);
				RectangleF rect = GameService.Content.DefaultFont14.GetStringRectangle(wrappedText);
				_textRectangle = new Rectangle(_delete.Bounds.Right + 6, 0, width, (height > (int)rect.Height) ? height : ((int)rect.Height));
				base.Height = Math.Max(height, _textRectangle.Height);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			string txt = string.Empty;
			if (MarkedCharacter != null)
			{
				_delete.Draw(this, spriteBatch, base.RelativeMousePosition);
				if (_delete.Hovered)
				{
					txt = string.Format(strings.DeletedCharacterNotification_DeleteTooltip, MarkedCharacter.Name);
				}
				_dismiss.Draw(this, spriteBatch, base.RelativeMousePosition);
				if (_dismiss.Hovered)
				{
					txt = string.Format(strings.DeletedCharacterNotification_DismissTooltip, MarkedCharacter.Name);
				}
				spriteBatch.DrawStringOnCtrl(this, string.Format(strings.DeletedCharacterNotification, MarkedCharacter.Name, MarkedCharacter.Created.ToString("d")), GameService.Content.DefaultFont14, _textRectangle, Color.White, wrap: true);
			}
			base.BasicTooltipText = txt;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (MarkedCharacter != null)
			{
				if (_delete.Hovered)
				{
					MarkedCharacter.Delete();
					Dispose();
				}
				if (_dismiss.Hovered)
				{
					Container parent = base.Parent;
					Dispose();
					parent?.Invalidate();
				}
			}
		}
	}
}
