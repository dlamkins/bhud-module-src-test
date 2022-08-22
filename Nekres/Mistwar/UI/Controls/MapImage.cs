using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
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
				((Control)this).SetProperty<ContinentFloorRegionMap>(ref _map, value, false, "Map");
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

		public void Toggle(float tDuration = 0.1f, bool silent = false)
		{
			if (((Control)this)._visible)
			{
				((Control)this)._visible = false;
				if (silent)
				{
					((Control)this).Hide();
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
			((Control)this).Show();
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
						await ChatUtil.PastText(wp.get_ChatLink());
					}
					else if (PInvoke.IsLShiftPressed())
					{
						await ChatUtil.InsertText(wp.get_ChatLink());
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
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Invalid comparison between Unknown and I4
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible() || !_texture.get_HasTexture() || WvwObjectives == null)
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
					if (!GameUtil.IsEmergencyWayPoint(wp) || (MistwarModule.ModuleInstance.DrawEmergencyWayPointsSetting.get_Value() && objectiveEntity.Owner == MistwarModule.ModuleInstance.WvwService.CurrentTeam))
					{
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
				Point fit = MapUtil.Refit(value, ((Rectangle)(ref val2)).get_TopLeft(), 0, 256);
				Rectangle tDest = default(Rectangle);
				((Rectangle)(ref tDest))._002Ector((int)(widthRatio * (float)fit.X), (int)(heightRatio * (float)fit.Y), (int)(ScaleRatio * (float)_playerArrow.get_Width()), (int)(ScaleRatio * (float)_playerArrow.get_Height()));
				double rot = Math.Atan2(GameService.Gw2Mumble.get_PlayerCamera().get_Forward().X, GameService.Gw2Mumble.get_PlayerCamera().get_Forward().Y) * 3.5999999046325684 / Math.PI;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _playerArrow, new Rectangle(tDest.X + tDest.Width / 4, tDest.Y + tDest.Height / 4, tDest.Width, tDest.Height), (Rectangle?)_playerArrow.get_Bounds(), Color.get_White(), (float)rot, new Vector2((float)_playerArrow.get_Width() / 2f, (float)_playerArrow.get_Height() / 2f), (SpriteEffects)0);
			}
		}
	}
}
