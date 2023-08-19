using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Nekres.Mistwar.Core.UI.Controls;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Core.Services
{
	internal class MarkerService : IDisposable
	{
		private MarkerBillboard _billboard;

		public MarkerService()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			MarkerBillboard markerBillboard = new MarkerBillboard();
			((Control)markerBillboard).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			Rectangle absoluteBounds = ((Control)GameService.Graphics.get_SpriteScreen()).get_AbsoluteBounds();
			((Control)markerBillboard).set_Size(((Rectangle)(ref absoluteBounds)).get_Size());
			((Control)markerBillboard).set_Location(new Point(0, 0));
			((Control)markerBillboard).set_Visible(false);
			_billboard = markerBillboard;
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}

		public void ReloadMarkers(IEnumerable<WvwObjectiveEntity> entities)
		{
			_billboard.WvwObjectives = entities;
		}

		public void Toggle(bool forceHide = false)
		{
			_billboard?.Toggle(forceHide);
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				Toggle();
			}
			else
			{
				Toggle(forceHide: true);
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				Toggle();
			}
			else
			{
				Toggle(forceHide: true);
			}
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			MarkerBillboard billboard = _billboard;
			if (billboard != null)
			{
				((Control)billboard).Dispose();
			}
		}
	}
}
