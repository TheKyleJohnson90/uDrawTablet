using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawLib
{
  public struct TabletDPadState
  {
    public bool UpHeld;
    public bool DownHeld;
    public bool LeftHeld;
    public bool RightHeld;

    //diagonal
    public bool UpLeftHeld;
    public bool DownLeftHeld;
    public bool DownRightHeld;
    public bool UpRightHeld;

  };
}
