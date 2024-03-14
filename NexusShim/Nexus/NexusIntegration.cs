using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using Blish_HUD;

namespace NexusShim.Nexus
{
	internal class NexusIntegration
	{
		private static readonly Logger Logger = Logger.GetLogger<NexusShim>();

		private const string NEXUS_MM_NAME = "DL_NEXUS_LINK_{0}";

		private MemoryMappedFile _file;

		private MemoryMappedViewAccessor _accessor;

		private string _nexusLinkName;

		private NexusLinkData _rawMem;

		public bool Active { get; private set; }

		public uint IconQuantity
		{
			get
			{
				if (!Active)
				{
					return 0u;
				}
				return _rawMem.AmountIcons;
			}
		}

		public IconMode IconMode
		{
			get
			{
				if (!Active)
				{
					return IconMode.Custom;
				}
				return (IconMode)_rawMem.Mode;
			}
		}

		public bool IconVertical
		{
			get
			{
				if (!Active)
				{
					return false;
				}
				return _rawMem.IsVerticalLayout;
			}
		}

		public void TryOpen()
		{
			try
			{
				if (GameService.GameIntegration.get_Gw2Instance().get_Gw2Process() != null)
				{
					_nexusLinkName = $"DL_NEXUS_LINK_{GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().Id}";
					_file = MemoryMappedFile.OpenExisting(_nexusLinkName, MemoryMappedFileRights.Read);
					_accessor = _file.CreateViewAccessor(0L, 49L, MemoryMappedFileAccess.Read);
					Active = true;
					Logger.Info("Successfully read Nexus' shared memory (" + _nexusLinkName + ").");
				}
			}
			catch (FileNotFoundException)
			{
				Active = false;
			}
			catch (Exception)
			{
				Active = false;
			}
		}

		public void Update()
		{
			if (Active)
			{
				try
				{
					_accessor.Read<NexusLinkData>(40L, out _rawMem);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed read Nexus memory (" + _nexusLinkName + ").");
					Active = false;
				}
			}
		}

		public void Unload()
		{
			_accessor?.Dispose();
			_file?.Dispose();
		}
	}
}
