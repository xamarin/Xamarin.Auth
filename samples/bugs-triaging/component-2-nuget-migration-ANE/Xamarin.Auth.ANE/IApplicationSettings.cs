using System;

namespace LeanKit.Core
{
    public interface IApplicationSettings
    {
		string AppName {get;}
		string HostName { get; set; }
		string UrlTemplate { get; set; }
    }
}