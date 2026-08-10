using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class SpamsControl : Panel
	{
		private readonly SpamClient _spamClient;

		private readonly SpamSource _source;

		private readonly Module _module;

		private readonly SpamContext _spamContext;

		private readonly SpamDetailsControl _spamDetailsControl;

		private readonly SpamsListControl _spamListControl;

		private bool _initialized;

		private const int CTRL_PADDING = 5;

		private const int LIST_WIDTH_PERCENT = 30;

		public SpamDetailsControl DetailsControl => _spamDetailsControl;

		public SpamsListControl ListControl => _spamListControl;

		public SpamsControl(SpamClient spamClient, SpamSource source, Module module, SpamContext spamContext)
			: this()
		{
			_spamClient = spamClient ?? throw new ArgumentNullException("spamClient");
			_source = source;
			_module = module ?? throw new ArgumentNullException("module");
			_spamContext = spamContext ?? throw new ArgumentNullException("spamContext");
			SpamsListControl spamsListControl = new SpamsListControl(_spamClient, _source);
			((Control)spamsListControl).set_Parent((Container)(object)this);
			((Panel)spamsListControl).set_CanScroll(true);
			((Panel)spamsListControl).set_ShowBorder(true);
			_spamListControl = spamsListControl;
			_spamListControl.SelectedSpamChanged += SpamListControl_SelectedSpamChanged;
			SpamDetailsControl spamDetailsControl = new SpamDetailsControl(_spamClient, _source, _module, _spamContext);
			((Control)spamDetailsControl).set_Parent((Container)(object)this);
			((Panel)spamDetailsControl).set_CanScroll(true);
			_spamDetailsControl = spamDetailsControl;
			_spamDetailsControl.EditingStarted += SpamDetailsControl_EditingStarted;
			_spamDetailsControl.EditingStopped += SpamDetailsControl_EditingStopped;
			_spamListControl.CategoriesLoaded += SpamListControl_CategoriesLoaded;
			List<SpamCategoryDto> categories = _spamListControl.GetCategories();
			if (categories != null && categories.Count > 0)
			{
				_spamDetailsControl.SetCategories(categories);
			}
			_initialized = true;
		}

		public override void RecalculateLayout()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			if (_initialized)
			{
				using (((Control)this).SuspendLayoutContext())
				{
					int totalWidth = ((Container)this).get_ContentRegion().Width;
					int listWidth = (int)((float)(totalWidth * 30) / 100f);
					((Control)_spamListControl).set_Left(0);
					((Control)_spamListControl).set_Top(0);
					((Control)_spamListControl).set_Width(listWidth);
					((Control)_spamListControl).set_Height(((Container)this).get_ContentRegion().Height);
					((Control)_spamDetailsControl).set_Left(listWidth + 5);
					((Control)_spamDetailsControl).set_Top(0);
					((Control)_spamDetailsControl).set_Width(totalWidth - listWidth - 5 - 30);
					((Control)_spamDetailsControl).set_Height(((Container)this).get_ContentRegion().Height);
				}
			}
		}

		private void SpamDetailsControl_EditingStopped(object sender, EventArgs e)
		{
			((Control)_spamListControl).set_Enabled(true);
		}

		private void SpamDetailsControl_EditingStarted(object sender, EventArgs e)
		{
			((Control)_spamListControl).set_Enabled(false);
		}

		private void SpamListControl_SelectedSpamChanged(object sender, SelectedSpamChangedEventArgs e)
		{
			_spamDetailsControl.SetSpam(e.SelectedSpam);
		}

		private void SpamListControl_CategoriesLoaded(object sender, CategoriesLoadedEventArgs e)
		{
			_spamDetailsControl.SetCategories(e.Categories);
		}

		protected override void DisposeControl()
		{
			if (_spamListControl != null)
			{
				_spamListControl.SelectedSpamChanged -= SpamListControl_SelectedSpamChanged;
				_spamListControl.CategoriesLoaded -= SpamListControl_CategoriesLoaded;
			}
			if (_spamDetailsControl != null)
			{
				_spamDetailsControl.EditingStarted -= SpamDetailsControl_EditingStarted;
				_spamDetailsControl.EditingStopped -= SpamDetailsControl_EditingStopped;
			}
			((Panel)this).DisposeControl();
		}
	}
}
