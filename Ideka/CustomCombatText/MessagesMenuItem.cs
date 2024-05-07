using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.CustomCombatText
{
	public class MessagesMenuItem : MenuItem
	{
		private class MessageDraw : AnchoredRect
		{
			public List<TemplateParser.Fragment> ParsedFragments { get; set; } = new List<TemplateParser.Fragment>();


			protected override void EarlyDraw(RectTarget target)
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				base.EarlyDraw(target);
				if (CTextModule.Settings.Debug.Value)
				{
					ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_Black(), 1f, 0f);
				}
				target.DrawFragments(ParsedFragments, 1f, shadow: true, Vector2.get_One() * 1.2f, stroke: false);
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<MessagesMenu>();

		private const int Spacing = 5;

		private readonly MessageDraw _rect;

		public float InnerHeight { get; private set; }

		public MessagesMenu.MessageKey Key { get; private set; }

		public MessagesMenuItem(MessagesMenu.MessageKey key, List<TemplateParser.MarkupFragment> mFrags, int fontSize)
			: this()
		{
			Key = key;
			_rect = new MessageDraw();
			UpdateVisuals(key, mFrags, fontSize);
		}

		public void UpdateVisuals(MessagesMenu.MessageKey key, List<TemplateParser.MarkupFragment> mFrags, int fontSize)
		{
			if (Key != key)
			{
				((Control)this).set_BasicTooltipText((string)null);
				Tooltip tooltip = ((Control)this).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)this).set_Tooltip((Tooltip)null);
			}
			Key = key;
			UpdateVisuals(mFrags, fontSize);
		}

		public void UpdateVisuals(List<TemplateParser.MarkupFragment> mFrags, int fontSize)
		{
			_rect.ParsedFragments = TemplateParser.FinalParse(mFrags, null, CTextModule.FontAssets.Get(null, fontSize), Key.Message, new _003C_003Ez__ReadOnlyArray<Message>(new Message[1] { Key.Message })).ToList();
			InnerHeight = (_rect.ParsedFragments.Any() ? _rect.ParsedFragments.Max((TemplateParser.Fragment x) => x.Size.Height) : 0f) + 5f;
		}

		public void CreateDebugTooltip()
		{
			if (((Control)this).get_BasicTooltipText() == null)
			{
				Tooltip tooltip = ((Control)this).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)this).set_Tooltip((Tooltip)null);
				((Control)this).set_BasicTooltipText($"ID: {Key.Cbt.get_Id()}\n" + $"Skill ID: raw: {Key.Cbt.get_Ev().get_SkillId()} used: {Key.Message.SkillId}\n" + "Raw skill name: " + Key.Cbt.get_SkillName() + "\n" + $"Icon ID: {Key.Message.SkillIconId}\n" + "\n" + $"Src ID: {Key.Cbt.get_Src().get_Id()} / {Key.Cbt.get_Ev().get_SrcInstId()} (self: {Key.Message.Src.get_Self() == 1})\n" + "Src raw name: " + Key.Cbt.get_Src().get_Name() + "\n" + $"Src prof: {(object)(ProfessionType)(byte)Key.Cbt.get_Src().get_Profession()} / {Key.Message.Src.get_Elite()}\n" + "\n" + $"Dst ID: {Key.Cbt.get_Dst().get_Id()} / {Key.Cbt.get_Ev().get_DstInstId()} (self: {Key.Message.Dst.get_Self() == 1})\n" + "Dst raw name: " + Key.Cbt.get_Dst().get_Name() + "\n" + $"Dst prof: {(object)(ProfessionType)(byte)Key.Cbt.get_Dst().get_Profession()} / {Key.Message.Dst.get_Elite()}");
			}
		}

		public void CreateSkillTooltip()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BasicTooltipText((string)null);
			if (((Control)this).get_Tooltip() == null && Key.Message.HsSkill != null)
			{
				try
				{
					Tooltip val = new Tooltip();
					((Container)val).set_HeightSizingMode((SizingMode)1);
					((Container)val).set_WidthSizingMode((SizingMode)1);
					((Control)this).set_Tooltip(val);
					SkillTooltip skillTooltip = new SkillTooltip(new SkillTooltipData(Key.Message.HsSkill, Key.Message.HsSkill!.Icon ?? Key.Message.SkillIconId));
					((Control)skillTooltip).set_Parent((Container)(object)((Control)this).get_Tooltip());
					((Control)skillTooltip).set_Location(Point.get_Zero());
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Tooltip generation failed.");
				}
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			RectangleF rect = _rect.Target(RectangleF.op_Implicit(((Control)this).get_AbsoluteBounds()));
			rect.Width -= 5f;
			rect.X += 2f;
			_rect.Draw(spriteBatch, (Control)(object)this, rect);
		}
	}
}
