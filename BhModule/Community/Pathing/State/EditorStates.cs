using System;
using System.IO;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Input;
using LiteDB;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BhModule.Community.Pathing.State
{
	public class EditorStates : ManagedState
	{
		private const string DIFFDB_FILE = "changes.ddb";

		private const ModifierKeys EDITOR_MODIFIERKEY = 4;

		private const Keys EDITOR_KEYKEY = 161;

		private ILiteDatabase _editorDatabase;

		private bool _multiSelect;

		public SafeList<IPathingEntity> SelectedPathingEntities { get; private set; } = new SafeList<IPathingEntity>();


		public EditorStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		protected override Task<bool> Initialize()
		{
			string diffDbPath = Path.Combine(DataDirUtil.GetSafeDataDir("user"), "changes.ddb");
			_editorDatabase = new LiteDatabase("Filename=" + diffDbPath + ";Connection=shared");
			return Task.FromResult(result: true);
		}

		private void KeyboardOnKeyReleased(object sender, KeyboardEventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (((Enum)(Keys)161).HasFlag((Enum)(object)e.get_Key()))
			{
				_multiSelect = false;
			}
		}

		private void MouseOnLeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				if ((object)null == null)
				{
					SelectedPathingEntities.Clear();
				}
				_multiSelect = true;
			}
		}

		public override Task Reload()
		{
			SelectedPathingEntities = new SafeList<IPathingEntity>();
			return Task.CompletedTask;
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override Task Unload()
		{
			SelectedPathingEntities = new SafeList<IPathingEntity>();
			_editorDatabase?.Dispose();
			return Task.CompletedTask;
		}
	}
}
