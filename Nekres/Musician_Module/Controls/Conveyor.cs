using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician_Module.Controls.Instrument;

namespace Nekres.Musician_Module.Controls
{
	public class Conveyor : Container
	{
		public static readonly Dictionary<GuildWarsControls, int> LaneCoordinatesX = new Dictionary<GuildWarsControls, int>
		{
			{
				(GuildWarsControls)2,
				13
			},
			{
				(GuildWarsControls)3,
				75
			},
			{
				(GuildWarsControls)4,
				136
			},
			{
				(GuildWarsControls)5,
				197
			},
			{
				(GuildWarsControls)6,
				260
			},
			{
				(GuildWarsControls)7,
				429
			},
			{
				(GuildWarsControls)8,
				491
			},
			{
				(GuildWarsControls)9,
				552
			},
			{
				(GuildWarsControls)10,
				614
			},
			{
				(GuildWarsControls)11,
				675
			}
		};

		private readonly Texture2D ConveyorTopSprite;

		private readonly Texture2D ConveyorBottomSprite;

		public Conveyor()
			: this()
		{
			ConveyorTopSprite = ConveyorTopSprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("conveyor_top.png");
			ConveyorBottomSprite = ConveyorBottomSprite ?? MusicianModule.ModuleInstance.ContentsManager.GetTexture("conveyor_bottom.png");
			((Control)this).set_Size(new Point(744, ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height()));
			((Control)this).set_ZIndex(0);
			UpdateLocation(null, null);
			((Control)Control.get_Graphics().get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
		}

		private void UpdateLocation(object sender, EventArgs e)
		{
			((Control)this).set_Location(new Point(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() / 2 - ((Control)this).get_Width() / 2, 0));
			((Control)this).set_Size(new Point(744, ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height()));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			int height = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, ConveyorTopSprite, new Rectangle(0, 0, 744, height - 90), (Rectangle?)null, Color.White, 0f, Vector2.Zero, SpriteEffects.None);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, ConveyorBottomSprite, new Rectangle(0, height - 93, 744, 75), (Rectangle?)null, Color.White, 0f, Vector2.Zero, SpriteEffects.None);
		}

		public void SpawnNoteBlock(GuildWarsControls key, InstrumentSkillType noteType, Color spriteColor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			((Control)new NoteBlock(key, noteType, spriteColor)).set_Parent((Container)(object)this);
		}
	}
}
