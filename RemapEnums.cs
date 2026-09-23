namespace YMM4ChannelRemap;

/// <summary>
/// チャンネルリマップのモード
/// </summary>
public enum RemapMode
{
    RGB,
    HSV,
}

/// <summary>
/// RGBモードでの出力チャンネルの元ネタ選択。
/// シェーダー側のselector値と一致させるため、明示的に値を割り当てている。
/// </summary>
public enum RgbChannelSource
{
    R = 0,
    G = 1,
    B = 2,
    A = 3,
    Y = 4,
    Zero = 5,
    One = 6,
}

/// <summary>
/// HSVモードでの出力チャンネルの元ネタ選択。
/// シェーダー側のselector値と一致させるため、明示的に値を割り当てている（4=YはHSVでは未使用のため欠番）。
/// </summary>
public enum HsvChannelSource
{
    H = 0,
    S = 1,
    V = 2,
    A = 3,
    Zero = 5,
    One = 6,
}
