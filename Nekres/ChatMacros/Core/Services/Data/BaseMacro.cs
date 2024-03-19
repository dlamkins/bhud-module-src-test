using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Input;
using LiteDB;
using Microsoft.Xna.Framework;

namespace Nekres.ChatMacros.Core.Services.Data
{
	internal abstract class BaseMacro
	{
		[BsonId(true)]
		public ObjectId Id { get; set; }

		[BsonField("title")]
		public string Title { get; set; }

		[BsonField("voice_commands")]
		public List<string> VoiceCommands { get; set; }

		[BsonField("key_binding")]
		public KeyBinding KeyBinding { get; set; }

		[BsonField("game_mode")]
		public GameMode GameModes { get; set; }

		[BsonField("map_ids")]
		public List<int> MapIds { get; set; }

		[BsonField("link_file")]
		public string LinkFile { get; set; }

		public event EventHandler<EventArgs> Triggered;

		protected BaseMacro()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			Title = string.Empty;
			KeyBinding val = new KeyBinding();
			val.set_Enabled(false);
			KeyBinding = val;
			MapIds = new List<int>();
			VoiceCommands = new List<string>();
			GameModes = GameMode.PvE | GameMode.WvW | GameMode.PvP;
		}

		public void Toggle(bool enable)
		{
			KeyBinding.set_Enabled(enable);
			if (enable)
			{
				KeyBinding.add_Activated((EventHandler<EventArgs>)OnKeyBindingActivated);
			}
			else
			{
				KeyBinding.remove_Activated((EventHandler<EventArgs>)OnKeyBindingActivated);
			}
		}

		private void OnKeyBindingActivated(object sender, EventArgs e)
		{
			this.Triggered?.Invoke(this, EventArgs.Empty);
		}

		public bool HasGameMode(GameMode mode)
		{
			return (GameModes & mode) == mode;
		}

		public bool HasMapId(int id)
		{
			List<int> mapIds = MapIds;
			if (mapIds != null && mapIds.Any())
			{
				return MapIds.Contains(id);
			}
			return true;
		}

		public virtual Color GetDisplayColor()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Color.get_White();
		}

		public static string[] GetCommands<T>(IEnumerable<T> macros) where T : BaseMacro
		{
			return macros?.Where((T x) => x.VoiceCommands != null).SelectMany((T x) => x.VoiceCommands).ToArray() ?? Array.Empty<string>();
		}
	}
}
