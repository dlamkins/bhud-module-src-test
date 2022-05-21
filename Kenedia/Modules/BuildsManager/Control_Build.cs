using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class Control_Build : Control
	{
		private Texture2D _Background;

		private Texture2D _SpecFrame;

		private Texture2D _SpecHighlightFrame;

		private Texture2D _PlaceHolderTexture;

		private Texture2D _EmptyTexture;

		private Texture2D _EmptyTraitLine;

		private Texture2D _EliteFrame;

		private Texture2D _Line;

		private Texture2D _SpecSideSelector;

		private Texture2D _SpecSideSelector_Hovered;

		private int _FrameWidth = 1;

		private int _LineThickness = 5;

		public int _Width = 643;

		public int _Height = 482;

		private int _HighlightLeft = 570;

		private int _TraitSize = 38;

		private int _SpecSelectSize = 64;

		private int _MiddleRowTop = 47;

		private int FrameThickness = 1;

		private int Gap = 5;

		private int Build_Width = 900;

		private int Skillbar_Height = 130;

		private int Build_Height = 575;

		private double _Scale = 1.0;

		private List<Specialization_Control> Specializations;

		public SkillBar_Control SkillBar;

		private CustomTooltip CustomTooltip;

		public EventHandler Changed;

		public Template Template => BuildsManager.ModuleInstance.Selected_Template;

		public double Scale
		{
			get
			{
				return _Scale;
			}
			set
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Expected O, but got Unknown
				_Scale = value;
				Point p = default(Point);
				((Point)(ref p))._002Ector(((Control)this).get_Location().X, ((Control)this).get_Location().Y);
				Point s = default(Point);
				((Point)(ref s))._002Ector(((Control)this).get_Size().X, ((Control)this).get_Size().Y);
				((Control)this).set_Size(new Point((int)((double)_Width * Scale), (int)((double)_Height * Scale)));
				foreach (Specialization_Control specialization in Specializations)
				{
					specialization.Scale = value;
				}
				SkillBar.Scale = value;
				UpdateLayout();
				((Control)this).OnResized(new ResizedEventArgs(p, s));
			}
		}

		public Control_Build(Container parent)
			: this()
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((Control)this).set_Size(new Point((int)((double)_Width * Scale), (int)((double)_Height * Scale)));
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			customTooltip.HeaderColor = new Color(255, 204, 119, 255);
			CustomTooltip = customTooltip;
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnClick);
			_SpecSideSelector_Hovered = BuildsManager.TextureManager.getControlTexture(_Controls.SpecSideSelector_Hovered);
			_SpecSideSelector = BuildsManager.TextureManager.getControlTexture(_Controls.SpecSideSelector);
			_EliteFrame = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.EliteFrame), 0, 4, 625, 130);
			_SpecHighlightFrame = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.SpecHighlight), 12, 5, 103, 116);
			_SpecFrame = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.SpecFrame), 0, 0, 647, 136);
			_EmptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 647, 136);
			_PlaceHolderTexture = BuildsManager.TextureManager._Icons[1];
			_EmptyTexture = BuildsManager.TextureManager._Icons[0];
			_Line = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.Line), new Rectangle(22, 15, 85, 5));
			_Background = _EmptyTraitLine;
			SkillBar_Control obj = new SkillBar_Control(((Control)this).get_Parent())
			{
				_Location = new Point(0, 0)
			};
			((Control)obj).set_Size(new Point(_Width, Skillbar_Height));
			SkillBar = obj;
			Specializations = new List<Specialization_Control>();
			for (int i = 0; i < Template.Build.SpecLines.Count; i++)
			{
				List<Specialization_Control> specializations = Specializations;
				Specialization_Control specialization_Control = new Specialization_Control(((Control)this).get_Parent(), i, new Point(0, 5 + Skillbar_Height + i * 134), CustomTooltip);
				((Control)specialization_Control).set_ZIndex(((Control)this).get_ZIndex() + 1);
				specialization_Control.Elite = i == 2;
				specializations.Add(specialization_Control);
			}
			((Control)this).add_Disposed((EventHandler<EventArgs>)delegate
			{
				((Control)CustomTooltip).Dispose();
			});
			UpdateTemplate();
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
		}

		private void OnChanged()
		{
			Changed?.Invoke(this, EventArgs.Empty);
		}

		private void OnClick(object sender, MouseEventArgs m)
		{
		}

		private void UpdateTemplate()
		{
			for (int i = 0; i < Template.Build.SpecLines.Count; i++)
			{
				Template.Build.SpecLines[i].Control = Specializations[i];
			}
		}

		private void UpdateLayout()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}

		public void SetTemplate()
		{
			_ = BuildsManager.ModuleInstance.Selected_Template;
		}
	}
}
