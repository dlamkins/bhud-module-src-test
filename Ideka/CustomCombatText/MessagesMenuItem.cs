using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameServices.ArcDps.V2.Models;
using Blish_HUD.Input;
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
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate(object _, MouseEventArgs e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)build).Show(e.get_MousePosition());
				[IteratorStateMachine(typeof(_003C_003C_002Dctor_003Eg__build_007C11_1_003Ed))]
				IEnumerable<ContextMenuStripItem> build()
				{
					return new _003C_003C_002Dctor_003Eg__build_007C11_1_003Ed(-2)
					{
						_003C_003E4__this = this
					};
				}
			});
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
			_rect.ParsedFragments = TemplateParser.FinalParse(mFrags, null, CTextModule.FontAssets.Get(null, fontSize), Key.Message, new _003C_003Ez__ReadOnlySingleElementList<Message>(Key.Message)).ToList();
			InnerHeight = (_rect.ParsedFragments.Any() ? _rect.ParsedFragments.Max((TemplateParser.Fragment x) => x.Size.Height) : 0f) + 5f;
		}

		public void CreateDebugTooltip()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_BasicTooltipText() == null)
			{
				Tooltip tooltip = ((Control)this).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)this).set_Tooltip((Tooltip)null);
				string[] obj = new string[18]
				{
					$"ID: {((CombatCallback)(ref Key.Cbt)).get_Id()}\n",
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
				CombatEvent @event = ((CombatCallback)(ref Key.Cbt)).get_Event();
				obj[1] = $"Skill ID: raw: {((CombatEvent)(ref @event)).get_SkillId()} used: {Key.Message.SkillId}\n";
				obj[2] = "Raw skill name: ";
				obj[3] = ((CombatCallback)(ref Key.Cbt)).get_SkillName();
				obj[4] = "\n";
				obj[5] = $"Icon ID: {Key.Message.SkillIconId}\n";
				obj[6] = "\n";
				Agent val = ((CombatCallback)(ref Key.Cbt)).get_Source();
				object arg = ((Agent)(ref val)).get_Id();
				@event = ((CombatCallback)(ref Key.Cbt)).get_Event();
				obj[7] = $"Src ID: {arg} / {((CombatEvent)(ref @event)).get_SourceInstanceId()} (self: {((Agent)(ref Key.Message.Src)).get_Self() == 1})\n";
				obj[8] = "Src raw name: ";
				val = ((CombatCallback)(ref Key.Cbt)).get_Source();
				obj[9] = ((Agent)(ref val)).get_Name();
				obj[10] = "\n";
				val = ((CombatCallback)(ref Key.Cbt)).get_Source();
				obj[11] = $"Src prof: {(object)(ProfessionType)(byte)((Agent)(ref val)).get_Profession()} / {((Agent)(ref Key.Message.Src)).get_Elite()}\n";
				obj[12] = "\n";
				val = ((CombatCallback)(ref Key.Cbt)).get_Destination();
				object arg2 = ((Agent)(ref val)).get_Id();
				@event = ((CombatCallback)(ref Key.Cbt)).get_Event();
				obj[13] = $"Dst ID: {arg2} / {((CombatEvent)(ref @event)).get_DestinationInstanceId()} (self: {((Agent)(ref Key.Message.Dst)).get_Self() == 1})\n";
				obj[14] = "Dst raw name: ";
				val = ((CombatCallback)(ref Key.Cbt)).get_Destination();
				obj[15] = ((Agent)(ref val)).get_Name();
				obj[16] = "\n";
				val = ((CombatCallback)(ref Key.Cbt)).get_Destination();
				obj[17] = $"Dst prof: {(object)(ProfessionType)(byte)((Agent)(ref val)).get_Profession()} / {((Agent)(ref Key.Message.Dst)).get_Elite()}";
				((Control)this).set_BasicTooltipText(string.Concat(obj));
			}
		}

		public void CreateSkillTooltip()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BasicTooltipText((string)null);
			if (((Control)this).get_Tooltip() == null && Key.Message.HsSkill != null)
			{
				try
				{
					Tooltip val = new Tooltip();
					((Container)val).set_HeightSizingMode((SizingMode)1);
					((Container)val).set_WidthSizingMode((SizingMode)1);
					((Control)this).set_Tooltip(val);
					SkillTooltip skillTooltip = new SkillTooltip(new SkillTooltipData(Key.Message.HsSkill));
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
