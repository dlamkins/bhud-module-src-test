using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagEditWindowX : Kenedia.Modules.Core.Views.TabbedWindow
	{
		private FilterBox _tagFilter;

		private Kenedia.Modules.Core.Controls.FlowPanel _tagPanel;

		private List<TagEditControl> _tagEditControls = new List<TagEditControl>();

		private readonly ButtonImage _addTag;

		private Dictionary<Kenedia.Modules.Core.Controls.FlowPanel, List<TagEditControl>> _tagControls = new Dictionary<Kenedia.Modules.Core.Controls.FlowPanel, List<TagEditControl>>();

		private Kenedia.Modules.Core.Controls.FlowPanel _ungroupedPanel;

		private Kenedia.Modules.Core.Controls.FlowPanel _startPanel;

		private TagEditControl _draggingTagEditControl;

		public TemplateTags TemplateTags { get; }

		public TagEditWindowX(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, TemplateTags templateTags)
			: base(background, windowRegion, contentRegion)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			TemplateTags = templateTags;
			_tagFilter = new FilterBox
			{
				Parent = this,
				Location = new Point(0, 0),
				Width = 530,
				SetLocalizedPlaceholder = () => strings_common.Search,
				FilteringOnTextChange = true,
				FilteringOnEnter = true,
				EnterPressedAction = TagFilter_EnterPressed(),
				TextChangedAction = TagFilter_TextChanged(),
				PerformFiltering = TagFilter_PerformFilter()
			};
			_addTag = new ButtonImage
			{
				Parent = this,
				Size = new Point(_tagFilter.Height),
				Location = new Point(_tagFilter.Right + 2, _tagFilter.Top),
				Texture = AsyncTexture2D.FromAssetId(255443),
				HoveredTexture = AsyncTexture2D.FromAssetId(255297),
				DisabledTexture = AsyncTexture2D.FromAssetId(255296),
				SetLocalizedTooltip = () => strings.AddTag,
				Enabled = true,
				ClickAction = AddTag_Click()
			};
			_tagPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Location = new Point(0, _tagFilter.Bottom + 2),
				Width = _addTag.Right,
				HeightSizingMode = SizingMode.Fill,
				ShowBorder = false,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.4f,
				ShowRightBorder = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ContentPadding = new RectangleDimensions(5),
				ControlPadding = new Vector2(0f, 5f),
				CanScroll = true
			};
			TemplateTags.TagAdded += new EventHandler<TemplateTag>(TemplateTags_TagAdded);
			TemplateTags.TagRemoved += new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			_tagPanel.ChildAdded += TagPanel_ChildsChanged;
			_tagPanel.ChildRemoved += TagPanel_ChildsChanged;
			CreateTagControls();
		}

		private Action<MouseEventArgs> AddTag_Click()
		{
			return delegate
			{
				TemplateTags.Add(new TemplateTag
				{
					Name = (string.IsNullOrEmpty(_tagFilter.Text) ? TemplateTag.DefaultName : _tagFilter.Text)
				});
			};
		}

		private Action<string> TagFilter_PerformFilter()
		{
			return delegate(string txt)
			{
				string value = txt.ToLower();
				bool flag = string.IsNullOrEmpty(value);
				foreach (TagControl current in _tagPanel.GetChildrenOfType<TagControl>())
				{
					current.Visible = flag || current.Tag.Name.ToLower().Contains(value);
				}
				_tagPanel.Invalidate();
			};
		}

		private Action<string> TagFilter_TextChanged()
		{
			return delegate(string txt)
			{
				string txt2 = txt;
				_addTag.Enabled = !string.IsNullOrEmpty(txt2.Trim()) && TemplateTags.FirstOrDefault((TemplateTag e) => e.Name.ToLower() == txt2.ToLower()) == null;
			};
		}

		private Action<string> TagFilter_EnterPressed()
		{
			return delegate(string txt)
			{
				string txt2 = txt;
				if (!string.IsNullOrEmpty(txt2.Trim()) && TemplateTags.FirstOrDefault((TemplateTag e) => e.Name.ToLower() == txt2.ToLower()) == null)
				{
					TemplateTags.Add(new TemplateTag
					{
						Name = txt2
					});
				}
			};
		}

		public Kenedia.Modules.Core.Controls.FlowPanel GetPanel(string title)
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			string title2 = title;
			Kenedia.Modules.Core.Controls.FlowPanel panel = null;
			if (!string.IsNullOrEmpty(title2))
			{
				Kenedia.Modules.Core.Controls.FlowPanel p = _tagControls.Keys.FirstOrDefault((Kenedia.Modules.Core.Controls.FlowPanel x) => x.Title == title2);
				if (p != null)
				{
					panel = p;
				}
				if (panel == null)
				{
					panel = new Kenedia.Modules.Core.Controls.FlowPanel
					{
						Title = title2,
						Parent = _tagPanel,
						Width = _tagPanel.Width - 25,
						WidthSizingMode = SizingMode.Standard,
						HeightSizingMode = SizingMode.AutoSize,
						AutoSizePadding = new Point(0, 2),
						OuterControlPadding = new Vector2(25f, 0f),
						CanCollapse = true
					};
				}
			}
			if (panel == null)
			{
				Kenedia.Modules.Core.Controls.FlowPanel flowPanel = _ungroupedPanel;
				if (flowPanel == null)
				{
					Kenedia.Modules.Core.Controls.FlowPanel obj = new Kenedia.Modules.Core.Controls.FlowPanel
					{
						Title = TagGroup.DefaultName,
						Parent = _tagPanel,
						Width = _tagPanel.Width - 25,
						WidthSizingMode = SizingMode.Standard,
						HeightSizingMode = SizingMode.AutoSize,
						AutoSizePadding = new Point(0, 2),
						OuterControlPadding = new Vector2(25f, 0f),
						CanCollapse = true
					};
					Kenedia.Modules.Core.Controls.FlowPanel flowPanel2 = obj;
					_ungroupedPanel = obj;
					flowPanel = flowPanel2;
				}
				panel = flowPanel;
			}
			if (!_tagControls.ContainsKey(panel))
			{
				_tagControls.Add(panel, new List<TagEditControl>());
				_tagPanel.SortChildren((Kenedia.Modules.Core.Controls.FlowPanel x, Kenedia.Modules.Core.Controls.FlowPanel y) => (!(x.Title == TagGroup.DefaultName)) ? x.Title.CompareTo(y.Title) : (-1));
			}
			return panel;
		}

		private void TagPanel_ChildsChanged(object sender, ChildChangedEventArgs e)
		{
			(sender as Kenedia.Modules.Core.Controls.FlowPanel)?.SortChildren((Kenedia.Modules.Core.Controls.FlowPanel x, Kenedia.Modules.Core.Controls.FlowPanel y) => (!(x.Title == TagGroup.DefaultName)) ? x.Title.CompareTo(y.Title) : (-1));
		}

		private void TemplateTags_TagRemoved(object sender, TemplateTag e)
		{
			RemoveTemplateTag(e);
		}

		private void TemplateTags_TagAdded(object sender, TemplateTag e)
		{
			AddTemplateTag(e);
		}

		private int SortTagControls(TagEditControl x, TagEditControl y)
		{
			if (x.Tag.Priority.CompareTo(y.Tag.Priority) != 0)
			{
				return x.Tag.Priority.CompareTo(y.Tag.Priority);
			}
			return x.Tag.Name.CompareTo(y.Tag.Name);
		}

		private void RemoveTemplateTag(TemplateTag e)
		{
			TemplateTag e2 = e;
			TagEditControl tagControl = null;
			KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagEditControl>> p = _tagControls.FirstOrDefault<KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagEditControl>>>((KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagEditControl>> x) => x.Value.Any((TagEditControl x) => x.Tag == e2));
			Kenedia.Modules.Core.Controls.FlowPanel panel = p.Key;
			tagControl = p.Value.FirstOrDefault((TagEditControl x) => x.Tag == e2);
			tagControl?.Dispose();
			_tagControls[panel].Remove(tagControl);
			if (!_tagControls[panel].Any())
			{
				_tagControls.Remove(panel);
				panel.Dispose();
				_tagPanel.SortChildren((Kenedia.Modules.Core.Controls.FlowPanel x, Kenedia.Modules.Core.Controls.FlowPanel y) => x.Title.CompareTo(y.Title));
			}
			else
			{
				panel.SortChildren<TagEditControl>(SortTagControls);
			}
		}

		private void AddTemplateTag(TemplateTag e)
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			TemplateTag e2 = e;
			Kenedia.Modules.Core.Controls.FlowPanel panel = _tagControls.Keys.FirstOrDefault((Kenedia.Modules.Core.Controls.FlowPanel x) => x.Title == e2.Group);
			if (panel == null)
			{
				object obj;
				if (string.IsNullOrEmpty(e2.Group))
				{
					obj = _ungroupedPanel;
					if (obj == null)
					{
						Kenedia.Modules.Core.Controls.FlowPanel obj2 = new Kenedia.Modules.Core.Controls.FlowPanel
						{
							Title = TagGroup.DefaultName,
							Parent = _tagPanel,
							Width = _tagPanel.Width - 25,
							WidthSizingMode = SizingMode.Standard,
							HeightSizingMode = SizingMode.AutoSize,
							AutoSizePadding = new Point(0, 2),
							OuterControlPadding = new Vector2(25f, 0f),
							CanCollapse = true
						};
						Kenedia.Modules.Core.Controls.FlowPanel flowPanel = obj2;
						_ungroupedPanel = obj2;
						obj = flowPanel;
					}
				}
				else
				{
					obj = new Kenedia.Modules.Core.Controls.FlowPanel
					{
						Title = e2.Group,
						Parent = _tagPanel,
						Width = _tagPanel.Width - 25,
						WidthSizingMode = SizingMode.Standard,
						HeightSizingMode = SizingMode.AutoSize,
						AutoSizePadding = new Point(0, 2),
						OuterControlPadding = new Vector2(25f, 0f),
						CanCollapse = true
					};
				}
				panel = (Kenedia.Modules.Core.Controls.FlowPanel)obj;
			}
			if (!_tagControls.ContainsKey(panel))
			{
				_tagControls.Add(panel, new List<TagEditControl>());
				_tagPanel.SortChildren((Kenedia.Modules.Core.Controls.FlowPanel x, Kenedia.Modules.Core.Controls.FlowPanel y) => x.Title.CompareTo(y.Title));
			}
			TagEditControl t;
			_tagControls[panel].Add(t = new TagEditControl
			{
				Parent = panel,
				Tag = e2,
				Width = panel.Width - 25,
				TemplateTags = TemplateTags
			});
			panel.SortChildren<TagEditControl>(SortTagControls);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			base.OnMouseMoved(e);
			MouseState state = GameService.Input.Mouse.State;
			((MouseState)(ref state)).get_LeftButton();
			_ = 1;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			_tagFilter?.SetSize(base.Width - 50, null);
			_addTag?.SetLocation(new Point(_tagFilter.Right + 2, _tagFilter.Top));
			_tagPanel?.SetLocation(new Point(0, _tagFilter.Bottom + 2));
			_tagPanel?.SetSize(_addTag.Right, null);
			ResizeTagControls();
		}

		public void Show(TemplateTag tag)
		{
			Show();
			List<TagEditControl> list = new List<TagEditControl>(_tagPanel.GetChildrenOfType<TagEditControl>());
			list.AddRange(_tagPanel.GetChildrenOfType<Kenedia.Modules.Core.Controls.FlowPanel>().SelectMany((Kenedia.Modules.Core.Controls.FlowPanel x) => x.GetChildrenOfType<TagEditControl>()));
			foreach (TagEditControl item in list)
			{
				item.Collapsed = item.Tag != tag;
			}
		}

		private void ResizeTagControls()
		{
			if (_tagPanel == null)
			{
				return;
			}
			List<TagEditControl> list = new List<TagEditControl>(_tagPanel.GetChildrenOfType<TagEditControl>());
			list.AddRange(_tagPanel.GetChildrenOfType<Kenedia.Modules.Core.Controls.FlowPanel>().SelectMany((Kenedia.Modules.Core.Controls.FlowPanel x) => x.GetChildrenOfType<TagEditControl>()));
			foreach (TagEditControl tag in list)
			{
				Kenedia.Modules.Core.Controls.FlowPanel fp = tag.Parent as Kenedia.Modules.Core.Controls.FlowPanel;
				if (fp != null)
				{
					fp.Width = _tagPanel.Width - 25;
					tag.Width = fp.Width - 25;
				}
			}
		}

		private void CreateTagControls()
		{
			foreach (TemplateTag tag in TemplateTags)
			{
				AddTemplateTag(tag);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
		}

		private void SetNewGroupFromDragging()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (_draggingTagEditControl == null)
			{
				return;
			}
			MouseState state = Control.Input.Mouse.State;
			if ((int)((MouseState)(ref state)).get_LeftButton() != 0)
			{
				return;
			}
			Kenedia.Modules.Core.Controls.FlowPanel p = null;
			using (Dictionary<Kenedia.Modules.Core.Controls.FlowPanel, List<TagEditControl>>.KeyCollection.Enumerator enumerator = _tagControls.Keys.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Kenedia.Modules.Core.Controls.FlowPanel current = enumerator.Current;
					Rectangle absoluteBounds = current.AbsoluteBounds;
					((Rectangle)(ref absoluteBounds)).Contains(_draggingTagEditControl.Location);
					p = current;
				}
			}
			if (p != null)
			{
				_draggingTagEditControl.Tag.Group = p.Title;
			}
			else if (_startPanel != null)
			{
				_draggingTagEditControl.Tag.Group = ((_startPanel.Title != TagGroup.DefaultName) ? _startPanel.Title : string.Empty);
			}
			_draggingTagEditControl = null;
			_startPanel = null;
		}
	}
}
