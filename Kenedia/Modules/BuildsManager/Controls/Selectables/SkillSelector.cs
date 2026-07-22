using System.Runtime.CompilerServices;
using Blish_HUD;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class SkillSelector : Selector<Skill>
	{
		private readonly DetailedTexture _selectingFrame = new DetailedTexture(157147);

		public Enviroment Enviroment
		{
			[CompilerGenerated]
			get
			{
				return _003CEnviroment_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CEnviroment_003Ek__BackingField, value, delegate(Enviroment v)
				{
					_003CEnviroment_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Enviroment>(OnEnviromentChanged));
			}
		}

		public SkillSelector()
		{
			ContentPanel.BorderWidth = new RectangleDimensions(2, 0, 2, 2);
			ContentPanel.ContentPadding = new RectangleDimensions(10);
			base.SelectableSize = new Point(56);
		}

		private void OnEnviromentChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Enviroment> e)
		{
			base.Controls.ForEach(delegate(Selectable<Skill> c)
			{
				SkillSelectable skillSelectable = c as SkillSelectable;
				if (skillSelectable != null)
				{
					skillSelectable.Enviroment = Enviroment;
				}
			});
		}

		protected override void OnDataApplied(Skill item)
		{
			base.OnDataApplied(item);
			base.Controls.ForEach(delegate(Selectable<Skill> c)
			{
				SkillSelectable skillSelectable = c as SkillSelectable;
				if (skillSelectable != null)
				{
					skillSelectable.IsSelected = c.Data == base.SelectedItem;
				}
			});
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			ContentPanel.BorderWidth = new RectangleDimensions(2);
			HeaderPanel.BorderWidth = new RectangleDimensions(0, 0, 0, 2);
			spriteBatch.DrawCenteredRotationOnCtrl(this, (Texture2D)_selectingFrame.Texture, _selectingFrame.Bounds, _selectingFrame.TextureRegion, Color.White, 0f, flipVertically: true, flipHorizontally: true);
		}

		protected override void Recalculate(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Point> e)
		{
			base.Recalculate(sender, e);
		}

		protected override Selectable<Skill> CreateSelectable(Skill item)
		{
			base.Type = SelectableType.Skill;
			base.Visible = true;
			return new SkillSelectable
			{
				Parent = FlowPanel,
				Size = base.SelectableSize,
				Data = item,
				OnClickAction = base.OnClickAction,
				IsSelected = (base.PassSelected && item.Equals(base.SelectedItem)),
				Enviroment = Enviroment
			};
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (ContentPanel != null && HeaderPanel != null)
			{
				ContentPanel.ContentPadding = new RectangleDimensions(8);
				ControlExtensions.SetBounds(bounds: new Rectangle(Point.Zero, new Point(ContentPanel.Width, base.SelectableSize.Y + 10)), c: HeaderPanel?);
				int pad = 20;
				_selectingFrame.Bounds = new Rectangle(-pad - 1, 0, HeaderPanel.Width + pad * 2 + 4, HeaderPanel.Height + 2);
			}
		}

		protected override void SetCapture()
		{
			if (HeaderPanel != null)
			{
				int selectorWidth = (int)(27.0 / 128.0 * (double)HeaderPanel.Width);
				int pad = 5;
				BlockInputRegion = new Rectangle(HeaderPanel.Location.Add(new Point((HeaderPanel.Width - selectorWidth) / 2, 0)), new Point(selectorWidth, HeaderPanel.Height)).Add(new Rectangle(-pad - 2, -pad, pad * 2 + 6, pad * 2));
				base.CaptureInput = !HeaderPanel.MouseOver || BlockInputRegion.Contains(base.RelativeMousePosition);
				HeaderPanel.CaptureInput = !HeaderPanel.MouseOver || BlockInputRegion.Contains(base.RelativeMousePosition);
			}
		}
	}
}
