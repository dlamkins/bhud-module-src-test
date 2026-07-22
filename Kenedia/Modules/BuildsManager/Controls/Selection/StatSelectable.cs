using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class StatSelectable : Kenedia.Modules.Core.Controls.Panel
	{
		private readonly AsyncTexture2D _textureVignette = AsyncTexture2D.FromAssetId(605003);

		private Rectangle _vignetteBounds;

		private readonly bool _created;

		private new readonly Kenedia.Modules.Core.Controls.Image _icon;

		private readonly Kenedia.Modules.Core.Controls.Label _name;

		private readonly Kenedia.Modules.Core.Controls.Label _statSummary;

		public Stat Stat
		{
			[CompilerGenerated]
			get
			{
				return _003CStat_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CStat_003Ek__BackingField, value, delegate(Stat v)
				{
					_003CStat_003Ek__BackingField = v;
				}, new Action(OnStatChanged));
			}
		}

		public double AttributeAdjustment
		{
			[CompilerGenerated]
			get
			{
				return _003CAttributeAdjustment_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CAttributeAdjustment_003Ek__BackingField, value, delegate(double v)
				{
					_003CAttributeAdjustment_003Ek__BackingField = v;
				}, new Action(OnMultiplierChanged));
			}
		}

		public Action OnClickAction { get; set; }

		public StatSelectable()
		{
			HeightSizingMode = SizingMode.AutoSize;
			base.BorderWidth = new RectangleDimensions(2);
			base.BorderColor = Color.Black;
			base.BackgroundColor = Color.Black * 0.4f;
			base.ContentPadding = new RectangleDimensions(5);
			_name = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				WrapText = false,
				AutoSizeHeight = true,
				Font = Control.Content.DefaultFont16,
				TextColor = ContentService.Colors.ColonialWhite
			};
			_statSummary = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				AutoSizeHeight = true,
				VerticalAlignment = VerticalAlignment.Top
			};
			_icon = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = this,
				Size = new Point(48),
				Location = new Point(2, 2)
			};
			_created = true;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			OnClickAction?.Invoke();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (_created)
			{
				_name?.SetSize(base.ContentRegion.Width - _icon.Width - 10, _name.Font.LineHeight);
				_name?.SetLocation(_icon.Right + 10, _icon.Top);
				_statSummary?.SetLocation(_name.Left, _name.Bottom);
				_statSummary?.SetSize(_name.Width, base.ContentRegion.Height - _name.Height);
				_vignetteBounds = _icon.LocalBounds.Add(0, 0, 10, 10);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _vignetteBounds, Rectangle.Empty, Color.Gray * 0.3f, 0f, Vector2.Zero);
			spriteBatch.DrawOnCtrl(this, _textureVignette, _vignetteBounds, _textureVignette.Bounds, Color.Black, 0f, Vector2.Zero);
		}

		private void OnStatChanged()
		{
			_name.SetLocalizedText = () => Stat?.Name;
			_statSummary.SetLocalizedText = () => Stat?.Attributes.ToString(AttributeAdjustment);
			_icon.Texture = Stat?.Icon.Texture;
			_icon.SourceRectangle = Stat?.Icon.TextureRegion ?? Rectangle.Empty;
		}

		private void OnMultiplierChanged()
		{
			_statSummary.SetLocalizedText = () => Stat?.Attributes.ToString(AttributeAdjustment);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Stat = null;
			_textureVignette.Dispose();
			base.DisposeControl();
		}
	}
}
