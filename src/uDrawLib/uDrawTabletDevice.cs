﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace uDrawLib
{
  public class uDrawTabletDevice : IDisposable
  {
    #region Declarations

    private const int _VENDOR_ID = 0x20D6;
    private const int _PRODUCT_ID = 0xCB17;
    private const int _MULTITOUCH_SENSITIVITY = 30;
    private const int _UNKNOWN_DATA1_SIZE = 4;
    private HIDDevice _device;

    /// <summary>
    /// Indicates the pressed state of every non-directional button on the tablet.
    /// </summary>
    public TabletButtonState ButtonState;

    /// <summary>
    /// Indicates the pressed state of each direction on the directional pad.
    /// </summary>
    public TabletDPadState DPadState;

    /// <summary>
    /// Indicates whether the nib/stylus, fingers, or nothing are currently pressing on the tablet.
    /// </summary>
    public TabletPressureType PressureType { get; private set; }

    /// <summary>
    /// The raw pressure being applied by the pen. Only valid with TabletPressureType.PenPressed.
    /// </summary>
    public ushort PenPressure { get; private set; }

    /// <summary>
    /// Indicates the distance between fingers. Only valid with TabletPressureType.Multitouch.
    /// </summary>
    public ushort MultitouchDistance { get; private set; }

    /// <summary>
    /// Indicates the absolute coordinates (from top-left corner) of the current pressure point.
    /// Only valid with TabletPressureType.PenPressed and TabletPressureType.FingerPressed.
    /// </summary>
    public Point PressurePoint { get; private set; }

    /// <summary>
    /// Unknown data that doesn't appear to change from 80 80 80 80...
    /// </summary>
    public byte[] UnknownData1 { get; private set; }

    /// <summary>
    /// Raw X-, Y-, and Z-axis data from the accelerometer.
    /// </summary>
    public TabletAccelerometerData AccelerometerData;

    private int _multitouchTimer;
    private ushort _previousMultitouchDistance;

    #endregion

    #region Constructors / Teardown

    public uDrawTabletDevice()
    {
      ButtonState = new TabletButtonState();
      DPadState = new TabletDPadState();
      UnknownData1 = new byte[_UNKNOWN_DATA1_SIZE];
      AccelerometerData = new TabletAccelerometerData();

      _device = new HIDDevice(_VENDOR_ID, _PRODUCT_ID);
      _device.DataReceived += _device_DataReceived;
    }

    #endregion

    #region Public Events

    public event EventHandler<EventArgs> ButtonStateChanged;
    public event EventHandler<EventArgs> DPadStateChanged;

    #endregion

    #region Public Properties

    /// <summary>
    /// Indicates whether pressure is being applied to the tablet.
    /// </summary>
    public bool IsPressed
    {
      get
      {
        bool ret = false;

        if (PressureType != TabletPressureType.NotPressed)
          ret = true;

        return ret;
      }
    }

    /// <summary>
    /// Indicates whether the user is currently zooming out by multitouch.
    /// </summary>
    public bool IsZooming
    {
      get
      {
        bool ret = false;

        if (MultitouchDistance > _previousMultitouchDistance)
          ret = true;

        return ret;
      }
    }

    /// <summary>
    /// Indicates whether the user is currently pinching by multitouch.
    /// </summary>
    public bool IsPinching
    {
      get
      {
        bool ret = false;

        if (MultitouchDistance < _previousMultitouchDistance)
          ret = true;

        return ret;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Detects whether the PS3 uDraw tablet device is currently attached to the PC.
    /// This is only detecting the presence of the USB dongle, NOT whether it is sync'd with the tablet.
    /// </summary>
    /// <returns></returns>
    public static bool IsDetected()
    {
      return HIDDevice.IsDetected(_VENDOR_ID, _PRODUCT_ID);
    }

    /// <summary>
    /// Disconnects the driver from the USB dongle.
    /// </summary>
    public void Disconnect()
    {
      _device.Disconnect();
    }

    public void Dispose()
    {
      try
      {
        Disconnect();
      }
      catch
      {
        //Don't care...
      }
    }

    #endregion

    #region Event Handlers

    private enum RawPressureType
    {
      NotPressed = 0x00,
      PenPressed = 0x40,
      FingerPressed = 0x80
    };

    private void _device_DataReceived(object sender, DataReceivedEventArgs e)
    {
      const int UNKNOWN_DATA1_OFFSET = 3;
      const int MULTITOUCH_DISTANCE_OFFSET = 12;
      const int PEN_PRESSURE_OFFSET = 13;
      const int PRESSURE_DATA_OFFSET = 15;
      const int ACCELEROMETER_X_OFFSET = 19;
      const int ACCELEROMETER_Y_OFFSET = 21;
      const int ACCELEROMETER_Z_OFFSET = 23;

      //Save the unknown data
      Array.Copy(e.Data, UNKNOWN_DATA1_OFFSET, UnknownData1, 0, _UNKNOWN_DATA1_SIZE);

      //Get the pressure state
      if (e.Data[11] == (byte)RawPressureType.NotPressed)
        PressureType = TabletPressureType.NotPressed;
      else if (e.Data[11] == (byte)RawPressureType.PenPressed)
        PressureType = TabletPressureType.PenPressed;
      else if (e.Data[11] == (byte)RawPressureType.FingerPressed)
        PressureType = TabletPressureType.FingerPressed;
      else
        PressureType = TabletPressureType.Multitouch;

      //Get the pen pressure
      PenPressure = (ushort)(e.Data[PEN_PRESSURE_OFFSET] | (e.Data[PEN_PRESSURE_OFFSET+1] << 8));

      //Get the multitouch distance
      _multitouchTimer++;
      if (_multitouchTimer > _MULTITOUCH_SENSITIVITY)
      {
        _multitouchTimer = 0;
        _previousMultitouchDistance = MultitouchDistance;
      }
      MultitouchDistance = e.Data[MULTITOUCH_DISTANCE_OFFSET];

      //Get the (singular) pressure point
      PressurePoint = new Point(e.Data[PRESSURE_DATA_OFFSET] * 0x100 + e.Data[PRESSURE_DATA_OFFSET+2],
        e.Data[PRESSURE_DATA_OFFSET+1] * 0x100 + e.Data[PRESSURE_DATA_OFFSET+3]);

      //Get the accelerometer data
      AccelerometerData.XAxis = (ushort)(e.Data[ACCELEROMETER_X_OFFSET] | (e.Data[ACCELEROMETER_X_OFFSET+1] << 8));
      AccelerometerData.YAxis = (ushort)(e.Data[ACCELEROMETER_Y_OFFSET] | (e.Data[ACCELEROMETER_Y_OFFSET+1] << 8));
      AccelerometerData.ZAxis = (ushort)(e.Data[ACCELEROMETER_Z_OFFSET] | (e.Data[ACCELEROMETER_Z_OFFSET+1] << 8));

      //Parse raw data for buttons
      bool changed = false;
      bool raw = (e.Data[0] & 0x01) > 0;
      changed |= ButtonState.SquareHeld != raw; ButtonState.SquareHeld = raw;
      raw = (e.Data[0] & 0x02) > 0;
      changed |= ButtonState.CrossHeld != raw; ButtonState.CrossHeld = raw;
      raw = (e.Data[0] & 0x04) > 0;
      changed |= ButtonState.CircleHeld != raw; ButtonState.CircleHeld = raw;
      raw = (e.Data[0] & 0x08) > 0;
      changed |= ButtonState.TriangleHeld != raw; ButtonState.TriangleHeld = raw;
      raw = (e.Data[1] & 0x01) > 0;
      changed |= ButtonState.SelectHeld != raw; ButtonState.SelectHeld = raw;
      raw = (e.Data[1] & 0x02) > 0;
      changed |= ButtonState.StartHeld != raw; ButtonState.StartHeld = raw;
      raw = (e.Data[1] & 0x10) > 0;
      changed |= ButtonState.PSHeld != raw; ButtonState.PSHeld = raw;

      if (changed && ButtonStateChanged != null)
        ButtonStateChanged(this, EventArgs.Empty);

      //Now parse raw data for D-pad changes
      changed = false;
      raw = (e.Data[2] == 0x0);
      changed |= DPadState.UpHeld != raw; DPadState.UpHeld = raw;
      raw = (e.Data[2] == 0x1);
      changed |= DPadState.UpRightHeld != raw; DPadState.UpRightHeld = raw;
      raw = (e.Data[2] == 0x2);
      changed |= DPadState.RightHeld != raw; DPadState.RightHeld = raw;
      raw = (e.Data[2] == 0x3);
      changed |= DPadState.DownRightHeld != raw; DPadState.DownRightHeld = raw;
      raw = (e.Data[2] == 0x4);
      changed |= DPadState.DownHeld != raw; DPadState.DownHeld = raw;
      raw = (e.Data[2] == 0x5);
      changed |= DPadState.DownLeftHeld != raw; DPadState.DownLeftHeld = raw;
      raw = (e.Data[2] == 0x6) ;
      changed |= DPadState.LeftHeld != raw; DPadState.LeftHeld = raw;
      raw = (e.Data[2] == 0x7);
      changed |= DPadState.UpLeftHeld != raw; DPadState.UpLeftHeld = raw;

      if (changed && DPadStateChanged != null)
        DPadStateChanged(this, EventArgs.Empty);
    }

    #endregion
  }
}
