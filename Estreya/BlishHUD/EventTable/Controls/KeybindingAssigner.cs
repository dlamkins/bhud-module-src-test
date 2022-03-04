using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class KeybindingAssigner : LabelBase
	{
		private const int UNIVERSAL_PADDING = 2;

		private int _nameWidth = 183;

		private KeyBinding _keyBinding;

		private Rectangle _nameRegion;

		private Rectangle _hotkeyRegion;

		private bool _overHotkey;

		public int NameWidth
		{
			get
			{
				return _nameWidth;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _nameWidth, value, true, "NameWidth");
			}
		}

		public string KeyBindingName
		{
			get
			{
				return base._text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref base._text, value, false, "KeyBindingName");
			}
		}

		public KeyBinding KeyBinding
		{
			get
			{
				return _keyBinding;
			}
			set
			{
				if (((Control)this).SetProperty<KeyBinding>(ref _keyBinding, value, false, "KeyBinding"))
				{
					((Control)this).set_Enabled(_keyBinding != null);
				}
			}
		}

		public bool WithName { get; }

		public event EventHandler<EventArgs> BindingChanged;

		protected void OnBindingChanged(EventArgs e)
		{
			this.BindingChanged?.Invoke(this, e);
		}

		public KeybindingAssigner(KeyBinding keyBinding, bool withName)
			: this()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			KeyBinding = (KeyBinding)(((object)keyBinding) ?? ((object)new KeyBinding()));
			WithName = withName;
			base._font = Control.get_Content().get_DefaultFont14();
			base._showShadow = true;
			base._cacheLabel = false;
			((Control)this).set_Size(new Point(340, 16));
		}

		public KeybindingAssigner(bool withName)
			: this(null, withName)
		{
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (_overHotkey && e.get_IsDoubleClick())
			{
				SetupNewAssignmentWindow();
			}
			((Control)this).OnClick(e);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_overHotkey = ((Control)this).get_RelativeMousePosition().X >= ((Rectangle)(ref _hotkeyRegion)).get_Left();
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_overHotkey = false;
			((Control)this).OnMouseLeft(e);
		}

		public override void RecalculateLayout()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			_nameRegion = new Rectangle(0, 0, _nameWidth, ((Control)this)._size.Y);
			_hotkeyRegion = new Rectangle(WithName ? (_nameWidth + 2) : 0, 0, ((Control)this)._size.X - (WithName ? (_nameWidth - 2) : 0), ((Control)this)._size.Y);
		}

		private void SetupNewAssignmentWindow()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			KeybindingAssignmentWindow val = new KeybindingAssignmentWindow(base._text, _keyBinding.get_ModifierKeys(), _keyBinding.get_PrimaryKey());
			((Control)val).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			KeybindingAssignmentWindow newHkAssign = val;
			newHkAssign.add_AssignmentAccepted((EventHandler<EventArgs>)delegate
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				_keyBinding.set_ModifierKeys(newHkAssign.get_ModifierKeys());
				_keyBinding.set_PrimaryKey(newHkAssign.get_PrimaryKey());
				OnBindingChanged(EventArgs.Empty);
			});
			((Control)newHkAssign).Show();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			if (WithName)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _nameRegion, Color.get_White() * 0.15f);
				((LabelBase)this).DrawText(spriteBatch, _nameRegion, (string)null);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _hotkeyRegion, Color.get_White() * ((((Control)this)._enabled && _overHotkey) ? 0.2f : 0.15f));
			if (((Control)this)._enabled)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _keyBinding.GetBindingDisplayText(), Control.get_Content().get_DefaultFont14(), RectangleExtension.OffsetBy(_hotkeyRegion, 1, 1), Color.get_Black(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _keyBinding.GetBindingDisplayText(), Control.get_Content().get_DefaultFont14(), _hotkeyRegion, Color.get_White(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}
	}
}
