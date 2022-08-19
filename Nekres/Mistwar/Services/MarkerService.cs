using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Nekres.Mistwar.Entities;
using Nekres.Mistwar.UI.Controls;

namespace Nekres.Mistwar.Services
{
	internal class MarkerService : IDisposable
	{
		private MarkerBillboard _billboard;

		public MarkerService(IEnumerable<WvwObjectiveEntity> currentObjectives = null)
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
			((Control)markerBillboard).set_Visible(currentObjectives != null);
			markerBillboard.WvwObjectives = currentObjectives;
			_billboard = markerBillboard;
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}

		public void ReloadMarkers(IEnumerable<WvwObjectiveEntity> entities)
		{
			_billboard.WvwObjectives = entities;
			Toggle(keepOpen: true);
		}

		public void Toggle(bool keepOpen = false)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				if (keepOpen && ((Control)_billboard).get_Visible())
				{
					((Control)_billboard).Hide();
					((Control)_billboard).Show();
				}
				else
				{
					_billboard?.Toggle();
				}
			}
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				Toggle();
			}
			else
			{
				((Control)_billboard).Hide();
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				Toggle(keepOpen: true);
			}
			else
			{
				((Control)_billboard).Hide();
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
