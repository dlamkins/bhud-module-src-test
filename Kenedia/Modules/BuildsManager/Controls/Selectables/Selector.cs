using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class Selector<T> : Kenedia.Modules.Core.Controls.FlowPanel where T : IBaseApiData
	{
		private readonly Kenedia.Modules.Core.Controls.Label _label;

		private Point _selectableSize = new Point(64);

		private Point _anchorOffset;

		protected readonly Kenedia.Modules.Core.Controls.Panel HeaderPanel;

		protected readonly Kenedia.Modules.Core.Controls.Panel ContentPanel;

		protected readonly Kenedia.Modules.Core.Controls.FlowPanel FlowPanel;

		protected Rectangle BlockInputRegion;

		public SelectableType Type { get; set; }

		public List<T> Items { get; } = new List<T>();


		public T SelectedItem
		{
			[CompilerGenerated]
			get
			{
				return _003CSelectedItem_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSelectedItem_003Ek__BackingField, value, delegate(T v)
				{
					_003CSelectedItem_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<T>(ApplySelected));
			}
		}

		public List<Selectable<T>> Controls { get; } = new List<Selectable<T>>();


		public Action<T> OnClickAction
		{
			[CompilerGenerated]
			get
			{
				return _003COnClickAction_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003COnClickAction_003Ek__BackingField, value, delegate(Action<T> v)
				{
					_003COnClickAction_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Action<T>>(ApplyAction));
			}
		}

		public int SelectablePerRow
		{
			[CompilerGenerated]
			get
			{
				return _003CSelectablePerRow_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSelectablePerRow_003Ek__BackingField, value, delegate(int v)
				{
					_003CSelectablePerRow_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public Point SelectableSize
		{
			get
			{
				return _selectableSize;
			}
			set
			{
				Common.SetProperty(ref _selectableSize, value, new ValueChangedEventHandler<Point>(Recalculate));
			}
		}

		public Control Anchor
		{
			[CompilerGenerated]
			get
			{
				return _003CAnchor_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CAnchor_003Ek__BackingField, value, delegate(Control v)
				{
					_003CAnchor_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public Point AnchorOffset
		{
			get
			{
				return _anchorOffset;
			}
			set
			{
				Common.SetProperty(ref _anchorOffset, value, new Action(RecalculateLayout));
			}
		}

		public string Label
		{
			get
			{
				return _label.Text;
			}
			set
			{
				_label.Text = value;
			}
		}

		public bool PassSelected { get; set; }

		protected virtual int AnchorZIndexOffset => 1000;

		public Selector()
		{
			_003CSelectablePerRow_003Ek__BackingField = 4;
			PassSelected = true;
			base._002Ector();
			HeaderPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this
			};
			base.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			ContentPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				BackgroundColor = new Color(16, 16, 16) * 0.9f,
				ContentPadding = new RectangleDimensions(8),
				BorderColor = Color.Black,
				BorderWidth = new RectangleDimensions(2),
				Parent = this
			};
			FlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = ContentPanel,
				FlowDirection = ControlFlowDirection.LeftToRight,
				ControlPadding = new Vector2(4f),
				ContentPadding = new RectangleDimensions(1)
			};
			_label = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = ContentPanel,
				Font = Control.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular),
				AutoSizeHeight = true,
				TextColor = Color.White,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Middle
			};
			Control.Input.Mouse.LeftMouseButtonPressed += Mouse_LeftMouseButtonPressed;
			Control.Input.Mouse.RightMouseButtonPressed += Mouse_RightMouseButtonPressed;
		}

		private void Mouse_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Visible)
			{
				if ((HeaderPanel?.MouseOver ?? false) && !BlockInputRegion.Contains(base.RelativeMousePosition))
				{
					base.Visible = false;
				}
				if (!base.MouseOver)
				{
					base.Visible = false;
				}
			}
		}

		private void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Visible)
			{
				if ((HeaderPanel?.MouseOver ?? false) && !BlockInputRegion.Contains(base.RelativeMousePosition))
				{
					base.Visible = false;
				}
				if (!base.MouseOver)
				{
					base.Visible = false;
				}
			}
		}

		private void ApplySelected(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<T> e)
		{
			OnDataApplied(e.NewValue);
		}

		protected virtual void OnDataApplied(T item)
		{
		}

		private void ApplyAction(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Action<T>> e)
		{
			Controls.ForEach(delegate(Selectable<T> c)
			{
				c.OnClickAction = OnClickAction;
			});
		}

		protected virtual void Recalculate(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Point> e)
		{
			Controls.ForEach(delegate(Selectable<T> c)
			{
				c.Size = SelectableSize;
			});
			RecalculateLayout();
		}

		protected virtual Selectable<T> CreateSelectable(T item)
		{
			SelectableType selectableType2 = (Type = ((item is Skill) ? SelectableType.Skill : ((item is Pet) ? SelectableType.Pet : SelectableType.None)));
			base.Visible = true;
			Selectable<T> obj = new Selectable<T>
			{
				Parent = FlowPanel,
				Size = SelectableSize,
				Data = item,
				OnClickAction = OnClickAction
			};
			int isSelected;
			if (PassSelected)
			{
				ref T val = ref item;
				T val2 = default(T);
				if (val2 == null)
				{
					val2 = val;
					val = ref val2;
				}
				isSelected = (val.Equals(SelectedItem) ? 1 : 0);
			}
			else
			{
				isSelected = 0;
			}
			obj.IsSelected = (byte)isSelected != 0;
			return obj;
		}

		public void Add(T item)
		{
			Items.Add(item);
			Controls.Add(CreateSelectable(item));
			RecalculateLayout();
		}

		public void Remove(T item)
		{
			T item2 = item;
			Items.Remove(item2);
			Selectable<T> selectable = Controls.FirstOrDefault((Selectable<T> c) => c.Data!.Equals(item2));
			if (selectable != null)
			{
				Controls.Remove(selectable);
				selectable.Dispose();
			}
			RecalculateLayout();
		}

		public void Clear()
		{
			Items.Clear();
			Controls.DisposeAll();
			Controls.Clear();
			RecalculateLayout();
		}

		public void SetItems(IEnumerable<T> items)
		{
			Items.Clear();
			Controls.DisposeAll();
			Controls.Clear();
			Items.AddRange(items);
			Controls.AddRange(items.Select(CreateSelectable));
			RecalculateLayout();
		}

		public void AddItems(IEnumerable<T> items)
		{
			Items.AddRange(items);
			Controls.AddRange(items.Select(CreateSelectable));
			RecalculateLayout();
		}

		public void RemoveItems(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				Items.Remove(item);
				Selectable<T> selectable = Controls.FirstOrDefault((Selectable<T> c) => c.Data!.Equals(item));
				if (selectable != null)
				{
					Controls.Remove(selectable);
					selectable.Dispose();
				}
			}
			RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			FlowPanel?.Invalidate();
			ContentPanel?.Invalidate();
			if (FlowPanel != null)
			{
				Point p = FlowPanel.Size.Substract(FlowPanel.ContentRegion.Size);
				FlowPanel.Width = p.X + SelectableSize.X * SelectablePerRow + (int)FlowPanel.ControlPadding.X * (SelectablePerRow - 1);
				FlowPanel.Height = p.Y + SelectableSize.Y * Math.Max(1, (int)Math.Ceiling((decimal)Items.Count / (decimal)SelectablePerRow)) + (int)FlowPanel.ControlPadding.Y * Math.Max(1, (int)Math.Ceiling((decimal)Items.Count / ((decimal)SelectablePerRow - 1m)));
				FlowPanel.RecalculateLayout();
			}
			if (_label != null && FlowPanel != null)
			{
				_label.Width = FlowPanel.Width;
				_label.Location = new Point(0, FlowPanel.Bottom);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			Control anchor = Anchor;
			if (anchor == null || !anchor.IsDrawn())
			{
				base.Visible = false;
			}
			base.Draw(spriteBatch, drawBounds, scissor);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (base.Visible)
			{
				if (base.Parent == Control.Graphics.SpriteScreen && Anchor != null)
				{
					ZIndex = GetAnchorRootZIndex(Anchor) + AnchorZIndexOffset;
				}
				SetCapture();
				MoveToAnchor();
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
		}

		protected virtual void SetCapture()
		{
			if (HeaderPanel != null)
			{
				base.CaptureInput = !HeaderPanel.MouseOver;
				HeaderPanel.CaptureInput = !HeaderPanel.MouseOver;
			}
		}

		protected virtual void MoveToAnchor()
		{
			if (Anchor != null)
			{
				base.Location = new Point(Anchor.AbsoluteBounds.Center.X - base.Width / 2 + AnchorOffset.X, Anchor.AbsoluteBounds.Top + AnchorOffset.Y);
			}
		}

		public static int GetAnchorRootZIndex(Control anchor)
		{
			if (anchor == null)
			{
				return 0;
			}
			Control current = anchor;
			while (current.Parent != null && current.Parent != Control.Graphics.SpriteScreen)
			{
				current = current.Parent;
			}
			return current.ZIndex;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Control.Input.Mouse.LeftMouseButtonPressed -= Mouse_LeftMouseButtonPressed;
			Control.Input.Mouse.RightMouseButtonPressed -= Mouse_RightMouseButtonPressed;
		}
	}
}
