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

		private Dictionary<SettingEntry, IComplianceRequisite> _registeredForDisabledUpdates;

		public event EventHandler<ComplianceUpdated> RangeUpdated;

		public event EventHandler<ComplianceUpdated> DisabledUpdated;

		public SettingEventState(StateConfiguration configuration)
			: base(configuration)
		{
		}

		protected override Task Initialize()
		{
			_registeredForRangeUpdates = new Dictionary<SettingEntry, IComplianceRequisite>();
			_registeredForDisabledUpdates = new Dictionary<SettingEntry, IComplianceRequisite>();
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			_registeredForRangeUpdates?.Clear();
			_registeredForRangeUpdates = null;
			_registeredForDisabledUpdates?.Clear();
			_registeredForDisabledUpdates = null;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			CheckRangeUpdates();
			CheckDisabledUpdates();
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

		public void AddForDisabledCheck(SettingEntry settingEntry, IComplianceRequisite defaultRange = null)
		{
			if (!_registeredForDisabledUpdates.ContainsKey(settingEntry))
			{
				_registeredForDisabledUpdates.Add(settingEntry, defaultRange);
				_logger.Debug("Started tracking setting \"" + settingEntry.get_EntryKey() + "\" for disabled updates.");
			}
		}

		public void RemoveFromDisabledCheck(SettingEntry settingEntry)
		{
			if (_registeredForDisabledUpdates.ContainsKey(settingEntry))
			{
				_registeredForDisabledUpdates.Remove(settingEntry);
				_logger.Debug("Stopped tracking setting \"" + settingEntry.get_EntryKey() + "\" for disabled updates.");
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
						_registeredForRangeUpdates[setting] = numberRange;
						if (priorRange != numberRange)
						{
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

		private void CheckDisabledUpdates()
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _registeredForDisabledUpdates.Count; i++)
			{
				KeyValuePair<SettingEntry, IComplianceRequisite> settingPair = _registeredForDisabledUpdates.ElementAt(i);
				bool changed = false;
				SettingEntry setting = settingPair.Key;
				IComplianceRequisite priorRange = settingPair.Value;
				List<IComplianceRequisite> disabledRanges = (from r in SettingComplianceExtensions.GetComplianceRequisite(setting)
					where r is SettingDisabledComplianceRequisite
					select r).ToList();
				if (disabledRanges.Count == 0)
				{
					if (priorRange != null)
					{
						_registeredForDisabledUpdates[setting] = (IComplianceRequisite)(object)new SettingDisabledComplianceRequisite(false);
						changed = true;
					}
				}
				else
				{
					IComplianceRequisite disabledRange = disabledRanges.First();
					_registeredForDisabledUpdates[setting] = disabledRange;
					if (priorRange != disabledRange)
					{
						changed = true;
					}
				}
				if (changed)
				{
					try
					{
						this.DisabledUpdated?.Invoke(this, new ComplianceUpdated(setting, _registeredForDisabledUpdates[setting]));
					}
					catch (Exception)
					{
					}
				}
			}
		}
	}
}
