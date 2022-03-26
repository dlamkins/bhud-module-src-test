using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Mumble.Models;
using Manlaan.HPGrid.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.HPGrid.Controls
{
	internal class DrawGrid : Container
	{
		public List<GridPhase> Phases = null;

		public bool ShowDesc = false;

		private Texture2D _arrowImg;

		private List<ArrowNote> _arrowNotes = new List<ArrowNote>();

		private Point _resolution = new Point(0, 0);

		public DrawGrid()
			: this()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			_resolution = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			((Control)this).add_Disposed((EventHandler<EventArgs>)OnDisposed);
			((Control)this).set_Visible(true);
			_arrowImg = Module.ModuleInstance.ContentsManager.GetTexture("arrow.png");
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMousePressed);
		}

		private void OnDisposed(object sender, EventArgs e)
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMousePressed);
		}

		private void OnMousePressed(object o, MouseEventArgs e)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			foreach (ArrowNote _note in _arrowNotes)
			{
				if (_note.InRadius(new Vector2((float)Control.get_Input().get_Mouse().get_Position()
					.X, (float)Control.get_Input().get_Mouse().get_Position()
					.Y), 6f))
				{
					ScreenNotification.ShowNotification(_note.Note, (NotificationType)0, (Texture2D)null, 4);
				}
			}
		}

		public void SetSize()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected I4, but got Unknown
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			int w = 0;
			int h = 0;
			int x = 0;
			int y = 0;
			float UIScale = GameService.Graphics.get_UIScaleMultiplier();
			UiSize uISize = GameService.Gw2Mumble.get_UI().get_UISize();
			UiSize val = uISize;
			switch ((int)val)
			{
			case 3:
				w = (int)(311f / UIScale);
				h = (int)(18f / UIScale);
				y = (int)(106f / UIScale);
				x = (int)((float)(_resolution.X / 2 - 218) / UIScale);
				break;
			case 2:
				w = (int)(280f / UIScale);
				h = (int)(15f / UIScale);
				y = (int)(97f / UIScale);
				x = (int)((float)(_resolution.X / 2 - 198) / UIScale);
				break;
			case 1:
				w = (int)(253f / UIScale);
				h = (int)(15f / UIScale);
				y = (int)(87f / UIScale);
				x = (int)((float)(_resolution.X / 2 - 178) / UIScale);
				break;
			case 0:
				w = (int)(228f / UIScale);
				h = (int)(13f / UIScale);
				y = (int)(78f / UIScale);
				x = (int)((float)(_resolution.X / 2 - 160) / UIScale);
				break;
			}
			((Control)this).set_Size(new Point(w + 15, h + 15));
			((Control)this).set_Location(new Point(x, y));
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			if (Phases == null)
			{
				return;
			}
			_arrowNotes.Clear();
			foreach (GridPhase phase in Phases)
			{
				Color color = FindColor(phase.Color);
				float pct = (float)phase.Percent / 100f * ((float)((Control)this).get_Size().X - 15f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle((int)pct, 0, 1, ((Control)this).get_Size().Y - 15), (Rectangle?)null, color);
				if (ShowDesc)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _arrowImg, new Rectangle((int)pct - 5, ((Control)this).get_Size().Y - 15, 11, 11), (Rectangle?)null, Color.get_White());
					_arrowNotes.Add(new ArrowNote
					{
						Loc = new Vector2((float)(((Control)this).get_Location().X + (int)pct), (float)(((Control)this).get_Location().Y + ((Control)this).get_Size().Y - 8)),
						Note = phase.Percent + "%: " + phase.Description
					});
				}
			}
		}

		private Color FindColor(string colorname)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (colorname == null)
			{
				colorname = "Black";
			}
			Color systemColor = Color.FromName(colorname);
			return new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
		}
	}
}
