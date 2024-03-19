using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Storage;
using DanceDanceRotationModule.Util;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace DanceDanceRotationModule.Views
{
	public class NotesContainer : Container
	{
		internal class CurrentSequenceInfo
		{
			internal List<ActiveNote> ActiveNotes = new List<ActiveNote>();

			internal List<HitText> HitTexts = new List<HitText>();

			internal List<AbilityIcon> AbilityIcons = new List<AbilityIcon>();

			internal bool IsStarted { get; set; }

			internal bool IsPaused { get; set; }

			internal TimeSpan StartTime { get; set; }

			internal TimeSpan PausedTime { get; set; }

			internal int SequenceIndex { get; set; }

			internal int AbilityIconIndex { get; set; }

			public void Reset()
			{
				IsStarted = false;
				IsPaused = false;
				SequenceIndex = 0;
				StartTime = TimeSpan.Zero;
				AbilityIconIndex = 0;
				foreach (ActiveNote activeNote in ActiveNotes)
				{
					activeNote.Dispose();
				}
				ActiveNotes.Clear();
				foreach (HitText hitText in HitTexts)
				{
					hitText.Dispose();
				}
				HitTexts.Clear();
				foreach (AbilityIcon abilityIcon in AbilityIcons)
				{
					abilityIcon.Dispose();
				}
				AbilityIcons.Clear();
			}
		}

		internal class WindowInfo
		{
			internal NotesOrientation Orientation { get; private set; }

			internal int LaneCount { get; private set; }

			internal int NoteWidth { get; private set; }

			internal int NoteHeight { get; private set; }

			internal int LaneSpacing { get; private set; }

			internal int VerticalPadding { get; private set; }

			internal int HorizontalPadding { get; private set; }

			internal int CenterPadding { get; private set; }

			internal Point NewNotePosition { get; private set; }

			internal int NextAbilityIconsHeight { get; private set; }

			internal Point NextAbilityIconsLocation { get; private set; }

			internal Point TargetLocation { get; private set; }

			internal int HitPerfect { get; private set; }

			internal Range<double> HitRangePerfect { get; private set; }

			internal Range<double> HitRangeGreat { get; private set; }

			internal Range<double> HitRangeGood { get; private set; }

			internal Range<double> HitRangeBoo { get; private set; }

			internal int DestroyNotePosition { get; private set; }

			internal double NotePositionChangePerSecond { get; private set; }

			internal double TimeToReachEndMs { get; private set; }

			internal TimeSpan NoteCollisionCheck { get; private set; }

			public void Recalculate(int width, int height, SongData songData, NotesOrientation orientation)
			{
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				//IL_016a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0185: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_032a: Unknown result type (might be due to invalid IL or missing references)
				//IL_037e: Unknown result type (might be due to invalid IL or missing references)
				//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0429: Unknown result type (might be due to invalid IL or missing references)
				//IL_0449: Unknown result type (might be due to invalid IL or missing references)
				//IL_0488: Unknown result type (might be due to invalid IL or missing references)
				//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0539: Unknown result type (might be due to invalid IL or missing references)
				//IL_0586: Unknown result type (might be due to invalid IL or missing references)
				//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
				Orientation = orientation;
				NotesOrientation orientation2 = Orientation;
				if ((uint)orientation2 > 3u && orientation2 == NotesOrientation.AbilityBarStyle)
				{
					LaneCount = 10;
				}
				else if (DanceDanceRotationModule.Settings.CompactMode.get_Value())
				{
					LaneCount = 3;
				}
				else
				{
					LaneCount = 6;
				}
				if (IsVerticalOrientation())
				{
					VerticalPadding = (int)(HitText.MovePerSecond * (HitText.TotalLifeTimeMs / 1000.0));
					HorizontalPadding = 10;
					LaneSpacing = (height - HorizontalPadding * 2) / 100;
				}
				else
				{
					VerticalPadding = (int)(HitText.MovePerSecond * (HitText.TotalLifeTimeMs / 1000.0));
					HorizontalPadding = 0;
					LaneSpacing = (height - VerticalPadding * 2) / 100;
				}
				int nextAbilitiesCount = DanceDanceRotationModule.Settings.ShowNextAbilitiesCount.get_Value();
				if (nextAbilitiesCount > 0)
				{
					NoteHeight = (height - 2 * VerticalPadding - LaneSpacing * (LaneCount - 1)) / (LaneCount + 1);
					NextAbilityIconsHeight = NoteHeight;
					switch (Orientation)
					{
					case NotesOrientation.RightToLeft:
						NextAbilityIconsLocation = new Point((int)((float)width * 0.14f) - NoteWidth / 2, 0);
						break;
					case NotesOrientation.LeftToRight:
						NextAbilityIconsLocation = new Point((int)((float)width * 0.86f) - NoteWidth / 2 - (nextAbilitiesCount - 1) * NoteWidth, 0);
						break;
					case NotesOrientation.TopToBottom:
					case NotesOrientation.AbilityBarStyle:
						NextAbilityIconsLocation = new Point(HorizontalPadding, height - NoteHeight);
						break;
					case NotesOrientation.BottomToTop:
						NextAbilityIconsLocation = new Point(HorizontalPadding, 0);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					NoteHeight = (height - 2 * VerticalPadding - LaneSpacing * (LaneCount - 1)) / LaneCount;
					NextAbilityIconsLocation = new Point(0, 0);
					NextAbilityIconsHeight = 0;
				}
				if (Orientation == NotesOrientation.AbilityBarStyle)
				{
					NoteWidth = (width - 2 * HorizontalPadding - LaneSpacing * (LaneCount - 1)) / (LaneCount + 2);
					NoteHeight = NoteWidth;
					CenterPadding = NoteWidth * 2;
				}
				else if (IsVerticalOrientation())
				{
					NoteWidth = (width - 2 * HorizontalPadding - LaneSpacing * (LaneCount - 1)) / LaneCount;
					NoteHeight = NoteWidth;
					CenterPadding = 0;
				}
				else
				{
					NoteHeight = (height - 2 * VerticalPadding - LaneSpacing * (LaneCount - 1)) / (LaneCount + ((NextAbilityIconsHeight > 0) ? 1 : 0));
					NoteWidth = NoteHeight;
					CenterPadding = 0;
				}
				NotePositionChangePerSecond = Math.Max(songData.NotePositionChangePerSecond, 75);
				int perfectPosition;
				switch (Orientation)
				{
				case NotesOrientation.RightToLeft:
					NewNotePosition = new Point(width, 0);
					perfectPosition = (int)Math.Max(0.14f * (float)width, (double)NoteWidth * 1.5);
					TimeToReachEndMs = (double)Math.Abs(NewNotePosition.X + NoteWidth / 2 - perfectPosition) / NotePositionChangePerSecond * 1000.0;
					TargetLocation = new Point((int)((double)perfectPosition - (double)NoteWidth / 2.0), VerticalPadding + NextAbilityIconsHeight);
					DestroyNotePosition = -NoteWidth;
					break;
				case NotesOrientation.LeftToRight:
					NewNotePosition = new Point(-NoteWidth, 0);
					perfectPosition = (int)Math.Min(0.86f * (float)width, (double)width - (double)NoteWidth * 1.5);
					TimeToReachEndMs = (double)Math.Abs(NewNotePosition.X + NoteWidth / 2 - perfectPosition) / NotePositionChangePerSecond * 1000.0;
					TargetLocation = new Point((int)((double)perfectPosition - (double)NoteWidth / 2.0), VerticalPadding + NextAbilityIconsHeight);
					DestroyNotePosition = width;
					break;
				case NotesOrientation.TopToBottom:
				case NotesOrientation.AbilityBarStyle:
					NewNotePosition = new Point(0, -NoteHeight);
					perfectPosition = (int)Math.Min(0.86f * (float)(height - NextAbilityIconsHeight), (double)height - (double)NoteHeight * 1.5 - (double)NextAbilityIconsHeight);
					TimeToReachEndMs = (double)Math.Abs(NewNotePosition.Y + NoteHeight / 2 - perfectPosition) / NotePositionChangePerSecond * 1000.0;
					TargetLocation = new Point(HorizontalPadding, (int)((double)perfectPosition - (double)NoteHeight / 2.0));
					DestroyNotePosition = height - NextAbilityIconsHeight;
					break;
				case NotesOrientation.BottomToTop:
					NewNotePosition = new Point(0, height);
					perfectPosition = (int)Math.Max((float)NextAbilityIconsHeight + 0.14f * (float)(height - NextAbilityIconsHeight), (double)NextAbilityIconsHeight + (double)NoteHeight * 1.5);
					TimeToReachEndMs = Math.Abs((double)(NewNotePosition.Y + NoteHeight / 2 - perfectPosition) / NotePositionChangePerSecond * 1000.0);
					TargetLocation = new Point(HorizontalPadding, (int)((double)perfectPosition - (double)NoteHeight / 2.0));
					DestroyNotePosition = NextAbilityIconsHeight;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				HitPerfect = perfectPosition;
				HitRangePerfect = ConstructHitRange(perfectPosition, NotePositionChangePerSecond, 33);
				HitRangeGreat = ConstructHitRange(perfectPosition, NotePositionChangePerSecond, 92);
				HitRangeGood = ConstructHitRange(perfectPosition, NotePositionChangePerSecond, 142);
				HitRangeBoo = ConstructHitRange(perfectPosition, NotePositionChangePerSecond, 225);
				switch (Orientation)
				{
				case NotesOrientation.RightToLeft:
				case NotesOrientation.LeftToRight:
					NoteCollisionCheck = TimeSpan.FromMilliseconds((double)NoteWidth / NotePositionChangePerSecond * 1000.0);
					break;
				case NotesOrientation.TopToBottom:
				case NotesOrientation.BottomToTop:
				case NotesOrientation.AbilityBarStyle:
					NoteCollisionCheck = TimeSpan.FromMilliseconds((double)NoteHeight / NotePositionChangePerSecond * 1000.0);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			private Range<double> ConstructHitRange(int perfectPosition, double notePositionChangePerSecond, int windowMs)
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				return new Range<double>((double)perfectPosition - notePositionChangePerSecond * ((double)windowMs / 1000.0), (double)perfectPosition + notePositionChangePerSecond * ((double)windowMs / 1000.0));
			}

			public Point GetNewNoteSize()
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				return new Point(NoteWidth, NoteHeight);
			}

			public Point GetNewNoteSizeSmall()
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				return new Point(NoteWidth * 3 / 4, NoteHeight * 3 / 4);
			}

			public Point GetNewNoteLocation(int lane)
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0111: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				return (Point)(Orientation switch
				{
					NotesOrientation.RightToLeft => new Point(NewNotePosition.X, VerticalPadding + NextAbilityIconsHeight + lane * (NoteHeight + LaneSpacing)), 
					NotesOrientation.LeftToRight => new Point(NewNotePosition.X, VerticalPadding + NextAbilityIconsHeight + lane * (NoteHeight + LaneSpacing)), 
					NotesOrientation.TopToBottom => new Point(HorizontalPadding + lane * (NoteWidth + LaneSpacing), NewNotePosition.Y), 
					NotesOrientation.AbilityBarStyle => new Point((lane >= LaneCount / 2) ? (HorizontalPadding + CenterPadding + lane * (NoteWidth + LaneSpacing)) : (HorizontalPadding + lane * (NoteWidth + LaneSpacing)), NewNotePosition.Y), 
					NotesOrientation.BottomToTop => new Point(HorizontalPadding + lane * (NoteWidth + LaneSpacing), NewNotePosition.Y), 
					_ => throw new ArgumentOutOfRangeException(), 
				});
			}

			public Vector2 GetNoteChangeLocation(GameTime gameTime)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				float moveAmount = (float)(NotePositionChangePerSecond * (double)gameTime.get_ElapsedGameTime().Milliseconds / 1000.0);
				return GetNoteChangeLocation(moveAmount);
			}

			public Vector2 GetNoteChangeLocation(float moveAmount)
			{
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				switch (Orientation)
				{
				case NotesOrientation.RightToLeft:
					return new Vector2(-1f * moveAmount, 0f);
				case NotesOrientation.LeftToRight:
					return new Vector2(moveAmount, 0f);
				case NotesOrientation.TopToBottom:
				case NotesOrientation.AbilityBarStyle:
					return new Vector2(0f, moveAmount);
				case NotesOrientation.BottomToTop:
					return new Vector2(0f, -1f * moveAmount);
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			public bool IsVerticalOrientation()
			{
				return OrientationExtensions.IsVertical(Orientation);
			}
		}

		internal enum HitType
		{
			Perfect,
			Great,
			Good,
			Boo,
			Miss
		}

		internal class ActiveNote
		{
			private const float FadeInTime = 0.7f;

			private const float HitAnimationTime = 0.2f;

			private const int HitAnimationScaleDivisor = 4;

			private WindowInfo _windowInfo;

			private bool _isHit;

			private bool _allowMovement = true;

			internal Note Note { get; set; }

			internal Image Image { get; set; }

			internal Label Label { get; set; }

			internal double XPosition { get; set; }

			internal double YPosition { get; set; }

			internal bool ShouldRemove { get; private set; }

			public event EventHandler<HitType> OnHit;

			public ActiveNote(WindowInfo windowInfo, Note note, int lane, Container parent)
			{
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Expected O, but got Unknown
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0110: Unknown result type (might be due to invalid IL or missing references)
				//IL_0126: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_013d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_015b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0168: Unknown result type (might be due to invalid IL or missing references)
				//IL_0176: Unknown result type (might be due to invalid IL or missing references)
				//IL_0250: Unknown result type (might be due to invalid IL or missing references)
				//IL_0255: Unknown result type (might be due to invalid IL or missing references)
				//IL_025c: Unknown result type (might be due to invalid IL or missing references)
				//IL_025d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0267: Unknown result type (might be due to invalid IL or missing references)
				//IL_026e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0275: Unknown result type (might be due to invalid IL or missing references)
				//IL_027c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0283: Unknown result type (might be due to invalid IL or missing references)
				//IL_028a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0297: Expected O, but got Unknown
				//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_031e: Unknown result type (might be due to invalid IL or missing references)
				_windowInfo = windowInfo;
				Note = note;
				SettingEntry<KeyBinding> keyBinding = DanceDanceRotationModule.Settings.GetKeyBindingForNoteType(note.NoteType);
				string hotkeyText = ((keyBinding != null) ? KeysExtensions.NotesString(keyBinding.get_Value()) : "?");
				AsyncTexture2D noteBackground = (DanceDanceRotationModule.Settings.ShowAbilityIconsForNotes.get_Value() ? Resources.Instance.GetAbilityIcon(note.AbilityId) : AsyncTexture2D.op_Implicit(NoteTypeExtensions.NoteImage(note.NoteType)));
				Image val = new Image(noteBackground);
				((Control)val).set_Size(windowInfo.GetNewNoteSize());
				((Control)val).set_Location(_windowInfo.GetNewNoteLocation(lane));
				((Control)val).set_ZIndex(2);
				((Control)val).set_Opacity(0.7f);
				((Control)val).set_Parent(parent);
				Image = val;
				bool num = DanceDanceRotationModule.Settings.AutoHitWeapon1.get_Value() && note.NoteType == NoteType.Weapon1 && !note.OverrideAuto;
				if (num)
				{
					Point oldSize = ((Control)Image).get_Size();
					((Control)Image).set_ZIndex(1);
					((Control)Image).set_Size(_windowInfo.GetNewNoteSizeSmall());
					((Control)Image).set_Location(new Point(((Control)Image).get_Location().X + (oldSize.X - ((Control)Image).get_Size().X) / 2, ((Control)Image).get_Location().Y + (oldSize.Y - ((Control)Image).get_Size().Y) / 2));
				}
				BitmapFont font = ((hotkeyText.Length < 3) ? ((((Control)Image).get_Height() > 34) ? GameService.Content.get_DefaultFont32() : ((((Control)Image).get_Height() > 20) ? GameService.Content.get_DefaultFont18() : ((((Control)Image).get_Height() <= 16) ? GameService.Content.get_DefaultFont12() : GameService.Content.get_DefaultFont14()))) : ((((Control)Image).get_Height() > 48) ? GameService.Content.get_DefaultFont32() : ((((Control)Image).get_Height() > 32) ? GameService.Content.get_DefaultFont18() : ((((Control)Image).get_Height() <= 24) ? GameService.Content.get_DefaultFont12() : GameService.Content.get_DefaultFont14()))));
				Label val2 = new Label();
				val2.set_Text(hotkeyText);
				val2.set_TextColor(Color.get_White());
				val2.set_Font(font);
				val2.set_StrokeText(true);
				val2.set_ShowShadow(true);
				val2.set_AutoSizeHeight(true);
				val2.set_AutoSizeWidth(true);
				((Control)val2).set_Parent(parent);
				Label = val2;
				((Control)Label).set_Location(new Point((int)XPosition + (((Control)Image).get_Width() - ((Control)Label).get_Width()) / 2, ((Control)Image).get_Location().Y + (((Control)Image).get_Height() - ((Control)Label).get_Height()) / 2));
				XPosition = ((Control)Image).get_Location().X + ((Control)Image).get_Width() / 2;
				YPosition = ((Control)Image).get_Location().Y + ((Control)Image).get_Height() / 2;
				_isHit = false;
				ShouldRemove = false;
				((Control)Image).set_Opacity(0f);
				((Control)Label).set_Opacity(0f);
				((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Image>(Image, (object)new
				{
					Opacity = 1f
				}, 0.7f, 0f, true);
				if (!num && DanceDanceRotationModule.Settings.ShowHotkeys.get_Value())
				{
					((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Label>(Label, (object)new
					{
						Opacity = 1f
					}, 0.7f, 0f, true);
				}
			}

			public void Update(Vector2 positionChange)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0120: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				XPosition += positionChange.X;
				YPosition += positionChange.Y;
				if (!_isHit && DanceDanceRotationModule.Settings.AutoHitWeapon1.get_Value() && Note.NoteType == NoteType.Weapon1 && !Note.OverrideAuto && IsPastPerfect())
				{
					_isHit = true;
					PlayHitAnimation();
					return;
				}
				if (IsPastDestroy())
				{
					ShouldRemove = true;
					return;
				}
				if (!_isHit && IsPastMiss())
				{
					SetHit(HitType.Miss);
					PlayMissAnimation();
				}
				if (_allowMovement)
				{
					((Control)Image).set_Location(new Point((int)XPosition - ((Control)Image).get_Width() / 2, (int)YPosition - ((Control)Image).get_Height() / 2));
					((Control)Label).set_Location(new Point(((Control)Image).get_Location().X + (((Control)Image).get_Width() - ((Control)Label).get_Width()) / 2, ((Control)Image).get_Location().Y + (((Control)Image).get_Height() - ((Control)Label).get_Height()) / 2));
				}
			}

			public bool OnHotkeyPressed()
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				if (_isHit)
				{
					return false;
				}
				if (DanceDanceRotationModule.Settings.AutoHitWeapon1.get_Value() && Note.NoteType == NoteType.Weapon1)
				{
					return false;
				}
				if (XPosition > _windowInfo.HitRangeBoo.get_Max())
				{
					return false;
				}
				double axisToCheck = (_windowInfo.IsVerticalOrientation() ? YPosition : XPosition);
				HitType hitType;
				if (_windowInfo.HitRangePerfect.IsInBetween(axisToCheck, false, false))
				{
					hitType = HitType.Perfect;
					ScreenNotification.ShowNotification("Perfect", (NotificationType)0, (Texture2D)null, 4);
				}
				else if (_windowInfo.HitRangeGreat.IsInBetween(axisToCheck, false, false))
				{
					hitType = HitType.Great;
					ScreenNotification.ShowNotification("Great", (NotificationType)0, (Texture2D)null, 4);
				}
				else if (_windowInfo.HitRangeGood.IsInBetween(axisToCheck, false, false))
				{
					hitType = HitType.Good;
					ScreenNotification.ShowNotification("Good", (NotificationType)0, (Texture2D)null, 4);
				}
				else if (_windowInfo.HitRangeBoo.IsInBetween(axisToCheck, false, false))
				{
					hitType = HitType.Boo;
					ScreenNotification.ShowNotification("Boo", (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					hitType = HitType.Miss;
				}
				if (hitType != HitType.Miss)
				{
					SetHit(hitType);
					PlayHitAnimation();
				}
				return true;
			}

			public void Dispose()
			{
				((Control)Image).Dispose();
				((Control)Label).Dispose();
			}

			private void SetHit(HitType hitType)
			{
				if (!_isHit)
				{
					_isHit = true;
					this.OnHit?.Invoke(this, hitType);
				}
			}

			private void PlayHitAnimation()
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				_allowMovement = false;
				Point startSize = ((Control)Image).get_Size();
				Point startPosition = ((Control)Image).get_Location();
				((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Image>(Image, (object)new
				{
					Size = new Point(startSize.X / 4, startSize.Y / 4),
					Location = new Point(startPosition.X + ((Control)Image).get_Width() / 4, startPosition.Y + ((Control)Image).get_Height() / 4),
					Opacity = 0f
				}, 0.2f, 0f, true).OnComplete((Action)delegate
				{
					ShouldRemove = true;
				});
				((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Label>(Label, (object)new
				{
					Opacity = 0f
				}, 0.1f, 0f, true);
			}

			private void PlayMissAnimation()
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)Image).set_BackgroundColor(Color.get_Red());
				((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Image>(Image, (object)new
				{
					Opacity = 0f
				}, 0.4f, 0f, true);
				((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Label>(Label, (object)new
				{
					Opacity = 0f
				}, 0.1f, 0f, true);
			}

			internal void ShowTooltip()
			{
				string abilityName = Resources.Instance.GetAbilityName(Note.AbilityId);
				if (abilityName.Length != 0)
				{
					abilityName += ", ";
				}
				string tooltip = $"{abilityName}Time: {Note.TimeInRotation.TotalMilliseconds / 1000.0:0.000}s, Dur: {Note.Duration.TotalMilliseconds:n0}ms";
				((Control)Image).set_BasicTooltipText(tooltip);
				((Control)Label).set_BasicTooltipText(tooltip);
			}

			internal void HideTooltip()
			{
				((Control)Image).set_BasicTooltipText("");
				((Control)Label).set_BasicTooltipText("");
			}

			private bool IsPastPerfect()
			{
				switch (_windowInfo.Orientation)
				{
				case NotesOrientation.RightToLeft:
					return XPosition <= (double)_windowInfo.HitPerfect;
				case NotesOrientation.LeftToRight:
					return XPosition >= (double)_windowInfo.HitPerfect;
				case NotesOrientation.TopToBottom:
				case NotesOrientation.AbilityBarStyle:
					return YPosition >= (double)_windowInfo.HitPerfect;
				case NotesOrientation.BottomToTop:
					return YPosition <= (double)_windowInfo.HitPerfect;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			private bool IsPastMiss()
			{
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				switch (_windowInfo.Orientation)
				{
				case NotesOrientation.RightToLeft:
					return XPosition <= _windowInfo.HitRangeBoo.get_Min();
				case NotesOrientation.LeftToRight:
					return XPosition >= _windowInfo.HitRangeBoo.get_Max();
				case NotesOrientation.TopToBottom:
				case NotesOrientation.AbilityBarStyle:
					return YPosition >= _windowInfo.HitRangeBoo.get_Max();
				case NotesOrientation.BottomToTop:
					return YPosition <= _windowInfo.HitRangeBoo.get_Min();
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			private bool IsPastDestroy()
			{
				switch (_windowInfo.Orientation)
				{
				case NotesOrientation.RightToLeft:
					return XPosition <= (double)_windowInfo.DestroyNotePosition;
				case NotesOrientation.LeftToRight:
					return XPosition >= (double)_windowInfo.DestroyNotePosition;
				case NotesOrientation.TopToBottom:
				case NotesOrientation.AbilityBarStyle:
					return YPosition >= (double)_windowInfo.DestroyNotePosition;
				case NotesOrientation.BottomToTop:
					return YPosition <= (double)_windowInfo.DestroyNotePosition;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		internal class HitText
		{
			internal static double MovePerSecond = 10.0;

			internal static double TotalLifeTimeMs = 1500.0;

			private Label _label;

			private double _yPos;

			private double _remainLifeMs;

			public HitText(Container parent, HitType hitType, Point location)
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Expected O, but got Unknown
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				string text;
				Color textColor;
				switch (hitType)
				{
				case HitType.Perfect:
					text = "Perfect";
					textColor = Color.get_Aqua();
					break;
				case HitType.Great:
					text = "Great";
					textColor = Color.get_CornflowerBlue();
					break;
				case HitType.Good:
					textColor = Color.get_SpringGreen();
					text = "Good";
					break;
				case HitType.Boo:
					textColor = Color.get_OrangeRed();
					text = "Boo";
					break;
				case HitType.Miss:
					textColor = Color.get_Red();
					text = "Miss";
					break;
				default:
					throw new ArgumentOutOfRangeException("hitType", hitType, null);
				}
				Label val = new Label();
				val.set_Text(text);
				val.set_TextColor(textColor);
				val.set_Font(GameService.Content.get_DefaultFont18());
				val.set_ShowShadow(true);
				val.set_AutoSizeHeight(true);
				val.set_AutoSizeWidth(true);
				((Control)val).set_ClipsBounds(false);
				((Control)val).set_Location(location);
				((Control)val).set_Parent(parent);
				_label = val;
				((Control)_label).set_Location(new Point(location.X - ((Control)_label).get_Width() / 2, location.Y - ((Control)_label).get_Height() / 2));
				_remainLifeMs = TotalLifeTimeMs;
				_yPos = ((Control)_label).get_Location().Y;
			}

			public void Update(GameTime gameTime)
			{
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				_remainLifeMs -= gameTime.get_ElapsedGameTime().Milliseconds;
				_yPos -= (double)gameTime.get_ElapsedGameTime().Milliseconds / 1000.0 * MovePerSecond;
				((Control)_label).set_Opacity(Math.Max(0f, (float)(_remainLifeMs / TotalLifeTimeMs)));
				((Control)_label).set_Location(new Point(((Control)_label).get_Location().X, (int)_yPos));
			}

			public bool ShouldDispose()
			{
				return _remainLifeMs < 0.0;
			}

			public void Dispose()
			{
				((Control)_label).Dispose();
			}
		}

		internal class AbilityIcon
		{
			private WindowInfo _windowInfo;

			private double TimeToDisposeAt;

			internal Note Note { get; }

			internal Image Image { get; }

			internal bool ShouldDispose { get; private set; }

			public AbilityIcon(WindowInfo windowInfo, Note note, SongData songData, Container parent)
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Expected O, but got Unknown
				_windowInfo = windowInfo;
				Note = note;
				Image val = new Image(Resources.Instance.GetAbilityIcon(note.AbilityId));
				((Control)val).set_Size(windowInfo.GetNewNoteSize());
				((Control)val).set_Location(windowInfo.GetNewNoteLocation(6));
				((Control)val).set_Opacity(0.7f);
				((Control)val).set_Parent(parent);
				Image = val;
				ShouldDispose = false;
				TimeToDisposeAt = Note.TimeInRotation.TotalMilliseconds + _windowInfo.TimeToReachEndMs * (double)songData.PlaybackRate;
			}

			public void Update(GameTime gameTime, TimeSpan timeInRotation)
			{
				if (timeInRotation.TotalMilliseconds > TimeToDisposeAt)
				{
					ShouldDispose = true;
				}
			}

			public void Dispose()
			{
				((Control)Image).Dispose();
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<NotesContainer>();

		private const float PerfectPosition = 0.14f;

		private const int LaneLineThickness = 2;

		private List<Note> _currentSequence = new List<Note>();

		private SongData _songData;

		private TimeSpan _lastGameTime;

		private CurrentSequenceInfo _info = new CurrentSequenceInfo();

		private WindowInfo _windowInfo = new WindowInfo();

		private Label _timeLabel;

		private object _lock = new object();

		private const float TargetOpacity = 0.5f;

		private bool _targetCreated;

		private Image _targetTop;

		private Image _targetBottom;

		private List<Image> _targetCircles = new List<Image>();

		private List<Image> _targetSpacers = new List<Image>();

		private List<Control> _laneLines;

		public event EventHandler<bool> OnStartStop;

		public NotesContainer()
			: this()
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			_windowInfo.Recalculate(((Control)this).get_Width(), ((Control)this).get_Height(), _songData, DanceDanceRotationModule.Settings.Orientation.get_Value());
			CreateTarget();
			CreateBackgroundLines();
			UpdateTarget();
			UpdateBackgroundLines();
			Label val = new Label();
			val.set_Text("");
			((Control)val).set_Visible(false);
			((Control)val).set_Width(100);
			val.set_AutoSizeHeight(true);
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(Color.get_LightGray());
			((Control)val).set_Location(new Point(10, 10));
			((Control)val).set_Parent((Container)(object)this);
			_timeLabel = val;
			DanceDanceRotationModule.SongRepo.OnSelectedSongChanged += delegate(object sender, SelectedSongInfo songInfo)
			{
				if (songInfo.Song != null)
				{
					SetNoteSequence(songInfo.Song.Notes, songInfo.Data);
				}
			};
			DanceDanceRotationModule.Settings.ShowNextAbilitiesCount.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
			{
				Logger.Trace("ShowNextAbilitiesCount Updated. Resetting and recalculating layout and creating new target and lines");
				Reset();
				((Control)this).RecalculateLayout();
				AddInitialNotes();
			});
			DanceDanceRotationModule.Settings.Orientation.add_SettingChanged((EventHandler<ValueChangedEventArgs<NotesOrientation>>)delegate
			{
				Logger.Trace("Orientation Updated. Resetting and recalculating layout and creating new target and lines");
				Reset();
				((Control)this).RecalculateLayout();
				AddInitialNotes();
			});
			DanceDanceRotationModule.Settings.StartSongsWithFirstSkill.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				Logger.Trace("StartSongsWithFirstSkill Updated. Resetting and recalculating layout and creating new target and lines");
				Reset();
				((Control)this).RecalculateLayout();
				AddInitialNotes();
			});
			DanceDanceRotationModule.Settings.CompactMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				switch (DanceDanceRotationModule.Settings.Orientation.get_Value())
				{
				case NotesOrientation.RightToLeft:
				case NotesOrientation.LeftToRight:
				case NotesOrientation.TopToBottom:
				case NotesOrientation.BottomToTop:
					Logger.Trace("CompactMode Updated. Resetting and recalculating layout and creating new target and lines");
					Reset();
					lock (_lock)
					{
						_windowInfo.Recalculate(((Control)this).get_Width(), ((Control)this).get_Height(), _songData, DanceDanceRotationModule.Settings.Orientation.get_Value());
						CreateTarget();
						CreateBackgroundLines();
					}
					((Control)this).RecalculateLayout();
					AddInitialNotes();
					break;
				case NotesOrientation.AbilityBarStyle:
					Logger.Trace($"CompactMode Updated, but the orientation is {DanceDanceRotationModule.Settings.Orientation.get_Value()}, which is not supported");
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			});
		}

		public override void RecalculateLayout()
		{
			((Control)this).RecalculateLayout();
			if (_targetCreated)
			{
				lock (_lock)
				{
					_windowInfo.Recalculate(((Control)this).get_Width(), ((Control)this).get_Height(), _songData, DanceDanceRotationModule.Settings.Orientation.get_Value());
					UpdateTarget();
					UpdateBackgroundLines();
					RecalculateLayoutAbilityIcons();
				}
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			Reset();
			AddInitialNotes();
		}

		public void SetNoteSequence(List<Note> notes, SongData songData)
		{
			lock (_lock)
			{
				Logger.Trace("Setting Notes Sequence");
				Reset();
				_currentSequence.Clear();
				_currentSequence = new List<Note>(notes);
				_songData = songData;
				_windowInfo.Recalculate(((Control)this).get_Width(), ((Control)this).get_Height(), _songData, DanceDanceRotationModule.Settings.Orientation.get_Value());
				AddInitialNotes();
			}
		}

		public void ToggleStart()
		{
			if (_info.IsStarted)
			{
				ScreenNotification.ShowNotification("Stopped", (NotificationType)0, (Texture2D)null, 4);
				Reset();
				AddInitialNotes();
			}
			else
			{
				ScreenNotification.ShowNotification("Starting", (NotificationType)0, (Texture2D)null, 4);
				Reset();
				AddInitialNotes();
				Play();
			}
		}

		public void Play()
		{
			if (_currentSequence.Count == 0)
			{
				Logger.Warn("Play pressed, but no song loaded.");
			}
			((Control)_timeLabel).set_Visible(false);
			if (_info.IsPaused)
			{
				Logger.Trace("Resuming Notes");
				_info.IsPaused = false;
				_info.StartTime += _lastGameTime - _info.PausedTime;
				HideAllTooltips();
			}
			if (!_info.IsStarted)
			{
				_info.IsStarted = true;
				this.OnStartStop?.Invoke(this, _info.IsStarted);
			}
		}

		public void Pause()
		{
			if (_info.IsStarted && !_info.IsPaused)
			{
				Logger.Trace("Pausing Notes");
				_info.IsPaused = true;
				_info.PausedTime = _lastGameTime;
				TimeSpan totalInGameTime = _info.PausedTime - _info.StartTime;
				_timeLabel.set_Text($"{totalInGameTime.Minutes}\u2009:\u2009{totalInGameTime.Seconds:00}");
				((Control)_timeLabel).set_Visible(true);
				ShowAllTooltips();
			}
			else
			{
				Logger.Trace("Pause called, but notes are not started");
			}
		}

		public void Stop()
		{
			Reset();
			AddInitialNotes();
			ShowAllTooltips();
		}

		private void Reset()
		{
			lock (_lock)
			{
				bool isStarted = _info.IsStarted;
				_info.Reset();
				((Control)_timeLabel).set_Visible(false);
				if (isStarted)
				{
					Logger.Trace("Reset");
					this.OnStartStop?.Invoke(this, _info.IsStarted);
				}
			}
		}

		private void AddInitialNotes()
		{
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			lock (_lock)
			{
				if (_currentSequence.Count == 0)
				{
					return;
				}
				TimeSpan startTime = TimeSpan.FromSeconds(_songData.StartAtSecond);
				if (_songData.StartAtSecond > 0)
				{
					using List<Note>.Enumerator enumerator = _currentSequence.GetEnumerator();
					while (enumerator.MoveNext() && enumerator.Current.TimeInRotation < startTime)
					{
						_info.SequenceIndex++;
						_info.AbilityIconIndex++;
					}
				}
				TimeSpan totalMoveTime = ((DanceDanceRotationModule.Settings.StartSongsWithFirstSkill.get_Value() && _songData.StartAtSecond <= 0) ? TimeSpan.FromMilliseconds(_windowInfo.TimeToReachEndMs) : ((!(_windowInfo.TimeToReachEndMs >= 1.0)) ? TimeSpan.FromMilliseconds(_windowInfo.TimeToReachEndMs / 2.0) : TimeSpan.FromMilliseconds(_windowInfo.TimeToReachEndMs - 1000.0)));
				_info.StartTime = _lastGameTime;
				startTime = TimeSpanExtension.Divide(startTime, new decimal(_songData.PlaybackRate)).Add(totalMoveTime);
				_info.StartTime -= startTime;
				_info.IsPaused = true;
				_info.PausedTime = _lastGameTime;
				TimeSpan endTimeRotation = TimeSpanExtension.Multiply(totalMoveTime, new decimal(_songData.PlaybackRate)).Add(TimeSpan.FromSeconds(_songData.StartAtSecond));
				while (_info.SequenceIndex < _currentSequence.Count && _currentSequence[_info.SequenceIndex].TimeInRotation < endTimeRotation)
				{
					Note nextNote = _currentSequence[_info.SequenceIndex];
					AddNote(nextNote, _info.SequenceIndex);
					_info.SequenceIndex++;
				}
				if (_info.ActiveNotes.Count > 0)
				{
					for (int index = 0; index < _info.ActiveNotes.Count; index++)
					{
						ActiveNote activeNote = _info.ActiveNotes[index];
						TimeSpan timeInRotation = activeNote.Note.TimeInRotation;
						double moveBase = _windowInfo.NotePositionChangePerSecond * (totalMoveTime.TotalMilliseconds - (timeInRotation.TotalMilliseconds - (double)_songData.StartAtSecond * 1000.0) / (double)_songData.PlaybackRate) / 1000.0;
						Vector2 moveAmount = _windowInfo.GetNoteChangeLocation((float)moveBase);
						activeNote.Update(moveAmount);
					}
					AddInitialAbilityIcons();
				}
			}
		}

		public bool IsStarted()
		{
			return _info.IsStarted;
		}

		public bool IsPaused()
		{
			if (_info.IsStarted)
			{
				return _info.IsPaused;
			}
			return false;
		}

		public void UpdateNotes(GameTime gameTime)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			_lastGameTime = gameTime.get_TotalGameTime();
			if (!_info.IsStarted || _info.IsPaused)
			{
				return;
			}
			TimeSpan timeInRotation = _lastGameTime - _info.StartTime;
			double playbackRate = _songData.PlaybackRate;
			timeInRotation = TimeSpanExtension.Multiply(timeInRotation, new decimal(playbackRate));
			Vector2 moveAmount = _windowInfo.GetNoteChangeLocation(gameTime);
			for (int index3 = _info.ActiveNotes.Count - 1; index3 >= 0; index3--)
			{
				ActiveNote activeNote = _info.ActiveNotes[index3];
				activeNote.Update(moveAmount);
				if (activeNote.ShouldRemove)
				{
					activeNote.Dispose();
					_info.ActiveNotes.RemoveAt(index3);
				}
			}
			for (int index2 = _info.HitTexts.Count - 1; index2 >= 0; index2--)
			{
				HitText hitText = _info.HitTexts[index2];
				hitText.Update(gameTime);
				if (hitText.ShouldDispose())
				{
					hitText.Dispose();
					_info.HitTexts.RemoveAt(index2);
				}
			}
			if (_info.SequenceIndex < _currentSequence.Count)
			{
				Note nextNote = _currentSequence[_info.SequenceIndex];
				if (nextNote.TimeInRotation < timeInRotation)
				{
					AddNote(nextNote, _info.SequenceIndex);
					_info.SequenceIndex++;
				}
			}
			else if (_info.ActiveNotes.Count == 0)
			{
				Logger.Trace("No more active notes. Resetting.");
				Reset();
				AddInitialNotes();
				return;
			}
			for (int index = _info.AbilityIcons.Count - 1; index >= 0; index--)
			{
				AbilityIcon abilityIcon = _info.AbilityIcons[index];
				abilityIcon.Update(gameTime, timeInRotation);
				if (abilityIcon.ShouldDispose)
				{
					abilityIcon.Dispose();
					_info.AbilityIcons.RemoveAt(index);
					if (_info.AbilityIconIndex < _currentSequence.Count)
					{
						AddAbilityIcon(_currentSequence[_info.AbilityIconIndex]);
						_info.AbilityIconIndex++;
						RecalculateLayoutAbilityIcons();
					}
				}
			}
		}

		public void OnHotkeyPressed(NoteType noteType)
		{
			if (!_info.IsStarted)
			{
				if (!DanceDanceRotationModule.Settings.StartSongsWithFirstSkill.get_Value() || _songData.StartAtSecond != 0 || _info.ActiveNotes.Count <= 0 || _info.ActiveNotes[0].Note.TimeInRotation.TotalMilliseconds != 0.0 || !_info.ActiveNotes[0].OnHotkeyPressed())
				{
					return;
				}
				Play();
			}
			foreach (ActiveNote activeNote in _info.ActiveNotes)
			{
				if (activeNote.Note.NoteType == noteType && activeNote.OnHotkeyPressed())
				{
					break;
				}
			}
		}

		private void AddNote(Note note, int index)
		{
			note.NoteType = _songData.RemapNoteType(note.NoteType);
			ActiveNote activeNote = new ActiveNote(lane: (!DanceDanceRotationModule.Settings.CompactMode.get_Value() || _windowInfo.Orientation == NotesOrientation.AbilityBarStyle) ? NoteTypeExtensions.NoteLane(_windowInfo.Orientation, note.NoteType) : GetCompactModeLane(note, index), windowInfo: _windowInfo, note: note, parent: (Container)(object)this);
			activeNote.OnHit += delegate(object sender, HitType hitType)
			{
				AddHitText(activeNote, hitType);
			};
			_info.ActiveNotes.Add(activeNote);
		}

		private void AddHitText(ActiveNote note, HitType hitType)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			HitText hitText = new HitText((Container)(object)this, hitType, new Point(((Control)note.Image).get_Location().X + ((Control)note.Image).get_Width() / 2, ((Control)note.Image).get_Location().Y + ((Control)note.Image).get_Height() / 2));
			_info.HitTexts.Add(hitText);
		}

		private int GetCompactModeLane(Note note, int index)
		{
			int lane = 0;
			if (!DanceDanceRotationModule.Settings.AutoHitWeapon1.get_Value() || note.NoteType != NoteType.Weapon1)
			{
				for (int i = index - 1; i >= 0; i--)
				{
					Note previousNote = _currentSequence[i];
					if (!DanceDanceRotationModule.Settings.AutoHitWeapon1.get_Value() || previousNote.NoteType != NoteType.Weapon1)
					{
						if (previousNote.TimeInRotation + _windowInfo.NoteCollisionCheck > note.TimeInRotation)
						{
							int laneForPreviousNote = GetCompactModeLane(previousNote, i);
							lane += laneForPreviousNote + 1;
							if (lane >= _windowInfo.LaneCount)
							{
								lane %= _windowInfo.LaneCount;
							}
						}
						break;
					}
				}
			}
			return lane;
		}

		private void ShowAllTooltips()
		{
			foreach (ActiveNote activeNote in _info.ActiveNotes)
			{
				activeNote.ShowTooltip();
			}
		}

		private void HideAllTooltips()
		{
			foreach (ActiveNote activeNote in _info.ActiveNotes)
			{
				activeNote.HideTooltip();
			}
		}

		private void CreateBackgroundLines()
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			if (_laneLines != null && _laneLines.Count == _windowInfo.LaneCount)
			{
				return;
			}
			if (_laneLines != null)
			{
				foreach (Control laneLine in _laneLines)
				{
					laneLine.Dispose();
				}
			}
			_laneLines = new List<Control>();
			for (int lane = 0; lane < _windowInfo.LaneCount; lane++)
			{
				List<Control> laneLines = _laneLines;
				Image val = new Image();
				((Control)val).set_BackgroundColor(Color.get_White());
				((Control)val).set_Height(2);
				((Control)val).set_Width(((Control)this).get_Width());
				((Control)val).set_Opacity(0.1f);
				((Control)val).set_Parent((Container)(object)this);
				laneLines.Add((Control)val);
			}
		}

		private void UpdateBackgroundLines()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			if (_laneLines == null || _laneLines.Count < _windowInfo.LaneCount)
			{
				return;
			}
			int staticPosition;
			switch (_windowInfo.Orientation)
			{
			case NotesOrientation.RightToLeft:
				staticPosition = _windowInfo.TargetLocation.X + _windowInfo.NoteWidth;
				break;
			case NotesOrientation.LeftToRight:
				staticPosition = _windowInfo.TargetLocation.X - ((Control)this).get_Width();
				break;
			case NotesOrientation.TopToBottom:
			case NotesOrientation.AbilityBarStyle:
				staticPosition = _windowInfo.TargetLocation.Y - ((Control)this).get_Height();
				break;
			case NotesOrientation.BottomToTop:
				staticPosition = _windowInfo.TargetLocation.Y + _windowInfo.NoteHeight;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			for (int lane = 0; lane < _windowInfo.LaneCount; lane++)
			{
				Point location = _windowInfo.GetNewNoteLocation(lane);
				if (_windowInfo.IsVerticalOrientation())
				{
					location.X += _windowInfo.GetNewNoteSize().X / 2 - 1;
					location.Y = staticPosition;
					_laneLines[lane].set_Height(((Control)this).get_Height());
					_laneLines[lane].set_Width(2);
				}
				else
				{
					location.X = staticPosition;
					location.Y += _windowInfo.GetNewNoteSize().Y / 2 - 1;
					_laneLines[lane].set_Height(2);
					_laneLines[lane].set_Width(((Control)this).get_Width());
				}
				_laneLines[lane].set_Location(location);
			}
		}

		private void CreateTarget()
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Expected O, but got Unknown
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Expected O, but got Unknown
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Expected O, but got Unknown
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Expected O, but got Unknown
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Expected O, but got Unknown
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Expected O, but got Unknown
			Image targetTop = _targetTop;
			if (targetTop != null)
			{
				((Control)targetTop).Dispose();
			}
			Image targetBottom = _targetBottom;
			if (targetBottom != null)
			{
				((Control)targetBottom).Dispose();
			}
			foreach (Image targetCircle in _targetCircles)
			{
				((Control)targetCircle).Dispose();
			}
			_targetCircles.Clear();
			foreach (Image targetSpacer in _targetSpacers)
			{
				((Control)targetSpacer).Dispose();
			}
			_targetSpacers.Clear();
			if (_windowInfo.IsVerticalOrientation())
			{
				Image val = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetLeft));
				((Control)val).set_Width(24);
				((Control)val).set_Height(64);
				((Control)val).set_Location(new Point(0, 0));
				((Control)val).set_Opacity(0.5f);
				((Control)val).set_Parent((Container)(object)this);
				_targetTop = val;
				Image val2 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetRight));
				((Control)val2).set_Width(24);
				((Control)val2).set_Height(64);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Opacity(0.5f);
				((Control)val2).set_Parent((Container)(object)this);
				_targetBottom = val2;
				for (int l = 0; l < _windowInfo.LaneCount; l++)
				{
					List<Image> targetCircles = _targetCircles;
					Image val3 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetCircle));
					((Control)val3).set_Width(64);
					((Control)val3).set_Height(64);
					((Control)val3).set_Location(new Point(0, 0));
					((Control)val3).set_Opacity(0.5f);
					((Control)val3).set_Parent((Container)(object)this);
					targetCircles.Add(val3);
				}
				for (int k = 0; k < _windowInfo.LaneCount - 1; k++)
				{
					List<Image> targetSpacers = _targetSpacers;
					Image val4 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetSpacer));
					((Control)val4).set_Width(24);
					((Control)val4).set_Height(64);
					((Control)val4).set_Location(new Point(0, 0));
					((Control)val4).set_Opacity(0.5f);
					((Control)val4).set_Parent((Container)(object)this);
					targetSpacers.Add(val4);
				}
			}
			else
			{
				Image val5 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetTop));
				((Control)val5).set_Width(64);
				((Control)val5).set_Height(24);
				((Control)val5).set_Location(new Point(0, 0));
				((Control)val5).set_Opacity(0.5f);
				((Control)val5).set_Parent((Container)(object)this);
				_targetTop = val5;
				Image val6 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetBottom));
				((Control)val6).set_Width(64);
				((Control)val6).set_Height(24);
				((Control)val6).set_Location(new Point(0, 0));
				((Control)val6).set_Opacity(0.5f);
				((Control)val6).set_Parent((Container)(object)this);
				_targetBottom = val6;
				_targetCircles = new List<Image>(_windowInfo.LaneCount);
				for (int j = 0; j < _windowInfo.LaneCount; j++)
				{
					List<Image> targetCircles2 = _targetCircles;
					Image val7 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetCircle));
					((Control)val7).set_Width(64);
					((Control)val7).set_Height(64);
					((Control)val7).set_Location(new Point(0, 0));
					((Control)val7).set_Opacity(0.5f);
					((Control)val7).set_Parent((Container)(object)this);
					targetCircles2.Add(val7);
				}
				_targetSpacers = new List<Image>(_windowInfo.LaneCount - 1);
				for (int i = 0; i < _windowInfo.LaneCount - 1; i++)
				{
					List<Image> targetSpacers2 = _targetSpacers;
					Image val8 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.DdrTargetSpacer));
					((Control)val8).set_Width(64);
					((Control)val8).set_Height(24);
					((Control)val8).set_Location(new Point(0, 0));
					((Control)val8).set_Opacity(0.5f);
					((Control)val8).set_Parent((Container)(object)this);
					targetSpacers2.Add(val8);
				}
			}
			_targetCreated = true;
		}

		private void UpdateTarget()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			if (_windowInfo.IsVerticalOrientation())
			{
				int targetHeight = _windowInfo.NoteHeight;
				int xPos2 = _windowInfo.TargetLocation.X - ((Control)_targetTop).get_Width();
				int yPos2 = _windowInfo.TargetLocation.Y;
				int roundEdgesWidth = _windowInfo.HorizontalPadding;
				((Control)_targetTop).set_Width(roundEdgesWidth);
				((Control)_targetTop).set_Height(targetHeight);
				((Control)_targetTop).set_Location(new Point(xPos2, yPos2));
				xPos2 += ((Control)_targetTop).get_Width();
				for (int index2 = 0; index2 < _targetCircles.Count; index2++)
				{
					if (index2 == _targetCircles.Count / 2)
					{
						xPos2 += _windowInfo.CenterPadding;
					}
					((Control)_targetCircles[index2]).set_Width(_windowInfo.NoteWidth);
					((Control)_targetCircles[index2]).set_Height(targetHeight);
					((Control)_targetCircles[index2]).set_Location(new Point(xPos2, yPos2));
					xPos2 += ((Control)_targetCircles[index2]).get_Width();
					if (index2 == _targetCircles.Count / 2 - 1 && _windowInfo.CenterPadding > 0)
					{
						((Control)_targetSpacers[index2]).set_Opacity(0.01f);
					}
					else if (index2 < _targetSpacers.Count)
					{
						((Control)_targetSpacers[index2]).set_Width(_windowInfo.LaneSpacing);
						((Control)_targetSpacers[index2]).set_Height(targetHeight);
						((Control)_targetSpacers[index2]).set_Location(new Point(xPos2, yPos2));
						((Control)_targetSpacers[index2]).set_Opacity(0.5f);
						xPos2 += ((Control)_targetSpacers[index2]).get_Width();
					}
				}
				((Control)_targetBottom).set_Width(roundEdgesWidth);
				((Control)_targetBottom).set_Height(targetHeight);
				((Control)_targetBottom).set_Location(new Point(xPos2, yPos2));
				return;
			}
			int targetWidth = _windowInfo.NoteWidth;
			int xPos = _windowInfo.TargetLocation.X;
			int yPos = _windowInfo.TargetLocation.Y - ((Control)_targetTop).get_Height();
			int roundEdgesHeight = (int)Math.Min(_windowInfo.VerticalPadding - 4, (double)targetWidth * 0.375);
			((Control)_targetTop).set_Height(roundEdgesHeight);
			((Control)_targetTop).set_Width(targetWidth);
			((Control)_targetTop).set_Location(new Point(xPos, yPos));
			yPos += ((Control)_targetTop).get_Height();
			for (int index = 0; index < _targetCircles.Count; index++)
			{
				((Control)_targetCircles[index]).set_Height(_windowInfo.NoteHeight);
				((Control)_targetCircles[index]).set_Width(targetWidth);
				((Control)_targetCircles[index]).set_Location(new Point(xPos, yPos));
				yPos += ((Control)_targetCircles[index]).get_Height();
				if (index < _targetSpacers.Count)
				{
					((Control)_targetSpacers[index]).set_Height(_windowInfo.LaneSpacing);
					((Control)_targetSpacers[index]).set_Width(targetWidth);
					((Control)_targetSpacers[index]).set_Location(new Point(xPos, yPos));
					yPos += ((Control)_targetSpacers[index]).get_Height();
				}
			}
			((Control)_targetBottom).set_Height(roundEdgesHeight);
			((Control)_targetBottom).set_Width(targetWidth);
			((Control)_targetBottom).set_Location(new Point(xPos, yPos));
		}

		private void AddInitialAbilityIcons()
		{
			int totalAbilityIcons = DanceDanceRotationModule.Settings.ShowNextAbilitiesCount.get_Value();
			if (totalAbilityIcons > 0)
			{
				int index = _info.AbilityIconIndex;
				for (int size = Math.Min(_info.AbilityIconIndex + totalAbilityIcons, _currentSequence.Count); index < size; index++)
				{
					AddAbilityIcon(_currentSequence[index]);
					_info.AbilityIconIndex++;
				}
				RecalculateLayoutAbilityIcons();
			}
		}

		private void AddAbilityIcon(Note note)
		{
			AbilityIcon icon = new AbilityIcon(_windowInfo, note, _songData, (Container)(object)this);
			_info.AbilityIcons.Add(icon);
		}

		private void RecalculateLayoutAbilityIcons()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			int size = _windowInfo.NoteWidth;
			int xPos = _windowInfo.NextAbilityIconsLocation.X;
			int yPos = _windowInfo.NextAbilityIconsLocation.Y;
			for (int index = 0; index < _info.AbilityIcons.Count; index++)
			{
				AbilityIcon abilityIcon = _info.AbilityIcons[index];
				((Control)abilityIcon.Image).set_Size(new Point(size, size));
				((Control)abilityIcon.Image).set_Location(new Point(xPos, yPos));
				xPos += size;
			}
		}

		protected override void DisposeControl()
		{
			((Container)this).DisposeControl();
			((Control)_targetTop).Dispose();
			((Control)_targetBottom).Dispose();
			foreach (Image targetCircle in _targetCircles)
			{
				((Control)targetCircle).Dispose();
			}
			foreach (Image targetSpacer in _targetSpacers)
			{
				((Control)targetSpacer).Dispose();
			}
		}
	}
}
