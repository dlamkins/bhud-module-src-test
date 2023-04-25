using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	internal class DrawRadial : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<DrawRadial>();

		private readonly Helper _helper;

		private readonly TextureCache _textureCache;

		private StandardButton _settingsButton;

		private Label _noMountsLabel;

		private List<RadialMount> RadialMounts = new List<RadialMount>();

		private int radius;

		private int mountIconSize;

		private int _maxRadialDiameter;

		private Point SpawnPoint;

		public EventHandler OnSettingsButtonClicked { get; internal set; }

		private RadialMount SelectedMount => RadialMounts.SingleOrDefault((RadialMount m) => m.Selected);

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
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
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
			val.set_Text("NO MOUNTS CONFIGURED, GO TO SETTINGS: ");
			_noMountsLabel = val;
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
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			RadialMounts.Clear();
			List<Mount> mounts = Module._availableOrderedMounts;
			if (!mounts.Any())
			{
				((Control)_noMountsLabel).Show();
				((Control)_settingsButton).Show();
				return;
			}
			((Control)_noMountsLabel).Hide();
			((Control)_settingsButton).Hide();
			Mount mountToPutInCenter = _helper.GetCenterMount();
			if (mountToPutInCenter != null && mountToPutInCenter.IsAvailable)
			{
				if (Module._settingMountRadialRemoveCenterMount.get_Value())
				{
					mounts.Remove(mountToPutInCenter);
				}
				Texture2D texture2 = _textureCache.GetMountImgFile(mountToPutInCenter.ImageFileName);
				int loc = radius;
				RadialMounts.Add(new RadialMount
				{
					Texture = texture2,
					Mount = mountToPutInCenter,
					ImageX = loc,
					ImageY = loc,
					Default = true
				});
			}
			double startAngle = Math.PI * Math.Floor(Module._settingMountRadialStartAngle.get_Value() * 360f) / 180.0;
			double currentAngle = startAngle;
			double partAngleStep = Math.PI * 2.0 / (double)mounts.Count();
			foreach (Mount mount in mounts)
			{
				double angleMid = currentAngle + partAngleStep / 2.0;
				double angleEnd = currentAngle + partAngleStep;
				Texture2D texture = _textureCache.GetMountImgFile(mount.ImageFileName);
				int x = (int)Math.Round((double)radius + (double)radius * Math.Cos(angleMid));
				int y = (int)Math.Round((double)radius + (double)radius * Math.Sin(angleMid));
				RadialMounts.Add(new RadialMount
				{
					Texture = texture,
					Mount = mount,
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
			foreach (RadialMount radialMount in RadialMounts)
			{
				if ((double)length < (double)mountIconSize * Math.Sqrt(2.0) / 2.0)
				{
					radialMount.Selected = radialMount.Default;
				}
				else
				{
					radialMount.Selected = radialMount.AngleBegin <= angle && radialMount.AngleEnd > angle;
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, radialMount.Texture, new Rectangle(radialMount.ImageX, radialMount.ImageY, mountIconSize, mountIconSize), (Rectangle?)null, Color.get_White() * (radialMount.Selected ? 1f : Module._settingMountRadialIconOpacity.get_Value()));
			}
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		private void DrawDbg(SpriteBatch spriteBatch, int position, string s)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, s, GameService.Content.get_DefaultFont32(), new Rectangle(new Point(0, position), new Point(400, 400)), Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		public async Task TriggerSelectedMountAsync()
		{
			await (SelectedMount?.Mount.DoMountAction() ?? Task.CompletedTask);
		}

		private async Task HandleShown(object sender, EventArgs e)
		{
			Logger.Debug("HandleShown entered");
			if (!GameService.Input.get_Mouse().get_CursorIsVisible() && !((SettingEntry)Module._settingMountRadialToggleActionCameraKeyBinding).get_IsNull())
			{
				IsActionCamToggledOnMount = true;
				await _helper.TriggerKeybind(Module._settingMountRadialToggleActionCameraKeyBinding);
				Logger.Debug("HandleShown turned off action cam");
			}
			_maxRadialDiameter = Math.Min(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			mountIconSize = (int)((float)(_maxRadialDiameter / 4) * Module._settingMountRadialIconSizeModifier.get_Value());
			radius = (int)((float)(_maxRadialDiameter / 2 - mountIconSize / 2) * Module._settingMountRadialRadiusModifier.get_Value());
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
			((Control)this).set_Location(new Point(SpawnPoint.X - radius - mountIconSize / 2, SpawnPoint.Y - radius - mountIconSize / 2));
		}

		private async Task HandleHidden(object sender, EventArgs e)
		{
			Logger.Debug("HandleHidden entered");
			if (IsActionCamToggledOnMount)
			{
				await _helper.TriggerKeybind(Module._settingMountRadialToggleActionCameraKeyBinding);
				IsActionCamToggledOnMount = false;
				Logger.Debug("HandleHidden turned back on action cam");
			}
			await TriggerSelectedMountAsync();
		}
	}
}
