using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace uDrawTablet
{
    public partial class ButtonPreferences : Form
    {

        #region P/Invoke Crud

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lPAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFilePath);

        #endregion

        #region Declarations

        private const string _INI_FILE = "Settings.ini";
        private const string _BUTTON_SECTION = "Buttons";

        private const string _LEFT_CLICK = "LeftClick";
        private const string _RIGHT_CLICK = "RightClick";
        private const string _DO_NOTHING = "DoNothing";
        private const string _OPEN_APP = "Open:";
        private readonly string[] _BUTTONS = { "Circle", "Square", "Triangle", "Cross", "Select" , "Start" , "PS" };
        private string txtAppPath = @"C:\Windows\explorer.exe";
        private string optSelected = _DO_NOTHING;

        public string btnSelected = "PS";

        #endregion

        #region Constructors / Teardown

        public ButtonPreferences()
        {
                       
            InitializeComponent();
            //this.Hide();
            //LoadPreferences();
            //this.ShowDialog();
            //this.Refresh();
        }

        #endregion

        #region Public Methods

        public void LoadPreferences()
        {
            //string temp;
            var sb = new StringBuilder(255);
            GetPrivateProfileString(_BUTTON_SECTION, btnSelected, optSelected, sb, sb.Capacity,
              Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
            optSelected = sb.ToString();
            if(btnSelected==_BUTTONS[0]){
                    MouseInterface.CircleButton = optSelected;
            }else if(btnSelected==_BUTTONS[1]){                
                    MouseInterface.SquareButton = optSelected;
            }else if(btnSelected==_BUTTONS[2]){
                    MouseInterface.TriangleButton = optSelected;
            }else if(btnSelected==_BUTTONS[3]){                
                    MouseInterface.CrossButton = optSelected;
            }else if(btnSelected==_BUTTONS[4]){
                    MouseInterface.SelectButton = optSelected;
            }else if(btnSelected==_BUTTONS[5]){
                    MouseInterface.StartButton = optSelected;
            }else if(btnSelected==_BUTTONS[6]){
                    MouseInterface.PSButton = optSelected;
            }else{
                optSelected = _DO_NOTHING;
            }
            //now what option
            if(optSelected==_RIGHT_CLICK)
            {
                this.optRight.Checked = true;
            }
            else if(optSelected==_LEFT_CLICK)
            {
                this.optLeft.Checked = true;
            }
            else if(optSelected==_OPEN_APP)
            {
                this.optOpenApp.Checked = true;
                this.btnSpecify.Enabled = true;
                this.AppLocation.Enabled = true;
            }
            else if (optSelected == _DO_NOTHING)
            {
                this.optDefault.Checked = true;
            }
            else
            {
                this.optDefault.Checked = true;
            }
            //Default
            this.AppLocation.Text = txtAppPath;
            //Show
            this.Show();
        }
        #endregion

        #region Event Handlers

        private void btnDefault_Click(object sender, EventArgs e)
        {
            optDefault.Checked=true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //Check a few files
            if (optSelected.ToLower().EndsWith(".exe"))
            {
                if(!optSelected.StartsWith(_OPEN_APP))
                {
                    optSelected = _OPEN_APP + optSelected;
                }
            }


            //Save preferences
            if (btnSelected == _BUTTONS[0])
            {
                MouseInterface.CircleButton = optSelected;
            }
            else if (btnSelected == _BUTTONS[1])
            {
                MouseInterface.SquareButton = optSelected;
            }
            else if (btnSelected == _BUTTONS[2])
            {
                MouseInterface.TriangleButton = optSelected;
            }
            else if (btnSelected == _BUTTONS[3])
            {
                MouseInterface.CrossButton = optSelected;
            }
            else if (btnSelected == _BUTTONS[4])
            {
                MouseInterface.SelectButton = optSelected;
            }
            else if (btnSelected == _BUTTONS[5])
            {
                MouseInterface.StartButton = optSelected;
            }
            else if (btnSelected == _BUTTONS[6])
            {
                MouseInterface.PSButton = optSelected;
            }
            else
            {
                optSelected = _DO_NOTHING;
            }

            WritePrivateProfileString(_BUTTON_SECTION, btnSelected, optSelected,
      Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));

            //this.Close();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //this.Close();
            this.Hide();
        }


        private void btnSpecify_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Application Files|*.exe";
            openFileDialog1.Title = "Select a Application File";
            openFileDialog1.Multiselect = false;
            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .EXE file was selected, open it.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Assign the cursor in the dialog to the new file property.
                txtAppPath = openFileDialog1.FileName;
                this.AppLocation.Text = txtAppPath;
                optSelected = AppLocation.Text;
            }

        }
        private void optOpenApp_CheckedChanged(object sender, EventArgs e)
        {
            if (optOpenApp.Checked)
            {
                btnSpecify.Enabled = true;
                AppLocation.Enabled = true;
                AppLocation.Text = txtAppPath;
                optSelected = _OPEN_APP + AppLocation.Text;
            }
            else
            {
                btnSpecify.Enabled = false;
                AppLocation.Enabled = false;
            }
        }

        private void optRight_CheckedChanged(object sender, EventArgs e)
        {
            if (optRight.Checked)
            {
                optSelected = _RIGHT_CLICK;
            }
        }

        private void optLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (optLeft.Checked)
            {
                optSelected = _LEFT_CLICK;
            }
        }

        private void optDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (optDefault.Checked)
            {
                optSelected = _DO_NOTHING;
            }
        }

        #endregion
        
    }
}
