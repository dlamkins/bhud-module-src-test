using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace LoreBridge.Views
{
	internal class PolicyConfirmationView : View
	{
		private Container _buildPanel;

		public TaskCompletionSource<bool> ResultTask { get; } = new TaskCompletionSource<bool>();


		protected override void Build(Container buildPanel)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Expected O, but got Unknown
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Expected O, but got Unknown
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			_buildPanel = buildPanel;
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			val.set_Title("CONTENT USE POLICY AGREEMENT");
			val.set_ShowBorder(true);
			val.set_BackgroundTexture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("tooltip")));
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			((Control)val).set_ClipsBounds(false);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel container = val;
			FormattedLabel text = new FormattedLabelBuilder().CreatePart("The LoreBridge module provides translation functionality which may violate ", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				b.SetFontSize((FontSize)16);
			}).CreatePart("ArenaNet's Content Use Policy", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				b.SetFontSize((FontSize)16).MakeBold().SetTextColor(Color.get_Red());
			}).CreatePart(".", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				b.SetFontSize((FontSize)16);
			})
				.CreatePart("\n\nPlease read ", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)16);
				})
				.CreatePart("Content Use Policy", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					b.SetFontSize((FontSize)16).MakeBold().SetTextColor(Color.get_Cyan())
						.SetLink((Action)OpenPolicyLink);
				})
				.CreatePart(" before accepting.\n\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)16);
				})
				.SetWidth(((Control)buildPanel).get_Width() - 30)
				.AutoSizeHeight()
				.Wrap()
				.Build();
			((Control)text).set_Location(new Point(15, 15));
			((Control)text).set_Parent((Container)(object)container);
			Checkbox val2 = new Checkbox();
			((Control)val2).set_Parent((Container)(object)container);
			val2.set_Text("I have read and understand the risks");
			((Control)val2).set_Location(new Point(15, ((Control)text).get_Bottom() + 40));
			((Control)val2).set_Width(200);
			val2.set_Checked(false);
			Checkbox checkBox = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)container);
			val3.set_Text("Accept");
			((Control)val3).set_Width(100);
			((Control)val3).set_Location(new Point((((Control)container).get_Width() - 210) / 2, ((Control)checkBox).get_Bottom() + 20));
			((Control)val3).set_Enabled(false);
			StandardButton acceptButton = val3;
			((Control)acceptButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Close(result: true);
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)container);
			val4.set_Text("Decline");
			((Control)val4).set_Width(100);
			((Control)val4).set_Location(new Point(((Control)acceptButton).get_Right() + 10, ((Control)checkBox).get_Bottom() + 20));
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Close(result: false);
			});
			checkBox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				((Control)acceptButton).set_Enabled(checkBox.get_Checked());
			});
		}

		private static void OpenPolicyLink()
		{
			Process.Start("https://www.arena.net/en/legal/content-terms-of-use/");
		}

		private void Close(bool result)
		{
			ResultTask.TrySetResult(result);
			((Control)_buildPanel).Dispose();
		}

		public PolicyConfirmationView()
			: this()
		{
		}
	}
}
