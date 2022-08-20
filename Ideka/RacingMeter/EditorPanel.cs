using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ideka.RacingMeter
{
	public class EditorPanel : Panel, IPanelOverride
	{
		private static readonly Logger Logger = Logger.GetLogger<EditorPanel>();

		private const int Spacing = 10;

		private readonly RaceEditor _editor;

		private readonly StandardButton _backButton;

		private readonly StandardButton _newButton;

		private readonly StandardButton _importCsvButton;

		private readonly StandardButton _deleteButton;

		private readonly RacePointsEditPanel _pointsPanel;

		private readonly RaceMetaEditPanel _metaPanel;

		private readonly RacePointEditPanel _pointPanel;

		private readonly StandardButton _undoButton;

		private readonly StandardButton _redoButton;

		private readonly StandardButton _saveButton;

		private readonly StandardButton _testButton;

		private readonly StandardButton _testFromButton;

		public Panel Panel => (Panel)(object)this;

		public Texture2D Icon { get; } = RacingModule.ContentsManager.GetTexture("EditIcon.png");


		public string Caption => Strings.RaceEditor;

		public EditorPanel(RaceEditor editor)
			: this()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Expected O, but got Unknown
			_editor = editor;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.BackToRacing);
			_backButton = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Strings.NewRace);
			_newButton = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Strings.ImportCsv);
			_importCsvButton = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(Strings.DeleteRace);
			_deleteButton = val4;
			RacePointsEditPanel racePointsEditPanel = new RacePointsEditPanel(_editor);
			((Control)racePointsEditPanel).set_Parent((Container)(object)this);
			_pointsPanel = racePointsEditPanel;
			RaceMetaEditPanel raceMetaEditPanel = new RaceMetaEditPanel(_editor);
			((Control)raceMetaEditPanel).set_Parent((Container)(object)this);
			_metaPanel = raceMetaEditPanel;
			RacePointEditPanel racePointEditPanel = new RacePointEditPanel(_editor);
			((Control)racePointEditPanel).set_Parent((Container)(object)this);
			_pointPanel = racePointEditPanel;
			KeyboundButton keyboundButton = new KeyboundButton(new KeyBinding((ModifierKeys)1, (Keys)90));
			((Control)keyboundButton).set_Parent((Container)(object)this);
			((StandardButton)keyboundButton).set_Text(Strings.Undo);
			_undoButton = (StandardButton)(object)keyboundButton;
			KeyboundButton keyboundButton2 = new KeyboundButton(new KeyBinding((ModifierKeys)1, (Keys)89));
			((Control)keyboundButton2).set_Parent((Container)(object)this);
			((StandardButton)keyboundButton2).set_Text(Strings.Redo);
			_redoButton = (StandardButton)(object)keyboundButton2;
			KeyboundButton keyboundButton3 = new KeyboundButton(new KeyBinding((ModifierKeys)1, (Keys)83));
			((Control)keyboundButton3).set_Parent((Container)(object)this);
			((StandardButton)keyboundButton3).set_Text(Strings.Save);
			_saveButton = (StandardButton)(object)keyboundButton3;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(Strings.TestRace);
			_testButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.TestFromHere);
			_testFromButton = val6;
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (await ConfirmDiscard(Strings.BackToRacing))
				{
					_editor.DiscardChanges();
					RacingModule.Racer.EditMode = false;
				}
			});
			((Control)_newButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (await ConfirmDiscard(Strings.NewRace))
				{
					_editor.DiscardChanges();
					RacingModule.Racer.FullRace = DataExtensions.NewRace();
				}
			});
			((Control)_importCsvButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				bool flag = _editor.FullRace != null;
				if (flag)
				{
					flag = !(await RacingModule.ConfirmationModal.ShowAsync(Strings.AreYouSure, Strings.CsvImportWarning, Strings.ImportCsv, Strings.Cancel));
				}
				if (!flag)
				{
					using AsyncFileDialog<OpenFileDialog> ofd = new AsyncFileDialog<OpenFileDialog>();
					ofd.Dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
					if (await ofd.Show() == DialogResult.OK)
					{
						try
						{
							Race intermediate = Race.FromCsvFile(ofd.Dialog.FileName);
							FullRace target = _editor.FullRace ?? DataExtensions.NewRace();
							_editor.DiscardChanges();
							target.Race.ImportPointsFrom(intermediate);
							RacingModule.Racer.FullRace = target;
						}
						catch (Exception e)
						{
							FriendlyError.Report(Logger, Strings.ErrorRaceImport, e);
						}
					}
				}
			});
			((Control)_deleteButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (await RacingModule.ConfirmationModal.ShowAsync(Strings.AreYouSure, Strings.ConfirmRaceDeletion, Strings.DeleteRace, Strings.Cancel))
				{
					_editor.DeleteRace();
				}
			});
			((Control)_undoButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.Undo();
			});
			((Control)_redoButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.Redo();
			});
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.Save();
			});
			((Control)_testButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.Test();
			});
			((Control)_testFromButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.Test(null);
			});
			_editor.RaceLoaded += RaceLoaded;
			_editor.CanUndoRedoChanged += CanUndoRedoChanged;
			_editor.IsDirtyChanged += IsDirtyChanged;
			_editor.PointSelected += PointSelected;
			CanUndoRedoChanged(_editor.CanUndo, _editor.CanRedo);
			IsDirtyChanged(_editor.IsDirty);
			PointSelected(_editor.Selected);
			UpdateLayout();
		}

		private async Task<bool> ConfirmDiscard(string accept)
		{
			bool flag = !_editor.IsDirty;
			if (!flag)
			{
				flag = await RacingModule.ConfirmationModal.ShowAsync(Strings.AreYouSure, Strings.UnsavedChanges, accept, Strings.Cancel);
			}
			return flag;
		}

		private void RaceLoaded(FullRace _)
		{
			StandardButton deleteButton = _deleteButton;
			StandardButton saveButton = _saveButton;
			bool flag;
			((Control)_testButton).set_Enabled(flag = _editor.Race != null);
			bool enabled;
			((Control)saveButton).set_Enabled(enabled = flag);
			((Control)deleteButton).set_Enabled(enabled);
		}

		private void CanUndoRedoChanged(bool canUndo, bool canRedo)
		{
			((Control)_undoButton).set_Enabled(canUndo);
			((Control)_redoButton).set_Enabled(canRedo);
		}

		private void IsDirtyChanged(bool isDirty)
		{
			_saveButton.set_Text(Strings.Save + (isDirty ? "*" : ""));
		}

		private void PointSelected(RacePoint point)
		{
			((Control)_testFromButton).set_Enabled(point != null);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			if (_editor != null)
			{
				((Control)_backButton).set_Location(Point.get_Zero());
				((Control)(object)_backButton).ArrangeLeftRight(10, (Control)_newButton, (Control)_importCsvButton);
				((Control)(object)_backButton).ArrangeTopDown(10, (Control)_pointsPanel);
				((Control)_pointsPanel).set_Width(200);
				((Control)(object)_pointsPanel).HeightFillDown();
				((Control)(object)_pointsPanel).ArrangeLeftRight(10, (Control)_metaPanel);
				((Control)(object)_metaPanel).ArrangeTopDown(10, (Control)_pointPanel);
				((Control)_deleteButton).set_Top(0);
				((Control)_deleteButton).set_Right(((Container)this).get_ContentRegion().Width);
				((Control)_saveButton).set_Right(((Container)this).get_ContentRegion().Width);
				((Control)_saveButton).set_Bottom(((Container)this).get_ContentRegion().Height);
				((Control)(object)_saveButton).ArrangeRightLeft(10, (Control)_redoButton, (Control)_undoButton);
				((Control)(object)_saveButton).ArrangeBottomUp(10, (Control)_testFromButton);
				((Control)(object)_testFromButton).ArrangeRightLeft(10, (Control)_testButton);
			}
		}

		protected override void DisposeControl()
		{
			_editor.RaceLoaded -= RaceLoaded;
			_editor.CanUndoRedoChanged -= CanUndoRedoChanged;
			_editor.IsDirtyChanged -= IsDirtyChanged;
			_editor.PointSelected -= PointSelected;
			StandardButton undoButton = _undoButton;
			if (undoButton != null)
			{
				((Control)undoButton).Dispose();
			}
			StandardButton redoButton = _redoButton;
			if (redoButton != null)
			{
				((Control)redoButton).Dispose();
			}
			StandardButton saveButton = _saveButton;
			if (saveButton != null)
			{
				((Control)saveButton).Dispose();
			}
			Texture2D icon = Icon;
			if (icon != null)
			{
				((GraphicsResource)icon).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
