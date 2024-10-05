using System;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class BaseSelection : Kenedia.Modules.Core.Controls.Panel
	{
		public readonly FilterBox Search;

		protected readonly Kenedia.Modules.Core.Controls.FlowPanel SelectionContent;

		public Action<object> OnClickAction { get; set; }

		public Kenedia.Modules.Core.Controls.FlowPanel SelectionContainer => SelectionContent;

		public Rectangle SelectionBounds
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				Rectangle val = SelectionContent.LocalBounds;
				Point location = ((Rectangle)(ref val)).get_Location();
				val = SelectionContent.ContentRegion;
				return new Rectangle(location, ((Rectangle)(ref val)).get_Size());
			}
		}

		public BaseSelection()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			HeightSizingMode = SizingMode.Fill;
			WidthSizingMode = SizingMode.Fill;
			Search = new FilterBox
			{
				Parent = this,
				Location = new Point(2, 0),
				SetLocalizedPlaceholder = () => strings_common.Search,
				FilteringOnTextChange = true
			};
			SelectionContent = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Location = new Point(0, Search.Bottom + 5),
				ShowBorder = true,
				HeightSizingMode = SizingMode.Fill,
				WidthSizingMode = SizingMode.Fill,
				CanScroll = true,
				ShowRightBorder = true,
				ControlPadding = new Vector2(5f),
				ContentPadding = new RectangleDimensions(5)
			};
			SelectionContent.Resized += OnSelectionContent_Resized;
		}

		protected virtual void OnSelectionContent_Resized(object sender, ResizedEventArgs e)
		{
			_ = SelectionContent;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			SelectionContent.Resized -= OnSelectionContent_Resized;
			SelectionContent?.Dispose();
			Search?.Dispose();
		}
	}
}
