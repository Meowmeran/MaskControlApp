/// <summary>
/// Command types sent from Unity → Mask.
/// Must mirror the CommandType enum in NetworkManager.h on the ESP8266.
/// </summary>
public enum MaskCommandType : byte
{
    SetMode = 0x01,
    SetExpression = 0x02,
    SetBrightness = 0x03,
    FunctionCall = 0x04,
    SetFrame = 0x05,
    SetColor = 0x06,
}

/// <summary>
/// Mode values for SetMode commands.
/// Must mirror Core::Mode on the ESP8266.
/// </summary>
public enum MaskMode : byte
{
    Off = 3,
    Active = 0,
    Manual = 1,
    BlockGame = 2,
    ERROR = 4
}