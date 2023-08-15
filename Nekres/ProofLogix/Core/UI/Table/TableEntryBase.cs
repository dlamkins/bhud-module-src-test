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

		private int _maxStatusIconCellWidth = 11;

		private int _maxTimestampCellWidth = 147;

		private int _maxClassIconCellWidth = 36;

		private int _maxCharacterNameCellWidth = 120;

		private int _maxAccountNameCellWidth = 120;

		private int _maxTokenCellWidth = 50;

		private Rectangle _statusIconBounds;

		private Rectangle _timestampBounds;

		private Rectangle _classIconBounds;

		private Rectangle _characterNameBounds;

		private Rectangle _accountNameBounds;

		private List<Rectangle> _tokenBounds;

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

		public int MaxStatusIconCellWidth
		{
			get
			{
				return _maxStatusIconCellWidth;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _maxStatusIconCellWidth, value, false, "MaxStatusIconCellWidth");
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
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			if (((Rectangle)(ref _statusIconBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				this.ColumnClick?.Invoke(this, new ValueEventArgs<int>(4));
				return;
			}
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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			ObservableCollection<TableConfig.Column> columns = ProofLogix.Instance.TableConfig.get_Value().Columns;
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Status))
			{
				_statusIconBounds = new Rectangle(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxStatusIconCellWidth, bounds.Height);
				PaintStatus(spriteBatch, _statusIconBounds);
				UpdateTooltip(_statusIconBounds, GetStatusTooltip());
			}
			else
			{
				_statusIconBounds = Rectangle.get_Empty();
			}
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Timestamp))
			{
				string timestamp = AssetUtil.Truncate(Timestamp, MaxTimestampCellWidth, Font);
				_timestampBounds = new Rectangle(((Rectangle)(ref _statusIconBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxTimestampCellWidth, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, timestamp, Font, _timestampBounds, Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
				UpdateTooltip(_timestampBounds, GetTimestampTooltip());
			}
			else
			{
				_timestampBounds = new Rectangle(((Rectangle)(ref _statusIconBounds)).get_Right(), 0, 0, 0);
			}
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Class))
			{
				_classIconBounds = new Rectangle(((Rectangle)(ref _timestampBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxClassIconCellWidth, bounds.Height);
				Rectangle centered = default(Rectangle);
				((Rectangle)(ref centered))._002Ector(_classIconBounds.X + (_classIconBounds.Width - _classIconBounds.Height) / 2, _classIconBounds.Y + (bounds.Height - _classIconBounds.Height) / 2, _classIconBounds.Height, _classIconBounds.Height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ClassIcon), centered);
				UpdateTooltip(_classIconBounds, GetClassTooltip());
			}
			else
			{
				_classIconBounds = new Rectangle(((Rectangle)(ref _timestampBounds)).get_Right(), 0, 0, 0);
			}
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Character))
			{
				string characterName = AssetUtil.Truncate(CharacterName, MaxCharacterNameCellWidth, Font);
				_characterNameBounds = new Rectangle(((Rectangle)(ref _classIconBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxCharacterNameCellWidth + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, characterName, Font, _characterNameBounds, Color.get_White(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
				UpdateTooltip(_characterNameBounds, GetCharacterTooltip());
			}
			else
			{
				_characterNameBounds = new Rectangle(((Rectangle)(ref _classIconBounds)).get_Right(), 0, 0, 0);
			}
			if (((Collection<TableConfig.Column>)(object)columns).Contains(TableConfig.Column.Account))
			{
				string accountName = AssetUtil.Truncate(AccountName, MaxAccountNameCellWidth, Font);
				_accountNameBounds = new Rectangle(((Rectangle)(ref _characterNameBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxAccountNameCellWidth + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, bounds.Height);
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
				tokenBounds = new Rectangle(((Rectangle)(ref tokenBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0, MaxTokenCellWidth, bounds.Height);
				tempTokenBounds.Add(tokenBounds);
				PaintToken(spriteBatch, tokenBounds, id);
				UpdateTooltip(tokenBounds, GetTokenTooltip(id));
			}
			_tokenBounds = tempTokenBounds;
			((Control)this).set_Width(((Rectangle)(ref tokenBounds)).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
		}

		protected virtual string GetStatusTooltip()
		{
			return string.Empty;
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

		protected abstract void PaintStatus(SpriteBatch spriteBatch, Rectangle bounds);

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
