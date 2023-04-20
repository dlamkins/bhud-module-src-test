using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Settings.Enums;
using RaidClears.Shared.Controls;

namespace RaidClears.Features.Fractals.Services
{
	public class FractalMapWatcherService : IDisposable
	{
		protected bool _isOnFractalMap;

		protected Encounters.Fractal? _fractal;

		protected string _fractalApiName = string.Empty;

		protected string _fractalName = string.Empty;

		public event EventHandler<string>? FractalComplete;

		public event EventHandler<List<string>>? CompletedFractal;

		public FractalMapWatcherService()
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}

		public void DispatchCurrentClears()
		{
			Dictionary<Encounters.Fractal, DateTime> clears = new Dictionary<Encounters.Fractal, DateTime>();
			if (!Service.FractalPersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
			{
				clears = new Dictionary<Encounters.Fractal, DateTime>();
			}
			List<string> clearedStrikesThisReset = new List<string>();
			foreach (KeyValuePair<Encounters.Fractal, DateTime> entry in clears)
			{
				if (entry.Value >= Service.ResetWatcher.LastDailyReset)
				{
					clearedStrikesThisReset.Add(entry.Key.GetApiLabel());
				}
			}
			this.CompletedFractal?.Invoke(this, clearedStrikesThisReset);
		}

		protected void MarkCompleted(Encounters.Fractal fractal)
		{
			Service.FractalPersistance.SaveClear(Service.CurrentAccountName, fractal);
			DispatchCurrentClears();
		}

		protected void Reset()
		{
			_fractal = null;
			_isOnFractalMap = false;
			_fractalApiName = string.Empty;
			_fractalName = string.Empty;
		}

		private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			if (Enum.IsDefined(typeof(MapIds.FractalMaps), e.get_Value()))
			{
				Reset();
				_isOnFractalMap = true;
				_fractalApiName = ((MapIds.FractalMaps)e.get_Value()).GetApiLabel();
				_fractalName = ((MapIds.FractalMaps)e.get_Value()).GetLabel();
				_fractal = ((MapIds.FractalMaps)e.get_Value()).GetFractal();
			}
			else
			{
				if (!_isOnFractalMap)
				{
					return;
				}
				switch (Service.Settings.FractalSettings.CompletionMethod.get_Value())
				{
				case StrikeComplete.MAP_CHANGE:
					this.FractalComplete?.Invoke(this, _fractalApiName);
					if (_fractal.HasValue)
					{
						MarkCompleted(_fractal.Value);
					}
					break;
				case StrikeComplete.POPUP:
				{
					ConfirmDialog dialog = new ConfirmDialog(_fractalName, Strings.Strike_Confirm_Message, new ButtonDefinition[2]
					{
						new ButtonDefinition(Strings.Strike_Confirm_Btn_Yes, DialogResult.OK),
						new ButtonDefinition(Strings.Strike_Confirm_Btn_No, DialogResult.Cancel)
					});
					DialogResult num = await dialog.ShowDialog();
					((Control)dialog).Dispose();
					if (num == DialogResult.OK)
					{
						this.FractalComplete?.Invoke(this, _fractalApiName);
						if (_fractal.HasValue)
						{
							MarkCompleted(_fractal.Value);
						}
					}
					break;
				}
				}
				Reset();
			}
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}
	}
}
