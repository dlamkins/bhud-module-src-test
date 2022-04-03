using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Editor.Entity;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Entities;
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
			_editorDatabase = new LiteDatabase(diffDbPath);
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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			if (!((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				return;
			}
			Ray mouseRay = PickingUtil.CalculateRay(e.get_MousePosition(), GameService.Gw2Mumble.get_PlayerCamera().get_View(), GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			ICanPick pickedEntity = null;
			foreach (IEntity entity2 in from entity in GameService.Graphics.get_World().get_Entities()
				orderby entity.get_DrawOrder()
				select entity)
			{
				ICanPick pickEntity = entity2 as ICanPick;
				if (pickEntity == null || !pickEntity.RayIntersects(mouseRay))
				{
					continue;
				}
				pickedEntity = pickEntity;
				IPathingEntity pathable = entity2 as IPathingEntity;
				if (pathable == null)
				{
					IAxisHandle handle = entity2 as IAxisHandle;
					if (handle == null)
					{
						continue;
					}
					handle.HandleActivated(mouseRay);
					break;
				}
				if (SelectedPathingEntities.Contains(pathable))
				{
					SelectedPathingEntities.Remove(pathable);
					break;
				}
				if (_multiSelect)
				{
					SelectedPathingEntities.Add(pathable);
					break;
				}
				SelectedPathingEntities.SetRange(new IPathingEntity[1] { pathable });
				break;
			}
			if (pickedEntity == null)
			{
				SelectedPathingEntities.Clear();
			}
			_multiSelect = true;
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
