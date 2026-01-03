using System;
using System.Collections.Generic;
using Blish_HUD.Controls;

namespace LoreBridge.Controls
{
	public class FormattedLabelBuilderCustom
	{
		private readonly List<FormattedLabelPartCustom> _parts = new List<FormattedLabelPartCustom>();

		private bool _wrapText;

		private int _width;

		private int _height;

		private bool _autoSizeHeight;

		private bool _autoSizeWidth;

		private bool _showShadow;

		public FormattedLabelPartBuilderCustom CreatePart(string text)
		{
			return new FormattedLabelPartBuilderCustom(text);
		}

		public FormattedLabelBuilderCustom CreatePart(string text, Action<FormattedLabelPartBuilderCustom> creationFunc)
		{
			FormattedLabelPartBuilderCustom builder = new FormattedLabelPartBuilderCustom(text);
			creationFunc?.Invoke(builder);
			_parts.Add(builder.Build());
			return this;
		}

		public FormattedLabelBuilderCustom Wrap()
		{
			_wrapText = true;
			return this;
		}

		public FormattedLabelBuilderCustom SetWidth(int width)
		{
			_autoSizeWidth = false;
			_width = width;
			return this;
		}

		public FormattedLabelBuilderCustom AutoSizeHeight()
		{
			_height = 0;
			_autoSizeHeight = true;
			return this;
		}

		public FormattedLabelBuilderCustom ShowShadow()
		{
			_showShadow = true;
			return this;
		}

		public FormattedLabelCustom Build()
		{
			FormattedLabelCustom formattedLabelCustom = new FormattedLabelCustom(_parts, _wrapText, _autoSizeWidth, _autoSizeHeight, _showShadow);
			((Control)formattedLabelCustom).set_Width(_width);
			((Control)formattedLabelCustom).set_Height(_height);
			return formattedLabelCustom;
		}
	}
}
