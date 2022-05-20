using System;
using System.Collections.Generic;
using Blish_HUD.Controls;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel
{
	public class FormattedLabelBuilder
	{
		private readonly List<FormattedLabelPart> _parts = new List<FormattedLabelPart>();

		private bool _wrapText;

		private int _width;

		private int _height;

		private bool _autoSizeHeight;

		private bool _autoSizeWidth;

		private HorizontalAlignment _horizontalAlignment;

		private VerticalAlignment _verticalAlignment = (VerticalAlignment)1;

		public FormattedLabelPartBuilder CreatePart(string text)
		{
			return new FormattedLabelPartBuilder(text);
		}

		public FormattedLabelBuilder CreatePart(string text, Action<FormattedLabelPartBuilder> creationFunc = null)
		{
			FormattedLabelPartBuilder builder = new FormattedLabelPartBuilder(text);
			creationFunc?.Invoke(builder);
			_parts.Add(builder.Build());
			return this;
		}

		public FormattedLabelBuilder CreatePart(FormattedLabelPartBuilder builder)
		{
			_parts.Add(builder.Build());
			return this;
		}

		public FormattedLabelBuilder Wrap()
		{
			_wrapText = true;
			return this;
		}

		public FormattedLabelBuilder SetWidth(int width)
		{
			_autoSizeWidth = false;
			_width = width;
			return this;
		}

		public FormattedLabelBuilder SetHeight(int height)
		{
			_autoSizeHeight = false;
			_height = height;
			return this;
		}

		public FormattedLabelBuilder AutoSizeWidth()
		{
			_width = 0;
			_autoSizeWidth = true;
			return this;
		}

		public FormattedLabelBuilder AutoSizeHeight()
		{
			_height = 0;
			_autoSizeHeight = true;
			return this;
		}

		public FormattedLabelBuilder SetHorizontalAlignment(HorizontalAlignment horizontalAlignment)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_horizontalAlignment = horizontalAlignment;
			return this;
		}

		public FormattedLabelBuilder SetVerticalAlignment(VerticalAlignment verticalAlignment)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_verticalAlignment = verticalAlignment;
			return this;
		}

		public FormattedLabel Build()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabel formattedLabel = new FormattedLabel(_parts, _wrapText, _autoSizeWidth, _autoSizeHeight, _horizontalAlignment, _verticalAlignment);
			((Control)formattedLabel).set_Width(_width);
			((Control)formattedLabel).set_Height(_height);
			return formattedLabel;
		}
	}
}
