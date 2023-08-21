using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.UI.LookingForOpener
{
	public class LfoResultView : View
	{
		private readonly LfoResults _results;

		public LfoResultView(LfoResults results)
			: this()
		{
			_results = results;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			Raid.Wing.Event encounter = ProofLogix.Instance.Resources.GetWings().SelectMany((Raid.Wing wing) => wing.Events).First((Raid.Wing.Event ev) => ev.Id.Equals(_results.EncounterId));
			Point size = LabelUtil.GetLabelSize((FontSize)32, encounter.Name, hasPrefix: true);
			FormattedLabel header = new FormattedLabelBuilder().SetWidth(size.X).SetHeight(size.Y).CreatePart(encounter.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
			{
				o.SetFontSize((FontSize)32);
				o.SetHyperLink(AssetUtil.GetWikiLink(encounter.Name));
				o.SetPrefixImage(encounter.Icon);
			})
				.Build();
			((Control)header).set_Parent(buildPanel);
			((Control)header).set_Left(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			((Control)header).set_Top(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Left(4);
			((Control)val).set_Top(((Control)header).get_Bottom());
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height - ((Control)header).get_Height() - 7);
			val.set_ControlPadding(new Vector2((float)((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, (float)((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			val.set_OuterControlPadding(new Vector2((float)((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, (float)((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y));
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			FlowPanel flow = val;
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				((Control)flow).set_Width(e.get_CurrentRegion().Width);
				((Control)flow).set_Height(e.get_CurrentRegion().Height - ((Control)header).get_Height() - 7);
			});
			if (_results.Opener.IsEmpty)
			{
				string text = "No volunteers found.";
				FontSize fontSize = (FontSize)24;
				Point labelSize2 = LabelUtil.GetLabelSize(fontSize, text, hasPrefix: true);
				((Control)new FormattedLabelBuilder().SetHeight(labelSize2.Y).SetWidth(labelSize2.X).CreatePart(text, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					o.SetFontSize(fontSize);
					o.SetPrefixImage(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("common/1444522")));
					o.SetTextColor(Color.get_Red());
				})
					.Build()).set_Parent((Container)(object)flow);
				((View<IPresenter>)this).Build(buildPanel);
				return;
			}
			foreach (Volunteer volunteer in _results.Opener.Volunteers)
			{
				Point labelSize = LabelUtil.GetLabelSize((FontSize)24, volunteer.AccountName + volunteer.Updated.AsTimeAgo());
				((Control)new FormattedLabelBuilder().SetHeight(labelSize.Y).SetWidth(labelSize.X).CreatePart(volunteer.AccountName, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					o.SetLink((Action)delegate
					{
						GameService.Content.PlaySoundEffectByName("button-click");
						CopyText(volunteer.AccountName);
					});
				})
					.CreatePart(volunteer.Updated.ToLocalTime().AsTimeAgo(), (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						o.SetFontSize((FontSize)11);
						o.MakeItalic();
					})
					.Build()).set_Parent((Container)(object)flow);
			}
			((View<IPresenter>)this).Build(buildPanel);
		}

		private async void CopyText(string text)
		{
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text))
			{
				ScreenNotification.ShowNotification("'" + text + "' copied to clipboard.", (NotificationType)0, (Texture2D)null, 4);
			}
		}
	}
}
