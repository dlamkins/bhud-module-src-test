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
				if (_characterControl == value)
				{
					return;
				}
				_characterControl = value;
				CharacterCard internalCharacterCard = _internalCharacterCard;
				if (internalCharacterCard != null)
				{
					((Control)internalCharacterCard).Dispose();
				}
				if (value != null)
				{
					CharacterCard characterCard = new CharacterCard(value);
					((Control)characterCard).set_Parent((Container)(object)this);
					characterCard.IsDraggingTarget = true;
					((Control)characterCard).set_Enabled(false);
					((Control)characterCard).set_Visible(true);
					characterCard.BackgroundColor = Color.get_Black() * 0.8f;
					_internalCharacterCard = characterCard;
					_internalCharacterCard.UniformWithAttached(force: true);
				}
				else
				{
					CharacterCard internalCharacterCard2 = _internalCharacterCard;
					if (internalCharacterCard2 != null)
					{
						((Control)internalCharacterCard2).Dispose();
					}
					_internalCharacterCard = null;
				}
			}
		}

		public bool IsActive
		{
			get
			{
				if (_internalCharacterCard != null)
				{
					return ((Control)this).get_Visible();
				}
				return false;
			}
		}

		public DraggingControl()
			: this()
		{
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((Control)this).set_ZIndex(2147483646);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
		}

		public void StartDragging(CharacterCard characterCard)
		{
			CharacterControl = characterCard;
			_lastlayoutRefreshed = Common.Now + 5.0;
			_layoutRefreshed = false;
			((Control)this).Show();
		}

		public void EndDragging()
		{
			if (_internalCharacterCard != null)
			{
				((Container)this).RemoveChild((Control)(object)_internalCharacterCard);
				CharacterCard internalCharacterCard = _internalCharacterCard;
				if (internalCharacterCard != null)
				{
					((Control)internalCharacterCard).Dispose();
				}
				_internalCharacterCard = null;
			}
			_lastlayoutRefreshed = 0.0;
			_characterControl = null;
			((Control)this).Hide();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			if (_internalCharacterCard != null)
			{
				if (!_layoutRefreshed && ((Control)this).get_Visible() && gameTime.get_TotalGameTime().TotalMilliseconds - _lastlayoutRefreshed >= 0.0)
				{
					_lastlayoutRefreshed = gameTime.get_TotalGameTime().TotalMilliseconds;
					_layoutRefreshed = true;
					_internalCharacterCard.UniformWithAttached(force: true);
				}
				MouseHandler i = Control.get_Input().get_Mouse();
				((Control)this).set_Location(new Point(i.get_Position().X - 15, i.get_Position().Y - 15));
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
		}
	}
}
