using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class CommandList
	{
		private readonly struct EventReport : IDisposable
		{
			private readonly CommandList _commandList;

			private readonly bool _couldUndo;

			private readonly bool _couldRedo;

			private readonly bool _wasDirty;

			public EventReport(CommandList commandList)
			{
				_commandList = commandList;
				_couldUndo = _commandList.CanUndo;
				_couldRedo = _commandList.CanRedo;
				_wasDirty = _commandList.IsDirty;
			}

			public void Dispose()
			{
				bool canUndo = _commandList.CanUndo;
				bool canRedo = _commandList.CanRedo;
				if (canUndo != _couldUndo || canRedo != _couldRedo)
				{
					_commandList.CanUndoRedoChanged?.Invoke(canUndo, canRedo);
				}
				bool isDirty = _commandList.IsDirty;
				if (isDirty != _wasDirty)
				{
					_commandList.IsDirtyChanged?.Invoke(isDirty);
				}
			}
		}

		private readonly List<IEditorCommand> _commands;

		private int _index;

		private int _saved;

		public bool CanUndo => _index >= 0;

		public bool CanRedo => _index < _commands.Count - 1;

		public bool IsDirty
		{
			get
			{
				if (_saved < -1 || _saved > _index)
				{
					return true;
				}
				if (_index == _saved || !_commands.Any())
				{
					return false;
				}
				for (int i = _saved + 1; i <= _index; i++)
				{
					if (_commands[i].Modifying)
					{
						return true;
					}
				}
				return false;
			}
		}

		public EditState State { get; }

		public event Action<bool>? IsDirtyChanged;

		public event Action<bool, bool>? CanUndoRedoChanged;

		public CommandList(EditState state)
		{
			State = state;
			_commands = new List<IEditorCommand>();
			_index = -1;
			_saved = -1;
			base._002Ector();
		}

		public void Clear()
		{
			using (new EventReport(this))
			{
				_commands.Clear();
				_index = -1;
				_saved = -1;
			}
		}

		public void Run(IEditorCommand command)
		{
			try
			{
				if (!command.Do(State))
				{
					return;
				}
			}
			catch (CommandException ex)
			{
				ScreenNotification.ShowNotification(ex.Message, (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			using (new EventReport(this))
			{
				_commands.RemoveRange(_index + 1, _commands.Count - (_index + 1));
				_commands.Add(command);
				_index++;
				if (_saved >= _index)
				{
					_saved = -2;
				}
			}
		}

		public void Saved()
		{
			using (new EventReport(this))
			{
				_saved = -1;
				foreach (var item in _commands.Take(_index + 1).Enumerate())
				{
					var (i, _) = item;
					if (item.item.Modifying)
					{
						_saved = i;
					}
				}
			}
		}

		public bool Undo()
		{
			if (!CanUndo)
			{
				return false;
			}
			using (new EventReport(this))
			{
				_commands[_index--].Undo(State);
				return true;
			}
		}

		public bool Redo()
		{
			if (!CanRedo)
			{
				return false;
			}
			using (new EventReport(this))
			{
				_commands[++_index].Do(State);
				return true;
			}
		}
	}
}
