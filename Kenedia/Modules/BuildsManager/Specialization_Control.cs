using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class Specialization_Control : Control
	{
		private const int _FrameWidth = 1;

		private const int _LineThickness = 5;

		public const int _Width = 643;

		public const int _Height = 133;

		private const int _HighlightLeft = 570;

		private const int _TraitSize = 38;

		private int _SpecSelectSize = 64;

		private const int _MiddleRowTop = 47;

		private double _Scale = 1.0;

		public bool SelectorActive;

		public bool Elite;

		public int Index;

		private API.Specialization _Specialization;

		public EventHandler Changed;

		private Texture2D _SpecSideSelector_Hovered;

		private Texture2D _SpecSideSelector;

		private Texture2D _EliteFrame;

		private Texture2D _SpecHighlightFrame;

		private Texture2D _SpecFrame;

		private Texture2D _EmptyTraitLine;

		private Texture2D _Line;

		private Point DefaultLocation;

		private Rectangle AbsoluteBounds;

		private Rectangle ContentBounds;

		private Rectangle HighlightBounds;

		private Rectangle SelectorBounds;

		private Rectangle SpecSelectorBounds;

		private Rectangle WeaponTraitBounds;

		private ConnectorLine FirstLine = new ConnectorLine();

		private CustomTooltip CustomTooltip;

		private SpecializationSelector_Control Selector;

		public List<Trait_Control> _MinorTraits = new List<Trait_Control>();

		public List<Trait_Control> _MajorTraits = new List<Trait_Control>();

		private bool _Created;

		private bool SpecHovered;

		public double Scale
		{
			get
			{
				return _Scale;
			}
			set
			{
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				_Scale = value;
				foreach (Trait_Control majorTrait in _MajorTraits)
				{
					majorTrait.Scale = value;
				}
				foreach (Trait_Control minorTrait in _MinorTraits)
				{
					minorTrait.Scale = value;
				}
				((Control)this).set_Location(DefaultLocation.Scale(value));
				((Control)this).set_Size(ClassExtensions.Scale(new Point(643, 133), value));
				UpdateLayout();
			}
		}

		public Template Template => BuildsManager.ModuleInstance.Selected_Template;

		public API.Specialization Specialization
		{
			get
			{
				if (Template == null)
				{
					return null;
				}
				return Template.Build.SpecLines[Index].Specialization;
			}
		}

		public Specialization_Control(Container parent, int index, Point p, CustomTooltip customTooltip)
			: this()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_065a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			CustomTooltip = customTooltip;
			Index = index;
			((Control)this).set_Size(new Point(643, 133));
			DefaultLocation = p;
			((Control)this).set_Location(p);
			_SpecSideSelector_Hovered = BuildsManager.TextureManager.getControlTexture(_Controls.SpecSideSelector_Hovered);
			_SpecSideSelector = BuildsManager.TextureManager.getControlTexture(_Controls.SpecSideSelector);
			_EliteFrame = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.EliteFrame), 0, 4, 625, 130);
			_SpecHighlightFrame = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.SpecHighlight), 12, 5, 103, 116);
			_SpecFrame = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.SpecFrame), 0, 0, 647, 136);
			_EmptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 647, 136);
			_Line = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.Line), new Rectangle(22, 15, 85, 5));
			List<Trait_Control> list = new List<Trait_Control>();
			Trait_Control trait_Control = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(215, 47), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MinorTraits[0] : null, CustomTooltip, this);
			((Control)trait_Control).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control.TraitIndex = 0;
			trait_Control.SpecIndex = Index;
			trait_Control.TraitType = API.traitType.Minor;
			list.Add(trait_Control);
			Trait_Control trait_Control2 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(360, 47), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MinorTraits[1] : null, CustomTooltip, this);
			((Control)trait_Control2).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control2.TraitIndex = 1;
			trait_Control2.SpecIndex = Index;
			trait_Control2.TraitType = API.traitType.Minor;
			list.Add(trait_Control2);
			Trait_Control trait_Control3 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(505, 47), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MinorTraits[2] : null, CustomTooltip, this);
			((Control)trait_Control3).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control3.TraitIndex = 2;
			trait_Control3.SpecIndex = Index;
			trait_Control3.TraitType = API.traitType.Minor;
			list.Add(trait_Control3);
			_MinorTraits = list;
			List<Trait_Control> list2 = new List<Trait_Control>();
			Trait_Control trait_Control4 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(285, 6), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[0] : null, CustomTooltip, this);
			((Control)trait_Control4).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control4.TraitIndex = 0;
			trait_Control4.SpecIndex = Index;
			trait_Control4.TraitType = API.traitType.Major;
			list2.Add(trait_Control4);
			Trait_Control trait_Control5 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(285, 47), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[1] : null, CustomTooltip, this);
			((Control)trait_Control5).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control5.TraitIndex = 1;
			trait_Control5.SpecIndex = Index;
			trait_Control5.TraitType = API.traitType.Major;
			list2.Add(trait_Control5);
			Trait_Control trait_Control6 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(285, 88), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[2] : null, CustomTooltip, this);
			((Control)trait_Control6).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control6.TraitIndex = 2;
			trait_Control6.SpecIndex = Index;
			trait_Control6.TraitType = API.traitType.Major;
			list2.Add(trait_Control6);
			Trait_Control trait_Control7 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(430, 6), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[3] : null, CustomTooltip, this);
			((Control)trait_Control7).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control7.TraitIndex = 3;
			trait_Control7.SpecIndex = Index;
			trait_Control7.TraitType = API.traitType.Major;
			list2.Add(trait_Control7);
			Trait_Control trait_Control8 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(430, 47), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[4] : null, CustomTooltip, this);
			((Control)trait_Control8).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control8.TraitIndex = 4;
			trait_Control8.SpecIndex = Index;
			trait_Control8.TraitType = API.traitType.Major;
			list2.Add(trait_Control8);
			Trait_Control trait_Control9 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(430, 88), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[5] : null, CustomTooltip, this);
			((Control)trait_Control9).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control9.TraitIndex = 5;
			trait_Control9.SpecIndex = Index;
			trait_Control9.TraitType = API.traitType.Major;
			list2.Add(trait_Control9);
			Trait_Control trait_Control10 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(575, 6), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[6] : null, CustomTooltip, this);
			((Control)trait_Control10).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control10.TraitIndex = 6;
			trait_Control10.SpecIndex = Index;
			trait_Control10.TraitType = API.traitType.Major;
			list2.Add(trait_Control10);
			Trait_Control trait_Control11 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(575, 47), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[7] : null, CustomTooltip, this);
			((Control)trait_Control11).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control11.TraitIndex = 7;
			trait_Control11.SpecIndex = Index;
			trait_Control11.TraitType = API.traitType.Major;
			list2.Add(trait_Control11);
			Trait_Control trait_Control12 = new Trait_Control(((Control)this).get_Parent(), ClassExtensions.Add(new Point(575, 88), ((Control)this).get_Location()), (Specialization != null) ? Specialization.MajorTraits[8] : null, CustomTooltip, this);
			((Control)trait_Control12).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control12.TraitIndex = 8;
			trait_Control12.SpecIndex = Index;
			trait_Control12.TraitType = API.traitType.Major;
			list2.Add(trait_Control12);
			_MajorTraits = list2;
			Template.Build.SpecLines.Find((SpecLine e) => e.Specialization == Specialization);
			foreach (Trait_Control majorTrait in _MajorTraits)
			{
				((Control)majorTrait).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					UpdateLayout();
					foreach (API.Trait current in Template.Build.SpecLines[Index].Traits)
					{
						BuildsManager.Logger.Debug(current.Name);
					}
				});
			}
			SpecializationSelector_Control obj = new SpecializationSelector_Control
			{
				Index = Index,
				Specialization_Control = this
			};
			((Control)obj).set_Parent(((Control)this).get_Parent());
			((Control)obj).set_Visible(false);
			((Control)obj).set_ZIndex(((Control)this).get_ZIndex() + 2);
			obj.Elite = Elite;
			Selector = obj;
			UpdateLayout();
			((Control)this).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				UpdateLayout();
			});
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				UpdateLayout();
			});
			((Control)this).add_Click((EventHandler<MouseEventArgs>)Control_Click);
			_Created = true;
		}

		private void Control_Click(object sender, MouseEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_MouseOver())
			{
				return;
			}
			Rectangle val = HighlightBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			bool highlight = ((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition());
			val = SelectorBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()) || highlight)
			{
				if (((Control)Selector).get_Visible())
				{
					((Control)Selector).set_Visible(false);
					return;
				}
				((Control)Selector).set_Visible(true);
				SpecializationSelector_Control selector = Selector;
				val = ((Control)this).get_LocalBounds();
				((Control)selector).set_Location(((Rectangle)(ref val)).get_Location().Add(new Point(SelectorBounds.Width, 0)));
				SpecializationSelector_Control selector2 = Selector;
				val = ((Control)this).get_LocalBounds();
				((Control)selector2).set_Size(((Rectangle)(ref val)).get_Size().Add(new Point(-SelectorBounds.Width, 0)));
				Selector.Elite = Elite;
				Selector.Specialization = Specialization;
			}
		}

		public void UpdateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			AbsoluteBounds = ClassExtensions.Scale(new Rectangle(0, 0, 645, 135), Scale).Add(((Control)this).get_Location());
			ContentBounds = ClassExtensions.Scale(new Rectangle(1, 1, 643, 133), Scale).Add(((Control)this).get_Location());
			SelectorBounds = ClassExtensions.Scale(new Rectangle(1, 1, 15, 133), Scale).Add(((Control)this).get_Location());
			HighlightBounds = ClassExtensions.Scale(new Rectangle(73, (133 - _SpecHighlightFrame.get_Height()) / 2, _SpecHighlightFrame.get_Width(), _SpecHighlightFrame.get_Height()), Scale).Add(((Control)this).get_Location());
			SpecSelectorBounds = ClassExtensions.Scale(new Rectangle(1, 1, 643, 133), Scale).Add(((Control)this).get_Location());
			FirstLine.Bounds = new Rectangle(((Rectangle)(ref HighlightBounds)).get_Right() - 5.Scale(_Scale), ((Rectangle)(ref HighlightBounds)).get_Center().Y, 225 - ((Rectangle)(ref HighlightBounds)).get_Right(), 5.Scale(_Scale));
			WeaponTraitBounds = ClassExtensions.Scale(new Rectangle(((Rectangle)(ref HighlightBounds)).get_Right() - 38 - 6, 133 + HighlightBounds.Height - 165, 38, 38), Scale).Add(((Control)this).get_Location());
			Rectangle val = HighlightBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			SpecHovered = ((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition());
			if (SpecHovered)
			{
				((Control)this).set_BasicTooltipText(Specialization?.Name);
			}
			else
			{
				((Control)this).set_BasicTooltipText((string)null);
			}
			foreach (Trait_Control trait in _MajorTraits)
			{
				if (trait.Selected)
				{
					Trait_Control minor = _MinorTraits[trait.Trait.Tier - 1];
					float rotation = 0f;
					switch (trait.Trait.Order)
					{
					case 0:
						rotation = -0.5560341f;
						break;
					case 1:
						rotation = 0f;
						break;
					case 2:
						rotation = 0.5560341f;
						break;
					}
					trait.PreLine.Rotation = rotation;
					trait.PostLine.Rotation = 0f - rotation;
					val = ((Control)minor).get_LocalBounds();
					Point minor_Pos = ((Rectangle)(ref val)).get_Center();
					val = ((Control)trait).get_LocalBounds();
					Point majorPos = ((Rectangle)(ref val)).get_Center();
					ConnectorLine preLine = trait.PreLine;
					int x = minor_Pos.X;
					int y = minor_Pos.Y;
					val = ((Control)minor).get_AbsoluteBounds();
					Point center = ((Rectangle)(ref val)).get_Center();
					val = ((Control)trait).get_AbsoluteBounds();
					preLine.Bounds = new Rectangle(x, y, center.Distance2D(((Rectangle)(ref val)).get_Center()), 5.Scale(_Scale));
					if (trait.Selected && trait.Trait.Tier != 3)
					{
						minor = _MinorTraits[trait.Trait.Tier];
						ConnectorLine postLine = trait.PostLine;
						int x2 = majorPos.X;
						int y2 = majorPos.Y;
						val = ((Control)trait).get_AbsoluteBounds();
						Point center2 = ((Rectangle)(ref val)).get_Center();
						val = ((Control)minor).get_AbsoluteBounds();
						postLine.Bounds = new Rectangle(x2, y2, center2.Distance2D(((Rectangle)(ref val)).get_Center()), 5.Scale(_Scale));
					}
				}
				else
				{
					trait.PreLine = new ConnectorLine();
					trait.PostLine = new ConnectorLine();
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			UpdateLayout();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Textures.get_Pixel(), AbsoluteBounds, (Rectangle?)AbsoluteBounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _EmptyTraitLine, ContentBounds, (Rectangle?)_EmptyTraitLine.get_Bounds(), new Color(135, 135, 135, 255), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (Specialization != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), AsyncTexture2D.op_Implicit(Specialization.Background._AsyncTexture), ContentBounds, (Rectangle?)Specialization.Background._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				_ = FirstLine.Bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _Line, FirstLine.Bounds, (Rectangle?)_Line.get_Bounds(), Color.get_White(), FirstLine.Rotation, Vector2.get_Zero(), (SpriteEffects)0);
				foreach (Trait_Control trait in _MajorTraits)
				{
					_ = trait.PreLine.Bounds;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _Line, trait.PreLine.Bounds, (Rectangle?)_Line.get_Bounds(), Color.get_White(), trait.PreLine.Rotation, Vector2.get_Zero(), (SpriteEffects)0);
					_ = trait.PostLine.Bounds;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _Line, trait.PostLine.Bounds, (Rectangle?)_Line.get_Bounds(), Color.get_White(), trait.PostLine.Rotation, Vector2.get_Zero(), (SpriteEffects)0);
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _SpecFrame, ContentBounds, (Rectangle?)_SpecFrame.get_Bounds(), Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _SpecHighlightFrame, HighlightBounds, (Rectangle?)_SpecHighlightFrame.get_Bounds(), (Color)((Specialization != null) ? Color.get_White() : new Color(32, 32, 32, 125)), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (Elite)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _EliteFrame, ContentBounds, (Rectangle?)_EliteFrame.get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				if (Specialization != null && Specialization.WeaponTrait != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), AsyncTexture2D.op_Implicit(Specialization.WeaponTrait.Icon._AsyncTexture), WeaponTraitBounds, (Rectangle?)Specialization.WeaponTrait.Icon._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				}
			}
			if (((Control)Selector).get_Visible())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Textures.get_Pixel(), SelectorBounds, (Rectangle?)_SpecSideSelector.get_Bounds(), new Color(0, 0, 0, 205), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			}
			Container parent = ((Control)this).get_Parent();
			Rectangle val = SelectorBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)parent, ((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()) ? _SpecSideSelector_Hovered : _SpecSideSelector, SelectorBounds, (Rectangle?)_SpecSideSelector.get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
		}

		public void PaintAfterChilds(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}

		public void SetTemplate()
		{
			_ = BuildsManager.ModuleInstance.Selected_Template;
			BuildsManager.Logger.Debug("Set Spec No. {0}", new object[1] { Index });
		}
	}
}
