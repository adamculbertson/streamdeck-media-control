# Stream Deck Media Control
Stream Deck plugin to control multimedia playback and adjust volume either globally or based on a selected application.

## Purpose
While the Stream Deck does have its own Volume Mixer style app, I wasn't really satisfied with it. For one, the multimedia controls simply send the key press. This does not work when in a Hyper-V VM or RDP session, which I use often. I also have an input device with the "Listen to this device" setting enabled. Most volume mixer applications do not show this item for some reason. Windows sometimes likes to reset its volume, so I use a button to quickly set it back to what I want it at.

## Development
The plugin does basically work, but there are still things I want to improve and do better. For one, the settings in the Property Inspector are a huge mess. Getting that working better is my first priority.

## Libraries
Uses BarRaider's awesome [StreamDeck Tools](https://github.com/BarRaider/streamdeck-tools) and [EasyPI v2](https://github.com/BarRaider/streamdeck-easypi-v2)

## Easy Build
See the script `Package-App.ps1` to compile and build the Stream Deck plugin. Example usage:
```powershell
.\Package-App.ps1 -UUID "me.adamculbertson.mediacontrol" -ProjectPath "C:\Projects\StreamDeckMediaControl" -ProjectName "StreamDeckMediaControl" -OutPath "C:\Projects\build"
```
The `-Force` parameter will clear the build directory if it already exists without prompting!

## LLM Disclosure
While I did make use of an LLM to get help with the code base, I did not use it to write huge portions of the code base. It was mainly debugging, with a few lines of code that I didn't know how to do exactly. There were some issues I had during development, and I am still learning C# and .NET. Most of the optimizations were actually done using JetBrains Rider instead.

## Icons
The icons were sourced from: https://pictogrammers.com/library/mdi/  
Most of them are available under the Apache 2.0 license.