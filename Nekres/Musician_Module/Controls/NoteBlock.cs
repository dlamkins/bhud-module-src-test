using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician_Module.Controls.Instrument;

namespace Nekres.Musician_Module.Controls
{
	public class NoteBlock : Control
	{
		private static readonly Dictionary<InstrumentSkillType, Texture2D> NoteTextures = new Dictionary<InstrumentSkillType, Texture2D>
		{
			{
				InstrumentSkillType.Note,
				MusicianModule.ModuleInstance.ContentsManager.GetTexture("note_block.png")
			},
			{
				InstrumentSkillType.IncreaseOctave,
				MusicianModule.ModuleInstance.ContentsManager.GetTexture("incr_octave.png")
			},
			{
				InstrumentSkillType.DecreaseOctave,
				MusicianModule.ModuleInstance.ContentsManager.GetTexture("decr_octave.png")
			},
			{
				InstrumentSkillType.StopPlaying,
				MusicianModule.ModuleInstance.ContentsManager.GetTexture("pause_block.png")
			}
		};

		private readonly Texture2D NoteSprite;

		private readonly Color SpriteColor;

		private readonly GuildWarsControls NoteKey;

		private readonly int XOffset;

		private Tween NoteAnim;

		public NoteBlock(GuildWarsControls _key, InstrumentSkillType _noteType, Color _spriteColor)
			: this()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_ZIndex(1);
			((Control)this).set_Size(new Point(56, 20));
			NoteSprite = NoteTextures[_noteType];
			SpriteColor = _spriteColor;
			NoteKey = _key;
			XOffset = Conveyor.LaneCoordinatesX[_key];
			((Control)this).set_Location(new Point(XOffset, -((Control)this).get_Width()));
			NoteAnim = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<NoteBlock>(this, (object)new
			{
				Top = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height() - 100
			}, 10f, 0f, true).OnComplete((Action)delegate
			{
				NoteAnim = null;
				((Control)this).Dispose();
			});
			((Control)Control.get_Graphics().get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
		}

		private void UpdateLocation(object sender, EventArgs e)
		{
			((Control)this).set_Size(new Point(56, 30));
			if (((Control)this).get_Location().Y < ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height() - (100 + ((Control)this).get_Size().Y))
			{
				return;
			}
			foreach (NoteBlock child in ((Container)MusicianModule.ModuleInstance.Conveyor).get_Children())
			{
				child.NoteAnim.Pause();
			}
			MusicianModule.ModuleInstance.MusicPlayer.Worker.Join();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, NoteSprite, new Rectangle(0, 0, 56, 20), (Rectangle?)null, SpriteColor, 0f, Vector2.Zero, SpriteEffects.None);
		}
	}
}
