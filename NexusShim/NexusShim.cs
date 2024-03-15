using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Microsoft.Xna.Framework;
using NexusShim.Nexus;
using NexusShim.UI;

namespace NexusShim
{
	[Export(typeof(Module))]
	public class NexusShim : Module
	{
		private const int UPDATE_INTERVAL = 750;

		private const int NEXUS_INTERVAL = 5000;

		private double _lastUpdate;

		private NexusIntegration _nexus;

		private uint _iconCount;

		[ImportingConstructor]
		public NexusShim([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override async Task LoadAsync()
		{
			_nexus = new NexusIntegration();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
			_nexus.TryOpen();
		}

		protected override void Update(GameTime gameTime)
		{
			double tm = gameTime.get_TotalGameTime().TotalMilliseconds;
			if (_nexus.Active && tm - _lastUpdate > 750.0)
			{
				_nexus.Update();
				UpdateCornerIcons();
				_lastUpdate = tm;
			}
			else if (!_nexus.Active && tm - _lastUpdate > 5000.0)
			{
				_nexus.TryOpen();
				_lastUpdate = tm;
			}
			CornerIcon.set_LeftOffset((int)(35.3 * (double)(_iconCount + 1)) + 2);
		}

		private void UpdateCornerIcons()
		{
			if (_nexus.IconMode == IconMode.Extend)
			{
				_iconCount = (_nexus.IconVertical ? 1u : _nexus.IconQuantity);
			}
			else
			{
				_iconCount = 0u;
			}
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new NexusStatusView(_nexus);
		}

		protected override void Unload()
		{
			_nexus?.Unload();
		}
	}
}
