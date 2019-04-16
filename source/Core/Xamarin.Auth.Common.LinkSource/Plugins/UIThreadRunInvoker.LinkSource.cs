using System;

namespace Plugin.Threading
{
    public partial class UIThreadRunInvoker
    {
        public void BeginInvokeOnUIThread(Action action)
        {
            #if PORTABLE                          // Portable Class Library PCL
            throw new NotSupportedException("Calling on Main UI Thread from PCL not supported");
            #else
            throw new NotSupportedException("Calling on Main UI Thread from undefined platform not supported");
            #endif

            //return;
        }
    }
}