using System;
using System.Collections.Generic;
using Blish_HUD.Controls;

namespace Kenedia.Modules.BuildsManager
{
	public class SpecLine : IDisposable
	{
		private bool disposed;

		public int Index;

		private API.Specialization _Specialization;

		public List<API.Trait> Traits = new List<API.Trait>();

		public EventHandler Changed;

		public Specialization_Control Control;

		public API.Specialization Specialization
		{
			get
			{
				return _Specialization;
			}
			set
			{
				_Specialization = value;
				Traits = new List<API.Trait>();
				Changed?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				_Specialization?.Dispose();
				Traits?.DisposeAll();
				Specialization_Control control = Control;
				if (control != null)
				{
					((Control)control).Dispose();
				}
			}
		}
	}
}
