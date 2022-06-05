using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager
{
	public class ProgressBar : Panel
	{
		public double _Progress = 0.33;

		public string _Text;

		public Color Done_Color;

		public Color Bar_Color;

		public Image _BackgroundTexture;

		public Image _FilledTexture;

		public Label _Label;

		public ProgressContainer _Bar;

		public Panel _Bar_Done;

		public double Progress
		{
			get
			{
				return _Progress;
			}
			set
			{
				_Progress = value;
				UpdateLayout();
			}
		}

		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
				UpdateLayout();
			}
		}

		public ProgressBar()
			: this()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Expected O, but got Unknown
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 10));
			val.set_Texture(AsyncTexture2D.op_Implicit(BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.GlidingFill_Gray)));
			_BackgroundTexture = val;
			ProgressContainer progressContainer = new ProgressContainer();
			((Control)progressContainer).set_Parent((Container)(object)this);
			((Control)progressContainer).set_Size(((Control)this).get_Size());
			progressContainer.FrameColor = Color.get_DarkOrange();
			_Bar = progressContainer;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(new Point((int)((double)((Control)this).get_Width() * Progress), ((Control)this).get_Height()));
			_Bar_Done = val2;
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)_Bar_Done);
			((Control)val3).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 2));
			val3.set_Texture(AsyncTexture2D.op_Implicit(BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.GlidingFill)));
			_FilledTexture = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Size(((Control)this).get_Size());
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			val4.set_Text("Display Text");
			val4.set_ShadowColor(Color.get_White());
			val4.set_TextColor(Color.get_Black());
			val4.set_ShowShadow(false);
			_Label = val4;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			((Control)_Bar).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 2));
			((Control)_Label).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 2));
			((Control)_BackgroundTexture).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 3));
			((Control)_FilledTexture).set_Size(new Point(((Control)this).get_Size().X, ((Control)this).get_Size().Y - 3));
			((Control)_Bar_Done).set_Size(new Point((int)((double)((Control)this).get_Width() * Progress), ((Control)this).get_Height()));
			_Label.set_Text(Text);
		}

		protected override void DisposeControl()
		{
			((Panel)this).DisposeControl();
			((Control)_Bar_Done).Dispose();
			((Control)_Bar).Dispose();
			((Control)_Label).Dispose();
			((Control)_FilledTexture).Dispose();
			((Control)_BackgroundTexture).Dispose();
		}
	}
}
