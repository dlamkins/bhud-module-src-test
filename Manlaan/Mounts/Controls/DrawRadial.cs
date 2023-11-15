using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Mounts;

namespace Manlaan.Mounts.Controls
{
	internal class DrawRadial : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<DrawRadial>();

		private readonly Helper _helper;

		private readonly TextureCache _textureCache;

		private StandardButton _settingsButton;

		private Label _errorLabel;

		private List<RadialThing> RadialThings = new List<RadialThing>();

		private int radius;

		private int thingIconSize;

		private int _maxRadialDiameter;

		private Point SpawnPoint;

		private float debugLineThickness = 2f;

		public EventHandler OnSettingsButtonClicked { get; internal set; }

		private RadialThing SelectedMount => RadialThings.SingleOrDefault((RadialThing m) => m.Selected);

		public override int ZIndex
		{
			get
			{
				return ((Control)this).get_ZIndex();
			}
			set
			{
				((Control)this).set_ZIndex(value);
			}
		}

		public bool IsActionCamToggledOnMount { get; private set; }

		public DrawRadial(Helper helper, TextureCache textureCache)
			: this()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			((Control)this).set_Visible(false);
			((Control)this).set_Padding(Thickness.Zero);
			_helper = helper;
			_textureCache = textureCache;
			((Control)this).add_Shown((EventHandler<EventArgs>)async delegate(object sender, EventArgs e)
			{
				await HandleShown(sender, e);
			});
			((Control)this).add_Hidden((EventHandler<EventArgs>)async delegate(object sender, EventArgs e)
			{
				await HandleHidden(sender, e);
			});
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(800, 500));
			val.set_Font(GameService.Content.get_DefaultFont32());
			val.set_TextColor(Color.get_Red());
			_errorLabel = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(250, 300));
			val2.set_Text(Strings.Settings_Button_Label);
			((Control)val2).set_Visible(false);
			_settingsButton = val2;
			((Control)_settingsButton).add_Click((EventHandler<MouseEventArgs>)delegate(object args, MouseEventArgs sender)
			{
				OnSettingsButtonClicked(args, (EventArgs)(object)sender);
			});
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			RadialThings.Clear();
			RadialThingSettings applicableRadialSettings = _helper.GetTriggeredRadialSettings();
			if (applicableRadialSettings == null)
			{
				return;
			}
			List<Thing> things = applicableRadialSettings.AvailableThings.ToList();
			if (!things.Any())
			{
				_errorLabel.set_Text("No actions configured in the " + applicableRadialSettings.Name + " context \n(or no keybinds specified for these actions)\nClick button to go to the relevant settings: ");
				((Control)_errorLabel).Show();
				((Control)_settingsButton).Show();
				return;
			}
			((Control)_errorLabel).Hide();
			((Control)_settingsButton).Hide();
			Thing thingToPutInCenter = applicableRadialSettings.GetCenterThing();
			if (thingToPutInCenter != null && thingToPutInCenter.IsAvailable)
			{
				if (applicableRadialSettings.RemoveCenterThing.get_Value())
				{
					things.Remove(thingToPutInCenter);
				}
				Texture2D texture2 = _textureCache.GetThingImgFile(thingToPutInCenter);
				int loc = radius;
				RadialThings.Add(new RadialThing
				{
					Texture = texture2,
					Thing = thingToPutInCenter,
					ImageX = loc,
					ImageY = loc,
					Default = true
				});
			}
			double startAngle = Math.PI * Math.Floor(Module._settingMountRadialStartAngle.get_Value() * 360f) / 180.0;
			if (DebugHelper.IsDebugEnabled())
			{
				Vector2 spawnPointVec = ((Point)(ref SpawnPoint)).ToVector2();
				Vector2 rectpos = spawnPointVec - new Vector2((float)(thingIconSize / 2), (float)(thingIconSize / 2));
				ShapeExtensions.DrawRectangle(spriteBatch, rectpos, new Size2((float)thingIconSize, (float)thingIconSize), Color.get_Red(), debugLineThickness, 0f);
				ShapeExtensions.DrawCircle(spriteBatch, spawnPointVec, 1f, 50, Color.get_Red(), debugLineThickness, 0f);
				ShapeExtensions.DrawCircle(spriteBatch, spawnPointVec, GetRadius(), 50, Color.get_Red(), debugLineThickness, 0f);
			}
			double currentAngle = startAngle;
			double partAngleStep = Math.PI * 2.0 / (double)things.Count();
			foreach (Thing thing in things)
			{
				double angleMid = currentAngle + partAngleStep / 2.0;
				double angleEnd = currentAngle + partAngleStep;
				Texture2D texture = _textureCache.GetThingImgFile(thing);
				int x = (int)Math.Round((double)radius + (double)radius * Math.Cos(angleMid));
				int y = (int)Math.Round((double)radius + (double)radius * Math.Sin(angleMid));
				if (DebugHelper.IsDebugEnabled())
				{
					float xDebugInner = (float)Math.Round((double)GetRadius() * Math.Cos(currentAngle)) + (float)SpawnPoint.X;
					float yDebugInner = (float)Math.Round((double)GetRadius() * Math.Sin(currentAngle)) + (float)SpawnPoint.Y;
					int debugRadiusOuter = 250;
					float xDebugOuter = (float)Math.Round((double)(2 * debugRadiusOuter) * Math.Cos(currentAngle)) + (float)SpawnPoint.X;
					float yDebugOuter = (float)Math.Round((double)(2 * debugRadiusOuter) * Math.Sin(currentAngle)) + (float)SpawnPoint.Y;
					ShapeExtensions.DrawLine(spriteBatch, new Vector2(xDebugInner, yDebugInner), new Vector2(xDebugOuter, yDebugOuter), Color.get_Red(), debugLineThickness, 0f);
				}
				RadialThings.Add(new RadialThing
				{
					Texture = texture,
					Thing = thing,
					ImageX = x,
					ImageY = y,
					AngleBegin = currentAngle,
					AngleEnd = angleEnd
				});
				currentAngle = angleEnd;
			}
			Point diff = Control.get_Input().get_Mouse().get_Position() - SpawnPoint;
			double angle;
			for (angle = Math.Atan2(diff.Y, diff.X); angle < startAngle; angle += Math.PI * 2.0)
			{
			}
			Vector2 val = new Vector2((float)diff.Y, (float)diff.X);
			float length = ((Vector2)(ref val)).Length();
			foreach (RadialThing radialMount in RadialThings)
			{
				if (length < GetRadius())
				{
					radialMount.Selected = radialMount.Default;
				}
				else
				{
					radialMount.Selected = radialMount.AngleBegin <= angle && radialMount.AngleEnd > angle;
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, radialMount.Texture, new Rectangle(radialMount.ImageX, radialMount.ImageY, thingIconSize, thingIconSize), (Rectangle?)null, Color.get_White() * (radialMount.Selected ? 1f : Module._settingMountRadialIconOpacity.get_Value()));
			}
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		private float GetRadius()
		{
			return (float)((double)thingIconSize * Math.Sqrt(2.0) / 2.0);
		}

		public async Task TriggerSelectedMountAsync()
		{
			await (SelectedMount?.Thing.DoAction() ?? Task.CompletedTask);
		}

		private async Task HandleShown(object sender, EventArgs e)
		{
			Logger.Debug("HandleShown entered");
			if (!GameService.Input.get_Mouse().get_CursorIsVisible() && !((SettingEntry)Module._settingMountRadialToggleActionCameraKeyBinding).get_IsNull())
			{
				IsActionCamToggledOnMount = true;
				await Helper.TriggerKeybind(Module._settingMountRadialToggleActionCameraKeyBinding);
				Logger.Debug("HandleShown turned off action cam");
			}
			_maxRadialDiameter = Math.Min(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			thingIconSize = (int)((float)(_maxRadialDiameter / 4) * Module._settingMountRadialIconSizeModifier.get_Value());
			radius = (int)((float)(_maxRadialDiameter / 2 - thingIconSize / 2) * Module._settingMountRadialRadiusModifier.get_Value());
			((Control)this).set_Size(new Point(_maxRadialDiameter, _maxRadialDiameter));
			if (Module._settingMountRadialSpawnAtMouse.get_Value())
			{
				SpawnPoint = Control.get_Input().get_Mouse().get_Position();
			}
			else
			{
				Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2, true);
				SpawnPoint = new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2);
			}
			((Control)this).set_Location(new Point(SpawnPoint.X - radius - thingIconSize / 2, SpawnPoint.Y - radius - thingIconSize / 2));
		}

		private async Task HandleHidden(object sender, EventArgs e)
		{
			Logger.Debug("HandleHidden entered");
			if (IsActionCamToggledOnMount)
			{
				await Helper.TriggerKeybind(Module._settingMountRadialToggleActionCameraKeyBinding);
				IsActionCamToggledOnMount = false;
				Logger.Debug("HandleHidden turned back on action cam");
			}
			await TriggerSelectedMountAsync();
		}
	}
}
