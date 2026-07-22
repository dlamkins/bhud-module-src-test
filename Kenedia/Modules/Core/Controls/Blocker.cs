using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class Blocker : Control
	{
		private List<(Rectangle r, float opacity, int thickness)> _borders = new List<(Rectangle, float, int)>();

		public Control CoveredControl
		{
			[CompilerGenerated]
			get
			{
				return _003CCoveredControl_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CCoveredControl_003Ek__BackingField, value, delegate(Control v)
				{
					_003CCoveredControl_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Control>(OnCoveredControlChanged));
			}
		}

		public int BorderWidth { get; set; } = 3;


		public string Text { get; set; } = string.Empty;


		public Color TextColor { get; set; } = Color.White;


		public BitmapFont TextFont { get; set; } = Control.Content.DefaultFont18;


		private void OnCoveredControlChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Control> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.Resized -= CoveredControl_Resized;
			}
			if (e.NewValue != null)
			{
				e.NewValue!.Resized += CoveredControl_Resized;
			}
		}

		private void CoveredControl_Resized(object sender, ResizedEventArgs e)
		{
			base.Size = e.CurrentSize;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_borders.Clear();
			ZIndex = (base.Parent?.ZIndex ?? 0) + 25;
			int strength = BorderWidth;
			int fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			for (int i = fadeLines - 1; i >= 0; i--)
			{
				_borders.Add((new Rectangle(i, i, base.Width - i * 2, base.Height - i * 2), GetFadeValue(i), 1));
			}
			if (fadeLines < strength)
			{
				_borders.Add((new Rectangle(fadeLines, fadeLines, base.Width - fadeLines * 2, base.Height - fadeLines * 2), GetFadeValue(int.MaxValue), strength - fadeLines));
			}
		}

		private float GetFadeValue(int i)
		{
			return i switch
			{
				2 => 0.75f, 
				1 => 0.5f, 
				0 => 0.25f, 
				_ => 1f, 
			};
		}

		private float GetFadeValue(int i, int fadeLines, int strength)
		{
			return (float)strength / (float)fadeLines * (float)i;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			foreach (var (r, opacity, thickness) in _borders)
			{
				spriteBatch.DrawFrame(this, r, Color.Black * opacity, thickness);
			}
			if (!string.IsNullOrEmpty(Text))
			{
				spriteBatch.DrawStringOnCtrl(this, Text, TextFont, bounds, TextColor, wrap: false, HorizontalAlignment.Center);
			}
		}
	}
}
