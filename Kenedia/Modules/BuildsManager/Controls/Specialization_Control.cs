using System;
using System.Collections.Generic;
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
	public class Specialization_Control : Control
	{
		private const int s_frameWidth = 1;

		private const int s_lineThickness = 5;

		private const int s_highlightLeft = 570;

		private const int s_traitSize = 38;

		private readonly int _middleRowTop;

		private double _scale = 1.0;

		public bool SelectorActive;

		public bool Elite;

		public int Index;

		public EventHandler Changed;

		private Texture2D _specSideSelectorHovered;

		private Texture2D _specSideSelector;

		private Texture2D _eliteFrame;

		private Texture2D _specHighlightFrame;

		private Texture2D _specFrame;

		private Texture2D _emptyTraitLine;

		private Texture2D _line;

		private Point _defaultLocation;

		private Rectangle _absoluteContentBounds;

		private Rectangle _contentBounds;

		private Rectangle _highlightBounds;

		private Rectangle _selectorBounds;

		private Rectangle _specSelectorBounds;

		private Rectangle _weaponTraitBounds;

		private readonly ConnectorLine _firstLine = new ConnectorLine();

		private readonly CustomTooltip _customTooltip;

		private readonly SpecializationSelector_Control _selector;

		public List<Trait_Control> MinorTraits = new List<Trait_Control>();

		public List<Trait_Control> MajorTraits = new List<Trait_Control>();

		private readonly bool _created;

		private bool _specHovered;

		public double Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				_scale = value;
				foreach (Trait_Control majorTrait in MajorTraits)
				{
					majorTrait.Scale = value;
				}
				foreach (Trait_Control minorTrait in MinorTraits)
				{
					minorTrait.Scale = value;
				}
				((Control)this).set_Location(_defaultLocation.Scale(value));
				((Control)this).set_Size(PointExtension.Scale(new Point(((Control)this).get_Width(), ((Control)this).get_Height()), value));
				UpdateLayout();
			}
		}

		public Template Template => BuildsManager.s_moduleInstance.Selected_Template;

		public API.Specialization Specialization => Template?.Build.SpecLines[Index].Specialization;

		public Specialization_Control(Container parent, int index, Point p, CustomTooltip customTooltip)
			: this()
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_043d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_066d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Width(643);
			((Control)this).set_Height(133);
			_middleRowTop = (((Control)this).get_Height() - 38) / 2;
			((Control)this).set_Parent(parent);
			_customTooltip = customTooltip;
			Index = index;
			((Control)this).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height()));
			_defaultLocation = p;
			((Control)this).set_Location(p);
			_specSideSelectorHovered = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.SpecSideSelector_Hovered);
			_specSideSelector = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.SpecSideSelector);
			_eliteFrame = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.EliteFrame), 0, 4, 625, 130);
			_specHighlightFrame = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.SpecHighlight), 12, 5, 103, 116);
			_specFrame = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.SpecFrame), 0, 0, 647, 136);
			_emptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.PlaceHolder_Traitline), 0, 0, 647, 136);
			_line = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Line), new Rectangle(22, 15, 85, 5));
			List<Trait_Control> list = new List<Trait_Control>();
			Trait_Control trait_Control = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(215, 47), ((Control)this).get_Location()), Specialization?.MinorTraits[0], _customTooltip, this);
			((Control)trait_Control).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control.TraitIndex = 0;
			trait_Control.SpecIndex = Index;
			trait_Control.TraitType = API.traitType.Minor;
			list.Add(trait_Control);
			Trait_Control trait_Control2 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(360, 47), ((Control)this).get_Location()), Specialization?.MinorTraits[1], _customTooltip, this);
			((Control)trait_Control2).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control2.TraitIndex = 1;
			trait_Control2.SpecIndex = Index;
			trait_Control2.TraitType = API.traitType.Minor;
			list.Add(trait_Control2);
			Trait_Control trait_Control3 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(505, 47), ((Control)this).get_Location()), Specialization?.MinorTraits[2], _customTooltip, this);
			((Control)trait_Control3).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control3.TraitIndex = 2;
			trait_Control3.SpecIndex = Index;
			trait_Control3.TraitType = API.traitType.Minor;
			list.Add(trait_Control3);
			MinorTraits = list;
			List<Trait_Control> list2 = new List<Trait_Control>();
			Trait_Control trait_Control4 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(285, 6), ((Control)this).get_Location()), Specialization?.MajorTraits[0], _customTooltip, this);
			((Control)trait_Control4).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control4.TraitIndex = 0;
			trait_Control4.SpecIndex = Index;
			trait_Control4.TraitType = API.traitType.Major;
			list2.Add(trait_Control4);
			Trait_Control trait_Control5 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(285, 47), ((Control)this).get_Location()), Specialization?.MajorTraits[1], _customTooltip, this);
			((Control)trait_Control5).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control5.TraitIndex = 1;
			trait_Control5.SpecIndex = Index;
			trait_Control5.TraitType = API.traitType.Major;
			list2.Add(trait_Control5);
			Trait_Control trait_Control6 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(285, 88), ((Control)this).get_Location()), Specialization?.MajorTraits[2], _customTooltip, this);
			((Control)trait_Control6).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control6.TraitIndex = 2;
			trait_Control6.SpecIndex = Index;
			trait_Control6.TraitType = API.traitType.Major;
			list2.Add(trait_Control6);
			Trait_Control trait_Control7 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(430, 6), ((Control)this).get_Location()), Specialization?.MajorTraits[3], _customTooltip, this);
			((Control)trait_Control7).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control7.TraitIndex = 3;
			trait_Control7.SpecIndex = Index;
			trait_Control7.TraitType = API.traitType.Major;
			list2.Add(trait_Control7);
			Trait_Control trait_Control8 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(430, 47), ((Control)this).get_Location()), Specialization?.MajorTraits[4], _customTooltip, this);
			((Control)trait_Control8).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control8.TraitIndex = 4;
			trait_Control8.SpecIndex = Index;
			trait_Control8.TraitType = API.traitType.Major;
			list2.Add(trait_Control8);
			Trait_Control trait_Control9 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(430, 88), ((Control)this).get_Location()), Specialization?.MajorTraits[5], _customTooltip, this);
			((Control)trait_Control9).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control9.TraitIndex = 5;
			trait_Control9.SpecIndex = Index;
			trait_Control9.TraitType = API.traitType.Major;
			list2.Add(trait_Control9);
			Trait_Control trait_Control10 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(575, 6), ((Control)this).get_Location()), Specialization?.MajorTraits[6], _customTooltip, this);
			((Control)trait_Control10).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control10.TraitIndex = 6;
			trait_Control10.SpecIndex = Index;
			trait_Control10.TraitType = API.traitType.Major;
			list2.Add(trait_Control10);
			Trait_Control trait_Control11 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(575, 47), ((Control)this).get_Location()), Specialization?.MajorTraits[7], _customTooltip, this);
			((Control)trait_Control11).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control11.TraitIndex = 7;
			trait_Control11.SpecIndex = Index;
			trait_Control11.TraitType = API.traitType.Major;
			list2.Add(trait_Control11);
			Trait_Control trait_Control12 = new Trait_Control(((Control)this).get_Parent(), PointExtension.Add(new Point(575, 88), ((Control)this).get_Location()), Specialization?.MajorTraits[8], _customTooltip, this);
			((Control)trait_Control12).set_ZIndex(((Control)this).get_ZIndex() + 1);
			trait_Control12.TraitIndex = 8;
			trait_Control12.SpecIndex = Index;
			trait_Control12.TraitType = API.traitType.Major;
			list2.Add(trait_Control12);
			MajorTraits = list2;
			Template.Build.SpecLines.Find((SpecLine e) => e.Specialization == Specialization);
			foreach (Trait_Control majorTrait in MajorTraits)
			{
				((Control)majorTrait).add_Click((EventHandler<MouseEventArgs>)Trait_Click);
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
			_selector = obj;
			_created = true;
			UpdateLayout();
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			((Control)this).OnMoved(e);
			UpdateLayout();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Control)this).OnResized(e);
			UpdateLayout();
		}

		private void Trait_Click(object sender, MouseEventArgs e)
		{
			UpdateLayout();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			if (!((Control)this).get_MouseOver())
			{
				return;
			}
			Rectangle val = _highlightBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			bool highlight = ((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition());
			val = _selectorBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()) || highlight)
			{
				if (((Control)_selector).get_Visible())
				{
					((Control)_selector).set_Visible(false);
					return;
				}
				((Control)_selector).set_Visible(true);
				SpecializationSelector_Control selector = _selector;
				val = ((Control)this).get_LocalBounds();
				((Control)selector).set_Location(((Rectangle)(ref val)).get_Location().Add(new Point(_selectorBounds.Width, 0)));
				SpecializationSelector_Control selector2 = _selector;
				val = ((Control)this).get_LocalBounds();
				((Control)selector2).set_Size(((Rectangle)(ref val)).get_Size().Add(new Point(-_selectorBounds.Width, 0)));
				_selector.Elite = Elite;
				_selector.Specialization = Specialization;
			}
		}

		public void UpdateLayout()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			if (!_created)
			{
				return;
			}
			_absoluteContentBounds = RectangleExtension.Scale(new Rectangle(0, 0, ((Control)this).get_Width() + 2, ((Control)this).get_Height() + 2), Scale).Add(((Control)this).get_Location());
			_contentBounds = RectangleExtension.Scale(new Rectangle(1, 1, ((Control)this).get_Width(), ((Control)this).get_Height()), Scale).Add(((Control)this).get_Location());
			_selectorBounds = RectangleExtension.Scale(new Rectangle(1, 1, 15, ((Control)this).get_Height()), Scale).Add(((Control)this).get_Location());
			_highlightBounds = RectangleExtension.Scale(new Rectangle(((Control)this).get_Width() - 570, (((Control)this).get_Height() - _specHighlightFrame.get_Height()) / 2, _specHighlightFrame.get_Width(), _specHighlightFrame.get_Height()), Scale).Add(((Control)this).get_Location());
			_specSelectorBounds = RectangleExtension.Scale(new Rectangle(1, 1, ((Control)this).get_Width(), ((Control)this).get_Height()), Scale).Add(((Control)this).get_Location());
			_firstLine.Bounds = new Rectangle(((Rectangle)(ref _highlightBounds)).get_Right() - 5.Scale(_scale), ((Rectangle)(ref _highlightBounds)).get_Center().Y, 225 - ((Rectangle)(ref _highlightBounds)).get_Right(), 5.Scale(_scale));
			_weaponTraitBounds = RectangleExtension.Scale(new Rectangle(((Rectangle)(ref _highlightBounds)).get_Right() - 38 - 6, ((Control)this).get_Height() + _highlightBounds.Height - 165, 38, 38), Scale).Add(((Control)this).get_Location());
			Rectangle val = _highlightBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			_specHovered = ((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition());
			if (_specHovered)
			{
				((Control)this).set_BasicTooltipText(Specialization?.Name);
			}
			else
			{
				((Control)this).set_BasicTooltipText((string)null);
			}
			foreach (Trait_Control trait in MajorTraits)
			{
				if (trait.Selected)
				{
					Trait_Control minor = MinorTraits[trait.Trait.Tier - 1];
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
					preLine.Bounds = new Rectangle(x, y, center.Distance2D(((Rectangle)(ref val)).get_Center()), 5.Scale(_scale));
					if (trait.Selected && trait.Trait.Tier != 3)
					{
						minor = MinorTraits[trait.Trait.Tier];
						ConnectorLine postLine = trait.PostLine;
						int x2 = majorPos.X;
						int y2 = majorPos.Y;
						val = ((Control)trait).get_AbsoluteBounds();
						Point center2 = ((Rectangle)(ref val)).get_Center();
						val = ((Control)minor).get_AbsoluteBounds();
						postLine.Bounds = new Rectangle(x2, y2, center2.Distance2D(((Rectangle)(ref val)).get_Center()), 5.Scale(_scale));
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
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Textures.get_Pixel(), _absoluteContentBounds, (Rectangle?)_absoluteContentBounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _emptyTraitLine, _contentBounds, (Rectangle?)_emptyTraitLine.get_Bounds(), new Color(135, 135, 135, 255), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (Specialization != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), AsyncTexture2D.op_Implicit(Specialization.Background._AsyncTexture), _contentBounds, (Rectangle?)Specialization.Background._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				_ = _firstLine.Bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _line, _firstLine.Bounds, (Rectangle?)_line.get_Bounds(), Color.get_White(), _firstLine.Rotation, Vector2.get_Zero(), (SpriteEffects)0);
				foreach (Trait_Control trait in MajorTraits)
				{
					_ = trait.PreLine.Bounds;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _line, trait.PreLine.Bounds, (Rectangle?)_line.get_Bounds(), Color.get_White(), trait.PreLine.Rotation, Vector2.get_Zero(), (SpriteEffects)0);
					_ = trait.PostLine.Bounds;
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _line, trait.PostLine.Bounds, (Rectangle?)_line.get_Bounds(), Color.get_White(), trait.PostLine.Rotation, Vector2.get_Zero(), (SpriteEffects)0);
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _specFrame, _contentBounds, (Rectangle?)_specFrame.get_Bounds(), Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _specHighlightFrame, _highlightBounds, (Rectangle?)_specHighlightFrame.get_Bounds(), (Color)((Specialization != null) ? Color.get_White() : new Color(32, 32, 32, 125)), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (Elite)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), _eliteFrame, _contentBounds, (Rectangle?)_eliteFrame.get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				if (Specialization != null && Specialization.WeaponTrait != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), AsyncTexture2D.op_Implicit(Specialization.WeaponTrait.Icon._AsyncTexture), _weaponTraitBounds, (Rectangle?)Specialization.WeaponTrait.Icon._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				}
			}
			if (((Control)_selector).get_Visible())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Textures.get_Pixel(), _selectorBounds, (Rectangle?)_specSideSelector.get_Bounds(), new Color(0, 0, 0, 205), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			}
			Container parent = ((Control)this).get_Parent();
			Rectangle val = _selectorBounds.Add(new Point(-((Control)this).get_Location().X, -((Control)this).get_Location().Y));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)parent, ((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()) ? _specSideSelectorHovered : _specSideSelector, _selectorBounds, (Rectangle?)_specSideSelector.get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			foreach (Trait_Control majorTrait in MajorTraits)
			{
				((Control)majorTrait).remove_Click((EventHandler<MouseEventArgs>)Trait_Click);
			}
			((IEnumerable<IDisposable>)MajorTraits)?.DisposeAll();
			MajorTraits.Clear();
			((IEnumerable<IDisposable>)MinorTraits)?.DisposeAll();
			MinorTraits.Clear();
			SpecializationSelector_Control selector = _selector;
			if (selector != null)
			{
				((Control)selector).Dispose();
			}
			_specSideSelectorHovered = null;
			_specSideSelector = null;
			_eliteFrame = null;
			_specHighlightFrame = null;
			_specFrame = null;
			_emptyTraitLine = null;
			_line = null;
		}
	}
}
