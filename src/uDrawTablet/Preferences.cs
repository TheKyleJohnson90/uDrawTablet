using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using uDrawLib;

namespace uDrawTablet
{
  public partial class Preferences : Form
  {
    #region P/Invoke Crud

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string lPAppName, string lpKeyName, string lpString, string lpFileName);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFilePath);

    #endregion

    #region Declarations

    private const string _INI_FILE = "Settings.ini";
    private const string _DEFAULT_SECTION = "Default";
    private const string _KEY_PEN_THRESHOLD = "PenThreshold";
    private const string _KEY_BUTTON_SPEED = "ButtonSpeed";

    private const string _CIRCLE_BUTTON = "ButtonCircle";
    private const string _SQUARE_BUTTON = "ButtonSquare";
    private const string _CROSS_BUTTON = "ButtonCross";
    private const string _TRIANGLE_BUTTON = "ButtonTriangle";
    private const string _START_BUTTON = "ButtonStart";
    private const string _SELECT_BUTTON = "ButtonSelect";

    private ButtonPreferences btnPreferences;
    private NotifyIcon _icon;
    private ContextMenu _menu;
    private bool _inPreferences;

    #endregion

    #region Constructors / Teardown

    public Preferences()
    {
      InitializeComponent();

      _menu = new ContextMenu();
      _menu.MenuItems.Add("Preferences", OnPreferences);
      _menu.MenuItems.Add("About", OnAbout);
      _menu.MenuItems.Add("Exit", OnExit);

      _icon = new NotifyIcon();
      _icon.Text = Assembly.GetExecutingAssembly().GetName().Name;
      _icon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
      _icon.ContextMenu = _menu;
      _icon.Visible = true;

      if (!uDrawTabletDevice.IsDetected())
      {
        MessageBox.Show(this, "PS3 uDraw tablet was not detected!", "Not Detected", MessageBoxButtons.OK,
          MessageBoxIcon.Exclamation);

        //Application.Exit();
      }
      else
      {
          this.btnPreferences = new ButtonPreferences();
          //this.btnPreferences.LoadPreferences();
        MouseInterface.Start(this);
      }
    }

    #endregion

    #region Public Methods

    public void ShowPreferences()
    {
      OnPreferences(this, EventArgs.Empty);
    }

    public void LoadPreferences()
    {
        //Movement
      var sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_PEN_THRESHOLD, MouseInterface.PenPressureThreshold.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      int threshold = MouseInterface.PenPressureThreshold; int.TryParse(sb.ToString(), out threshold);
      MouseInterface.PenPressureThreshold = threshold;
      sb = new StringBuilder(255); 
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_BUTTON_SPEED, MouseInterface.ButtonMoveSpeed.ToString(), sb, sb.Capacity,
          Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      int speed = MouseInterface.ButtonMoveSpeed; int.TryParse(sb.ToString(), out speed);
      MouseInterface.ButtonMoveSpeed = speed;
        //Buttons
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _CIRCLE_BUTTON, MouseInterface.CircleButton.ToString(), sb, sb.Capacity,
          Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      string circle = MouseInterface.CircleButton; 
      MouseInterface.CircleButton = circle;
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _CROSS_BUTTON, MouseInterface.CrossButton.ToString(), sb, sb.Capacity,
          Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      string cross = MouseInterface.CrossButton;
      MouseInterface.CircleButton = cross;
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _SQUARE_BUTTON, MouseInterface.SquareButton.ToString(), sb, sb.Capacity,
          Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      string square = MouseInterface.CrossButton;
      MouseInterface.CircleButton = square;
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _TRIANGLE_BUTTON, MouseInterface.TriangleButton.ToString(), sb, sb.Capacity,
          Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      string triangle = MouseInterface.CrossButton;
      MouseInterface.CircleButton = triangle;
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _START_BUTTON, MouseInterface.StartButton.ToString(), sb, sb.Capacity,
          Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      string start = MouseInterface.CrossButton;
      MouseInterface.CircleButton = start;
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _SELECT_BUTTON, MouseInterface.SelectButton.ToString(), sb, sb.Capacity,
          Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      string select = MouseInterface.CrossButton;
      MouseInterface.CircleButton = select;

    }

    #endregion

    #region Overrides

    protected override void SetVisibleCore(bool value)
    {
      if (!this.IsHandleCreated)
      {
        value = false;
        CreateHandle();
      }

      base.SetVisibleCore(value);
    }

    protected override void Dispose(bool isDisposing)
    {
      if (isDisposing)
      {
        if (components != null)
          components.Dispose();

        //Get rid of the icon
        _icon.Dispose();
      }

      base.Dispose(isDisposing);
    }

    #endregion

    #region Event Handlers

    private void btnCircle_Click(object sender, EventArgs e)
    {
        //ButtonPreferences btnPreferences = new ButtonPreferences();
        btnPreferences.btnSelected = "Circle";
        btnPreferences.LoadPreferences();
    }

    private void btnTriangle_Click(object sender, EventArgs e)
    {
        //ButtonPreferences btnPreferences = new ButtonPreferences();
        btnPreferences.btnSelected = "Triangle";
        btnPreferences.LoadPreferences();
    }

    private void btnCross_Click(object sender, EventArgs e)
    {
        //ButtonPreferences btnPreferences = new ButtonPreferences();
        btnPreferences.btnSelected = "Cross";
        btnPreferences.LoadPreferences();
    }

    private void btnSquare_Click(object sender, EventArgs e)
    {
        //ButtonPreferences btnPreferences = new ButtonPreferences();
        btnPreferences.btnSelected = "Square";
        btnPreferences.LoadPreferences();
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        //ButtonPreferences btnPreferences = new ButtonPreferences();
        btnPreferences.btnSelected = "Start";
        btnPreferences.LoadPreferences();
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
        //ButtonPreferences btnPreferences = new ButtonPreferences();
        btnPreferences.btnSelected = "Select";
        btnPreferences.LoadPreferences();
    }
    private void btnPS_Click(object sender, EventArgs e)
    {
        //ButtonPreferences btnPreferences = new ButtonPreferences();
        btnPreferences.btnSelected = "PS";
        btnPreferences.LoadPreferences();
    }

      private void btnSave_Click(object sender, EventArgs e)
    {
      //Save preferences
      MouseInterface.PenPressureThreshold = trbPenClick.Maximum - trbPenClick.Value + trbPenClick.Minimum;
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_PEN_THRESHOLD, MouseInterface.PenPressureThreshold.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));
      MouseInterface.ButtonMoveSpeed = trbButtonMoveSpeed.Maximum - trbButtonMoveSpeed.Value + trbButtonMoveSpeed.Minimum;
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_BUTTON_SPEED, MouseInterface.ButtonMoveSpeed.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), _INI_FILE));

      this.Close();
    }

      private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void Main_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_inPreferences)
      {
        _inPreferences = false;

        e.Cancel = true;
        this.Hide();
      }
    }
    
    #endregion

    #region Local Methods

    private void _ShowPreferences()
    {
      const int TICK_FREQUENCY = 0x10;
      const int PEN_PRESSURE_MINIMUM = 0x70;
      const int PEN_PRESSURE_MAXIMUM = 0xFF;
        //
      const int MOVE_TICK_FREQUENCY = 0x01;
      const int MOVE_SPEED_MINIMUM = 0x01;
      const int MOVE_SPEED_MAXIMUM = 0x05;

      if (this.InvokeRequired)
      {
        this.Invoke(new MethodInvoker(_ShowPreferences));
      }
      else
      {
        //Load preferences
        LoadPreferences();
        trbPenClick.Minimum = PEN_PRESSURE_MINIMUM;
        trbPenClick.Maximum = PEN_PRESSURE_MAXIMUM;
        trbPenClick.TickFrequency = -TICK_FREQUENCY;
        trbPenClick.Value = trbPenClick.Maximum - MouseInterface.PenPressureThreshold + trbPenClick.Minimum;
          //
        trbButtonMoveSpeed.Minimum = MOVE_SPEED_MINIMUM;
        trbButtonMoveSpeed.Maximum = MOVE_SPEED_MAXIMUM;
        trbButtonMoveSpeed.LargeChange = MOVE_TICK_FREQUENCY;
        trbButtonMoveSpeed.Value = trbButtonMoveSpeed.Maximum - MouseInterface.ButtonMoveSpeed + trbButtonMoveSpeed.Minimum;

        this.Show();
      }
    }

    private void OnPreferences(object sender, EventArgs e)
    {
      _inPreferences = true;

      _ShowPreferences();
    }

    private void OnAbout(object sender, EventArgs e)
    {
        About myabout = new About();
        myabout.ShowDialog();
        myabout.Show();
    }

    private void OnExit(object sender, EventArgs e)
    {
      try
      {
        _inPreferences = false;
        MouseInterface.Stop();
      }
      finally
      {
        Application.Exit();
      }
    }

    #endregion



  }
}
