using System;
using System.Drawing;
using Blish_HUD;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;

namespace Nekres.Inquest_Module.Core.Controllers
{
	internal class AutoClickController : IDisposable
	{
		private DateTime _nextHoldClick = DateTime.UtcNow;

		private DateTime _nextToggleClick = DateTime.UtcNow;

		private Point _togglePos;

		private bool _toggleActive;

		private KeyBinding AutoClickHoldKey => InquestModule.ModuleInstance.AutoClickHoldKeySetting.get_Value();

		private KeyBinding AutoClickToggleKey => InquestModule.ModuleInstance.AutoClickToggleKeySetting.get_Value();

		public AutoClickController()
		{
			AutoClickHoldKey.set_Enabled(true);
			AutoClickToggleKey.set_Enabled(true);
			AutoClickToggleKey.add_Activated((EventHandler<EventArgs>)OnAutoClickToggleActivate);
		}

		private void OnAutoClickToggleActivate(object o, EventArgs e)
		{
			_togglePos = Mouse.GetPosition();
			_toggleActive = !_toggleActive;
		}

		public void Update()
		{
			if (!_toggleActive && AutoClickHoldKey.get_IsTriggering() && DateTime.UtcNow > _nextHoldClick)
			{
				Mouse.DoubleClick((MouseButton)0, -1, -1, true);
				_nextHoldClick = DateTime.UtcNow.AddMilliseconds(50.0);
			}
			if (_toggleActive && DateTime.UtcNow > _nextToggleClick)
			{
				Mouse.SetPosition(_togglePos.X, _togglePos.Y, true);
				Mouse.DoubleClick((MouseButton)0, -1, -1, true);
				_nextToggleClick = DateTime.UtcNow.AddMilliseconds(RandomUtil.GetRandom(50, 500));
			}
		}

		public void Dispose()
		{
			AutoClickToggleKey.remove_Activated((EventHandler<EventArgs>)OnAutoClickToggleActivate);
		}
	}
}
