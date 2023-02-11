using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.State
{
	public class SettingEventState : ManagedState
	{
		private static readonly Logger _logger = Logger.GetLogger<SettingEventState>();

		private Dictionary<SettingEntry, IComplianceRequisite> _registeredForRangeUpdates;

		public event EventHandler<ComplianceUpdated> RangeUpdated;

		public SettingEventState(StateConfiguration configuration)
			: base(configuration)
		{
		}

		protected override Task Initialize()
		{
			_registeredForRangeUpdates = new Dictionary<SettingEntry, IComplianceRequisite>();
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			_registeredForRangeUpdates?.Clear();
			_registeredForRangeUpdates = null;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			CheckRangeUpdates();
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		public void AddForRangeCheck(SettingEntry settingEntry, IComplianceRequisite defaultRange = null)
		{
			if (!_registeredForRangeUpdates.ContainsKey(settingEntry))
			{
				_registeredForRangeUpdates.Add(settingEntry, defaultRange);
				_logger.Debug("Started tracking setting \"" + settingEntry.get_EntryKey() + "\" for range updates.");
			}
		}

		public void RemoveFromRangeCheck(SettingEntry settingEntry)
		{
			if (_registeredForRangeUpdates.ContainsKey(settingEntry))
			{
				_registeredForRangeUpdates.Remove(settingEntry);
				_logger.Debug("Stopped tracking setting \"" + settingEntry.get_EntryKey() + "\" for range updates.");
			}
		}

		private void CheckRangeUpdates()
		{
			for (int i = 0; i < _registeredForRangeUpdates.Count; i++)
			{
				KeyValuePair<SettingEntry, IComplianceRequisite> settingPair = _registeredForRangeUpdates.ElementAt(i);
				bool changed = false;
				SettingEntry setting = settingPair.Key;
				IComplianceRequisite priorRange = settingPair.Value;
				IEnumerable<IComplianceRequisite> ranges = SettingComplianceExtensions.GetComplianceRequisite(setting);
				if (setting is SettingEntry<int> || setting is SettingEntry<float>)
				{
					List<IComplianceRequisite> numberRanges = ranges.Where((IComplianceRequisite r) => r is IntRangeRangeComplianceRequisite || r is FloatRangeRangeComplianceRequisite).ToList();
					if (numberRanges.Count == 0)
					{
						if (priorRange != null)
						{
							_registeredForRangeUpdates[setting] = null;
							changed = true;
						}
					}
					else
					{
						IComplianceRequisite numberRange = numberRanges.First();
						if (priorRange != numberRange)
						{
							_registeredForRangeUpdates[setting] = numberRange;
							changed = true;
						}
					}
				}
				if (changed)
				{
					try
					{
						this.RangeUpdated?.Invoke(this, new ComplianceUpdated(setting, _registeredForRangeUpdates[setting]));
					}
					catch (Exception)
					{
					}
				}
			}
		}
	}
}
