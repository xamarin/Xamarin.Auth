# Details

## JSch Using

Simple method to start SSH shell and execute command:

        private void Execute (string command)
		{
			string value = null;

			System.Threading.Tasks.Task<string> t = null;
			t = System.Threading.Tasks.Task.Run
            (
            	()
            	=>
				{
					jsch = new JCraft.JSch.JSch();
		
				
					config = new Java.Util.Properties();
					// Avoid asking for key confirmation
					config.SetProperty("StrictHostKeyChecking", "no");
					config.SetProperty("compression.s2c", "zlib,none");
					config.SetProperty("compression.c2s", "zlib,none");

					string OnProvideAssistData = null;
					JCraft.JSch.Session session = null;
					try 
					{
						session = jsch.GetSession(username, ip_address, port);
						session.SetConfig(config);
						session.SetPassword(pwd);
						session.Connect();
					} 
					catch (JCraft.JSch.JSchException e) 
					{
						asd = "NOT_Executed";
						System.Console.WriteLine("NOT_executed");
						System.Console.WriteLine(e.Message);
						value = asd;

						return value;
					}          

					JCraft.JSch.ChannelExec channel = null;
					channel = (JCraft.JSch.ChannelExec)session.OpenChannel("exec");
					channel.SetCommand(command);
					channel.Connect();

					//InputStream input = channel.getInputStream();
					System.IO.Stream input = channel.InputStream;

					using (var reader = new System.IO.StreamReader(input, System.Text.Encoding.UTF8))
					{
						value = reader.ReadToEnd();
						// Do something with the value
					}
					channel.Disconnect();

					return value;
				}
			);

			value = t.Result;

			editTextCommandOutput.Text = value;

			return;
		}


Note: JSch usage on UI thread will cause exception, thus async/await Task.Run();
 

###

