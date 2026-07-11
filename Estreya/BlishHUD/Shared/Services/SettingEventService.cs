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
		private class RegisteredSetting
		{
			public SettingEntry Setting { get; set; }

			public IComplianceRequisite ComplianceRequisite { get; set; }
		}

		private static readonly Logger _logger = Logger.GetLogger<SettingEventService>();

		private List<RegisteredSetting> _registeredForDisabledUpdates;

		private List<RegisteredSetting> _registeredForRangeUpdates;

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
			_registeredForRangeUpdates = new List<RegisteredSetting>();
			_registeredForDisabledUpdates = new List<RegisteredSetting>();
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
				if (!_registeredForRangeUpdates.Any(delegate(RegisteredSetting p)
				{
					SettingEntry setting = p.Setting;
					return ((setting != null) ? setting.get_EntryKey() : null) == settingEntry.get_EntryKey();
				}))
				{
					_registeredForRangeUpdates.Add(new RegisteredSetting
					{
						Setting = settingEntry,
						ComplianceRequisite = defaultRange
					});
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
				_registeredForRangeUpdates.RemoveAll(delegate(RegisteredSetting p)
				{
					SettingEntry setting = p.Setting;
					return ((setting != null) ? setting.get_EntryKey() : null) == settingEntry.get_EntryKey();
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
				if (!_registeredForDisabledUpdates.Any(delegate(RegisteredSetting p)
				{
					SettingEntry setting = p.Setting;
					return ((setting != null) ? setting.get_EntryKey() : null) == settingEntry.get_EntryKey();
				}))
				{
					_registeredForDisabledUpdates.Add(new RegisteredSetting
					{
						Setting = settingEntry,
						ComplianceRequisite = defaultRange
					});
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
				_registeredForDisabledUpdates.RemoveAll(delegate(RegisteredSetting p)
				{
					SettingEntry setting = p.Setting;
					return ((setting != null) ? setting.get_EntryKey() : null) == settingEntry.get_EntryKey();
				});
			}
			_logger.Debug("Stopped tracking setting \"" + settingEntry.get_EntryKey() + "\" for disabled updates.");
		}

		private void CheckRangeUpdates()
		{
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			if (!_rangeStateLock.IsFree())
			{
				return;
			}
			using (_rangeStateLock.Lock())
			{
				for (int i = 0; i < _registeredForRangeUpdates.Count; i++)
				{
					RegisteredSetting registeredEntry = _registeredForRangeUpdates[i];
					bool changed = false;
					IEnumerable<IComplianceRequisite> ranges = SettingComplianceExtensions.GetComplianceRequisite(registeredEntry.Setting);
					SettingEntry setting = registeredEntry.Setting;
					IComplianceRequisite numberRange;
					int num;
					if ((setting is SettingEntry<int> || setting is SettingEntry<float>) ? true : false)
					{
						List<IComplianceRequisite> numberRanges = ranges.Where((IComplianceRequisite r) => (r is IntRangeRangeComplianceRequisite || r is FloatRangeRangeComplianceRequisite) ? true : false).ToList();
						if (!numberRanges.Any())
						{
							if (registeredEntry.ComplianceRequisite != null)
							{
								registeredEntry.ComplianceRequisite = null;
								changed = true;
							}
						}
						else
						{
							numberRange = numberRanges.First();
							if (registeredEntry.ComplianceRequisite != numberRange)
							{
								if (registeredEntry.ComplianceRequisite == null)
								{
									goto IL_0179;
								}
								if (numberRange is IntRangeRangeComplianceRequisite)
								{
									IntRangeRangeComplianceRequisite intRange = (IntRangeRangeComplianceRequisite)(object)numberRange;
									IComplianceRequisite complianceRequisite = registeredEntry.ComplianceRequisite;
									if (complianceRequisite is IntRangeRangeComplianceRequisite)
									{
										IntRangeRangeComplianceRequisite priorIntRange = (IntRangeRangeComplianceRequisite)(object)complianceRequisite;
										if (((IntRangeRangeComplianceRequisite)(ref intRange)).get_MinValue() != ((IntRangeRangeComplianceRequisite)(ref priorIntRange)).get_MinValue() || ((IntRangeRangeComplianceRequisite)(ref intRange)).get_MaxValue() != ((IntRangeRangeComplianceRequisite)(ref priorIntRange)).get_MaxValue())
										{
											goto IL_0179;
										}
									}
								}
								if (numberRange is FloatRangeRangeComplianceRequisite)
								{
									FloatRangeRangeComplianceRequisite floatRange = (FloatRangeRangeComplianceRequisite)(object)numberRange;
									IComplianceRequisite complianceRequisite = registeredEntry.ComplianceRequisite;
									if (complianceRequisite is FloatRangeRangeComplianceRequisite)
									{
										FloatRangeRangeComplianceRequisite priorFloatRange = (FloatRangeRangeComplianceRequisite)(object)complianceRequisite;
										num = ((((FloatRangeRangeComplianceRequisite)(ref floatRange)).get_MinValue() != ((FloatRangeRangeComplianceRequisite)(ref priorFloatRange)).get_MinValue() || ((FloatRangeRangeComplianceRequisite)(ref floatRange)).get_MaxValue() != ((FloatRangeRangeComplianceRequisite)(ref priorFloatRange)).get_MaxValue()) ? 1 : 0);
										goto IL_017a;
									}
								}
								num = 0;
								goto IL_017a;
							}
						}
					}
					goto IL_0183;
					IL_0179:
					num = 1;
					goto IL_017a;
					IL_017a:
					changed = (byte)num != 0;
					registeredEntry.ComplianceRequisite = numberRange;
					goto IL_0183;
					IL_0183:
					if (changed)
					{
						try
						{
							this.RangeUpdated?.Invoke(this, new ComplianceUpdated(registeredEntry.Setting, registeredEntry.ComplianceRequisite));
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
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			if (!_disabledStateLock.IsFree())
			{
				return;
			}
			using (_disabledStateLock.Lock())
			{
				for (int i = 0; i < _registeredForDisabledUpdates.Count; i++)
				{
					RegisteredSetting registeredEntry = _registeredForDisabledUpdates[i];
					bool changed = false;
					List<IComplianceRequisite> disabledRanges = (from r in SettingComplianceExtensions.GetComplianceRequisite(registeredEntry.Setting)
						where r is SettingDisabledComplianceRequisite
						select r).ToList();
					if (!disabledRanges.Any())
					{
						if (registeredEntry.ComplianceRequisite != null)
						{
							registeredEntry.ComplianceRequisite = (IComplianceRequisite)(object)new SettingDisabledComplianceRequisite(false);
							changed = true;
						}
					}
					else
					{
						IComplianceRequisite disabledRange = (registeredEntry.ComplianceRequisite = disabledRanges.First());
						if (registeredEntry.ComplianceRequisite != disabledRange)
						{
							changed = true;
						}
					}
					if (changed)
					{
						try
						{
							this.DisabledUpdated?.Invoke(this, new ComplianceUpdated(registeredEntry.Setting, registeredEntry.ComplianceRequisite));
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
