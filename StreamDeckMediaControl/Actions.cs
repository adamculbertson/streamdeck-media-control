using BarRaider.SdTools;
using JetBrains.Annotations;

namespace StreamDeckMediaControl;

/*
 * TODO:
 *  - Dynamic PropertyInspector settings, something like:
 *    -  [{"type": "dropdown", "name": "option1", "options": {"value 1", "value 2"}, {"type": "text", "name": "option 2", "value": "default value"}]
 *    - Only show the settings if it applies to the given button type
 *  - Separate actions for single press and long press (such as previous track for long, next track for short)
 */

// Defines all the actions for the plugin, but does not handle th actual logic
// The logic is handled in the PluginHelper class

[PluginActionId("me.adamculbertson.mediacontrol.playpause")]
[UsedImplicitly]
public class ActionPlayPause(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.PlayPause;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.play")]
[UsedImplicitly]
public class ActionPlay(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.Play;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.pause")]
[UsedImplicitly]
public class ActionPause(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const  ButtonActionType ButtonType = ButtonActionType.Pause;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }}

[PluginActionId("me.adamculbertson.mediacontrol.stop")]
[UsedImplicitly]
public class ActionStop(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const  ButtonActionType ButtonType = ButtonActionType.Stop;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.previous")]
[UsedImplicitly]
public class ActionPrevious(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.Previous;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.next")]
[UsedImplicitly]
public class ActionNext(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const  ButtonActionType ButtonType = ButtonActionType.Next;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.volup")]
[UsedImplicitly]
public class ActionVolumeUp(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.MediaVolumeUp;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.voldown")]
[UsedImplicitly]
public class ActionVolumeDown(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.MediaVolumeDown;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.mute")]
[UsedImplicitly]
public class ActionVolumeMute(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.MediaVolumeMute;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.appvolup")]
[UsedImplicitly]
public class ActionAppVolumeUp(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType  ButtonType = ButtonActionType.AppVolumeUp;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.appvoldown")]
[UsedImplicitly]
public class ActionAppVolumeDown(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType  ButtonType = ButtonActionType.AppVolumeDown;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.appmute")]
[UsedImplicitly]
public class ActionAppVolumeMute(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.AppVolumeMute;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}

[PluginActionId("me.adamculbertson.mediacontrol.appvolset")]
[UsedImplicitly]
public class ActionAppVolumeSet(ISDConnection connection, InitialPayload payload) : KeypadBase(connection, payload)
{
    private readonly PluginHelper _helper = new(payload, ButtonType, connection);
    private const ButtonActionType ButtonType = ButtonActionType.AppVolumeSet;

    public override void KeyPressed(KeyPayload payload)
    {
        _helper.KeyPressed(payload);
    }

    public override void KeyReleased(KeyPayload payload)
    {
        _helper.KeyReleased(payload);
    }

    public override void ReceivedSettings(ReceivedSettingsPayload payload)
    {
        _helper.ReceivedSettings(payload);
    }

    public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
    {
        PluginHelper.ReceivedGlobalSettings(payload);
    }

    public override void OnTick()
    {
        _helper.OnTick();
    }

    public override void Dispose()
    {
        _helper.Dispose();
        GC.SuppressFinalize(this);
    }
}