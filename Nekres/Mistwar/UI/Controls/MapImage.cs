using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Glide;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.UI.Controls
{
	internal class MapImage : Container
	{
		private IEnumerable<WvwObjectiveEntity> _wvwObjectives;

		private ContinentFloorRegionMap _map;

		protected AsyncTexture2D _texture;

		private SpriteEffects _spriteEffects;

		private Rectangle? _sourceRectangle;

		private Color _tint = Color.get_White();

		private Effect _grayscaleEffect;

		private SpriteBatchParameters _grayscaleSpriteBatchParams;

		private Texture2D _playerArrow;

		private Dictionary<int, Rectangle> _wayPointBounds;

		public IEnumerable<WvwObjectiveEntity> WvwObjectives
		{
			get
			{
				return _wvwObjectives;
			}
			set
			{
				((Control)this).SetProperty<IEnumerable<WvwObjectiveEntity>>(ref _wvwObjectives, value, false, "WvwObjectives");
			}
		}

		public ContinentFloorRegionMap Map
		{
			get
			{
				return _map;
			}
			set
			{
				if (((Control)this).SetProperty<ContinentFloorRegionMap>(ref _map, value, false, "Map"))
				{
					_wayPointBounds?.Clear();
				}
			}
		}

		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			private init
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _texture, value, false, "Texture");
			}
		}

		public SpriteEffects SpriteEffects
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _spriteEffects;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<SpriteEffects>(ref _spriteEffects, value, false, "SpriteEffects");
			}
		}

		public Rectangle SourceRectangle
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				return (Rectangle)(((_003F?)_sourceRectangle) ?? _texture.get_Texture().get_Bounds());
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Rectangle?>(ref _sourceRectangle, (Rectangle?)value, false, "SourceRectangle");
			}
		}

		public Color Tint
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _tint;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _tint, value, false, "Tint");
			}
		}

		public float TextureOpacity { get; private set; }

		public float ScaleRatio { get; private set; } = MathHelper.Clamp(MistwarModule.ModuleInstance.ScaleRatioSetting.get_Value() / 100f, 0f, 1f);


		public MapImage()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			_wayPointBounds = new Dictionary<int, Rectangle>();
			_playerArrow = MistwarModule.ModuleInstance.ContentsManager.GetTexture("156081.png");
			((Control)this)._spriteBatchParameters = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_grayscaleEffect = MistwarModule.ModuleInstance.ContentsManager.GetEffect<Effect>("effects\\grayscale.mgfx");
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(_grayscaleEffect);
			_grayscaleSpriteBatchParams = val;
			Texture = new AsyncTexture2D();
			Texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
			MistwarModule.ModuleInstance.ScaleRatioSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
		}

		public void Toggle(bool forceHide = false, bool silent = false, float tDuration = 0.1f)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			silent = silent || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch();
			if (forceHide || !GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || ((Control)this)._visible)
			{
				((Control)this)._visible = false;
				if (silent)
				{
					((Control)this).set_Visible(false);
					return;
				}
				GameService.Content.PlaySoundEffectByName("window-close");
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(this, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)((Control)this).Hide);
				return;
			}
			((Control)this)._visible = true;
			((Control)this).set_Visible(true);
			if (!silent)
			{
				GameService.Content.PlaySoundEffectByName("page-open-" + RandomUtil.GetRandom(1, 3));
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(this, (object)new
				{
					Opacity = 1f
				}, 0.35f, 0f, true);
			}
		}

		internal void SetOpacity(float opacity)
		{
			TextureOpacity = opacity;
			_grayscaleEffect.get_Parameters().get_Item("Opacity").SetValue(opacity);
		}

		public void SetColorIntensity(float colorIntensity)
		{
			_grayscaleEffect.get_Parameters().get_Item("Intensity").SetValue(MathHelper.Clamp(colorIntensity, 0f, 1f));
		}

		private void OnScaleRatioChanged(object o, ValueChangedEventArgs<float> e)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			ScaleRatio = MathHelper.Clamp(e.get_NewValue() / 100f, 0f, 1f);
			if (_texture.get_HasTexture())
			{
				Rectangle bounds = _texture.get_Texture().get_Bounds();
				((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
				((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			foreach (KeyValuePair<int, Rectangle> bound in _wayPointBounds.ToList())
			{
				Rectangle value = bound.Value;
				if (!((Rectangle)(ref value)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					continue;
				}
				ContinentFloorRegionMapPoi wp = Map.get_PointsOfInterest().Values.FirstOrDefault((ContinentFloorRegionMapPoi x) => x.get_Id() == bound.Key);
				if (wp != null)
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					if (PInvoke.IsLControlPressed())
					{
						await ChatUtil.Send(wp.get_ChatLink(), MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value());
					}
					else if (PInvoke.IsLShiftPressed())
					{
						await ChatUtil.Insert(wp.get_ChatLink(), MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value());
					}
					else if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(wp.get_ChatLink()))
					{
						ScreenNotification.ShowNotification("Waypoint copied to clipboard!", (NotificationType)0, (Texture2D)null, 4);
					}
				}
				break;
			}
			_003C_003En__0(e);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			List<KeyValuePair<int, Rectangle>> wps = _wayPointBounds.ToList();
			foreach (KeyValuePair<int, Rectangle> bound in wps)
			{
				Rectangle value = bound.Value;
				if (!((Rectangle)(ref value)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					continue;
				}
				ContinentFloorRegionMapPoi wp = Map.get_PointsOfInterest().Values.FirstOrDefault((ContinentFloorRegionMapPoi x) => x.get_Id() == bound.Key);
				if (wp == null || wp.get_Name() == null)
				{
					break;
				}
				string wpName = wp.get_Name();
				if (wp.get_Name().StartsWith(" "))
				{
					WvwObjectiveEntity obj = WvwObjectives.FirstOrDefault((WvwObjectiveEntity x) => x.WayPoints.Any((ContinentFloorRegionMapPoi y) => y.get_Id() == wp.get_Id()));
					if (obj == null)
					{
						break;
					}
					wpName = MistwarModule.ModuleInstance.WvwService.GetWorldName(obj.Owner) + wpName;
				}
				((Control)this).set_BasicTooltipText(wpName);
			}
			if (wps.All(delegate(KeyValuePair<int, Rectangle> x)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Rectangle value2 = x.Value;
				return !((Rectangle)(ref value2)).Contains(((Control)this).get_RelativeMousePosition());
			}))
			{
				((Control)this).set_BasicTooltipText(string.Empty);
			}
			((Control)this).OnMouseMoved(e);
		}

		protected override void DisposeControl()
		{
			if (_texture != null)
			{
				_texture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
				_texture.Dispose();
			}
			Effect grayscaleEffect = _grayscaleEffect;
			if (grayscaleEffect != null)
			{
				((GraphicsResource)grayscaleEffect).Dispose();
			}
			Texture2D playerArrow = _playerArrow;
			if (playerArrow != null)
			{
				((GraphicsResource)playerArrow).Dispose();
			}
			MistwarModule.ModuleInstance.ScaleRatioSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
			((Container)this).DisposeControl();
		}

		private void OnTextureSwapped(object o, ValueChangedEventArgs<Texture2D> e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			SourceRectangle = e.get_NewValue().get_Bounds();
			Rectangle bounds = e.get_NewValue().get_Bounds();
			((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
			((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Invalid comparison between Unknown and I4
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Unknown result type (might be due to invalid IL or missing references)
			//IL_051a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_0535: Unknown result type (might be due to invalid IL or missing references)
			//IL_053a: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0552: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0632: Unknown result type (might be due to invalid IL or missing references)
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_064d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_0670: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0702: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0732: Unknown result type (might be due to invalid IL or missing references)
			//IL_075a: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			if (!GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || !((Control)this).get_Visible() || !_texture.get_HasTexture() || WvwObjectives == null)
			{
				return;
			}
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _grayscaleSpriteBatchParams);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texture), bounds, (Rectangle?)SourceRectangle, _tint, 0f, Vector2.get_Zero(), _spriteEffects);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this)._spriteBatchParameters);
			float widthRatio = (float)bounds.Width / (float)SourceRectangle.Width;
			float heightRatio = (float)bounds.Height / (float)SourceRectangle.Height;
			if (MistwarModule.ModuleInstance.DrawSectorsSetting.get_Value())
			{
				foreach (WvwObjectiveEntity item in WvwObjectives.OrderBy((WvwObjectiveEntity x) => x.Owner == MistwarModule.ModuleInstance.WvwService.CurrentTeam))
				{
					Color teamColor = item.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value(), (int)(TextureOpacity * 255f));
					Vector2[] sectorBounds = item.Bounds.Select((Func<Point, Vector2>)delegate(Point p)
					{
						//IL_0020: Unknown result type (might be due to invalid IL or missing references)
						//IL_002b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0030: Unknown result type (might be due to invalid IL or missing references)
						//IL_0035: Unknown result type (might be due to invalid IL or missing references)
						//IL_0036: Unknown result type (might be due to invalid IL or missing references)
						//IL_003d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0044: Unknown result type (might be due to invalid IL or missing references)
						Point val4 = PointExtensions.ToBounds(new Point((int)(widthRatio * (float)p.X), (int)(heightRatio * (float)p.Y)), ((Control)this).get_AbsoluteBounds());
						return new Vector2((float)val4.X, (float)val4.Y);
					}).ToArray();
					ShapeExtensions.DrawPolygon(spriteBatch, new Vector2(0f, 0f), (IReadOnlyList<Vector2>)sectorBounds, teamColor, 4f, 0f);
				}
			}
			Rectangle dest = default(Rectangle);
			Coordinates2 val;
			Rectangle wpDest = default(Rectangle);
			foreach (WvwObjectiveEntity objectiveEntity in WvwObjectives)
			{
				if (objectiveEntity.Icon != null)
				{
					if (!MistwarModule.ModuleInstance.DrawRuinMapSetting.get_Value() && (int)objectiveEntity.Type == 6)
					{
						continue;
					}
					int width = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Width());
					int height = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Height());
					((Rectangle)(ref dest))._002Ector((int)(widthRatio * (float)objectiveEntity.Center.X), (int)(heightRatio * (float)objectiveEntity.Center.Y), width, height);
					spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, 1f, 0.75f, MistwarModule.ModuleInstance.DrawObjectiveNamesSetting.get_Value());
				}
				foreach (ContinentFloorRegionMapPoi wp in objectiveEntity.WayPoints)
				{
					if (GameUtil.IsEmergencyWayPoint(wp))
					{
						if (!MistwarModule.ModuleInstance.DrawEmergencyWayPointsSetting.get_Value() || objectiveEntity.Owner != MistwarModule.ModuleInstance.WvwService.CurrentTeam)
						{
							continue;
						}
						if (!objectiveEntity.HasEmergencyWaypoint())
						{
							_wayPointBounds.Remove(wp.get_Id());
							continue;
						}
					}
					else if (!objectiveEntity.HasRegularWaypoint())
					{
						_wayPointBounds.Remove(wp.get_Id());
						continue;
					}
					double num = widthRatio;
					val = wp.get_Coord();
					int num2 = (int)(num * ((Coordinates2)(ref val)).get_X()) - (int)(widthRatio * (ScaleRatio * 64f) / 2f);
					double num3 = heightRatio;
					val = wp.get_Coord();
					((Rectangle)(ref wpDest))._002Ector(num2, (int)(num3 * ((Coordinates2)(ref val)).get_Y()) - (int)(heightRatio * (ScaleRatio * 64f) / 2f), (int)(ScaleRatio * 64f), (int)(ScaleRatio * 64f));
					Texture2D tex = objectiveEntity.GetWayPointIcon(((Rectangle)(ref wpDest)).Contains(((Control)this).get_RelativeMousePosition()));
					if (!_wayPointBounds.ContainsKey(wp.get_Id()))
					{
						_wayPointBounds.Add(wp.get_Id(), wpDest);
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, wpDest);
				}
			}
			if (Map != null)
			{
				Vector3 v = GameService.Gw2Mumble.get_PlayerCamera().get_Position() * 39.37008f;
				Rectangle val2 = Map.get_ContinentRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double x2 = ((Coordinates2)(ref val)).get_X();
				double num4 = v.X;
				val2 = Map.get_MapRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double num5 = num4 - ((Coordinates2)(ref val)).get_X();
				val2 = Map.get_MapRect();
				double num6 = num5 / ((Rectangle)(ref val2)).get_Width();
				val2 = Map.get_ContinentRect();
				float num7 = (float)(x2 + num6 * ((Rectangle)(ref val2)).get_Width());
				val2 = Map.get_ContinentRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double y = ((Coordinates2)(ref val)).get_Y();
				double num8 = v.Y;
				val2 = Map.get_MapRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double num9 = num8 - ((Coordinates2)(ref val)).get_Y();
				val2 = Map.get_MapRect();
				double num10 = num9 / ((Rectangle)(ref val2)).get_Height();
				val2 = Map.get_ContinentRect();
				Vector2 val3 = new Vector2(num7, (float)(y - num10 * ((Rectangle)(ref val2)).get_Height()));
				Vector2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter().ToXnaVector2();
				Vector2 pos = Vector2.Transform(val3 - mapCenter, Matrix.CreateRotationZ(0f));
				Coordinates2 value = new Coordinates2((double)pos.X, (double)pos.Y);
				val2 = Map.get_ContinentRect();
				Point fit = MapUtil.Refit(value, ((Rectangle)(ref val2)).get_TopLeft());
				Rectangle tDest = default(Rectangle);
				((Rectangle)(ref tDest))._002Ector((int)(widthRatio * (float)fit.X), (int)(heightRatio * (float)fit.Y), (int)(ScaleRatio * (float)_playerArrow.get_Width()), (int)(ScaleRatio * (float)_playerArrow.get_Height()));
				double rot = Math.Atan2(GameService.Gw2Mumble.get_PlayerCamera().get_Forward().X, GameService.Gw2Mumble.get_PlayerCamera().get_Forward().Y) * 3.5999999046325684 / Math.PI;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _playerArrow, new Rectangle(tDest.X + tDest.Width / 4, tDest.Y + tDest.Height / 4, tDest.Width, tDest.Height), (Rectangle?)_playerArrow.get_Bounds(), Color.get_White(), (float)rot, new Vector2((float)_playerArrow.get_Width() / 2f, (float)_playerArrow.get_Height() / 2f), (SpriteEffects)0);
			}
			if (MistwarModule.ModuleInstance.WvwService.IsLoading)
			{
				Rectangle spinnerBnds = default(Rectangle);
				((Rectangle)(ref spinnerBnds))._002Ector(bounds.Width / 2, bounds.Height - 100, 70, 70);
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, spinnerBnds);
				Size2 size = Control.get_Content().get_DefaultFont32().MeasureString(MistwarModule.ModuleInstance.WvwService.LoadingMessage);
				Rectangle dest2 = default(Rectangle);
				((Rectangle)(ref dest2))._002Ector((int)((float)(spinnerBnds.X + spinnerBnds.Width / 2) - size.Width / 2f), ((Rectangle)(ref spinnerBnds)).get_Bottom(), (int)size.Width, (int)size.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, MistwarModule.ModuleInstance.WvwService.LoadingMessage, Control.get_Content().get_DefaultFont16(), dest2, Color.get_White(), false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}
	}
}
