using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DohrniiFoundation.IServices
{
    public interface IPlatformHelper
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
