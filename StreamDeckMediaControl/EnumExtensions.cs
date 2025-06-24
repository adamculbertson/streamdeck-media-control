namespace StreamDeckMediaControl;

public enum ButtonActionType
{
    [ButtonLabel("Play / Pause")]
    PlayPause,
    [ButtonLabel("Play")]
    Play,
    [ButtonLabel("Pause")]
    Pause,
    [ButtonLabel("Stop")]
    Stop,
    [ButtonLabel("Previous")]
    Previous,
    [ButtonLabel("Next")]
    Next,
    [ButtonLabel("Volume Up")]
    MediaVolumeUp,
    [ButtonLabel("Volume Down")]
    MediaVolumeDown,
    [ButtonLabel("Mute")]
    MediaVolumeMute,
    [ButtonLabel("Volume Up (Application)")]
    AppVolumeUp,
    [ButtonLabel("Volume Down (Application)")]
    AppVolumeDown,
    [ButtonLabel("Mute (Application)")]
    AppVolumeMute,
    [ButtonLabel("Set Volume (Application)")]
    AppVolumeSet
}

[AttributeUsage(AttributeTargets.Field)]
public class ButtonLabelAttribute(string label) : Attribute
{
    public string Label { get; } = label;
}

public static class EnumExtensions
{
    private static readonly Dictionary<string, ButtonActionType> LabelToEnumMap =
        Enum.GetValues<ButtonActionType>()
            .ToDictionary(e => e.GetLabel(), e => e);
    public static string GetLabel(this ButtonActionType action)
    {
        var type = action.GetType();
        var member = type.GetMember(action.ToString());

        if (member.Length <= 0) return action.ToString();
        var attrs = member[0].GetCustomAttributes(typeof(ButtonLabelAttribute), false);
        return attrs.Length > 0 ? ((ButtonLabelAttribute)attrs[0]).Label : action.ToString();
    }
    
    public static ButtonActionType? FromLabel(string label)
    {
        if (LabelToEnumMap.TryGetValue(label, out var result)) return result;
        return null;
    }
}