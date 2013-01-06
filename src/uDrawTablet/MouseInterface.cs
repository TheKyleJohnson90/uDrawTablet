using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using uDrawLib;

namespace uDrawTablet
{
  public static class MouseInterface
  {

    #region P/Invoke Crud

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public int X;
      public int Y;

      public static implicit operator Point(POINT point)
      {
        return new Point(point.X, point.Y);
      }
    }

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

    private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    private const int MOUSEEVENTF_MOVE = 0x0001;
    private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const int MOUSEEVENTF_LEFTUP = 0x0004;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const int MOUSEEVENTF_RIGHTUP = 0x0010;

    #endregion

    #region Declarations

    private const int _DEFAULT_PEN_PRESSURE_THRESHOLD = 0xD0;
    private const int _DEFAULT_BUTTON_MOVE_SPEED = 0x01;
    private const string _DEFAULT_BUTTON = "Default";
      //Application
    private static uDrawTabletDevice _tablet;
    private static Thread _handleTabletEventsThread;
    private static Preferences _preferences;
    private static TabletButtonState _lastButtonState;
      //Movement
    private static int _penPressureThreshold = _DEFAULT_PEN_PRESSURE_THRESHOLD;
    private static int _penButtonMoveSpeed = _DEFAULT_BUTTON_MOVE_SPEED;
      //Buttons
    private static string _penCircleButton = _DEFAULT_BUTTON;
    private static string _penSquareButton = _DEFAULT_BUTTON;
    private static string _penCrossButton = _DEFAULT_BUTTON;
    private static string _penTriangleButton = _DEFAULT_BUTTON;
    private static string _penStartButton = _DEFAULT_BUTTON;
    private static string _penSelectButton = _DEFAULT_BUTTON;

    private static string _penPSButton = _DEFAULT_BUTTON;


    #endregion

    #region Public Properties

    public static int PenPressureThreshold
    {
      get
      {
        return _penPressureThreshold;
      }
      set
      {
        _penPressureThreshold = value;
      }
    }
    public static int ButtonMoveSpeed
    {
        get
        {
            return _penButtonMoveSpeed;
        }
        set
        {
            _penButtonMoveSpeed = value;
        }
    }
    public static string CircleButton
    {
        get
        {
            return _penCircleButton;
        }
        set
        {
            _penCircleButton = value;
        }
    }
    public static string CrossButton
    {
        get
        {
            return _penCrossButton;
        }
        set
        {
            _penCrossButton = value;
        }
    }
    public static string SquareButton
    {
        get
        {
            return _penSquareButton;
        }
        set
        {
            _penSquareButton = value;
        }
    }
    public static string TriangleButton
    {
        get
        {
            return _penTriangleButton;
        }
        set
        {
            _penTriangleButton = value;
        }
    }
    public static string StartButton
    {
        get
        {
            return _penStartButton;
        }
        set
        {
            _penStartButton = value;
        }
    }
    public static string SelectButton
    {
        get
        {
            return _penSelectButton;
        }
        set
        {
            _penSelectButton = value;
        }
    }
    public static string PSButton
    {
        get
        {
            return _penPSButton;
        }
        set
        {
            _penPSButton = value;
        }
    }

    #endregion

    #region Public Methods

    public static void Start(Preferences preferences)
    {
      _preferences = preferences;
      preferences.LoadPreferences();

      _tablet = new uDrawTabletDevice();

      _lastButtonState = _tablet.ButtonState;
      _tablet.ButtonStateChanged += device_ButtonStateChanged;

      if (_handleTabletEventsThread == null)
      {
        _handleTabletEventsThread = new Thread(new ThreadStart(_HandleTabletEvents));
        _handleTabletEventsThread.Start();
      }
    }

    public static void Stop()
    {
      if (_handleTabletEventsThread != null)
      {
        _handleTabletEventsThread.Abort();
        _handleTabletEventsThread = null;
      }

      if (_tablet != null)
        _tablet.Disconnect();
    }

    #endregion

    #region Event Handlers

    private static void device_ButtonStateChanged(object sender, EventArgs e)
    {
      //Click Cross
      if (_tablet.ButtonState.CrossHeld != _lastButtonState.CrossHeld)
          uDrawButtonClick(CrossButton);
      //Click SQUARE
      if (_tablet.ButtonState.SquareHeld != _lastButtonState.SquareHeld)
          uDrawButtonClick(SquareButton);
      //Click TRIANGLE
      if (_tablet.ButtonState.TriangleHeld != _lastButtonState.TriangleHeld && _tablet.ButtonState.TriangleHeld)
          uDrawButtonClick(CircleButton);
      //Click Circle
      if (_tablet.ButtonState.CircleHeld != _lastButtonState.CircleHeld && _tablet.ButtonState.CircleHeld)
      {
          uDrawButtonClick(CircleButton);
      }
      //Click Select
      if (_tablet.ButtonState.SelectHeld != _lastButtonState.SelectHeld && _tablet.ButtonState.SelectHeld)
      {
          uDrawButtonClick(SelectButton);
      }
      //Click Start
      if (_tablet.ButtonState.StartHeld != _lastButtonState.StartHeld && _tablet.ButtonState.StartHeld)
      {
          uDrawButtonClick(StartButton);
      }
      //Click PS
      if (_tablet.ButtonState.PSHeld != _lastButtonState.PSHeld && _tablet.ButtonState.PSHeld)
          _preferences.ShowPreferences();


      _lastButtonState = _tablet.ButtonState;
    }

    #endregion

    #region Local Methods

    private static void _HandleTabletEvents()
    {
      const double TABLET_PAD_WIDTH = 1920;
      const double TABLET_PAD_HEIGHT = 1080;
      bool lastPressure = _tablet.PenPressure >= _penPressureThreshold;
      //bool lastSpeed = _tablet.ButtonSpeed >= _penButtonMoveSpeed;
      TabletPressureType type = _tablet.PressureType;
      Point lastPoint = _tablet.PressurePoint;

      do
      {
        if (_tablet.DPadState.DownHeld)
            mouse_event(MOUSEEVENTF_MOVE, 0, _penButtonMoveSpeed, 0, UIntPtr.Zero);
        if (_tablet.DPadState.UpHeld)
            mouse_event(MOUSEEVENTF_MOVE, 0, -_penButtonMoveSpeed, 0, UIntPtr.Zero);
        if (_tablet.DPadState.LeftHeld)
            mouse_event(MOUSEEVENTF_MOVE, -_penButtonMoveSpeed, 0, 0, UIntPtr.Zero);
        if (_tablet.DPadState.RightHeld)
            mouse_event(MOUSEEVENTF_MOVE, _penButtonMoveSpeed, 0, 0, UIntPtr.Zero);

        if (_tablet.DPadState.DownLeftHeld)
            mouse_event(MOUSEEVENTF_MOVE, -_penButtonMoveSpeed, _penButtonMoveSpeed, 0, UIntPtr.Zero);
        if (_tablet.DPadState.UpLeftHeld)
            mouse_event(MOUSEEVENTF_MOVE, -_penButtonMoveSpeed, -_penButtonMoveSpeed, 0, UIntPtr.Zero);
        if (_tablet.DPadState.DownRightHeld)
            mouse_event(MOUSEEVENTF_MOVE, _penButtonMoveSpeed, _penButtonMoveSpeed, 0, UIntPtr.Zero);
        if (_tablet.DPadState.UpRightHeld)
            mouse_event(MOUSEEVENTF_MOVE, _penButtonMoveSpeed, -_penButtonMoveSpeed, 0, UIntPtr.Zero);
        if (lastPressure != (_tablet.PenPressure >= _penPressureThreshold))
          mouse_event(_tablet.PenPressure >= _penPressureThreshold ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        lastPressure = (_tablet.PenPressure >= _penPressureThreshold);

        if (_tablet.PressureType == type && type == TabletPressureType.PenPressed)
        {
          //Calculate the absolute coordinates of the new mouse position
          double x = (_tablet.PressurePoint.X / TABLET_PAD_WIDTH) * 65536;
          double y = (_tablet.PressurePoint.Y / TABLET_PAD_HEIGHT) * 65536;

          if (Cursor.Position.X != x && Cursor.Position.Y != y)
            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, (int)x, (int)y, 0, UIntPtr.Zero);
        }
        type = _tablet.PressureType;
        lastPoint = _tablet.PressurePoint;

        Thread.Sleep(1);
      } while (true);
    }

    private static void uDrawButtonClick(string btn)
    {
        if(btn=="LeftClick"){
                mouse_event(true ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }else if(btn=="RightClick"){
                mouse_event(true ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        }else if(btn=="DoNothing"){
                _preferences.ShowPreferences();
        }else if (btn.StartsWith("Open:")){
                System.Diagnostics.Process.Start(btn.Replace("Open:",""));
        }
        else{
                _preferences.ShowPreferences();
        }
    }
    #endregion

  }
}
