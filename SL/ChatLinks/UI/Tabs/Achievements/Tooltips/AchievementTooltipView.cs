using System;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using SL.Common.Controls;

namespace SL.ChatLinks.UI.Tabs.Achievements.Tooltips
{
	public sealed class AchievementTooltipView : View, ITooltipView, IView, IDisposable
	{
		private static readonly Color LightOrange = new Color(255, 204, 119);

		private static readonly Color Gray = new Color(153, 153, 153);

		private readonly FlowPanel _layout;

		public AchievementTooltipViewModel ViewModel { get; }

		public AchievementTooltipView(AchievementTooltipViewModel viewModel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(350);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(0f, 5f));
			_layout = val;
			ViewModel = viewModel;
			((View)this)._002Ector();
		}

		protected override void Unload()
		{
			Dispose();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_layout);
			val.set_Text(ViewModel.Name);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(LightOrange);
			val.set_WrapText(true);
			((Control)val).set_Width(((Control)_layout).get_Width());
			val.set_AutoSizeHeight(true);
			((Control)new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width() - 10).AutoSizeHeight().Wrap()
				.AddMarkup(ViewModel.Requirement)
				.Build()).set_Parent((Container)(object)_layout);
			((Control)new FormattedLabelBuilder().SetWidth(((Control)_layout).get_Width() - 10).AutoSizeHeight().Wrap()
				.AddMarkup(ViewModel.Description, delegate(FormattedLabelPartBuilder part)
				{
					part.SetFontSize((FontSize)16).MakeItalic();
				}, Gray)
				.Build()).set_Parent((Container)(object)_layout);
			((Control)_layout).set_Parent(buildPanel);
		}

		public void Dispose()
		{
			((Control)_layout).Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
