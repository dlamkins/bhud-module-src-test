using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Collections;
using Nekres.ProofLogix.Core.UI.Configs;

namespace Nekres.ProofLogix.Core.UI.Table
{
	public abstract class TableEntryBase : Control
	{
		private BitmapFont _font = GameService.Content.get_DefaultFont16();

		private int _maxTimestampCellWidth = 150;

		private int _maxClassIconCellWidth = 32;

		private int _maxCharacterNameCellWidth = 150;

		private int _maxAccountNameCellWidth = 150;

		private int _maxTokenCellWidth = 50;

		private Rectangle _timestampBounds;

		private Rectangle _classIconBounds;

		private Rectangle _characterNameBounds;

		private Rectangle _accountNameBounds;

		private List<Rectangle> _tokenBounds;

		private const char ELLIPSIS = '…';

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				((Control)this).SetProperty<BitmapFont>(ref _font, value, false, "Font");
			}
		}

		public int MaxTimestampCellWidth
		{
			get
			{
				return _maxTimestampCellWidth;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _maxTimestampCellWidth, value, false, "MaxTimestampCellWidth");
			}
		}

		public int MaxClassIconCellWidth
		{
			get
			{
				return _maxClassIconCellWidth;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _maxClassIconCellWidth, value, false, "MaxClassIconCellWidth");
			}
		}

		public int MaxCharacterNameCellWidth
		{
			get
			{
				return _maxCharacterNameCellWidth;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _maxCharacterNameCellWidth, value, false, "MaxCharacterNameCellWidth");
			}
		}

		public int MaxAccountNameCellWidth
		{
			get
			{
				return _maxAccountNameCellWidth;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _maxAccountNameCellWidth, value, false, "MaxAccountNameCellWidth");
			}
		}

		public int MaxTokenCellWidth
		{
			get
			{
				return _maxTokenCellWidth;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _maxTokenCellWidth, value, false, "MaxTokenCellWidth");
			}
		}

		protected abstract string Timestamp { get; }

		protected abstract AsyncTexture2D ClassIcon { get; }

		protected abstract string CharacterName { get; }

		protected abstract string AccountName { get; }

		protected bool IsHovering { get; private set; }

		public event EventHandler<ValueEventArgs<int>> ColumnClick;

		protected TableEntryBase()
			: this()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			_timestampBounds = Rectangle.get_Empty();
			_classIconBounds = Rectangle.get_Empty();
			_characterNameBounds = Rectangle.get_Empty();
			_accountNameBounds = Rectangle.get_Empty();
			_tokenBounds = new List<Rectangle>();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).set_BasicTooltipText(string.Empty);
			IsHovering = false;
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			IsHovering = true;
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			if (((Rectangle)(ref _timestampBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				this.ColumnClick?.Invoke(this, new ValueEventArgs<int>(0));
				return;
			}
			if (((Rectangle)(ref _classIconBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				this.ColumnClick?.Invoke(this, new ValueEventArgs<int>(1));
				return;
			}
			if (((Rectangle)(ref _characterNameBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				this.ColumnClick?.Invoke(this, new ValueEventArgs<int>(2));
				return;
			}
			if (((Rectangle)(ref _accountNameBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				this.ColumnClick?.Invoke(this, new ValueEventArgs<int>(3));
				return;
			}
			int i = Enum.GetValues(typeof(TableConfig.Column)).Length;
			foreach (Rectangle tokenBound2 in _tokenBounds)
			{
				Rectangle tokenBound = tokenBound2;
				if (((Rectangle)(ref tokenBound)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					this.ColumnClick?.Invoke(this, new ValueEventArgs<int>(i));
					break;
				}
				i++;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			ObservableCollection<TableConfig.Column> columns = ProofLogix.Instance.TableConfig.get_Value().Columns;
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Timestamp))
			{
				string timestamp = Cut(Timestamp, MaxTimestampCellWidth);
				_timestampBounds = new Rectangle(4, 0, MaxTimestampCellWidth, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, timestamp, Font, _timestampBounds, Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
				UpdateTooltip(_timestampBounds, GetTimestampTooltip());
			}
			else
			{
				_timestampBounds = Rectangle.get_Empty();
			}
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Class))
			{
				_classIconBounds = new Rectangle(((Rectangle)(ref _timestampBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, 36, bounds.Height);
				Rectangle centered = default(Rectangle);
				((Rectangle)(ref centered))._002Ector(_classIconBounds.X + (_classIconBounds.Width - _classIconBounds.Height) / 2, _classIconBounds.Y, _classIconBounds.Height, _classIconBounds.Height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ClassIcon), centered);
				UpdateTooltip(_classIconBounds, GetClassTooltip());
			}
			else
			{
				_classIconBounds = new Rectangle(((Rectangle)(ref _timestampBounds)).get_Right(), 0, 0, 0);
			}
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Character))
			{
				string characterName = Cut(CharacterName, MaxCharacterNameCellWidth);
				_characterNameBounds = new Rectangle(((Rectangle)(ref _classIconBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxCharacterNameCellWidth, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, characterName, Font, _characterNameBounds, Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
				UpdateTooltip(_characterNameBounds, GetCharacterTooltip());
			}
			else
			{
				_characterNameBounds = new Rectangle(((Rectangle)(ref _classIconBounds)).get_Right(), 0, 0, 0);
			}
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Account))
			{
				string accountName = Cut(AccountName, MaxAccountNameCellWidth);
				_accountNameBounds = new Rectangle(((Rectangle)(ref _characterNameBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxAccountNameCellWidth, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, accountName, Font, _accountNameBounds, Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
				UpdateTooltip(_accountNameBounds, GetAccountTooltip());
			}
			else
			{
				_accountNameBounds = new Rectangle(((Rectangle)(ref _characterNameBounds)).get_Right(), 0, 0, 0);
			}
			List<Rectangle> tempTokenBounds = new List<Rectangle>();
			Rectangle tokenBounds = _accountNameBounds;
			foreach (int id in (Collection<int>)(object)ProofLogix.Instance.TableConfig.get_Value().TokenIds)
			{
				tokenBounds = new Rectangle(((Rectangle)(ref tokenBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X * 3, 0, MaxTokenCellWidth, bounds.Height);
				tempTokenBounds.Add(tokenBounds);
				PaintToken(spriteBatch, tokenBounds, id);
				UpdateTooltip(tokenBounds, GetTokenTooltip(id));
			}
			_tokenBounds = tempTokenBounds;
			((Control)this).set_Width(((Rectangle)(ref tokenBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
		}

		protected string Cut(string text, int maxWidth)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			string result = text;
			for (int width = (int)Font.MeasureString(result).Width; width > maxWidth; width = (int)Font.MeasureString(result).Width)
			{
				result = result.Substring(0, result.Length - 1);
			}
			if (result.Length >= text.Length)
			{
				return result;
			}
			return result.TrimEnd() + "…";
		}

		protected virtual string GetTimestampTooltip()
		{
			return string.Empty;
		}

		protected virtual string GetClassTooltip()
		{
			return string.Empty;
		}

		protected virtual string GetCharacterTooltip()
		{
			return string.Empty;
		}

		protected virtual string GetAccountTooltip()
		{
			return string.Empty;
		}

		protected virtual string GetTokenTooltip(int tokenId)
		{
			return ProofLogix.Instance.Resources.GetItem(tokenId).Name;
		}

		protected abstract void PaintToken(SpriteBatch spriteBatch, Rectangle bounds, int tokenId);

		private void UpdateTooltip(Rectangle bounds, string basicTooltipText)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			if (((Rectangle)(ref bounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				((Control)this).set_BasicTooltipText(basicTooltipText);
			}
		}
	}
}
