using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Enums;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class Trait_Control : Control
	{
		private double _scale = 1.0;

		private const int _TraitSize = 38;

		private readonly Texture2D _Line;

		public int Index;

		public int TraitIndex;

		public int SpecIndex;

		public API.traitType TraitType;

		private readonly CustomTooltip CustomTooltip;

		private readonly Specialization_Control Specialization_Control;

		private readonly API.Trait _Trait;

		public bool Hovered;

		public Point DefaultPoint;

		public Point Point;

		public Rectangle Bounds;

		public Rectangle DefaultBounds;

		public ConnectorLine PreLine = new ConnectorLine();

		public ConnectorLine PostLine = new ConnectorLine();

		public EventHandler Changed;

		private readonly List<Point> _MinorTraits = new List<Point>
		{
			new Point(215, 47),
			new Point(360, 47),
			new Point(505, 47)
		};

		public double Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				_scale = value;
				UpdateLayout();
			}
		}

		public Template Template => BuildsManager.s_moduleInstance.Selected_Template;

		public API.Trait Trait
		{
			get
			{
				if (TraitType != API.traitType.Major)
				{
					return Template.Build.SpecLines[SpecIndex].Specialization?.MinorTraits[TraitIndex];
				}
				return Template.Build.SpecLines[SpecIndex].Specialization?.MajorTraits[TraitIndex];
			}
			set
			{
			}
		}

		public bool Selected
		{
			get
			{
				API.Trait trait = ((TraitType != API.traitType.Major) ? Template.Build.SpecLines[SpecIndex].Specialization?.MinorTraits[TraitIndex] : Template.Build.SpecLines[SpecIndex].Specialization?.MajorTraits[TraitIndex]);
				if (Template != null && trait != null)
				{
					if (trait.Type != API.traitType.Minor)
					{
						return Template.Build.SpecLines[SpecIndex].Traits.Contains(trait);
					}
					return true;
				}
				return false;
			}
		}

		public Trait_Control(Container parent, Point p, API.Trait trait, CustomTooltip customTooltip, Specialization_Control specialization_Control)
			: this()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			CustomTooltip = customTooltip;
			Specialization_Control = specialization_Control;
			DefaultPoint = p;
			((Control)this).set_Location(p);
			DefaultBounds = new Rectangle(0, 0, 38, 38);
			_Trait = trait;
			((Control)this).set_Size(new Point(38, 38));
			_Line = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Line), new Rectangle(22, 15, 85, 5));
			((Control)this).set_ClipsBounds(false);
			UpdateLayout();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseEntered(e);
			if (Trait != null)
			{
				((Control)CustomTooltip).set_Visible(true);
				if (CustomTooltip.CurrentObject != this)
				{
					CustomTooltip.CurrentObject = this;
					CustomTooltip.Header = Trait.Name;
					CustomTooltip.HeaderColor = new Color(255, 204, 119, 255);
					CustomTooltip.TooltipContent = new List<string> { (Trait.Description == string.Empty) ? "No Description in API" : Trait.Description };
				}
			}
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			if (CustomTooltip.CurrentObject == this)
			{
				((Control)CustomTooltip).set_Visible(false);
			}
		}

		protected override void OnClick(MouseEventArgs mouse)
		{
			((Control)this).OnClick(mouse);
			if (Trait == null || Trait.Type != API.traitType.Major || Template == null || !((Control)this).get_MouseOver())
			{
				return;
			}
			if (Template.Build.SpecLines[Specialization_Control.Index].Traits.Contains(Trait))
			{
				Template.Build.SpecLines[Specialization_Control.Index].Traits.Remove(Trait);
			}
			else
			{
				foreach (API.Trait t in Template.Build.SpecLines[Specialization_Control.Index].Traits.Where((API.Trait e) => e.Tier == Trait.Tier).ToList())
				{
					Template.Build.SpecLines[Specialization_Control.Index].Traits.Remove(t);
				}
				Template.Build.SpecLines[Specialization_Control.Index].Traits.Add(Trait);
			}
			Template.SetChanged();
		}

		private void UpdateLayout()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			Bounds = DefaultBounds.Scale(Scale);
			((Control)this).set_Location(DefaultPoint.Scale(Scale));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			if (Template != null)
			{
				_ = Bounds;
				API.Trait trait = ((TraitType != API.traitType.Major) ? Template.Build.SpecLines[SpecIndex].Specialization?.MinorTraits[TraitIndex] : Template.Build.SpecLines[SpecIndex].Specialization?.MajorTraits[TraitIndex]);
				if (trait != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(trait.Icon._AsyncTexture), Bounds, (Rectangle?)trait.Icon._AsyncTexture.get_Texture().get_Bounds(), Selected ? Color.get_White() : (((Control)this).get_MouseOver() ? Color.get_LightGray() : Color.get_Gray()), 0f, default(Vector2), (SpriteEffects)0);
				}
			}
		}

		public void SetTemplate()
		{
		}
	}
}
