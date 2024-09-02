using System;
using System.Text.RegularExpressions;
using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class DrfTokenTextBox : TextBox
	{
		private static readonly Regex _charactersNotAllowedInDrfTokenRegex = new Regex("[^a-f0-9-]");

		public event EventHandler SanitizedTextChanged;

		public DrfTokenTextBox(string text, string tooltip, BitmapFont font, Container parent)
			: this()
		{
			((TextInputBase)this).set_Text(text);
			((Control)this).set_BasicTooltipText(tooltip);
			((TextInputBase)this).set_PlaceholderText("Enter token...");
			((TextInputBase)this).set_Font(font);
			((Control)this).set_Width(350);
			((Control)this).set_Height(40);
			((Control)this).set_Parent(parent);
			((TextInputBase)this).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				string text2 = ((TextInputBase)this).get_Text();
				string lowerCaseAndRemoveIllegalCharacters = GetLowerCaseAndRemoveIllegalCharacters(text2);
				SanitizeTextBoxText(text2, lowerCaseAndRemoveIllegalCharacters);
				this.SanitizedTextChanged?.Invoke(this, EventArgs.Empty);
			});
		}

		private void SanitizeTextBoxText(string token, string sanitizedToken)
		{
			if (!(token == sanitizedToken))
			{
				int oldCursorIndex = ((TextInputBase)this).get_CursorIndex();
				((TextInputBase)this).set_Text(sanitizedToken);
				((TextInputBase)this).set_CursorIndex(DetermineNewCursorIndex(token, sanitizedToken, oldCursorIndex));
			}
		}

		private static int DetermineNewCursorIndex(string token, string sanitizedToken, int oldCursorIndex)
		{
			if (token.ToLower() == sanitizedToken)
			{
				return oldCursorIndex;
			}
			int newCursorIndex = ((oldCursorIndex >= sanitizedToken.Length) ? (sanitizedToken.Length - 1) : (oldCursorIndex - 1));
			if (newCursorIndex < 0)
			{
				newCursorIndex = 0;
			}
			return newCursorIndex;
		}

		private static string GetLowerCaseAndRemoveIllegalCharacters(string drfToken)
		{
			return _charactersNotAllowedInDrfTokenRegex.Replace(drfToken.ToLower(), "");
		}
	}
}
