# ToggleComment 2022
[![Build status](https://ci.appveyor.com/api/projects/status/github/talkitron/toggleComment?svg=true)](https://ci.appveyor.com/project/talKitron/togglecomment)

Toggle Comment 2022 is a simple visual studio extension based on the [ToggleComment by munyabe](https://marketplace.visualstudio.com/items?itemName=munyabe.ToggleComment) to comment out/uncomment the selected lines. This is the same feature as "Ctrl + /" works in Eclipse.

## Usage
Select lines, and execute the command. You can do it as follows.
![Code](https://github.com/talKitron/ToggleComment/blob/master/readme/before.png?raw=true)
↓
![Comment](https://github.com/talKitron/ToggleComment/blob/master/readme/after1.png?raw=true)
↓
![Comment out](https://github.com/talKitron/ToggleComment/blob/master/readme/after2.png?raw=true)

## Supported Languages
C, C++, C#, Visual Basic, F#, HTML, ASPX, Razor on ASP&#46;NET MVC, XAML, XML, JavaScript, TypeScript, PowerShell, Python, SQL.

## Key Bindings
The default settings are as follows. 

- Toggle Comment: Ctrl + /
If the shortcut is not registered, please set it as "TOOLS" -> "Options" -> "Environment" -> "Keyboard".
The command name is "Edit.ToggleComment".

## Menu Placement
- "Edit" -> "Advances" -> "Toggle Comment"
![Menu placement](https://github.com/talKitron/ToggleComment/blob/master/readme/menu_place.png?raw=true)

## Change Log
- v2.0
	- Support Visual Studio 2022.
	- Translated documentation into English.
- v1.8
	- Support Visual Studio 2019.
- v1.7
	- Fix a bug that it is not working with VS2015 or earlier.
- v1.6
	- Support Visual Studio 2017.
	- (Support Visual Studio 2010 has ended. For the 2010 version, please use the v1.5 in Github.)
- v1.5
	- Fix a bag about the narrow cursor at the start of a line.
- v1.4
	- Fix a bag that the command might not be executed repeatedly with HTML file.
- v1.3
	- Support Python, Razor on ASP&#46;NET MVC.
	- Fix a behavior when only a part of the line was selected.
	- Fix a selection state of the box selection mode.
	- Fix a bag that an exception were thrown when Visual Studio original command was invalid.
- v1.2
	- Support C, C++, F#, ASPX, TypeScript.
	- Change the non-support files behavior to "Comment Selection".
	- Fix a bug that one line too many commented.
- v1.1
	- Support C# block comment, XAL, XAML, PowerShell, SQL, HTML, JavaScript, CSS and Visual Basic.
- v1.0
	- Initial release !

## Additional Information
See [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=munyabe.ToggleComment) for additional info on how to use this extension.
