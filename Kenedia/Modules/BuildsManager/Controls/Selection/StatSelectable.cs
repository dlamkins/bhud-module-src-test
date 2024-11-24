using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Services;
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

		private double _attributeAdjustment;

		private Stat _stat;

		public Stat Stat
		{
			get
			{
				return _stat;
			}
			set
			{
				Common.SetProperty(ref _stat, value, new Action(OnStatChanged));
			}
		}

		public double AttributeAdjustment
		{
			get
			{
				return _attributeAdjustment;
			}
			set
			{
				Common.SetProperty(ref _attributeAdjustment, value, new Action(OnMultiplierChanged));
			}
		}

		public Action OnClickAction { get; set; }

		public StatSelectable()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			HeightSizingMode = SizingMode.AutoSize;
			base.BorderWidth = new RectangleDimensions(2);
			base.BorderColor = Color.get_Black();
			base.BackgroundColor = Color.get_Black() * 0.4f;
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
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_created)
			{
				_name?.SetSize(base.ContentRegion.Width - _icon.Width - 10, _name.Font.get_LineHeight());
				_name?.SetLocation(_icon.Right + 10, _icon.Top);
				_statSummary?.SetLocation(_name.Left, _name.Bottom);
				_statSummary?.SetSize(_name.Width, base.ContentRegion.Height - _name.Height);
				_vignetteBounds = _icon.LocalBounds.Add(0, 0, 10, 10);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _vignetteBounds, Rectangle.get_Empty(), Color.get_Gray() * 0.3f, 0f, Vector2.get_Zero(), (SpriteEffects)0);
			spriteBatch.DrawOnCtrl(this, _textureVignette, _vignetteBounds, _textureVignette.Bounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
		}

		private void OnStatChanged()
		{
			_name.SetLocalizedText = () => _stat?.Name;
			_statSummary.SetLocalizedText = () => _stat?.Attributes.ToString(AttributeAdjustment);
			_icon.Texture = TexturesService.GetTextureFromRef(_stat?.IconPath);
		}

		private void OnMultiplierChanged()
		{
			_statSummary.SetLocalizedText = () => _stat?.Attributes.ToString(AttributeAdjustment);
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
