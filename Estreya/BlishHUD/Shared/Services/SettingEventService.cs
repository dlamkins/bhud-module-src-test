using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Services
{
	public class SettingEventService : ManagedService
	{
		private static readonly Logger _logger = Logger.GetLogger<SettingEventService>();

		private List<(SettingEntry SettingEntry, IComplianceRequisite ComplianceRequisite)> _registeredForDisabledUpdates;

		private List<(SettingEntry SettingEntry, IComplianceRequisite ComplianceRequisite)> _registeredForRangeUpdates;

		private AsyncLock _disabledStateLock = new AsyncLock();

		private AsyncLock _rangeStateLock = new AsyncLock();

		public event EventHandler<ComplianceUpdated> RangeUpdated;

		public event EventHandler<ComplianceUpdated> DisabledUpdated;

		public SettingEventService(ServiceConfiguration configuration)
			: base(configuration)
		{
		}

		protected override Task Initialize()
		{
			_registeredForRangeUpdates = new List<(SettingEntry, IComplianceRequisite)>();
			_registeredForDisabledUpdates = new List<(SettingEntry, IComplianceRequisite)>();
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			using (_rangeStateLock.Lock())
			{
				_registeredForRangeUpdates?.Clear();
				_registeredForRangeUpdates = null;
			}
			using (_disabledStateLock.Lock())
			{
				_registeredForDisabledUpdates?.Clear();
				_registeredForDisabledUpdates = null;
			}
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
			if (settingEntry == null)
			{
				throw new ArgumentNullException("settingEntry");
			}
			using (_rangeStateLock.Lock())
			{
				if (!_registeredForRangeUpdates.Any(delegate((SettingEntry SettingEntry, IComplianceRequisite ComplianceRequisite) p)
				{
					var (val, _) = p;
					return ((val != null) ? val.get_EntryKey() : null) == settingEntry.get_EntryKey();
				}))
				{
					_registeredForRangeUpdates.Add((settingEntry, defaultRange));
					_logger.Debug("Started tracking setting \"" + settingEntry.get_EntryKey() + "\" for range updates.");
				}
			}
		}

		public void RemoveFromRangeCheck(SettingEntry settingEntry)
		{
			if (settingEntry == null)
			{
				throw new ArgumentNullException("settingEntry");
			}
			using (_rangeStateLock.Lock())
			{
				_registeredForRangeUpdates.RemoveAll(delegate((SettingEntry SettingEntry, IComplianceRequisite ComplianceRequisite) p)
				{
					var (val, _) = p;
					return ((val != null) ? val.get_EntryKey() : null) == settingEntry.get_EntryKey();
				});
			}
			_logger.Debug("Stopped tracking setting \"" + settingEntry.get_EntryKey() + "\" for range updates.");
		}

		public void AddForDisabledCheck(SettingEntry settingEntry, IComplianceRequisite defaultRange = null)
		{
			if (settingEntry == null)
			{
				throw new ArgumentNullException("settingEntry");
			}
			using (_disabledStateLock.Lock())
			{
				if (!_registeredForDisabledUpdates.Any(delegate((SettingEntry SettingEntry, IComplianceRequisite ComplianceRequisite) p)
				{
					var (val, _) = p;
					return ((val != null) ? val.get_EntryKey() : null) == settingEntry.get_EntryKey();
				}))
				{
					_registeredForDisabledUpdates.Add((settingEntry, defaultRange));
					_logger.Debug("Started tracking setting \"" + settingEntry.get_EntryKey() + "\" for disabled updates.");
				}
			}
		}

		public void RemoveFromDisabledCheck(SettingEntry settingEntry)
		{
			if (settingEntry == null)
			{
				throw new ArgumentNullException("settingEntry");
			}
			using (_disabledStateLock.Lock())
			{
				_registeredForDisabledUpdates.RemoveAll(delegate((SettingEntry SettingEntry, IComplianceRequisite ComplianceRequisite) p)
				{
					var (val, _) = p;
					return ((val != null) ? val.get_EntryKey() : null) == settingEntry.get_EntryKey();
				});
			}
			_logger.Debug("Stopped tracking setting \"" + settingEntry.get_EntryKey() + "\" for disabled updates.");
		}

		private void CheckRangeUpdates()
		{
			if (!_rangeStateLock.IsFree())
			{
				return;
			}
			using (_rangeStateLock.Lock())
			{
				for (int i = 0; i < _registeredForRangeUpdates.Count; i++)
				{
					(SettingEntry, IComplianceRequisite) settingPair = _registeredForRangeUpdates[i];
					bool changed = false;
					SettingEntry setting = settingPair.Item1;
					IComplianceRequisite priorRange = settingPair.Item2;
					IEnumerable<IComplianceRequisite> ranges = SettingComplianceExtensions.GetComplianceRequisite(setting);
					if ((setting is SettingEntry<int> || setting is SettingEntry<float>) ? true : false)
					{
						IEnumerable<IComplianceRequisite> numberRanges = ranges.Where((IComplianceRequisite r) => (r is IntRangeRangeComplianceRequisite || r is FloatRangeRangeComplianceRequisite) ? true : false);
						if (!numberRanges.Any())
						{
							if (priorRange != null)
							{
								settingPair.Item2 = null;
								changed = true;
							}
						}
						else if (priorRange != (settingPair.Item2 = numberRanges.First()))
						{
							changed = true;
						}
					}
					if (changed)
					{
						try
						{
							this.RangeUpdated?.Invoke(this, new ComplianceUpdated(setting, settingPair.Item2));
						}
						catch (Exception)
						{
						}
					}
				}
			}
		}

		private void CheckDisabledUpdates()
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			if (!_disabledStateLock.IsFree())
			{
				return;
			}
			using (_disabledStateLock.Lock())
			{
				for (int i = 0; i < _registeredForDisabledUpdates.Count; i++)
				{
					(SettingEntry, IComplianceRequisite) settingPair = _registeredForDisabledUpdates[i];
					bool changed = false;
					SettingEntry setting = settingPair.Item1;
					IComplianceRequisite priorRange = settingPair.Item2;
					IEnumerable<IComplianceRequisite> disabledRanges = from r in SettingComplianceExtensions.GetComplianceRequisite(setting)
						where r is SettingDisabledComplianceRequisite
						select r;
					if (!disabledRanges.Any())
					{
						if (priorRange != null)
						{
							settingPair.Item2 = (IComplianceRequisite)(object)new SettingDisabledComplianceRequisite(false);
							changed = true;
						}
					}
					else if (priorRange != (settingPair.Item2 = disabledRanges.First()))
					{
						changed = true;
					}
					if (changed)
					{
						try
						{
							this.DisabledUpdated?.Invoke(this, new ComplianceUpdated(setting, settingPair.Item2));
						}
						catch (Exception)
						{
						}
					}
				}
			}
		}
	}
}
