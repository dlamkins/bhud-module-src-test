using System;
using System.Text.RegularExpressions;
using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class NumberTextBox : TextBox
	{
		public event EventHandler? NumberTextChanged;

		public NumberTextBox(int maxNumberLength = 8)
			: this()
		{
			NumberTextBox numberTextBox = this;
			((TextInputBase)this).set_PlaceholderText("0");
			((TextInputBase)this).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				string text = ((TextInputBase)numberTextBox).get_Text();
				int cursorIndex = ((TextInputBase)numberTextBox).get_CursorIndex();
				string text2 = RemoveNonDigitsAndLeadingZerosAndTrimToMaxNumberLength(text, maxNumberLength);
				int cursorIndex2 = DetermineNewCursorIndex(text2, text, cursorIndex);
				((TextInputBase)numberTextBox).set_Text(text2);
				((TextInputBase)numberTextBox).set_CursorIndex(cursorIndex2);
				numberTextBox.NumberTextChanged?.Invoke(numberTextBox, EventArgs.Empty);
			});
		}

		private static int DetermineNewCursorIndex(string newText, string oldText, int oldCursorIndex)
		{
			if (newText == oldText)
			{
				return oldCursorIndex;
			}
			if (newText.Length == oldText.Length)
			{
				return oldCursorIndex;
			}
			int newCursorIndex = oldCursorIndex;
			if (newCursorIndex >= newText.Length)
			{
				newCursorIndex = newText.Length - 1;
			}
			if (newCursorIndex < 0)
			{
				newCursorIndex = 0;
			}
			return newCursorIndex;
		}

		private static string RemoveNonDigitsAndLeadingZerosAndTrimToMaxNumberLength(string text, int maxNumberLength = 0)
		{
			try
			{
				string number = Regex.Replace(text, "[^\\d]", "", RegexOptions.None, TimeSpan.FromMilliseconds(1000.0)).TrimStart('0');
				return (number.Length > maxNumberLength) ? number.Substring(0, maxNumberLength) : number;
			}
			catch (RegexMatchTimeoutException)
			{
				Module.Logger.Error("regex timedout for NumberTextBox.");
				return "0";
			}
		}
	}
}
