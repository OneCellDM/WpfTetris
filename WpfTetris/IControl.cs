using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTetris
{

    public delegate void CloseControl();
    public interface ICloseControl
    {
        event CloseControl CloseEvent;
    }
}
