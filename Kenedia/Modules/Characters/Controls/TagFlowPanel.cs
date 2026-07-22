using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class TagFlowPanel : FontFlowPanel
	{
		private List<Tag> Tags => base.Children.Cast<Tag>().ToList();

		private Rectangle CalculateTagPanelSize(int? width = null, bool fitLargest = false)
		{
			List<Tag> tags = Tags;
			if (tags.Count == 0)
			{
				return Rectangle.Empty;
			}
			tags = (from e in tags
				orderby e.Width descending, e.Text
				select e).ToList();
			List<Tag> added = new List<Tag>();
			int widest = ((Tags.Count > 0) ? Tags.Max((Tag e) => e.Width) : 0);
			widest += (int)base.OuterControlPadding.X + base.AutoSizePadding.X;
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
				height += t.Height + (int)base.ControlPadding.Y;
				curWidth = t.Width + (int)base.ControlPadding.X;
				if (curWidth + 25 < width)
				{
					foreach (Tag e2 in tags)
					{
						if (e2 != t && !added.Contains(e2) && e2.Width + (int)base.ControlPadding.X + curWidth <= width)
						{
							curWidth += e2.Width + (int)base.ControlPadding.X;
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
			return new Rectangle(base.Location, new Point(width.Value, height + (int)(base.OuterControlPadding.Y + (float)base.AutoSizePadding.Y)));
		}

		public void FitWidestTag(int? width = null)
		{
			Rectangle bounds = CalculateTagPanelSize(width);
			SortChildren((Tag a, Tag b) => a.TagPanelIndex.CompareTo(b.TagPanelIndex));
			base.Height = bounds.Height;
			base.Width = bounds.Width;
		}

		public override void Invalidate()
		{
			base.Invalidate();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			base.OnChildAdded(e);
			Rectangle bounds = CalculateTagPanelSize();
			base.Height = bounds.Height;
			base.Width = bounds.Width;
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			base.OnChildRemoved(e);
			Rectangle bounds = CalculateTagPanelSize();
			base.Height = bounds.Height;
			base.Width = bounds.Width;
		}

		protected override void OnFontChanged(object sender = null, EventArgs e = null)
		{
			base.OnFontChanged(sender, e);
			Rectangle bounds = CalculateTagPanelSize();
			base.Height = bounds.Height;
			base.Width = bounds.Width;
		}
	}
}
