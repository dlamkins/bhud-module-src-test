using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace SL.ChatLinks.UI
{
	public sealed class AsyncView : View
	{
		[CompilerGenerated]
		private Func<IView> _003Cother_003EP;

		private readonly ViewContainer _container;

		public AsyncView(Func<IView> other)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			_003Cother_003EP = other;
			_container = new ViewContainer();
			((View)this)._002Ector();
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			_container.Show(_003Cother_003EP());
			return Task.FromResult(result: true);
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_container).set_Parent(buildPanel);
			((Container)_container).set_WidthSizingMode((SizingMode)2);
			((Container)_container).set_HeightSizingMode((SizingMode)2);
		}
	}
}
