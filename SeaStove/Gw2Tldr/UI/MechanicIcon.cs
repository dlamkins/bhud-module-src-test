using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class MechanicIcon : Control
	{
		private readonly string _type;

		private readonly Color _color;

		private static readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

		public bool CaptureMouse { get; set; } = true;


		protected override CaptureType CapturesInput()
		{
			if (CaptureMouse)
			{
				return (CaptureType)4;
			}
			return (CaptureType)0;
		}

		public MechanicIcon(string type, Color color)
			: this()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			_type = type;
			_color = color;
		}

		public static void LoadTextures()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext context = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				string[] array = new string[7] { "movement", "group", "combat", "danger", "timer", "shield", "info" };
				foreach (string name in array)
				{
					using Stream stream = typeof(MechanicIcon).Assembly.GetManifestResourceStream("SeaStove.Gw2Tldr.Assets.icons." + name + ".png");
					Textures[name] = Texture2D.FromStream(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), stream);
				}
			}
			finally
			{
				((GraphicsDeviceContext)(ref context)).Dispose();
			}
		}

		public static void DisposeTextures()
		{
			foreach (Texture2D value in Textures.Values)
			{
				((GraphicsResource)value).Dispose();
			}
			Textures.Clear();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (!Textures.TryGetValue(_type, out var texture))
			{
				Textures.TryGetValue("info", out texture);
			}
			if (texture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, bounds, (OverlayTheme.Light && _color == Color.get_LightGray()) ? Color.get_DarkSlateGray() : _color);
			}
		}
	}
}
