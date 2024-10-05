using System;
using System.Collections.Generic;
using System.Linq;
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
		private Control? _anchor;

		private readonly Kenedia.Modules.Core.Controls.Label _label;

		private Point _selectableSize = new Point(64);

		private Action<T> _onClickAction;

		private T _selectedItem;

		private int _selectablePerRow = 4;

		private Point _anchorOffset;

		protected readonly Kenedia.Modules.Core.Controls.Panel HeaderPanel;

		protected readonly Kenedia.Modules.Core.Controls.Panel ContentPanel;

		protected readonly Kenedia.Modules.Core.Controls.FlowPanel FlowPanel;

		protected Rectangle BlockInputRegion;

		public SelectableType Type { get; set; }

		public List<T> Items { get; } = new List<T>();


		public T SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				Common.SetProperty(ref _selectedItem, value, new ValueChangedEventHandler<T>(ApplySelected));
			}
		}

		public List<Selectable<T>> Controls { get; } = new List<Selectable<T>>();


		public Action<T> OnClickAction
		{
			get
			{
				return _onClickAction;
			}
			set
			{
				Common.SetProperty(ref _onClickAction, value, new ValueChangedEventHandler<Action<T>>(ApplyAction));
			}
		}

		public int SelectablePerRow
		{
			get
			{
				return _selectablePerRow;
			}
			set
			{
				Common.SetProperty(ref _selectablePerRow, value, new Action(RecalculateLayout));
			}
		}

		public Point SelectableSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _selectableSize;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Common.SetProperty(ref _selectableSize, value, new ValueChangedEventHandler<Point>(Recalculate));
			}
		}

		public Control Anchor
		{
			get
			{
				return _anchor;
			}
			set
			{
				Common.SetProperty(ref _anchor, value, new Action(RecalculateLayout));
			}
		}

		public Point AnchorOffset
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _anchorOffset;
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
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

		public bool PassSelected { get; set; } = true;


		public Selector()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
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
				BorderColor = Color.get_Black(),
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
				TextColor = Color.get_White(),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Middle
			};
			Control.Input.Mouse.LeftMouseButtonPressed += Mouse_LeftMouseButtonPressed;
			Control.Input.Mouse.RightMouseButtonPressed += Mouse_RightMouseButtonPressed;
		}

		private void Mouse_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (base.Visible)
			{
				if ((HeaderPanel?.MouseOver ?? false) && !((Rectangle)(ref BlockInputRegion)).Contains(base.RelativeMousePosition))
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
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (base.Visible)
			{
				if ((HeaderPanel?.MouseOver ?? false) && !((Rectangle)(ref BlockInputRegion)).Contains(base.RelativeMousePosition))
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
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				c.Size = SelectableSize;
			});
			RecalculateLayout();
		}

		protected virtual Selectable<T> CreateSelectable(T item)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			FlowPanel?.Invalidate();
			ContentPanel?.Invalidate();
			if (FlowPanel != null)
			{
				Point size = FlowPanel.Size;
				Rectangle contentRegion = FlowPanel.ContentRegion;
				Point p = size.Substract(((Rectangle)(ref contentRegion)).get_Size());
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
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Control anchor = Anchor;
			if (anchor == null || !anchor.IsDrawn())
			{
				base.Visible = false;
			}
			base.Draw(spriteBatch, drawBounds, scissor);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (base.Visible)
			{
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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (Anchor != null)
			{
				Rectangle absoluteBounds = Anchor.AbsoluteBounds;
				int num = ((Rectangle)(ref absoluteBounds)).get_Center().X - base.Width / 2 + AnchorOffset.X;
				absoluteBounds = Anchor.AbsoluteBounds;
				base.Location = new Point(num, ((Rectangle)(ref absoluteBounds)).get_Top() + AnchorOffset.Y);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Control.Input.Mouse.LeftMouseButtonPressed -= Mouse_LeftMouseButtonPressed;
			Control.Input.Mouse.RightMouseButtonPressed -= Mouse_RightMouseButtonPressed;
		}
	}
}
