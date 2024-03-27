using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Nekres.FailScreens.Core.UI.Controls.Screens;

namespace Nekres.FailScreens.Core.Services
{
	internal class DefeatedService : IDisposable
	{
		public enum FailScreens
		{
			DarkSouls,
			GrandTheftAuto,
			RytlocksCritterRampage,
			Windows,
			AngryPepe
		}

		private Control _failScreen;

		public DefeatedService()
		{
			FailScreensModule.Instance.State.StateChanged += OnStateChanged;
		}

		private void OnStateChanged(object sender, ValueEventArgs<StateService.State> e)
		{
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			if (e.get_Value() != StateService.State.Defeated)
			{
				Control failScreen = _failScreen;
				if (failScreen != null)
				{
					failScreen.Dispose();
				}
				return;
			}
			FailScreens screen = FailScreensModule.Instance.FailScreen.get_Value();
			if (FailScreensModule.Instance.Random.get_Value())
			{
				int num = Enum.GetValues(typeof(FailScreens)).Cast<int>().Min();
				int max = Enum.GetValues(typeof(FailScreens)).Cast<int>().Max();
				screen = (FailScreens)RandomUtil.GetRandom(num, max);
			}
			Control buildScreen = CreateFailScreen(screen);
			if (buildScreen != null)
			{
				buildScreen.set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				buildScreen.set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
				_failScreen = buildScreen;
			}
		}

		private Control CreateFailScreen(FailScreens failScreen)
		{
			return (Control)(failScreen switch
			{
				FailScreens.DarkSouls => new DarkSouls(), 
				FailScreens.GrandTheftAuto => new GrantTheftAuto(), 
				FailScreens.RytlocksCritterRampage => new RytlocksCritterRampage(), 
				FailScreens.Windows => new WinXp(), 
				FailScreens.AngryPepe => new AngryPepe(), 
				_ => null, 
			});
		}

		public void Dispose()
		{
			FailScreensModule.Instance.State.StateChanged -= OnStateChanged;
			Control failScreen = _failScreen;
			if (failScreen != null)
			{
				failScreen.Dispose();
			}
		}
	}
}
