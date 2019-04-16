using System;
namespace ComicBook
{
    public static class Settings
    {
        static Settings()
        {
            
        }

        public static bool IsUsingNativeUI
        {
            get;
            set;
        } = true;

        public static bool IsFormsImplementationRenderers
        {
        	get;
        	set;
        } = false;

        public static bool IsFormsImplementationPresenters
        {
        	get;
        	set;
        } = true;

        public static bool IsFormsNavigationPushModal
        {
        	get;
        	set;
        } = true;

        public static bool IsFormsNavigationPush
        {
        	get;
        	set;
        } = true;

        public static bool IsIOSUsingWKWebView
        {
        	get;
        	set;
        } = true;

    }

}
