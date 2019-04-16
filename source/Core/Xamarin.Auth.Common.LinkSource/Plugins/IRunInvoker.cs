using System;

namespace Plugin.Threading
{
    public partial interface IRunInvoker
    {

        void BeginInvokeOnUIThread(Action action);
    }
}