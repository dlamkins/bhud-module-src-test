using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Stream_Out.UI.Models;
using Nekres.Stream_Out.UI.Presenters;

namespace Nekres.Stream_Out.UI.Views
{
	public class CustomSettingsView : View<CustomSettingsPresenter>
	{
		private FlowPanel _settingFlowPanel;

		private bool _lockBounds = true;

		private ViewContainer _lastSettingContainer;

		private FlowPanel _socialFlowPanel;

		public bool LockBounds
		{
			get
			{
				return _lockBounds;
			}
			set
			{
				if (_lockBounds != value)
				{
					_lockBounds = value;
					UpdateBoundsLocking(_lockBounds);
				}
			}
		}

		public event EventHandler<EventArgs> SocialButtonClicked;

		private void UpdateBoundsLocking(bool locked)
		{
			if (_settingFlowPanel != null)
			{
				_settingFlowPanel.ShowBorder = !locked;
				_settingFlowPanel.CanCollapse = !locked;
			}
		}

		public CustomSettingsView()
		{
		}

		public CustomSettingsView(CustomSettingsModel model)
		{
			WithPresenter(new CustomSettingsPresenter(this, model));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			_socialFlowPanel = new FlowPanel
			{
				Size = new Point(buildPanel.Width, 78),
				Location = new Point(0, 0),
				FlowDirection = ControlFlowDirection.SingleRightToLeft,
				ControlPadding = new Vector2(27f, 2f),
				OuterControlPadding = new Vector2(27f, 2f),
				WidthSizingMode = SizingMode.Fill,
				ShowBorder = true,
				Parent = buildPanel
			};
			foreach (CustomSettingsModel.Social social in Enum.GetValues(typeof(CustomSettingsModel.Social)))
			{
				Texture2D tex = base.Presenter.Model.GetSocialLogo(social);
				Image obj = new Image
				{
					Parent = _socialFlowPanel,
					Texture = tex
				};
				Rectangle bounds = tex.get_Bounds();
				obj.Size = PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), 54, _socialFlowPanel.Height - (int)_socialFlowPanel.ControlPadding.Y * 2);
				obj.BasicTooltipText = base.Presenter.Model.GetSocialUrl(social);
				obj.Click += _bttn_Click;
				obj.MouseEntered += _bttn_MouseEntered;
				obj.MouseLeft += _bttn_MouseLeft;
			}
			_settingFlowPanel = new FlowPanel
			{
				Size = new Point(buildPanel.Width, buildPanel.Height - _socialFlowPanel.Height),
				Location = new Point(0, _socialFlowPanel.Height),
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(5f, 2f),
				OuterControlPadding = new Vector2(10f, 15f),
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(0, 15),
				Parent = buildPanel
			};
			foreach (SettingEntry item in base.Presenter.Model.Settings.Where((SettingEntry s) => s.SessionDefined))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(item, _settingFlowPanel.Width)) != null)
				{
					_lastSettingContainer = new ViewContainer
					{
						WidthSizingMode = SizingMode.Fill,
						HeightSizingMode = SizingMode.AutoSize,
						Parent = _settingFlowPanel
					};
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = settingView as SettingsView;
					if (subSettingsView != null)
					{
						subSettingsView.LockBounds = false;
					}
				}
			}
		}

		private void _bttn_Click(object sender, MouseEventArgs e)
		{
			this.SocialButtonClicked?.Invoke(sender, e);
		}

		private void _bttn_MouseEntered(object sender, MouseEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Image)sender).Tint = Color.get_Gray();
		}

		private void _bttn_MouseLeft(object sender, MouseEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Image)sender).Tint = Color.get_White();
		}

		protected override void Unload()
		{
			foreach (Control child in _socialFlowPanel.Children)
			{
				child.Click -= _bttn_Click;
				child.MouseEntered -= _bttn_MouseEntered;
				child.MouseLeft -= _bttn_MouseLeft;
			}
		}
	}
}
