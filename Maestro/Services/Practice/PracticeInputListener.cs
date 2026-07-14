using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Maestro.Services.Playback;
using Maestro.Settings;
using Maestro.UI.Main;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services.Practice
{
	public class PracticeInputListener
	{
		private struct LaneBinding
		{
			public SettingEntry<KeyBinding> Entry;

			public int Lane;

			public bool IsSharp;
		}

		private static readonly Logger Logger = Logger.GetLogger<PracticeInputListener>();

		private readonly KeyboardService _keyboard;

		private readonly LaneBinding[] _bindings;

		private PracticeSession _session;

		private bool _subscribed;

		private const int SharpCount = 5;

		private static readonly int[] SharpLaneBySlot = new int[5] { 1, 2, 4, 5, 6 };

		public PracticeInputListener(KeyboardService keyboard, ModuleSettings settings)
		{
			_keyboard = keyboard;
			_bindings = new LaneBinding[13]
			{
				Bind(settings.SharpC, 1, isSharp: true),
				Bind(settings.SharpD, 2, isSharp: true),
				Bind(settings.SharpF, 4, isSharp: true),
				Bind(settings.SharpG, 5, isSharp: true),
				Bind(settings.SharpA, 6, isSharp: true),
				Bind(settings.NoteC, 1, isSharp: false),
				Bind(settings.NoteD, 2, isSharp: false),
				Bind(settings.NoteE, 3, isSharp: false),
				Bind(settings.NoteF, 4, isSharp: false),
				Bind(settings.NoteG, 5, isSharp: false),
				Bind(settings.NoteA, 6, isSharp: false),
				Bind(settings.NoteB, 7, isSharp: false),
				Bind(settings.NoteCHigh, 8, isSharp: false)
			};
		}

		private static LaneBinding Bind(SettingEntry<KeyBinding> entry, int lane, bool isSharp)
		{
			LaneBinding result = default(LaneBinding);
			result.Entry = entry;
			result.Lane = lane;
			result.IsSharp = isSharp;
			return result;
		}

		public void Attach(PracticeSession session)
		{
			_session = session;
			if (!_subscribed)
			{
				GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
				_subscribed = true;
			}
		}

		public void Detach()
		{
			if (_subscribed)
			{
				GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
				_subscribed = false;
				_session = null;
			}
		}

		private void OnKeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			if (_session == null || _keyboard.WasJustSent(e.get_Key()) || SongFilterBar.IsTextInputFocused || GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				return;
			}
			ModifierKeys modifiers = KeysUtil.ModifiersFromKeys((IEnumerable<Keys>)GameService.Input.get_Keyboard().get_KeysDown());
			if (((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				for (int slot = 0; slot < SharpLaneBySlot.Length; slot++)
				{
					KeyBinding binding4 = _bindings[5 + slot].Entry?.get_Value();
					if (binding4 != null && (int)binding4.get_PrimaryKey() != 0 && binding4.get_PrimaryKey() == e.get_Key() && (ModifierKeys)(binding4.get_ModifierKeys() | 2) == modifiers)
					{
						int sharpLane = SharpLaneBySlot[slot];
						Logger.Debug($"Practice press {e.get_Key()} (+{modifiers}) -> lane {sharpLane}# (sharp slot {slot + 1})");
						_session.OnPlayerNotePressed(sharpLane, isSharp: true, _session.Clock.CurrentMs);
						return;
					}
				}
			}
			LaneBinding[] bindings = _bindings;
			for (int i = 0; i < bindings.Length; i++)
			{
				LaneBinding b = bindings[i];
				KeyBinding binding = b.Entry?.get_Value();
				if (binding != null && (int)binding.get_PrimaryKey() != 0 && binding.get_PrimaryKey() == e.get_Key() && binding.get_ModifierKeys() == modifiers)
				{
					Logger.Debug(string.Format("Practice press {0} (+{1}) -> lane {2}{3}", e.get_Key(), modifiers, b.Lane, b.IsSharp ? "#" : ""));
					_session.OnPlayerNotePressed(b.Lane, b.IsSharp, _session.Clock.CurrentMs);
					return;
				}
			}
			bindings = _bindings;
			for (int i = 0; i < bindings.Length; i++)
			{
				LaneBinding b2 = bindings[i];
				KeyBinding binding2 = b2.Entry?.get_Value();
				if (binding2 != null && (int)binding2.get_PrimaryKey() != 0 && binding2.get_PrimaryKey() == e.get_Key() && (ModifierKeys)(binding2.get_ModifierKeys() & modifiers) == binding2.get_ModifierKeys())
				{
					Logger.Debug(string.Format("Practice press {0} (+{1}) ~> lane {2}{3} (tolerant)", e.get_Key(), modifiers, b2.Lane, b2.IsSharp ? "#" : ""));
					_session.OnPlayerNotePressed(b2.Lane, b2.IsSharp, _session.Clock.CurrentMs);
					return;
				}
			}
			bindings = _bindings;
			for (int i = 0; i < bindings.Length; i++)
			{
				LaneBinding b3 = bindings[i];
				KeyBinding binding3 = b3.Entry?.get_Value();
				if (binding3 != null && binding3.get_PrimaryKey() == e.get_Key())
				{
					Logger.Info(string.Format("Practice press {0} (+{1}) matched no binding (candidate lane {2}{3} wants +{4})", e.get_Key(), modifiers, b3.Lane, b3.IsSharp ? "#" : "", binding3.get_ModifierKeys()));
					break;
				}
			}
		}
	}
}
