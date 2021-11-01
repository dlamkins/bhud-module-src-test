using System;
using System.Linq;
using BhModule.Community.Pathing.Editor;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Tooltips;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Entities;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.Mumble.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Entity
{
	public class FlatMap : Control
	{
		private const int MAPWIDTH_MAX = 362;

		private const int MAPHEIGHT_MAX = 338;

		private const int MAPWIDTH_MIN = 170;

		private const int MAPHEIGHT_MIN = 170;

		private const int MAPOFFSET_MIN = 19;

		private readonly SpriteBatchParameters _pathableParameters;

		private readonly IRootPackState _packState;

		private double _lastMapViewChanged;

		private float _lastCameraPos;

		private IPathingEntity _activeEntity;

		private readonly DescriptionTooltipView _tooltipView;

		private readonly Tooltip _activeTooltip;

		private ContextMenuStrip _activeContextMenu;

		private ContextMenuStrip BuildPathableMenu(IPathingEntity pathingEntry)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip newMenu = new ContextMenuStrip();
			((Control)newMenu.AddMenuItem("Hide Parent Category")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_packState.CategoryStates.SetInactive(pathingEntry.Category.Namespace, isInactive: true);
			});
			if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				((Control)newMenu.AddMenuItem("Copy Parent Category Namespace")).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(pathingEntry.Category.Namespace);
				});
				((Control)newMenu.AddMenuItem("Edit Marker")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					MarkerEditWindow.SetPathingEntity(_packState, pathingEntry);
				});
				((Control)newMenu.AddMenuItem("Delete Marker")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ScreenNotification.ShowNotification("Not yet supported", (NotificationType)1, (Texture2D)null, 5);
				});
			}
			((Control)newMenu).add_Hidden((EventHandler<EventArgs>)delegate
			{
				((Control)_activeContextMenu).Dispose();
				_activeContextMenu = null;
			});
			_activeContextMenu = newMenu;
			return newMenu;
		}

		public FlatMap(IRootPackState packState)
			: this()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			((Control)this).set_ZIndex(-1073741824);
			_packState = packState;
			((Control)this).set_Location(new Point(1));
			_pathableParameters = ((Control)this).get_SpriteBatchParameters();
			((Control)this).set_SpriteBatchParameters(new SpriteBatchParameters((SpriteSortMode)0, BlendState.Opaque, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null));
			UpdateBounds();
			_tooltipView = new DescriptionTooltipView();
			_activeTooltip = new Tooltip((ITooltipView)(object)_tooltipView);
			((Control)this).set_Tooltip(_activeTooltip);
			GameService.Gw2Mumble.get_UI().add_UISizeChanged((EventHandler<ValueEventArgs<UiSize>>)UIOnUISizeChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)UIOnIsMapOpenChanged);
		}

		private void TriggerFadeIn()
		{
			GameTime currentGameTime = GameService.Overlay.get_CurrentGameTime();
			_lastMapViewChanged = ((currentGameTime != null) ? currentGameTime.get_TotalGameTime().TotalSeconds : 0.0);
		}

		private void UIOnUISizeChanged(object sender, ValueEventArgs<UiSize> e)
		{
			TriggerFadeIn();
		}

		private void UIOnIsMapOpenChanged(object sender, ValueEventArgs<bool> e)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			TriggerFadeIn();
			if (_activeContextMenu != null)
			{
				((Control)_activeContextMenu).set_Visible(false);
			}
			_lastCameraPos = GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z;
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnRightMouseButtonPressed(e);
			if (_activeEntity != null)
			{
				BuildPathableMenu(_activeEntity).Show(e.get_MousePosition());
			}
		}

		private int GetOffset(float curr, float max, float min, float val)
		{
			return (int)Math.Round((curr - min) / (max - min) * (val - 19f) + 19f, 0);
		}

		private void UpdateBounds()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			Size compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			if (((Size)(ref compassSize)).get_Width() < 1)
			{
				return;
			}
			compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			if (((Size)(ref compassSize)).get_Height() < 1)
			{
				return;
			}
			Point newSize = default(Point);
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				((Control)this).set_Location(Point.get_Zero());
				newSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			}
			else
			{
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				int offsetWidth = GetOffset(((Size)(ref compassSize)).get_Width(), 362f, 170f, 40f);
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				int offsetHeight = GetOffset(((Size)(ref compassSize)).get_Height(), 338f, 170f, 40f);
				if (GameService.Gw2Mumble.get_UI().get_IsCompassTopRight())
				{
					int width = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Width;
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					((Control)this).set_Location(new Point(width - ((Size)(ref compassSize)).get_Width() - offsetWidth + 1, 1));
				}
				else
				{
					int width2 = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Width;
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					int num = width2 - ((Size)(ref compassSize)).get_Width() - offsetWidth;
					int height = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Height;
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					((Control)this).set_Location(new Point(num, height - ((Size)(ref compassSize)).get_Height() - offsetHeight - 40));
				}
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				int num2 = ((Size)(ref compassSize)).get_Width() + offsetWidth;
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				((Point)(ref newSize))._002Ector(num2, ((Size)(ref compassSize)).get_Height() + offsetHeight);
			}
			((Control)this).set_Size(newSize);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			UpdateBounds();
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				if ((int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() == 0)
				{
					GameTime currentGameTime = GameService.Overlay.get_CurrentGameTime();
					if (((currentGameTime != null) ? new double?(currentGameTime.get_TotalGameTime().TotalSeconds) : null) - _lastMapViewChanged > 1.0 && Math.Abs(_lastCameraPos - GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z) > 0.1f)
					{
						((Control)this).Hide();
					}
				}
				_lastCameraPos = GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z;
			}
			((Control)this).DoUpdate(gameTime);
		}

		private void UpdateTooltip(IPathingEntity pathable, bool isAlternativeMenu = false)
		{
			string tooltipDescription = "";
			string tooltipTitle;
			if (pathable != null && isAlternativeMenu)
			{
				tooltipTitle = string.Join("\n > ", from category in pathable.Category.GetParentsDesc()
					select category.DisplayName.Trim());
				tooltipDescription = null;
			}
			else
			{
				IHasMapInfo mapPathable = pathable as IHasMapInfo;
				if (mapPathable == null)
				{
					((Control)this).set_Tooltip((Tooltip)null);
					((Control)_activeTooltip).Hide();
					return;
				}
				tooltipTitle = mapPathable.TipName;
				if (!string.IsNullOrWhiteSpace(mapPathable.TipDescription))
				{
					tooltipDescription = mapPathable.TipDescription + "\n\n";
				}
				tooltipDescription += $"{WorldUtil.WorldToGameCoord(pathable.DistanceToPlayer):##,###} away";
			}
			_tooltipView.Title = tooltipTitle;
			_tooltipView.Description = tooltipDescription;
			((Control)this).set_Tooltip(_activeTooltip);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Invalid comparison between Unknown and I4
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_IsInGame())
			{
				return;
			}
			((Rectangle)(ref bounds))._002Ector(((Control)this).get_Location(), ((Rectangle)(ref bounds)).get_Size());
			spriteBatch.Draw(Textures.get_TransparentPixel(), bounds, Color.get_Transparent());
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _pathableParameters);
			double scale = GameService.Gw2Mumble.get_UI().get_MapScale() * 0.897;
			double offsetX = (double)bounds.X + (double)bounds.Width / 2.0;
			double offsetY = (double)bounds.Y + (double)bounds.Height / 2.0;
			float opacity = MathHelper.Clamp((float)(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds - _lastMapViewChanged) / 0.65f, 0f, 1f) * 0.8f;
			IOrderedEnumerable<IPathingEntity> orderedEnumerable = from poi in _packState.Entities.ToList()
				orderby 0f - ((IEntity)poi).get_DrawOrder()
				select poi;
			bool showModTooltip = (GameService.Input.get_Keyboard().get_ActiveModifiers() & 4) == 4;
			_activeEntity = null;
			foreach (IPathingEntity pathable in orderedEnumerable)
			{
				RectangleF? hint = pathable.RenderToMiniMap(spriteBatch, bounds, (offsetX, offsetY), scale, opacity);
				if (((Control)this).get_MouseOver() && hint.HasValue)
				{
					RectangleF value = hint.Value;
					if (((RectangleF)(ref value)).Contains(Point2.op_Implicit(GameService.Input.get_Mouse().get_Position())))
					{
						_activeEntity = pathable;
					}
				}
			}
			UpdateTooltip(_activeEntity, showModTooltip);
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_UI().remove_UISizeChanged((EventHandler<ValueEventArgs<UiSize>>)UIOnUISizeChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)UIOnIsMapOpenChanged);
			((Control)this).DisposeControl();
		}
	}
}
