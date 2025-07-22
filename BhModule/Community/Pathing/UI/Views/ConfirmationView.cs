using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Controls;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Views
{
	internal class ConfirmationView : View
	{
		protected FormattedLabel Text { get; set; }

		protected StandardButton ConfirmButton { get; set; }

		protected BlueButton OpenButton { get; set; }

		protected StandardButton DenyButton { get; set; }

		private PathingCategory _category { get; }

		private IPackState _packState { get; }

		public ConfirmationView(PathingCategory category, IPackState packState)
			: this()
		{
			_category = category;
			_packState = packState;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Expected O, but got Unknown
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			val.set_ShowBorder(true);
			val.set_BackgroundTexture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("tooltip")));
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			((Control)val).set_ClipsBounds(false);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_ZIndex(2147483645);
			Panel container = val;
			FormattedLabelBuilder builder = new FormattedLabelBuilder().CreatePart("Do you wish to activate all the parents of this category?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				b.SetFontSize((FontSize)18).SetTextColor(Color.get_Orange()).MakeBold();
			}).CreatePart("\n \nThe category you selected is not active because one or more of the parent categories listed below is not active.\n \n ", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				b.SetFontSize((FontSize)16);
			});
			List<PathingCategory> parents = _category.GetParentsDesc().ToList();
			bool firstParent = true;
			foreach (PathingCategory parent in parents)
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
			Text = builder.SetWidth(344).AutoSizeHeight().Wrap()
				.Build();
			((Control)Text).set_Location(new Point(15, 15));
			((Control)Text).set_Parent((Container)(object)container);
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)container);
			val2.set_Text("Yes");
			((Control)val2).set_Width(85);
			((Control)val2).set_Location(new Point(15, ((Control)Text).get_Height() + 30));
			ConfirmButton = val2;
			((Control)ConfirmButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (PathingCategory current in parents)
				{
					_packState.CategoryStates.SetInactive(current, isInactive: false);
				}
				((Control)buildPanel).Dispose();
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)container);
			val3.set_Text("Cancel");
			((Control)val3).set_Width(85);
			((Control)val3).set_Location(new Point(((Control)ConfirmButton).get_Right() + 5, ((Control)Text).get_Height() + 30));
			DenyButton = val3;
			((Control)DenyButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)buildPanel).Dispose();
			});
			BlueButton blueButton = new BlueButton();
			((Control)blueButton).set_Parent((Container)(object)container);
			blueButton.Text = "Open In Explorer";
			((Control)blueButton).set_Width(150);
			((Control)blueButton).set_Location(new Point(((Control)DenyButton).get_Right() + 50, ((Control)Text).get_Height() + 30));
			OpenButton = blueButton;
			((Control)OpenButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_packState.CategoryStates.TriggerOpenCategory(_category);
				((Control)buildPanel).Dispose();
			});
		}
	}
}
