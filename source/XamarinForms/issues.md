# Issues


## 2015-10-17 Visual Studio 2015 The "XamlCTask" task failed unexpectedly

Problem:

	Error		
	The "XamlCTask" task failed unexpectedly.
	System.IO.FileNotFoundException: Could not load file or assembly 'Mono.Cecil, Version=0.9.6.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756' or one of its dependencies. The system cannot find the file specified.
	File name: 'Mono.Cecil, Version=0.9.6.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756'

	Server stack trace: 
	   at Xamarin.Forms.Build.Tasks.XamlCTask.Compile()
	   at Xamarin.Forms.Build.Tasks.XamlCTask.Execute()
	   at System.Runtime.Remoting.Messaging.StackBuilderSink._PrivateProcessMessage(IntPtr md, Object[] args, Object server, Object[]& outArgs)
	   at System.Runtime.Remoting.Messaging.StackBuilderSink.SyncProcessMessage(IMessage msg)

	Exception rethrown at [0]: 
	   at System.Runtime.Remoting.Proxies.RealProxy.HandleReturnMessage(IMessage reqMsg, IMessage retMsg)
	   at System.Runtime.Remoting.Proxies.RealProxy.PrivateInvoke(MessageData& msgData, Int32 type)
	   at Microsoft.Build.Framework.ITask.Execute()
	   at Microsoft.Build.BackEnd.TaskExecutionHost.Microsoft.Build.BackEnd.ITaskExecutionHost.Execute()
	   at Microsoft.Build.BackEnd.TaskBuilder.<ExecuteInstantiatedTask>d__26.MoveNext()

	WRN: Assembly binding logging is turned OFF.
	To enable assembly bind failure logging, set the registry value [HKLM\Software\Microsoft\Fusion!EnableLog] (DWORD) to 1.
	Note: There is some performance penalty associated with assembly bind failure logging.
	To turn this feature off, remove the registry value [HKLM\Software\Microsoft\Fusion!EnableLog].	Xamarin.Auth.XamarinForms.WindowsPhone8	Y:\Projects\Xamarin\Xamarin.Auth.Collections\HolisticWare.Auth\source\packages\Xamarin.Forms.1.5.1.6455-pre1\build\portable-win+net45+wp80+win81+wpa81+MonoAndroid10+MonoTouch10+Xamarin.iOS10\Xamarin.Forms.targets	62

[Re]Solution/Workaround

Add Xamarin.Forms xaml Page to projects. This page can be deleted later on.	