using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.UI
{
	internal class KofiButton : View
	{
		private Color _backgroundColor = new Color(41, 171, 224);

		private Texture2D _cupBorder;

		private Texture2D _background;

		private BitmapFont _font;

		private const int CUP_SIZE = 38;

		private const int BOUNCE_COUNT = 15;

		private const float BOUNCE_DURATION = 2f;

		private const float SCALE_DURATION = 0.6f;

		private const float BOUNCE_ROTATION = -(float)Math.PI / 16f;

		private int _wiggleDirection = 1;

		private bool _nonOpp;

		private bool _isAnimating;

		private string _text
		{
			get
			{
				if (RandomUtil.GetRandom(0, 1) <= 0)
				{
					return Resources.Buy_Me_a_Coffee_;
				}
				return Resources.Support_Me_on_Ko_fi;
			}
		}

		public KofiButton()
			: this()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			_cupBorder = ChatMacros.Instance.ContentsManager.GetTexture("socials/cup_border.png");
			_background = ChatMacros.Instance.ContentsManager.GetTexture("socials/kofi_background.png");
			_font = ChatMacros.Instance.ContentsManager.GetBitmapFont("fonts/Quicksand-SemiBold.ttf", 24);
		}

		protected override void Unload()
		{
			Texture2D cupBorder = _cupBorder;
			if (cupBorder != null)
			{
				((GraphicsResource)cupBorder).Dispose();
			}
			Texture2D background = _background;
			if (background != null)
			{
				((GraphicsResource)background).Dispose();
			}
			_font?.Dispose();
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			Image val = new Image(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			val.set_Texture(AsyncTexture2D.op_Implicit(_background));
			val.set_Tint(_backgroundColor);
			Image background = val;
			RotatableImage rotatableImage = new RotatableImage();
			((Control)rotatableImage).set_Parent(buildPanel);
			((Control)rotatableImage).set_Width(38);
			((Control)rotatableImage).set_Height(38);
			((Control)rotatableImage).set_Left(42);
			((Control)rotatableImage).set_Top((buildPanel.get_ContentRegion().Height - 38) / 2);
			((Image)rotatableImage).set_Texture(AsyncTexture2D.op_Implicit(_cupBorder));
			RotatableImage cup = rotatableImage;
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(buildPanel.get_ContentRegion().Width - 38 - 8);
			((Control)val2).set_Height(buildPanel.get_ContentRegion().Height);
			((Control)val2).set_Left(((Control)cup).get_Left());
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_Text(_text);
			val2.set_Font((BitmapFont)(object)_font);
			Tween bouncer = null;
			((Control)buildPanel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				background.set_Tint(new Color(21, 151, 204));
				if (!_isAnimating)
				{
					RestartWiggle();
				}
			});
			((Control)buildPanel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				Tween obj = bouncer;
				if (obj != null)
				{
					obj.Cancel();
				}
				Reset();
				background.set_Tint(new Color(41, 171, 224));
			});
			((Control)buildPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://ko-fi.com/nekres");
			});
			((Control)buildPanel).set_BasicTooltipText("ko-fi.com/nekres");
			((View<IPresenter>)this).Build(buildPanel);
			void DoWiggle()
			{
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				_isAnimating = true;
				_nonOpp = !_nonOpp;
				_wiggleDirection = 1;
				bouncer = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<RotatableImage>(cup, (object)new
				{
					Size = new Point(43, 43),
					Left = 37,
					Top = (buildPanel.get_ContentRegion().Height - ((Control)cup).get_Height()) / 2
				}, 0.6f, 0f, true).OnComplete((Action)delegate
				{
					bouncer = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<RotatableImage>(cup, (object)new
					{
						Rotation = -(float)Math.PI / 16f * (float)_wiggleDirection
					}, 142f / (339f * (float)Math.PI), 0f, true).Reflect().Repeat(15)
						.Ease((Func<float, float>)Ease.BounceInOut)
						.Rotation((RotationUnit)1)
						.OnRepeat((Action)delegate
						{
							_wiggleDirection *= ((!(_nonOpp = !_nonOpp)) ? 1 : (-1));
						})
						.OnComplete((Action)delegate
						{
							//IL_0015: Unknown result type (might be due to invalid IL or missing references)
							//IL_0022: Unknown result type (might be due to invalid IL or missing references)
							bouncer = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<RotatableImage>(cup, (object)new
							{
								Size = new Point(38, 38),
								Left = 42,
								Top = (buildPanel.get_ContentRegion().Height - 38) / 2
							}, 0.6f, 0f, true).OnComplete((Action)RestartWiggle);
						});
				});
			}
			void Reset()
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				_isAnimating = false;
				cup.Rotation = 0f;
				((Control)cup).set_Size(new Point(38, 38));
				((Control)cup).set_Left(42);
				((Control)cup).set_Top((buildPanel.get_ContentRegion().Height - 38) / 2);
				_nonOpp = !_nonOpp;
				_wiggleDirection = 1;
			}
			void RestartWiggle()
			{
				Reset();
				DoWiggle();
			}
		}
	}
}
