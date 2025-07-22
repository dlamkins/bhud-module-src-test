using System;
using System.Collections.Generic;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Tooltips
{
	internal class CategoryPathTooltip : View, ITooltipView, IView
	{
		private readonly PathingCategory _category;

		private readonly IPackState _packState;

		protected FormattedLabel Text { get; set; }

		public CategoryPathTooltip(PathingCategory category, IPackState packState)
			: this()
		{
			_category = category;
			_packState = packState;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			val.set_ShowBorder(false);
			val.set_BackgroundTexture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("tooltip")));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_ClipsBounds(false);
			((Control)val).set_ZIndex(2147483645);
			Panel container = val;
			FormattedLabelBuilder builder = new FormattedLabelBuilder();
			IEnumerable<PathingCategory> parentsDesc = _category.GetParentsDesc();
			bool firstParent = true;
			foreach (PathingCategory parent in parentsDesc)
			{
				Color color = ((parent == _category) ? Color.get_LightBlue() : (firstParent ? Color.get_Orange() : Color.get_LightYellow()));
				bool inactive = _packState.CategoryStates.GetCategoryInactive(parent);
				builder = builder.CreatePart(" " + parent.DisplayName + "\n ", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					b.SetFontSize((FontSize)16).SetTextColor(color).SetPrefixImage(AsyncTexture2D.FromAssetId(inactive ? 154982 : 154979))
						.SetPrefixImageSize(new Point(20, 20));
				});
				firstParent = false;
			}
			Text = builder.SetWidth(420).AutoSizeHeight().Wrap()
				.Build();
			((Control)Text).set_Location(new Point(15, 15));
			((Control)Text).set_Parent((Container)(object)container);
		}

		protected override void Unload()
		{
			FormattedLabel text = Text;
			if (text != null)
			{
				((Control)text).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
