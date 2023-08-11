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
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
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
				FlowPanel val2 = new FlowPanel();
				((Control)val2).set_Parent((Container)(object)panel);
				((Control)val2).set_Width(((Container)panel).get_ContentRegion().Width - 24);
				((Container)val2).set_HeightSizingMode((SizingMode)1);
				((Panel)val2).set_Title(clear.Name);
				((Panel)val2).set_CanCollapse(true);
				val2.set_ControlPadding(new Vector2(5f, 5f));
				val2.set_OuterControlPadding(new Vector2(5f, 5f));
				val2.set_FlowDirection((ControlFlowDirection)3);
				FlowPanel wingCategory = val2;
				((Container)panel).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)wingCategory).set_Width(e.get_CurrentRegion().Width - 24);
				});
				foreach (Boss encounter in clear.Encounters)
				{
					Texture2D icon = (encounter.Cleared ? _greenTick : _redCross);
					Point size = LabelUtil.GetLabelSize((FontSize)20, encounter.Name, hasPrefix: true);
					((Control)new FormattedLabelBuilder().SetWidth(size.X).SetHeight(size.Y + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y).CreatePart(encounter.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						o.SetFontSize((FontSize)20);
						o.SetPrefixImage(AsyncTexture2D.op_Implicit(icon));
						o.SetHyperLink(AssetUtil.GetWikiLink(encounter.Name));
					})
						.Build()).set_Parent((Container)(object)wingCategory);
				}
			}
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
