using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Overview.Loading
{
	public class LoadingStatusView : View
	{
		public Label _nameLabel;

		public Label _dateLabel;

		public LoadingSpinner _loadingSpinner;

		public Image _statusImage;

		private readonly IList<IRecurringService> _recurringServices;

		public LoadingStatusView(IList<IRecurringService> recurringServices)
		{
			_recurringServices = recurringServices;
			WithPresenter(new LoadingStatusPresenter(this, recurringServices));
		}

		protected override void Build(Container buildPanel)
		{
			_statusImage = new Image
			{
				Parent = buildPanel,
				Size = new Point(16, 16),
				Tint = Color.White,
				Texture = ServiceContainer.TextureRepository.GetRefTexture("157330-cantint.png"),
				Location = new Point(10, 12),
				Visible = true
			};
			_nameLabel = new Label
			{
				Parent = buildPanel,
				Text = "GW2 API",
				Location = new Point(30, 10),
				Size = new Point(200, 20),
				ShowShadow = true
			};
			_dateLabel = new Label
			{
				Parent = buildPanel,
				Location = new Point(90, 10),
				TextColor = Color.White * 0.7f,
				Size = new Point(200, 20),
				ShowShadow = true,
				Visible = false
			};
			_loadingSpinner = new LoadingSpinner
			{
				Parent = buildPanel,
				Size = new Point(25, 25),
				Location = new Point(140, 7),
				Visible = false
			};
		}
	}
}
