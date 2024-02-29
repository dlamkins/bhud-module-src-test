using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery.Home
{
	public class HomeView : View<IHomePresenter>
	{
		private readonly IDiscoveryTabPresenter _discoveryTabPresenter;

		public ViewContainer Container { get; set; }

		public HomeViewModel Model { get; set; }

		public Container BuildPanel { get; set; }

		public EventHandler LightClick { get; set; }

		public EventHandler MediumClick { get; set; }

		public EventHandler HeavyClick { get; set; }

		public HomeView(HomeViewModel model, IDiscoveryTabPresenter discoveryPresenter)
		{
			Model = model;
			_discoveryTabPresenter = discoveryPresenter;
			WithPresenter(new HomePresenter(this, model));
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			Container = new ViewContainer
			{
				FadeView = true,
				Parent = buildPanel,
				Size = new Point(buildPanel.Width, buildPanel.Height),
				Padding = new Thickness(0f),
				Location = new Point(0, 0)
			};
			new Label
			{
				Parent = Container,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Location = new Point(150, 20),
				Font = GameService.Content.DefaultFont32,
				Text = "Welcome to the Mystic Crafting module."
			};
			new Label
			{
				Parent = Container,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Location = new Point(170, 70),
				TextColor = Color.LightYellow,
				Font = GameService.Content.DefaultFont16,
				Text = "Select one of the options below to start crafting your Obsidian Armor."
			};
			ImageButton obsidianButton1 = new ImageButton
			{
				Parent = Container,
				Texture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_1.png"),
				HoverTexture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_1_hover.png"),
				Size = new Point(260, 500),
				Location = new Point(0, 110)
			};
			obsidianButton1.Click += delegate(object s, MouseEventArgs e)
			{
				LightClick?.Invoke(s, e);
			};
			Label label = new Label();
			label.Parent = Container;
			label.AutoSizeWidth = true;
			label.AutoSizeHeight = true;
			label.Location = new Point(100, 620);
			label.Font = GameService.Content.DefaultFont32;
			label.TextColor = Color.LightYellow;
			label.Text = "Light";
			label.Click += delegate(object s, MouseEventArgs e)
			{
				LightClick?.Invoke(s, e);
			};
			ImageButton obsidianButton2 = new ImageButton
			{
				Parent = Container,
				Texture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_2.png"),
				HoverTexture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_2_hover.png"),
				Size = new Point(260, 500),
				Location = new Point(obsidianButton1.Right, 110)
			};
			obsidianButton2.Click += delegate(object s, MouseEventArgs e)
			{
				MediumClick?.Invoke(s, e);
			};
			Label label2 = new Label();
			label2.Parent = Container;
			label2.AutoSizeWidth = true;
			label2.AutoSizeHeight = true;
			label2.Location = new Point(340, 620);
			label2.Font = GameService.Content.DefaultFont32;
			label2.TextColor = Color.LightYellow;
			label2.Text = "Medium";
			label2.Click += delegate(object s, MouseEventArgs e)
			{
				MediumClick?.Invoke(s, e);
			};
			ImageButton imageButton = new ImageButton();
			imageButton.Parent = Container;
			imageButton.Texture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_3.png");
			imageButton.HoverTexture = ServiceContainer.TextureRepository.GetRefTexture("obsidian_3_hover.png");
			imageButton.Size = new Point(260, 500);
			imageButton.Location = new Point(obsidianButton2.Right, 110);
			imageButton.Click += delegate(object s, MouseEventArgs e)
			{
				HeavyClick?.Invoke(s, e);
			};
			Label label3 = new Label();
			label3.Parent = Container;
			label3.AutoSizeWidth = true;
			label3.AutoSizeHeight = true;
			label3.Location = new Point(620, 620);
			label3.Font = GameService.Content.DefaultFont32;
			label3.TextColor = Color.LightYellow;
			label3.Text = "Heavy";
			label3.Click += delegate(object s, MouseEventArgs e)
			{
				HeavyClick?.Invoke(s, e);
			};
		}
	}
}
