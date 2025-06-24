using System.Runtime.InteropServices;

namespace StreamDeckMediaControl;

public static class Multimedia
{
    private const int WmAppCommand = 0x0319;

    // Constants for media commands
    private const int AppCommandVolumeMute = 8;
    private const int AppCommandVolumeDown = 9;
    private const int AppCommandVolumeUp = 10;
    private const int AppCommandMediaPlay = 46;
    private const int AppCommandMediaPause = 47;
    private const int AppCommandMediaNextTrack = 11;
    private const int AppCommandMediaPreviousTrack = 12;
    private const int AppCommandMediaStop = 13;
    private const int AppCommandMediaPlayPause = 14;

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    private static void SendAppCommand(int appCommand)
    {
        // Converted from an AutoHotKey script
        var handle = FindWindow("Shell_TrayWnd", null);
        if (handle == IntPtr.Zero)
        {
            Console.WriteLine("Target window not found.");
            return;
        }

        IntPtr lParam = appCommand << 16;
        PostMessage(handle, WmAppCommand, IntPtr.Zero, lParam);
    }
    

    public static void PlayPause()
    {
        SendAppCommand(AppCommandMediaPlayPause);
    }

    public static void Play()
    {
        SendAppCommand(AppCommandMediaPlay);
    }

    public static void Pause()
    {
        SendAppCommand(AppCommandMediaPause);
    }

    public static void Stop()
    {
        SendAppCommand(AppCommandMediaStop);
    }
    
    public static void Previous()
    {
        SendAppCommand(AppCommandMediaPreviousTrack);
    }

    public static void Next()
    {
        SendAppCommand(AppCommandMediaNextTrack);
    }

    public static void VolumeUp()
    {
        SendAppCommand(AppCommandVolumeUp);
    }

    public static void VolumeDown()
    {
        SendAppCommand(AppCommandVolumeDown);
    }

    public static void VolumeMute()
    {
        SendAppCommand(AppCommandVolumeMute);
    }
}