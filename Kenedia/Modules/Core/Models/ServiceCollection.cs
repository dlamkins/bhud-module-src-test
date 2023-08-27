using System;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Models
{
	public class ServiceCollection : IDisposable
	{
		private bool _isDisposed;

		public GameStateDetectionService GameStateDetectionService { get; }

		public ClientWindowService ClientWindowService { get; }

		public SharedSettings SharedSettings { get; }

		public TexturesService TexturesService { get; }

		public InputDetectionService InputDetectionService { get; }

		public ServiceCollection(GameStateDetectionService gameState, ClientWindowService clientWindowService, SharedSettings sharedSettings, TexturesService texturesService, InputDetectionService inputDetectionService)
		{
			GameStateDetectionService = gameState;
			ClientWindowService = clientWindowService;
			SharedSettings = sharedSettings;
			TexturesService = texturesService;
			InputDetectionService = inputDetectionService;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				GameStateDetectionService.Dispose();
				TexturesService.Dispose();
			}
		}
	}
}
