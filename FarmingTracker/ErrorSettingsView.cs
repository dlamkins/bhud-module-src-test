using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace FarmingTracker
{
	public class ErrorSettingsView : View
	{
		private FormattedLabel _formattedLabel;

		private readonly string _infoText;

		public ErrorSettingsView(string infoText)
			: this()
		{
			_infoText = infoText;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_formattedLabel = new FormattedLabelBuilder().SetWidth(((Control)buildPanel).get_Width()).AutoSizeHeight().Wrap()
				.CreatePart(_infoText, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)16);
				})
				.Build();
			((Control)_formattedLabel).set_Parent(buildPanel);
		}

		protected override void Unload()
		{
			FormattedLabel formattedLabel = _formattedLabel;
			if (formattedLabel != null)
			{
				((Control)formattedLabel).Dispose();
			}
		}
	}
}
