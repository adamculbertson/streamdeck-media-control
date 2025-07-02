using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using BarRaider.SdTools;
using BarRaider.SdTools.Wrappers;
using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StreamDeckMediaControl;

// Helper that handles all the action classes. All the action does is define a ButtonType, while the helper does the rest.

public class PluginSettings
{
    public static PluginSettings CreateDefaultSettings()
    {
        return new PluginSettings
        {
            ApplicationName = string.Empty,
            ApplicationVolume = 50,
            ApplicationVolumeSize = 24,
            ApplicationVolumeChg = 10
        };
    }

    [JsonProperty(PropertyName = "applicationName")]
    public string? ApplicationName { get; set; }
        
    [JsonProperty(PropertyName = "applicationVolume")]
    public float ApplicationVolume { get; set; }
    
    [JsonProperty(PropertyName = "applicationVolumeChg")]
    public int ApplicationVolumeChg { get; set; }

    // Font size of the volume
    [JsonProperty(PropertyName = "applicationVolumeSize")]
    public int ApplicationVolumeSize { get; set; }
}


[SuppressMessage("ReSharper", "UnusedParameter.Global")] // The unused parameters are from the StreamDeck Tools library
public class PluginHelper
{
    private readonly PluginSettings? _settings;
    private readonly ButtonActionType _buttonType;
    private readonly ISDConnection _connection;
    
    // Actions that will update the button
    // Currently, these are just the various AppVolume* ones
    private static readonly HashSet<ButtonActionType> AppVolumeActions =
    [
        ButtonActionType.AppVolumeUp,
        ButtonActionType.AppVolumeDown,
        ButtonActionType.AppVolumeMute,
        ButtonActionType.AppVolumeSet
    ];
    
    private readonly MMDeviceEnumerator _enumerator = new();
    private DateTime? _keyTime;

    public PluginHelper(InitialPayload payload, ButtonActionType buttonType,
        ISDConnection connection)
    {
        _settings = payload.Settings == null ? PluginSettings.CreateDefaultSettings() : payload.Settings.ToObject<PluginSettings>();
        _buttonType = buttonType;
        _connection = connection;
        
        connection.OnPropertyInspectorDidAppear += OnPropertyInspectorDidAppear!;
        connection.OnSendToPlugin += OnSendToPlugin!;
        
        // Currently undefined callbacks
        // Need to also be implemented in Dispose() once added here
        //connection.OnApplicationDidLaunch += OnApplicationDidLaunch!;
        //connection.OnApplicationDidTerminate += OnApplicationDidTerminate!;
        //connection.OnDeviceDidConnect += OnDeviceDidConnect!;
        //connection.OnDeviceDidDisconnect += OnDeviceDidDisconnect!;
        //connection.OnPropertyInspectorDidDisappear += OnPropertyInspectorDidDisappear!;
        //connection.OnTitleParametersDidChange += OnTitleParametersDidChange!;
    }
    
    public void KeyPressed(KeyPayload payload)
    {
        _keyTime = DateTime.Now;
        try
        {
            // Check the type of action to perform
            if (AppVolumeActions.Contains(_buttonType))
            {
                if(_settings == null) return;
                // Application volume control action
                // This requires the ApplicationName to be set in the settings
                if (string.IsNullOrEmpty(_settings.ApplicationName))
                    return;

                var session = FindSession(_settings.ApplicationName);

                // Check for and handle the AppVolume* actions
                // Since this is ONLY for those actions, ignore the ReSharper check for missing enums
                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (_buttonType)
                {
                    case ButtonActionType.AppVolumeUp:
                        AppVolumeUp(session);
                        break;
                    case ButtonActionType.AppVolumeDown:
                        AppVolumeDown(session);
                        break;
                    case ButtonActionType.AppVolumeMute:
                        AppVolumeMute(session);
                        break;
                    case ButtonActionType.AppVolumeSet:
                        AppVolumeSet(session);
                        break;
                }

                return;
            }

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (_buttonType)
            {
                case ButtonActionType.PlayPause:
                    Multimedia.PlayPause();
                    break;
                case ButtonActionType.Play:
                    Multimedia.Play();
                    break;
                case ButtonActionType.Pause:
                    Multimedia.Pause();
                    break;
                case ButtonActionType.Stop:
                    Multimedia.Stop();
                    break;
                case ButtonActionType.Next:
                    Multimedia.Next();
                    break;
                case ButtonActionType.Previous:
                    Multimedia.Previous();
                    break;
                case ButtonActionType.MediaVolumeUp:
                    Multimedia.VolumeUp();
                    break;
                case ButtonActionType.MediaVolumeDown:
                    Multimedia.VolumeDown();
                    break;
                case ButtonActionType.MediaVolumeMute:
                    Multimedia.VolumeMute();
                    break;
            }
        }
        catch (Exception e)
        {
            Logger.Instance.LogMessage(TracingLevel.ERROR, $"Caught exception: {e}");
        }   
    }

    public void KeyReleased(KeyPayload payload)
    {
        if (!_keyTime.HasValue) return;

        var duration = DateTime.Now - _keyTime.Value;
        Logger.Instance.LogMessage(TracingLevel.INFO, $"Key was held for {duration.TotalMilliseconds} ms");
        _keyTime = null;
    }

    public void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        if (payload.Settings.TryGetValue("applicationVolume", out var token) &&
            token.Type is JTokenType.Float or JTokenType.Integer)
        {
            var value = Math.Clamp(token.Value<float>(), 0f, 100f);
            payload.Settings["applicationVolume"] = value;
            
        }
        
        Logger.Instance.LogMessage(TracingLevel.INFO, $"Received settings: {payload.Settings}");

        Tools.AutoPopulateSettings(_settings, payload.Settings);
        SaveSettings();
    }

    public static void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        Logger.Instance.LogMessage(TracingLevel.INFO, "Received global settings, but none are implemented");

    }
    
    public async void OnTick()
    {
        try
        {
            // Called every second to update the display of the button, or other tasks
            // Don't perform any actions if the settings aren't defined or the ApplicationName is null/an empty string
            if (_settings == null) return;
            if (string.IsNullOrEmpty(_settings.ApplicationName)) return;
        
            // If the button is not one that is in AppVolumeActions, then don't do anything
            if (!AppVolumeActions.Contains(_buttonType)) return;

            // Attempt to locate the session for the application name. If not found, then don't do anything
            var session = FindSession(_settings.ApplicationName);
            if (session == null) return;
            
            var volume = (int)Math.Floor(session.SimpleAudioVolume.Volume * 100); // Get the volume as a rounded down int

            // Draw the volume level onto the button
            using Image image = Tools.GenerateGenericKeyImage(out var graphics);
            graphics.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, image.Width, image.Height);

            //var tp = new TitleParameters(new FontFamily("Arial"), FontStyle.Bold, 18, Color.White, true, TitleVerticalAlignment.Middle);
            //graphics.AddTextPath(tp, image.Height, image.Width, $"{volume}%");
            var text = $"{volume}%";
            var font = new Font("Arial", _settings.ApplicationVolumeSize, FontStyle.Bold);
            var textSize = graphics.MeasureString(text, font);
            var x = (image.Width - textSize.Width) / 2;
            var y = (image.Height - textSize.Height) / 2 - 3; // Raise by 3 pixels

            graphics.DrawString(text, font, Brushes.White, x, y);

            font.Dispose();
            graphics.Dispose();

            await _connection.SetImageAsync(image);
        }
        catch (Exception e)
        {
            Logger.Instance.LogMessage(TracingLevel.ERROR, $"Caught exception: {e}");
        }        
    }

    public void Dispose()
    {
        Logger.Instance.LogMessage(TracingLevel.INFO, "Destructor called");
        // Remove callbacks to the Stream Deck SDK events (and disable CS8622 nullability warnings)
        _connection.OnPropertyInspectorDidAppear += OnPropertyInspectorDidAppear!;
        _connection.OnSendToPlugin += OnSendToPlugin!;
    }

    private void OnSendToPlugin(object sender, SDEventReceivedEventArgs<BarRaider.SdTools.Events.SendToPlugin> e)
    {
        // Send the list of audio applications to the Property Inspector
        if (e.Event.Payload["event"]?.ToString() == "setSessionNames")
        {
            SendSessionNames();
        }

        if (e.Event.Payload["event"]?.ToString() == "getSettingsFields")
        {
            GetSettings();
        }
        // Below are any items which will not work when settings are null

        // Currently have no items that will not work when settings are null
        //if (_settings != null)

    }

    private void OnPropertyInspectorDidAppear(object sender,
        SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidAppear> e)
    {
        GetSettings();
    }
    
    private void SaveSettings()
    {
        _connection.SetSettingsAsync(JObject.FromObject(_settings!));
    }
    
    [SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault")]
    private void GetSettings()
    {
        var payload = new JObject() 
        {
            ["event"] = "getSettingsFields",
        };
        var settingsFields = new JArray();

        switch (_buttonType)
        {
            // Actions that don't require settings will return an empty string
            case ButtonActionType.PlayPause:
            case ButtonActionType.Play:
            case ButtonActionType.Pause:
            case ButtonActionType.Stop:
            case ButtonActionType.Previous:
            case ButtonActionType.Next:
            case ButtonActionType.MediaVolumeUp:
            case ButtonActionType.MediaVolumeDown:
            case ButtonActionType.MediaVolumeMute:
                payload["settingsFields"] = settingsFields;
                _connection.SendToPropertyInspectorAsync(payload);
                return;
        }
        
        // Common settings among the AppVolume* buttons, the application name and font size
        var settingsAppName = new JObject()
        {
            ["type"] = "dropdown",
            ["name"] = "applicationName",
            ["label"] = "Audio Application",
            ["value"] = _settings?.ApplicationName,
            ["options"] = new JArray(GetSessionNames())
        };
        
        var settingsAppSize = new JObject
        {
            ["type"] = "range",
            ["name"] = "applicationVolumeSize",
            ["label"] = "App Volume Font Size",
            ["value"] = _settings?.ApplicationVolumeSize ?? 1,
            ["range"] = new JObject
            {
                ["min"] = 1,
                ["max"] = 50
            }
        };

        var refreshApps = new JObject()
        {
            ["type"] = "button",
            ["name"] = "refreshButton",
            ["label"] = "Refresh Applications",
            ["event"] = "setSessionNames",
            ["eventItem"] = "applicationName"
        };
        
        settingsFields.Add(settingsAppName);
        settingsFields.Add(refreshApps);
        settingsFields.Add(settingsAppSize);

        switch (_buttonType) {
            // Actions that do require settings
            // Volume up and down both share the same increment
            case ButtonActionType.AppVolumeUp:
            case ButtonActionType.AppVolumeDown:
                
                var label = _buttonType == ButtonActionType.AppVolumeUp ? "Increase volume by" : "Decrease volume by";

                var settingsAppVolume = new JObject
                {
                    ["type"] = "range",
                    ["name"] = "applicationVolumeChg",
                    ["label"] = label,
                    ["value"] = _settings?.ApplicationVolumeChg ?? 0,
                    ["range"] = new JObject
                    {
                        ["min"] = 0,
                        ["max"] = 100
                    }
                };
                
                settingsFields.Add(settingsAppVolume);
                break;                

            // Mute only needs the application name and font size
            case ButtonActionType.AppVolumeMute:
                break;

            // Volume Set needs the application name, font size, and the value to set to
            case ButtonActionType.AppVolumeSet:
                var settingsVolumeSet = new JObject
                {
                    ["type"] = "range",
                    ["name"] = "applicationVolume",
                    ["label"] = "Set Application Volume",
                    ["value"] = _settings?.ApplicationVolume ?? 0,
                    ["range"] = new JObject
                    {
                        ["min"] = 0,
                        ["max"] = 100
                    }
                };
                
                settingsFields.Add(settingsVolumeSet);
                break;
         }

        payload["settingsFields"] = settingsFields;
        _connection.SendToPropertyInspectorAsync(payload);
    }
    
    private AudioSessionControl? FindSession(string appName)
    {
        // Locate the audio session based on the given app name
        var device = _enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        var sessions = device.AudioSessionManager.Sessions;
        
        for (var i = 0; i < sessions.Count; i++)
        {
            var session = sessions[i];
            //var pid = session.GetProcessID;
            var name = session.DisplayName;
            //appVolume = session.SimpleAudioVolume.Volume * 100;
            
            // Check that the process name matches the given name
            if (string.Equals(name, appName, StringComparison.CurrentCultureIgnoreCase))
            {
                return session;
            }
        }

        // No match found
        return null;
    }
    
    private List<string> GetSessionNames()
    {
        // Get the audio sessions as a list to send to the Property Inspector
        List<string> output = [];
        
        var device = _enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        var sessions = device.AudioSessionManager.Sessions;

        for (var i = 0; i < sessions.Count; i++)
        {
            var session = sessions[i];
            var pid = session.GetProcessID;
            var name = session.DisplayName;

            // If no display name is available, use the PID for the application's name
            if (string.IsNullOrEmpty(name))
            {
                name = $"{pid}";
            }
            
            output.Add(name);
        }
        return output;
    }

    private void SendSessionNames()
    {
        // Send the list of audio sessions to the Property Inspector
        var sessions = GetSessionNames();
        var json = new JObject
        {
            ["event"] = "setSessionNames",
            ["value"] = _settings?.ApplicationName,
            ["options"] = new JArray(sessions)
        };
        _connection.SendToPropertyInspectorAsync(json);
    }
    
    private static void AppVolumeUp(AudioSessionControl? session)
    {
        if (session == null) return;
        Logger.Instance.LogMessage(TracingLevel.DEBUG, "AppVolumeUp()");


        // Increase the volume by 10%
        var volume = Math.Clamp(session.SimpleAudioVolume.Volume + 0.1f, 0f, 1f); // Keep the value between 0 and 1
        session.SimpleAudioVolume.Volume = volume; //Raise the volume by 10%
        Logger.Instance.LogMessage(TracingLevel.DEBUG, $"AppVolumeUp() set volume to {volume}");
    }

    private static void AppVolumeDown(AudioSessionControl? session)
    {
        if (session == null) return;

        // Decrease the volume by 10%
        var volume = Math.Clamp(session.SimpleAudioVolume.Volume - 0.1f, 0f, 1f); // Keep the value between 0 and 1
        session.SimpleAudioVolume.Volume = volume;
        Logger.Instance.LogMessage(TracingLevel.DEBUG, $"AppVolumeDown() set volume to {volume}");
    }

    private static void AppVolumeMute(AudioSessionControl? session)
    {
        if(session == null) return;
        Logger.Instance.LogMessage(TracingLevel.DEBUG, "AppVolumeMute()");


        // Toggle the mute function
        session.SimpleAudioVolume.Mute = !session.SimpleAudioVolume.Mute;
    }

    private void AppVolumeSet(AudioSessionControl? session)
    {
        if (session == null || _settings == null) return;

        var newVolume = _settings.ApplicationVolume / 100.0f;
        session.SimpleAudioVolume.Volume = newVolume;
        Logger.Instance.LogMessage(TracingLevel.DEBUG, $"AppVolumeSet() set volume to {newVolume}");
    }
}