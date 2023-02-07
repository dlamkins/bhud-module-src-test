using System;
using System.Collections;
using System.Collections.Generic;
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
				return ((IFontControl)((IEnumerable<Control>)((Container)this).get_Children()).FirstOrDefault((Control e) => e is IFontControl))?.Text;
			}
			set
			{
			}
		}

		protected virtual void OnFontChanged(object sender = null, EventArgs e = null)
		{
			foreach (IFontControl item in ((IEnumerable)((Container)this).get_Children()).Cast<IFontControl>())
			{
				item.Font = Font;
			}
		}

		protected override void DisposeControl()
		{
			((FlowPanel)this).DisposeControl();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			((FlowPanel)this).OnChildAdded(e);
			if (Font != null && ((object)e.get_ChangedChild()).GetType() == typeof(IFontControl))
			{
				(e.get_ChangedChild() as IFontControl).Font = Font;
			}
		}

		public FontFlowPanel()
			: this()
		{
		}
	}
}
