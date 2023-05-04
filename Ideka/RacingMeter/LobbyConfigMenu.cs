using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Ideka.NetCommon;

namespace Ideka.RacingMeter
{
	public class LobbyConfigMenu : IDisposable
	{
		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly ContextMenuStrip _menu;

		private readonly ContextMenuStripItem _markRacers;

		private bool _reflecting;

		public LobbyConfigMenu()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			DisposableCollection dc = _dc;
			ContextMenuStripItem val = new ContextMenuStripItem(Strings.SettingOnlineMarkRacers);
			val.set_CanCheck(true);
			((Control)val).set_BasicTooltipText(Strings.SettingOnlineMarkRacersText);
			_markRacers = dc.Add<ContextMenuStripItem>(val);
			_markRacers.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (!_reflecting)
				{
					RacingModule.Settings.OnlineMarkRacers.Value = _markRacers.get_Checked();
				}
			});
			_menu = _dc.Add<ContextMenuStrip>(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)menu));
			_dc.Add(RacingModule.Settings.OnlineMarkRacers.OnChangedAndNow(delegate(bool value)
			{
				_reflecting = true;
				_markRacers.set_Checked(value);
				_reflecting = false;
			}));
			IEnumerable<ContextMenuStripItem> menu()
			{
				yield return _markRacers;
			}
		}

		public void Show(Control? control)
		{
			_menu.Show(control);
		}

		public void Hide()
		{
			((Control)_menu).Hide();
		}

		public void Dispose()
		{
			_dc.Dispose();
		}
	}
}
