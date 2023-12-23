using System;
using System.Collections.Generic;
using System.Linq;
using BlishEmotesList;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using felix.BlishEmotes.Services;

namespace felix.BlishEmotes.UI.Controls
{
	public class IconSelection : Control
	{
		private class SelectionEntry : SelectionOption
		{
			public int X { get; set; }

			public int Y { get; set; }

			public bool Hovered { get; set; }

			public SelectionEntry(Texture2D texture, string textureRef)
				: base(texture, textureRef)
			{
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<IconSelection>();

		private List<SelectionOption> _options;

		private Control _attachedToControl;

		private int _columns = 10;

		private int _iconSize = 30;

		private int _gridGap = 5;

		private List<SelectionEntry> _selections;

		public List<SelectionOption> Options
		{
			get
			{
				return _options;
			}
			set
			{
				_options = value;
				UpdateSizeAndLocation();
			}
		}

		public Control AttachedToControl
		{
			get
			{
				return _attachedToControl;
			}
			set
			{
				_attachedToControl = value;
				UpdateSizeAndLocation();
			}
		}

		public int Columns
		{
			get
			{
				return _columns;
			}
			set
			{
				_columns = value;
				UpdateSizeAndLocation();
			}
		}

		public int IconSize
		{
			get
			{
				return _iconSize;
			}
			set
			{
				_iconSize = value;
				UpdateSizeAndLocation();
			}
		}

		public int GridGap
		{
			get
			{
				return _gridGap;
			}
			set
			{
				_gridGap = value;
				UpdateSizeAndLocation();
			}
		}

		public Texture2D Background { get; set; }

		public event EventHandler<SelectionOption> Selected;

		public IconSelection(Container parent, Control attachedToControl)
		{
			base.Parent = parent;
			AttachedToControl = attachedToControl;
			_selections = new List<SelectionEntry>();
			ZIndex = 997;
			base.Visible = false;
			Background = EmotesModule.ModuleInstance.TexturesManager.GetTexture(Textures.Background);
			UpdateSizeAndLocation();
			base.Shown += HandleShown;
			base.Hidden += HandleHidden;
		}

		private void UpdateSizeAndLocation()
		{
			int requiredRows = (Options?.Count ?? 0) / Columns + 1;
			Point contentSize = new Point(Columns * (IconSize + GridGap), requiredRows * (IconSize + GridGap));
			Point padding = new Point((int)base.Padding.Left + (int)base.Padding.Right, (int)base.Padding.Top + (int)base.Padding.Bottom);
			base.Size = contentSize + padding;
			Point attachedLoc = ((AttachedToControl == null) ? base.Parent.Location : LocalizedLocation(AttachedToControl, base.Parent));
			Point attachedSize = AttachedToControl?.Size ?? new Point(0, 0);
			int centerXOffset = base.Size.X / 2 - attachedSize.X / 2;
			base.Location = attachedLoc + new Point(0, attachedSize.Y + 2) - new Point(centerXOffset, 0);
		}

		private Point LocalizedLocation(Control control, Container localizedTo)
		{
			Point localized = control.Location;
			for (Container nextParent = control.Parent; nextParent != localizedTo; nextParent = nextParent.Parent)
			{
				if (nextParent == null)
				{
					Logger.Warn("control is not a descendant of localizedTo");
					return localized;
				}
				localized += nextParent.Location;
			}
			return localized;
		}

		private void HandleShown(object sender, EventArgs e)
		{
			Control.Input.Mouse.LeftMouseButtonPressed += OnLeftMouseButtonPressed;
		}

		private void HandleHidden(object sender, EventArgs e)
		{
			Control.Input.Mouse.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;
		}

		private void OnLeftMouseButtonPressed(object sender, EventArgs args)
		{
			SelectionEntry hovered = _selections.SingleOrDefault((SelectionEntry item) => item.Hovered);
			if (hovered != null)
			{
				this.Selected(this, hovered);
			}
			Hide();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.DrawOnCtrl(this, Background, bounds, new Rectangle(40, 30, bounds.Width, bounds.Height), Color.White);
			List<SelectionEntry> newEntries = CreateSelectionEntries();
			_selections.Clear();
			_selections.AddRange(newEntries);
			PaintSelectionEntries(spriteBatch);
		}

		private List<SelectionEntry> CreateSelectionEntries()
		{
			List<SelectionEntry> newEntries = new List<SelectionEntry>();
			for (int i = 0; i < Options.Count; i++)
			{
				SelectionOption emote = Options[i];
				int num = i % Columns;
				int row = i / Columns;
				int x = num * (IconSize + GridGap) + (int)base.Padding.Left;
				int y = row * (IconSize + GridGap) + (int)base.Padding.Top;
				newEntries.Add(new SelectionEntry(emote.Texture, emote.TextureRef)
				{
					X = x,
					Y = y,
					Hovered = false
				});
			}
			return newEntries;
		}

		private void PaintSelectionEntries(SpriteBatch spriteBatch)
		{
			foreach (SelectionEntry entry in _selections)
			{
				Rectangle entryBounds = new Rectangle(entry.X, entry.Y, IconSize, IconSize);
				entry.Hovered = entryBounds.Contains(base.RelativeMousePosition);
				if (entry.Hovered)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, entryBounds, null, Color.Gray);
				}
				spriteBatch.DrawOnCtrl(this, entry.Texture, entryBounds, null, Color.White);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			base.Shown -= HandleShown;
			base.Hidden -= HandleHidden;
		}
	}
}
