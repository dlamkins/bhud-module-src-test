using System;
using System.Linq;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Interfaces;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class FontFlowPanel : FlowPanel, IFontControl
	{
		private BitmapFont _font;

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (_font != value && value != null)
				{
					_font = value;
					OnFontChanged();
				}
			}
		}

		public string Text
		{
			get
			{
				return ((IFontControl)base.Children.FirstOrDefault((Control e) => e is IFontControl))?.Text;
			}
			set
			{
			}
		}

		protected virtual void OnFontChanged(object sender = null, EventArgs e = null)
		{
			foreach (IFontControl item in base.Children.Cast<IFontControl>())
			{
				item.Font = Font;
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			base.OnChildAdded(e);
			if (Font != null && e.ChangedChild.GetType() == typeof(IFontControl))
			{
				(e.ChangedChild as IFontControl).Font = Font;
			}
		}
	}
}
