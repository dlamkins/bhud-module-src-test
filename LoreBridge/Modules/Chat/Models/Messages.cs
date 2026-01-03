using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LoreBridge.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Chat.Models
{
	public sealed class Messages
	{
		[CompilerGenerated]
		private Settings _003Csettings_003EP;

		private readonly Dictionary<string, Color> _colorPairs;

		private readonly Random _random;

		private SortedList<ulong, Message> Value { get; }

		public event EventHandler<Message> Added;

		public event EventHandler<SortedList<ulong, Message>> Updated;

		public event EventHandler Cleared;

		public Messages(Settings settings)
		{
			_003Csettings_003EP = settings;
			_colorPairs = new Dictionary<string, Color>();
			_random = new Random();
			Value = new SortedList<ulong, Message>();
			base._002Ector();
		}

		public void Add(Message message)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(message.Name))
			{
				message.NameColor = GetNameColor(message.Name);
			}
			Value.Add(message.TimeStamp, message);
			ulong lastKey = Value.Keys[Value.Count - 1];
			if (message.TimeStamp == lastKey)
			{
				this.Added?.Invoke(this, message);
			}
			else
			{
				this.Updated?.Invoke(this, Value);
			}
		}

		public void Clear()
		{
			Value.Clear();
			this.Cleared?.Invoke(this, null);
		}

		private Color GetNameColor(string name)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			if (!_003Csettings_003EP.WindowColoredNames.get_Value() || string.IsNullOrEmpty(name))
			{
				return Color.get_LimeGreen();
			}
			if (_colorPairs.TryGetValue(name, out var pair))
			{
				return pair;
			}
			Color color = default(Color);
			((Color)(ref color))._002Ector(_random.Next(100, 255), _random.Next(100, 255), _random.Next(100, 255));
			_colorPairs.Add(name, color);
			return color;
		}
	}
}
