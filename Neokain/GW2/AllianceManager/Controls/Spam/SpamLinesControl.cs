using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Spams.SpamLines;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class SpamLinesControl : Panel
	{
		private List<SpamLineDto> _lines = new List<SpamLineDto>();

		private List<SpamLineControl> _lineControls = new List<SpamLineControl>();

		private bool _editMode;

		private StandardButton _addButton;

		private readonly IFontService _fontService;

		private SpamContext _spamContext;

		private const int CTRL_PADDING = 5;

		public bool HasValidationError => _lineControls.Any(delegate(SpamLineControl c)
		{
			ValidationResult currentValidation = c.CurrentValidation;
			return currentValidation != null && currentValidation.Status == ValidationStatus.Error;
		});

		public bool HasValidationWarning
		{
			get
			{
				if (!HasValidationError)
				{
					return _lineControls.Any(delegate(SpamLineControl c)
					{
						ValidationResult currentValidation = c.CurrentValidation;
						return currentValidation != null && currentValidation.Status == ValidationStatus.Warning;
					});
				}
				return false;
			}
		}

		public ValidationStatus WorstValidationStatus
		{
			get
			{
				if (HasValidationError)
				{
					return ValidationStatus.Error;
				}
				if (HasValidationWarning)
				{
					return ValidationStatus.Warning;
				}
				return ValidationStatus.Ok;
			}
		}

		public event EventHandler<ValidationStatus> ValidationChanged;

		public SpamLinesControl(IFontService fontService, SpamContext spamContext)
			: this()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			_fontService = fontService ?? throw new ArgumentNullException("fontService");
			_spamContext = spamContext ?? throw new ArgumentNullException("spamContext");
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("+");
			((Control)val).set_Width(30);
			((Control)val).set_Height(24);
			((Control)val).set_Top(0);
			((Control)val).set_Left(0);
			((Control)val).set_Enabled(false);
			_addButton = val;
			((Control)_addButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AddNewLine();
			});
		}

		public void SetSpamContext(SpamContext spamContext)
		{
			_spamContext = spamContext ?? throw new ArgumentNullException("spamContext");
			UpdateDisplay();
		}

		public void SetLines(IEnumerable<SpamLineDto> lines)
		{
			_lines = lines?.ToList() ?? new List<SpamLineDto>();
			UpdateDisplay();
		}

		public void SetEditMode(bool editMode)
		{
			_editMode = editMode;
			((Control)_addButton).set_Enabled(_editMode);
			UpdateDisplay();
		}

		public List<SpamLineDto> GetLines()
		{
			List<SpamLineDto> result = new List<SpamLineDto>();
			foreach (SpamLineControl lineControl in _lineControls)
			{
				SpamLineDto line = lineControl.GetLine();
				result.Add(line);
			}
			return result;
		}

		public override void RecalculateLayout()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (_addButton == null)
			{
				return;
			}
			int top = ((Control)_addButton).get_Bottom() + 5;
			foreach (SpamLineControl item in ((IEnumerable)((Container)this).get_Children()).OfType<SpamLineControl>())
			{
				((Control)item).set_Top(top);
				((Control)item).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)item).RecalculateLayout();
				top = ((Control)item).get_Bottom() + 5;
			}
			((Control)this).set_Height(top);
		}

		private void AddNewLine()
		{
			ChatType defaultTarget = GetDefaultChatType();
			SpamLineDto newLine = new SpamLineDto
			{
				Id = Guid.NewGuid(),
				LineText = string.Empty,
				Target = defaultTarget,
				TargetInfo1 = GetDefaultTargetInfo1(defaultTarget)
			};
			_lines.Add(newLine);
			UpdateDisplay();
		}

		private ChatType GetDefaultChatType()
		{
			return _spamContext.ContextType switch
			{
				SpamContextType.Guild => ChatType.Guild, 
				SpamContextType.Alliance => ChatType.Alliance, 
				SpamContextType.Account => ChatType.Guild, 
				_ => ChatType.Guild, 
			};
		}

		private string GetDefaultTargetInfo1(ChatType chatType)
		{
			switch (chatType)
			{
			case ChatType.Guild:
				return _spamContext.AvailableGuilds?.FirstOrDefault()?.GuildId.ToString();
			case ChatType.Alliance:
				if (_spamContext.ContextType == SpamContextType.Account)
				{
					return _spamContext.AvailableAlliances?.FirstOrDefault()?.AllianceId.ToString();
				}
				return null;
			default:
				return null;
			}
		}

		private void UpdateDisplay()
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			foreach (SpamLineControl lineControl in _lineControls)
			{
				((Control)lineControl).Dispose();
			}
			_lineControls.Clear();
			int top = ((Control)_addButton).get_Bottom() + 5;
			for (int i = 0; i < _lines.Count; i++)
			{
				SpamLineControl spamLineControl = new SpamLineControl(_lines[i], _editMode, _fontService, _spamContext);
				((Control)spamLineControl).set_Parent((Container)(object)this);
				((Control)spamLineControl).set_Top(top);
				((Control)spamLineControl).set_Left(0);
				((Control)spamLineControl).set_Width(((Container)this).get_ContentRegion().Width);
				((Container)spamLineControl).set_HeightSizingMode((SizingMode)1);
				SpamLineControl ctrl = spamLineControl;
				int rowIndex = i;
				ctrl.DeleteClicked += delegate
				{
					SaveCurrentLineStates();
					_lines.RemoveAt(rowIndex);
					UpdateDisplay();
				};
				ctrl.UpClicked += delegate
				{
					SaveCurrentLineStates();
					if (rowIndex > 0)
					{
						List<SpamLineDto> lines3 = _lines;
						int index3 = rowIndex;
						List<SpamLineDto> lines4 = _lines;
						int index4 = rowIndex - 1;
						SpamLineDto value3 = _lines[rowIndex - 1];
						SpamLineDto value4 = _lines[rowIndex];
						lines3[index3] = value3;
						lines4[index4] = value4;
						UpdateDisplay();
					}
				};
				ctrl.DownClicked += delegate
				{
					SaveCurrentLineStates();
					if (rowIndex < _lines.Count - 1)
					{
						List<SpamLineDto> lines = _lines;
						int index = rowIndex;
						List<SpamLineDto> lines2 = _lines;
						int index2 = rowIndex + 1;
						SpamLineDto value = _lines[rowIndex + 1];
						SpamLineDto value2 = _lines[rowIndex];
						lines[index] = value;
						lines2[index2] = value2;
						UpdateDisplay();
					}
				};
				ctrl.ValidationChanged += delegate
				{
					this.ValidationChanged?.Invoke(this, WorstValidationStatus);
				};
				_lineControls.Add(ctrl);
				top += ((Control)ctrl).get_Height() + 5;
			}
			((Control)this).set_Height(top);
			((Control)_addButton).set_Top(0);
			((Control)_addButton).set_Left(0);
			((Control)this).Invalidate();
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
			Container parent2 = ((Control)this).get_Parent();
			if (parent2 != null)
			{
				Container parent3 = ((Control)parent2).get_Parent();
				if (parent3 != null)
				{
					((Control)parent3).Invalidate();
				}
			}
		}

		private void SaveCurrentLineStates()
		{
			for (int i = 0; i < _lineControls.Count && i < _lines.Count; i++)
			{
				SpamLineDto updated = _lineControls[i].GetLine();
				_lines[i].LineText = updated.LineText;
				_lines[i].Target = updated.Target;
				_lines[i].TargetInfo1 = updated.TargetInfo1;
			}
		}
	}
}
