using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class CustomTooltip : Control
	{
		private Texture2D Background;

		private Texture2D Icon;

		private int _Height;

		private object _CurrentObject;

		private string _Header;

		public Color HeaderColor = Color.get_Orange();

		public Color ContentColor = Color.get_Honeydew();

		private List<string> _Content;

		public object CurrentObject
		{
			get
			{
				return _CurrentObject;
			}
			set
			{
				_CurrentObject = value;
				_Height = -1;
			}
		}

		public string Header
		{
			get
			{
				return _Header;
			}
			set
			{
				_Header = value;
				_Height = -1;
			}
		}

		public List<string> Content
		{
			get
			{
				return _Content;
			}
			set
			{
				_Content = value;
				_Height = -1;
			}
		}

		public CustomTooltip(Container parent)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(new Point(225, 275));
			Background = BuildsManager.TextureManager._Backgrounds[2];
			((Control)this).set_ZIndex(1000);
			((Control)this).set_Visible(false);
			Control.get_Input().get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(20, -10)));
			});
		}

		private void UpdateLayout()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			if (Header == null || Content == null)
			{
				return;
			}
			ContentService val = new ContentService();
			BitmapFont font = val.GetFont((FontFace)0, (FontSize)14, (FontStyle)0);
			BitmapFont headerFont = val.GetFont((FontFace)0, (FontSize)18, (FontStyle)0);
			int width = (int)headerFont.GetStringRectangle(Header).Width + 30;
			int height = 0;
			List<string> newStrings = new List<string>();
			foreach (string ss in Content)
			{
				string ss = Regex.Replace(ss, "<c=@reminder>", "\n\n");
				ss = Regex.Replace(ss, "<c=@abilitytype>", "");
				ss = Regex.Replace(ss, "</c>", "");
				ss = Regex.Replace(ss, "<br>", "");
				ss = ss.Replace(Environment.NewLine, "\n");
				newStrings.Add(ss);
				RectangleF rect = font.GetStringRectangle(ss);
				width = Math.Max(width, Math.Min((int)rect.Width + 20, 300));
			}
			foreach (string s in newStrings)
			{
				Rectangle yRect = font.CalculateTextRectangle(s, new Rectangle(0, 0, width, 0));
				height += yRect.Height;
			}
			Content = newStrings;
			Rectangle hRect = headerFont.CalculateTextRectangle(Header, new Rectangle(0, 0, width, 0));
			((Control)this).set_Height(10 + hRect.Height + 15 + height + 5);
			((Control)this).set_Width(width + 20);
			_Height = ((Control)this).get_Height();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			if (Header != null && Content != null)
			{
				if (_Height == -1)
				{
					UpdateLayout();
				}
				ContentService val = new ContentService();
				BitmapFont font = val.GetFont((FontFace)0, (FontSize)14, (FontStyle)0);
				BitmapFont headerFont = val.GetFont((FontFace)0, (FontSize)18, (FontStyle)0);
				headerFont.CalculateTextRectangle(Header, new Rectangle(0, 0, ((Control)this).get_Width() - 20, 0));
				RectangleF rect = font.GetStringRectangle(Header);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)bounds, new Color(55, 55, 55, 255), 0f, default(Vector2), (SpriteEffects)0);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Background, bounds, (Rectangle?)bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				Color color = Color.get_Black();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Header, headerFont, new Rectangle(10, 10, 0, (int)rect.Height), HeaderColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, string.Join(Environment.NewLine, Content), font, new Rectangle(10, 10 + (int)rect.Height + 15, ((Control)this).get_Width() - 20, ((Control)this).get_Height() - (10 + (int)rect.Height + 15)), ContentColor, true, (HorizontalAlignment)0, (VerticalAlignment)0);
			}
		}
	}
}
