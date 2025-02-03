using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class DraggingControl : Container
	{
		private CharacterCard _characterControl;

		private CharacterCard _internalCharacterCard;

		private bool _layoutRefreshed;

		private double _lastlayoutRefreshed;

		public CharacterCard CharacterControl
		{
			get
			{
				return _characterControl;
			}
			set
			{
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				if (_characterControl != value)
				{
					_characterControl = value;
					_internalCharacterCard?.Dispose();
					if (value != null)
					{
						_internalCharacterCard = new CharacterCard(value)
						{
							Parent = this,
							IsDraggingTarget = true,
							Enabled = false,
							Visible = true,
							BackgroundColor = Color.get_Black() * 0.8f
						};
						_internalCharacterCard.UniformWithAttached(force: true);
					}
					else
					{
						_internalCharacterCard?.Dispose();
						_internalCharacterCard = null;
					}
				}
			}
		}

		public bool IsActive
		{
			get
			{
				if (_internalCharacterCard != null)
				{
					return base.Visible;
				}
				return false;
			}
		}

		public DraggingControl()
		{
			base.Parent = GameService.Graphics.SpriteScreen;
			base.Visible = false;
			ZIndex = 2147483646;
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
		}

		public void StartDragging(CharacterCard characterCard)
		{
			CharacterControl = characterCard;
			_lastlayoutRefreshed = Common.Now + 5.0;
			_layoutRefreshed = false;
			Show();
		}

		public void EndDragging()
		{
			if (_internalCharacterCard != null)
			{
				RemoveChild(_internalCharacterCard);
				_internalCharacterCard?.Dispose();
				_internalCharacterCard = null;
			}
			_lastlayoutRefreshed = 0.0;
			_characterControl = null;
			Hide();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			if (_internalCharacterCard != null)
			{
				if (!_layoutRefreshed && base.Visible && gameTime.get_TotalGameTime().TotalMilliseconds - _lastlayoutRefreshed >= 0.0)
				{
					_lastlayoutRefreshed = gameTime.get_TotalGameTime().TotalMilliseconds;
					_layoutRefreshed = true;
					_internalCharacterCard.UniformWithAttached(force: true);
				}
				MouseHandler i = Control.Input.Mouse;
				base.Location = new Point(i.Position.X - 15, i.Position.Y - 15);
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
		}
	}
}
