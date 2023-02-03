using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ideka.RacingMeter
{
	public class EditorPanel : Panel, IUIPanel
	{
		private static readonly Logger Logger = Logger.GetLogger<EditorPanel>();

		private const int Spacing = 10;

		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly PanelStack _panelStack;

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

		public EditorPanel(PanelStack panelStack, MeasurerRealtime measurer, FullRace? race)
			: this()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Expected O, but got Unknown
			_panelStack = panelStack;
			_editor = _dc.Add<RaceEditor>(new RaceEditor(measurer, race));
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
			DisposableCollection dc = _dc;
			KeyboundButton keyboundButton = new KeyboundButton(new KeyBinding((ModifierKeys)1, (Keys)90));
			((Control)keyboundButton).set_Parent((Container)(object)this);
			((StandardButton)keyboundButton).set_Text(Strings.Undo);
			_undoButton = (StandardButton)(object)dc.Add<KeyboundButton>(keyboundButton);
			DisposableCollection dc2 = _dc;
			KeyboundButton keyboundButton2 = new KeyboundButton(new KeyBinding((ModifierKeys)1, (Keys)89));
			((Control)keyboundButton2).set_Parent((Container)(object)this);
			((StandardButton)keyboundButton2).set_Text(Strings.Redo);
			_redoButton = (StandardButton)(object)dc2.Add<KeyboundButton>(keyboundButton2);
			DisposableCollection dc3 = _dc;
			KeyboundButton keyboundButton3 = new KeyboundButton(new KeyBinding((ModifierKeys)1, (Keys)83));
			((Control)keyboundButton3).set_Parent((Container)(object)this);
			((StandardButton)keyboundButton3).set_Text(Strings.Save);
			_saveButton = (StandardButton)(object)dc3.Add<KeyboundButton>(keyboundButton3);
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(Strings.TestRace);
			_testButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.TestFromHere);
			_testFromButton = val6;
			UpdateLayout();
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (await ConfirmDiscard(Strings.BackToRacing))
				{
					_editor.DiscardChanges();
					_panelStack.GoBack();
				}
			});
			((Control)_newButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (await ConfirmDiscard(Strings.NewRace))
				{
					_editor.DiscardChanges();
					_editor.FullRace = DataExtensions.NewRace();
				}
			});
			((Control)_importCsvButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (await RacingModule.ConfirmationModal.ShowAsync(Strings.AreYouSure, Strings.CsvImportWarning, Strings.ImportCsv, Strings.Cancel))
				{
					using AsyncFileDialog<OpenFileDialog> ofd = new AsyncFileDialog<OpenFileDialog>();
					ofd.Dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
					if (await ofd.Show() == DialogResult.OK)
					{
						try
						{
							Race intermediate = Race.FromCsvFile(ofd.Dialog.FileName);
							FullRace target = _editor.FullRace;
							_editor.DiscardChanges();
							target.Race.ImportPointsFrom(intermediate);
							_editor.FullRace = target;
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
			_editor.CanUndoRedoChanged += new Action<bool, bool>(CanUndoRedoChanged);
			_editor.IsDirtyChanged += new Action<bool>(IsDirtyChanged);
			_editor.PointSelected += new Action<RacePoint>(PointSelected);
			_editor.TestRequested += new Action<int>(TestRequested);
			CanUndoRedoChanged(_editor.CanUndo, _editor.CanRedo);
			IsDirtyChanged(_editor.IsDirty);
			PointSelected(_editor.Selected);
			this.SoftChild((Control)(object)_editor);
		}

		private void TestRequested(int testCheckpoint)
		{
			_panelStack.Push(new TestPanel(_panelStack, _editor.Measurer, _editor.FullRace, testCheckpoint));
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

		private void CanUndoRedoChanged(bool canUndo, bool canRedo)
		{
			((Control)_undoButton).set_Enabled(canUndo);
			((Control)_redoButton).set_Enabled(canRedo);
		}

		private void IsDirtyChanged(bool isDirty)
		{
			_saveButton.set_Text(Strings.Save + (isDirty ? "*" : ""));
		}

		private void PointSelected(RacePoint? point)
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
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
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
			_editor.CanUndoRedoChanged -= new Action<bool, bool>(CanUndoRedoChanged);
			_editor.IsDirtyChanged -= new Action<bool>(IsDirtyChanged);
			_editor.PointSelected -= new Action<RacePoint>(PointSelected);
			_editor.TestRequested -= new Action<int>(TestRequested);
			_dc.Dispose();
			((GraphicsResource)Icon).Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
