using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.Shared.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class TextBoxSuggestions : FlowPanel
	{
		public enum CaseMatchingMode
		{
			IgnoreCase
		}

		public enum SuggestionMode
		{
			StartsWith,
			Contains
		}

		private Container _attachToParent;

		private TextBox _textBox;

		private bool UpdateFromCode;

		public int MaxHeight { get; set; } = 400;


		public string[] Suggestions { get; set; }

		public SuggestionMode Mode { get; set; }

		public StringComparison StringComparison { get; set; } = StringComparison.InvariantCulture;


		public TextBoxSuggestions(TextBox textBox, Container attachToParent)
			: this()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			_textBox = textBox;
			_attachToParent = attachToParent;
			((Panel)this).set_CanScroll(true);
			((Control)this).set_BackgroundColor(Color.get_Black());
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((TextInputBase)_textBox).add_TextChanged((EventHandler<EventArgs>)TextBox_TextChanged);
			((Control)_textBox).add_Resized((EventHandler<ResizedEventArgs>)_textBox_Resized);
			((Control)_textBox).add_Moved((EventHandler<MovedEventArgs>)_textBox_Moved);
			((TextInputBase)_textBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)_textBox_InputFocusChanged);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
		}

		private void _textBox_InputFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				TextBox_TextChanged(_textBox, EventArgs.Empty);
			}
		}

		private void _textBox_Moved(object sender, MovedEventArgs e)
		{
			UpdateSizeAndLocation();
		}

		private void _textBox_Resized(object sender, ResizedEventArgs e)
		{
			UpdateSizeAndLocation();
		}

		private void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (!((Control)this).get_MouseOver())
			{
				RemoveSuggestionList();
			}
		}

		private void TextBox_TextChanged(object sender, EventArgs e)
		{
			if (Suggestions == null || Suggestions.Length == 0 || UpdateFromCode)
			{
				return;
			}
			TextBox textBox = (TextBox)((sender is TextBox) ? sender : null);
			string currentText = ((TextInputBase)textBox).get_Text().Trim();
			List<string> suggestions = null;
			if (!string.IsNullOrWhiteSpace(currentText))
			{
				suggestions = Suggestions.Where((string completionItem) => Mode switch
				{
					SuggestionMode.StartsWith => completionItem.StartsWith(currentText, StringComparison), 
					SuggestionMode.Contains => completionItem.Contains(currentText, StringComparison), 
					_ => false, 
				}).Take(50).ToList();
			}
			if (suggestions != null && suggestions.Count > 0)
			{
				BuildSuggestionList(suggestions);
			}
			else
			{
				RemoveSuggestionList();
			}
		}

		private void BuildSuggestionList(List<string> suggestions)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).ClearChildren();
			((Control)this).set_Parent(_attachToParent);
			foreach (string suggestion in suggestions)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(suggestion);
				((Control)val).set_Width(((Control)this).get_Width());
				((Control)val).add_Click((EventHandler<MouseEventArgs>)Label_Click);
			}
			int heightSum = ((IEnumerable<Control>)((Container)this).get_Children()).Sum((Control child) => child.get_Height());
			((Control)this).set_Height(MathHelper.Clamp(heightSum, 0, MaxHeight));
		}

		private void RemoveSuggestionList()
		{
			((IEnumerable<Control>)((Container)this).get_Children()).Select((Control child) => (Label)(object)((child is Label) ? child : null)).ToList().ForEach(delegate(Label label)
			{
				((Control)label).remove_Click((EventHandler<MouseEventArgs>)Label_Click);
			});
			((Container)this).ClearChildren();
			((Control)this).set_Parent((Container)null);
		}

		private void Label_Click(object sender, MouseEventArgs e)
		{
			Label label = (Label)((sender is Label) ? sender : null);
			try
			{
				UpdateFromCode = true;
				((TextInputBase)_textBox).set_Text(label.get_Text());
				RemoveSuggestionList();
			}
			finally
			{
				UpdateFromCode = false;
			}
		}

		protected override void DisposeControl()
		{
			RemoveSuggestionList();
			((TextInputBase)_textBox).remove_TextChanged((EventHandler<EventArgs>)TextBox_TextChanged);
			((Control)_textBox).remove_Resized((EventHandler<ResizedEventArgs>)_textBox_Resized);
			((Control)_textBox).remove_Moved((EventHandler<MovedEventArgs>)_textBox_Moved);
			((TextInputBase)_textBox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)_textBox_InputFocusChanged);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
			Suggestions = null;
			_attachToParent = null;
			_textBox = null;
			((FlowPanel)this).DisposeControl();
		}

		private void UpdateSizeAndLocation()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(((Control)_textBox).get_Left(), ((Control)_textBox).get_Bottom()));
			((Control)this).set_Width(((Control)_textBox).get_Width());
			((Container)this).get_Children().ToList().ForEach(delegate(Control child)
			{
				child.set_Width(((Control)this).get_Width());
			});
		}
	}
}
