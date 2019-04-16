# Secure / Sensitive / Private Data Folder

This folder consists of:

*	c# files with private/senstive/secret/hidden data		
	in this case for OAtuh providers for this sample	
	*	c# code is 
		*	included into project in project file (csproj) by the means of wildcards		
		*	excluded/ignored for source versioning by the means of .gitignore

## Goal and Implementation

*	goal		
	*	make samples compile clean
	*	hide private/sensitive/secret data like API keys (Client Key, Customer key)

## Project File (csproj)

    <Compile Include="Data\SecurePrivateSecretHidden\*.cs" />

## .gitignore

igonre patterns (personal):


	**/*.hidden.cs
	**/*.hidden.md
	**/*.hidden.sh
	**/*.hidden.bat
	**/*.hidden.cmd
	**/*.secure*.cs
	**/*.secure*.md
	**/*.secure*.sh
	**/*.secure*.bat
	**/*.secure*.cmd
	**/*.secret*.cs
	**/*.secret*.md
	**/*.secret*.sh
	**/*.secret*.bat
	**/*.secret*.cmd
	**/*.sensitive.cs
	**/*.sensitive.md
	**/*.sensitive.sh
	**/*.sensitive.bat
	**/*.sensitive.cmd
	**/*.private.cs
	**/*.private.md
	**/*.private.sh
	**/*.private.bat
	**/*.private.cmd


