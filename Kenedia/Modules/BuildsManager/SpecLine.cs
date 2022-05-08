using System;
using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager
{
	public class SpecLine
	{
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
	}
}
