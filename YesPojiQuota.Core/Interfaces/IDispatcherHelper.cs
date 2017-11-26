using System;
using System.Collections.Generic;
using System.Text;

namespace YesPojiQuota.Core.Interfaces
{
    public interface IDispatcherHelper
    {
        void CheckBeginInvokeOnUi(Action action);
    }
}
