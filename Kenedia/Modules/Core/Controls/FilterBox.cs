using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Controls
{
	public class FilterBox : TextBox
	{
		private double _lastFiltering;

		private bool _performFiltering;

		public double FilteringDelay { get; set; }

		public Action<string> PerformFiltering { get; set; }

		public bool FilteringOnEnter { get; set; }

		public bool FilteringOnTextChange { get; set; }

		public FilterBox()
		{
			((TextInputBase)this).add_TextChanged((EventHandler<EventArgs>)FilterBox_TextChanged);
		}

		public void RequestFilter()
		{
			_performFiltering = true;
		}

		public void ForceFilter()
		{
			PerformFiltering?.Invoke(((TextInputBase)this).get_Text());
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((TextInputBase)this).DoUpdate(gameTime);
			if (_performFiltering && gameTime.get_TotalGameTime().TotalMilliseconds - _lastFiltering >= FilteringDelay)
			{
				_lastFiltering = gameTime.get_TotalGameTime().TotalMilliseconds;
				_performFiltering = false;
				PerformFiltering?.Invoke(((TextInputBase)this).get_Text());
			}
		}

		protected override void OnEnterPressed(EventArgs e)
		{
			base.OnEnterPressed(e);
			if (FilteringOnEnter)
			{
				_performFiltering = true;
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			((TextInputBase)this).remove_TextChanged((EventHandler<EventArgs>)FilterBox_TextChanged);
		}

		private void FilterBox_TextChanged(object sender, EventArgs e)
		{
			if (FilteringOnTextChange)
			{
				_performFiltering = true;
			}
		}
	}
}
