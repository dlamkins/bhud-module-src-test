using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	public class DebugControl : Control
	{
		private ConcurrentDictionary<string, Func<string>> StringsToDisplay { get; set; }

		public DebugControl()
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(new Point(0, 0));
			((Control)this).set_Size(new Point(1920, 1920));
			((Control)this).set_Visible(true);
			StringsToDisplay = new ConcurrentDictionary<string, Func<string>>();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void Add(string key, Func<string> value)
		{
			StringsToDisplay[key] = value;
		}

		public bool Remove(string key)
		{
			Func<string> value;
			return StringsToDisplay.TryRemove(key, out value);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (!DebugHelper.IsDebugEnabled())
			{
				return;
			}
			int i = 0;
			foreach (KeyValuePair<string, Func<string>> item in StringsToDisplay.OrderBy((KeyValuePair<string, Func<string>> s) => s.Key))
			{
				DrawDbg(spriteBatch, i, item.Key + ": " + item.Value());
				i += 30;
			}
		}

		private void DrawDbg(SpriteBatch spriteBatch, int position, string s)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, s, GameService.Content.get_DefaultFont32(), new Rectangle(new Point(0, position), new Point(400, 400)), Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
