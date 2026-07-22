using System;
using System.Collections.ObjectModel;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Views
{
	public class CharacterRoutineWindow : Kenedia.Modules.Core.Views.StandardWindow
	{
		private readonly CharacterRoutineService _service;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _contentPanel;

		private readonly CharacterRoutineSidebar _sidebar;

		private readonly CharacterRoutineDetailPanel _detailPanel;

		private bool _created;

		public CharacterRoutineService Service => _service;

		public CharacterRoutineWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Settings settings, CharacterRoutineService service, TextureManager textureManager, CharacterSwapping characterSwapping, ObservableCollection<Character_Model> characterModels)
			: base(background, windowRegion, contentRegion)
		{
			_service = service;
			_contentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				ControlPadding = new Vector2(5f, 0f)
			};
			_sidebar = new CharacterRoutineSidebar(_service)
			{
				Parent = _contentPanel
			};
			_detailPanel = new CharacterRoutineDetailPanel(textureManager, _service, characterSwapping, settings, characterModels, contentRegion.Width - 210)
			{
				Parent = _contentPanel
			};
			_created = true;
		}

		public void SwitchToNextRoutineStep()
		{
			_service.SwitchToNextIncompleteRoutineStep();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			if (_created)
			{
				_detailPanel.Width = base.ContentRegion.Width - 210;
			}
			base.Width = Math.Max(base.Width, 530);
			base.Height = Math.Max(base.Height, 400);
		}

		protected override void DisposeControl()
		{
			_contentPanel?.Dispose();
			_sidebar?.Dispose();
			_detailPanel?.Dispose();
			base.DisposeControl();
		}
	}
}
