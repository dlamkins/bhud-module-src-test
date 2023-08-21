using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.UI.Clears
{
	public sealed class ClearsView : View
	{
		private readonly IReadOnlyList<Clear> _clears;

		private readonly Texture2D _greenTick;

		private readonly Texture2D _redCross;

		private ClearsView()
			: this()
		{
			_greenTick = ProofLogix.Instance.ContentsManager.GetTexture("green-tick.gif");
			_redCross = ProofLogix.Instance.ContentsManager.GetTexture("red-cross.gif");
		}

		public ClearsView(List<Clear> clears)
			: this()
		{
			_clears = clears;
		}

		public ClearsView(List<string> clears)
			: this()
		{
			List<Raid> raids = ProofLogix.Instance.Resources.GetRaids();
			_clears = (from wing in raids.SelectMany((Raid raid) => raid.Wings)
				where !string.IsNullOrEmpty(wing.Id)
				select new Clear
				{
					Name = ProofLogix.Instance.Resources.GetMapName(wing.MapId),
					Encounters = wing.Events.Select((Raid.Wing.Event ev) => new Boss
					{
						Name = ev.Name,
						Cleared = clears.Any((string id) => id.Equals(ev.Id))
					}).ToList()
				}).ToList();
		}

		protected override void Unload()
		{
			((GraphicsResource)_greenTick).Dispose();
			((GraphicsResource)_redCross).Dispose();
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(4f, 7f));
			FlowPanel panel = val;
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				((Control)panel).set_Width(e.get_CurrentRegion().Width);
				((Control)panel).set_Height(e.get_CurrentRegion().Height);
			});
			foreach (Clear clear in _clears)
			{
				bool completed = clear.Encounters.All((Boss encounter) => encounter.Cleared);
				bool none = !clear.Encounters.Any((Boss encounter) => encounter.Cleared);
				FlowPanelWithIcon flowPanelWithIcon = new FlowPanelWithIcon(AsyncTexture2D.op_Implicit(completed ? _greenTick : (none ? _redCross : Textures.get_TransparentPixel())));
				((Control)flowPanelWithIcon).set_Parent((Container)(object)panel);
				((Control)flowPanelWithIcon).set_Width(((Container)panel).get_ContentRegion().Width - 24);
				((Container)flowPanelWithIcon).set_HeightSizingMode((SizingMode)1);
				((Panel)flowPanelWithIcon).set_Title(clear.Name);
				((Panel)flowPanelWithIcon).set_CanCollapse(true);
				((FlowPanel)flowPanelWithIcon).set_ControlPadding(new Vector2(5f, 5f));
				((FlowPanel)flowPanelWithIcon).set_OuterControlPadding(new Vector2(5f, 5f));
				((FlowPanel)flowPanelWithIcon).set_FlowDirection((ControlFlowDirection)3);
				((Panel)flowPanelWithIcon).set_Collapsed(completed || none);
				FlowPanelWithIcon wingCategory = flowPanelWithIcon;
				((Container)panel).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)wingCategory).set_Width(e.get_CurrentRegion().Width - 24);
				});
				foreach (Boss encounter2 in clear.Encounters)
				{
					Texture2D icon = (encounter2.Cleared ? _greenTick : _redCross);
					Point size = LabelUtil.GetLabelSize((FontSize)20, encounter2.Name, hasPrefix: true);
					((Control)new FormattedLabelBuilder().SetWidth(size.X).SetHeight(size.Y + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y).CreatePart(encounter2.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						o.SetFontSize((FontSize)20);
						o.SetPrefixImage(AsyncTexture2D.op_Implicit(icon));
						o.SetHyperLink(AssetUtil.GetWikiLink(encounter2.Name));
					})
						.Build()).set_Parent((Container)(object)wingCategory);
				}
			}
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
