using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterUserSettingsLeftMenu
    {
        private readonly UserControl _appearance;
        private readonly IViewUserSettingsRightManuallyShownTabs _manuallyShownTabs;
        private readonly UserControl _rules;
        private readonly IViewUserSettings _viewUserSettings;
        private readonly IViewUserSettingsRightAdvanced _viewUserSettingsRightAdvanced;
        private readonly IViewUserSettingsMenuLeft _viewUserSettingsMenuLeft;
        private readonly IViewUserSettingsRightHelp _userSettingsRightHelp;

        public PresenterUserSettingsLeftMenu
        (
            IViewUserSettingsMenuLeft viewUserSettingsMenuLeft,
            IViewUserSettings viewUserSettings,
            IViewUserSettingsRightAppearance appearance,
            IViewUserSettingsRightAutoShownTabs rules,
            IViewUserSettingsRightManuallyShownTabs manuallyShownTabs,
            IViewUserSettingsRightAdvanced viewUserSettingsRightAdvanced,
            IViewUserSettingsRightHelp userSettingsRightHelp
        )
        {
            _viewUserSettingsMenuLeft = viewUserSettingsMenuLeft;
            _viewUserSettings = viewUserSettings;
            _appearance = (UserControl)appearance;
            _rules = (UserControl)rules;
            _manuallyShownTabs = manuallyShownTabs;
            _viewUserSettingsRightAdvanced = viewUserSettingsRightAdvanced;
            _userSettingsRightHelp = userSettingsRightHelp;

            //subscribe to events
            if (_viewUserSettings != null) _viewUserSettings.Load += OnLoadedViewUserSettings;
            if (_viewUserSettingsMenuLeft == null) return;
            _viewUserSettingsMenuLeft.MenuDrawNode += OnMenuDrawNode;
            _viewUserSettingsMenuLeft.MouseClickNode += OnMouseClickNode;
        }

        private void OnMouseClickNode(object sender, TreeNodeMouseClickEventArgs treeNodeMouseClickEventArgs)
        {


            if (treeNodeMouseClickEventArgs.Button != MouseButtons.Left) return;
            switch (treeNodeMouseClickEventArgs.Node.Text)
            {
                case "Appearance":
                    {
                        if (_viewUserSettingsMenuLeft.Treeview1.SelectedNode.Text == Resources.PresenterUserSettingsLeftMenu_OnMouseClickNode_Appearance) return;
                        _viewUserSettingsMenuLeft.Treeview1.SelectedNode = treeNodeMouseClickEventArgs.Node;
                        _viewUserSettings.ChangeRightView(_appearance);
                        _viewUserSettings.SetGroupBoxText(treeNodeMouseClickEventArgs.Node.Text);
                        break;
                    }
                case "AutoShownTabs":
                    {
                        if (_viewUserSettingsMenuLeft.Treeview1.SelectedNode.Text == Resources.PresenterUserSettingsLeftMenu_OnMouseClickNode_AutoShownTabs) return;
                        _viewUserSettingsMenuLeft.Treeview1.SelectedNode = treeNodeMouseClickEventArgs.Node;
                        _viewUserSettings.ChangeRightView(_rules);
                        _viewUserSettings.SetGroupBoxText(treeNodeMouseClickEventArgs.Node.Text);
                        break;
                    }
                case "ManuallyShownTabs":
                    {
                        if (_viewUserSettingsMenuLeft.Treeview1.SelectedNode.Text == Resources.PresenterUserSettingsLeftMenu_OnMouseClickNode_ManuallyShownTabs) return;
                        _viewUserSettingsMenuLeft.Treeview1.SelectedNode = treeNodeMouseClickEventArgs.Node;
                        _viewUserSettings.ChangeRightView((UserControl)_manuallyShownTabs);
                        _viewUserSettings.SetGroupBoxText(treeNodeMouseClickEventArgs.Node.Text);
                        break;
                    }
                case "Advanced":
                    {
                        if (_viewUserSettingsMenuLeft.Treeview1.SelectedNode.Text == Resources.PresenterUserSettingsLeftMenu_OnMouseClickNode_Advanced) return;
                        _viewUserSettingsMenuLeft.Treeview1.SelectedNode = treeNodeMouseClickEventArgs.Node;
                        _viewUserSettings.ChangeRightView((UserControl)_viewUserSettingsRightAdvanced);
                        _viewUserSettings.SetGroupBoxText(treeNodeMouseClickEventArgs.Node.Text);
                        break;
                    }
                case "Help":
                    {
                        if (_viewUserSettingsMenuLeft.Treeview1.SelectedNode.Text == Resources.Help) return;
                        _viewUserSettingsMenuLeft.Treeview1.SelectedNode = treeNodeMouseClickEventArgs.Node;
                        _viewUserSettings.ChangeRightView((UserControl)_userSettingsRightHelp);
                        _viewUserSettings.SetGroupBoxText(treeNodeMouseClickEventArgs.Node.Text);
                        break;
                    }
            }
        }

        private void OnLoadedViewUserSettings(object sender, EventArgs e)
        {
            _viewUserSettingsMenuLeft.InitiateControl();
            _viewUserSettingsMenuLeft.AddNodes();
            _viewUserSettingsMenuLeft.Controls.Add(_viewUserSettingsMenuLeft.Treeview1);
            _viewUserSettings.ChangeRightView(_appearance);
        }

        private void OnMenuDrawNode(object sender,
            DrawTreeNodeEventArgs e)
        {
            _viewUserSettingsMenuLeft.ModifiedTreeviewDrawNode(e);
        }
    }
}