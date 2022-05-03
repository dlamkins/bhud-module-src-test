using System;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.UI.Controls;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public class UiStates : ManagedState
	{
		private InfoWindow _info;

		private Label _infoLabel;

		private SafeList<string> _infoList;

		public FlatMap Map { get; private set; }

		public SmallInteract Interact { get; private set; }

		public UiStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		public override async Task Reload()
		{
			await Unload();
			await Initialize();
		}

		private void InitMap()
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)MapOpenedChanged);
			if (Map == null)
			{
				FlatMap flatMap = new FlatMap(_rootPackState);
				((Control)flatMap).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				Map = flatMap;
			}
		}

		private void UpdateMapState()
		{
			if (Map != null)
			{
				((Control)Map).set_Visible(!(GameService.Gw2Mumble.get_UI().get_IsMapOpen() ? _rootPackState.UserResourceStates.Ignore.Map : _rootPackState.UserResourceStates.Ignore.Compass).Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			}
		}

		private void MapOpenedChanged(object sender, ValueEventArgs<bool> e)
		{
			UpdateMapState();
		}

		private void CurrentMapChanged(object sender, ValueEventArgs<int> e)
		{
			UpdateMapState();
		}

		private void InitInfo()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			if (_info == null)
			{
				InfoWindow infoWindow = new InfoWindow(_rootPackState);
				((Control)infoWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)infoWindow).set_Visible(false);
				_info = infoWindow;
				_info.Hide(withFade: false);
				Label val = new Label();
				((Control)val).set_Width(350);
				val.set_WrapText(true);
				val.set_AutoSizeHeight(true);
				((Control)val).set_Location(new Point(70, 60));
				val.set_VerticalAlignment((VerticalAlignment)1);
				val.set_HorizontalAlignment((HorizontalAlignment)0);
				val.set_Font(GameService.Content.get_DefaultFont18());
				((Control)val).set_Parent((Container)(object)_info);
				_infoLabel = val;
				_infoList = new SafeList<string>();
			}
		}

		private void UpdateInfoText()
		{
			string currentInfo = _infoList.ToList().LastOrDefault() ?? string.Empty;
			if (string.IsNullOrEmpty(currentInfo))
			{
				_info.Hide(withFade: true);
				return;
			}
			_infoLabel.set_Text(currentInfo.Replace(" ", "  "));
			((Control)_info).Show();
		}

		public void AddInfoString(string info)
		{
			_infoList.Add(info);
			UpdateInfoText();
		}

		public void RemoveInfoString(string info)
		{
			_infoList.Remove(info);
			UpdateInfoText();
		}

		private void InitInteract()
		{
			SmallInteract smallInteract = new SmallInteract(_rootPackState);
			((Control)smallInteract).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			Interact = smallInteract;
		}

		protected override Task<bool> Initialize()
		{
			InitInfo();
			InitMap();
			InitInteract();
			return Task.FromResult(result: true);
		}

		public override Task Unload()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)MapOpenedChanged);
			InfoWindow info = _info;
			if (info != null)
			{
				((Control)info).Hide();
			}
			_infoList.ToList().ForEach(RemoveInfoString);
			_infoList.Clear();
			SmallInteract interact = Interact;
			if (interact != null)
			{
				((Control)interact).Dispose();
			}
			return Task.CompletedTask;
		}

		public override void Update(GameTime gameTime)
		{
			if (!_rootPackState.UserConfiguration.GlobalPathablesEnabled.get_Value())
			{
				InfoWindow info = _info;
				if (info != null)
				{
					((Control)info).Hide();
				}
			}
		}
	}
}
