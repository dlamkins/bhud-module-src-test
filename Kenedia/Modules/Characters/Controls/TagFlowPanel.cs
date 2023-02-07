using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class TagFlowPanel : FontFlowPanel
	{
		private List<Tag> Tags => ((IEnumerable)((Container)this).get_Children()).Cast<Tag>().ToList();

		private Rectangle CalculateTagPanelSize(int? width = null, bool fitLargest = false)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			List<Tag> tags = Tags;
			if (tags.Count == 0)
			{
				return Rectangle.get_Empty();
			}
			tags = (from e in tags
				orderby ((Control)e).get_Width() descending, e.Text
				select e).ToList();
			List<Tag> added = new List<Tag>();
			int widest = ((Tags.Count > 0) ? Tags.Max((Tag e) => ((Control)e).get_Width()) : 0);
			widest += (int)((FlowPanel)this).get_OuterControlPadding().X + ((Container)this).get_AutoSizePadding().X;
			int valueOrDefault = width.GetValueOrDefault();
			if (!width.HasValue)
			{
				valueOrDefault = widest;
				width = valueOrDefault;
			}
			width = Math.Max(widest, width.Value);
			int height = 0;
			int curWidth = 0;
			int index = 0;
			tags.LastOrDefault();
			foreach (Tag t in tags)
			{
				if (added.Contains(t))
				{
					continue;
				}
				t.TagPanelIndex = index;
				height += ((Control)t).get_Height() + (int)((FlowPanel)this).get_ControlPadding().Y;
				curWidth = ((Control)t).get_Width() + (int)((FlowPanel)this).get_ControlPadding().X;
				if (curWidth + 25 < width)
				{
					foreach (Tag e2 in tags)
					{
						if (e2 != t && !added.Contains(e2) && ((Control)e2).get_Width() + (int)((FlowPanel)this).get_ControlPadding().X + curWidth <= width)
						{
							curWidth += ((Control)e2).get_Width() + (int)((FlowPanel)this).get_ControlPadding().X;
							index = (e2.TagPanelIndex = index + 1);
							added.Add(e2);
							if (curWidth + 25 >= width)
							{
								curWidth = 0;
								break;
							}
						}
					}
				}
				else if (added.Count + 1 == tags.Count)
				{
					curWidth = 0;
				}
				added.Add(t);
				index++;
			}
			return new Rectangle(((Control)this).get_Location(), new Point(width.Value, height + (int)(((FlowPanel)this).get_OuterControlPadding().Y + (float)((Container)this).get_AutoSizePadding().Y)));
		}

		public void FitWidestTag(int? width = null)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = CalculateTagPanelSize(width);
			((FlowPanel)this).SortChildren<Tag>((Comparison<Tag>)((Tag a, Tag b) => a.TagPanelIndex.CompareTo(b.TagPanelIndex)));
			((Control)this).set_Height(bounds.Height);
			((Control)this).set_Width(bounds.Width);
		}

		public override void Invalidate()
		{
			((Control)this).Invalidate();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			base.OnChildAdded(e);
			Rectangle bounds = CalculateTagPanelSize();
			((Control)this).set_Height(bounds.Height);
			((Control)this).set_Width(bounds.Width);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).OnChildRemoved(e);
			Rectangle bounds = CalculateTagPanelSize();
			((Control)this).set_Height(bounds.Height);
			((Control)this).set_Width(bounds.Width);
		}

		protected override void OnFontChanged(object sender = null, EventArgs e = null)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			base.OnFontChanged(sender, e);
			Rectangle bounds = CalculateTagPanelSize();
			((Control)this).set_Height(bounds.Height);
			((Control)this).set_Width(bounds.Width);
		}
	}
}
