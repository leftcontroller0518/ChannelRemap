// ChannelRemap.hlsl
// YMM4「チャンネルリマップ」映像エフェクト用ピクセルシェーダー
// Direct2Dのカスタムエフェクト規約(d2d1effecthelpers.hlsli)に準拠したシンプルな1入力シェーダー。
// Shader Model 4.0 (ps_4_0) でコンパイルします。

Texture2D InputTexture : register(t0);
SamplerState InputSampler : register(s0);

// 定数バッファ（16byte境界に合わせて8個のintを確保）
// mode    : 0 = RGB, 1 = HSV
// selR/H  : 出力 R (RGBモード) / H (HSVモード) の元チャンネル選択
// selG/S  : 出力 G / S の元チャンネル選択
// selB/V  : 出力 B / V の元チャンネル選択
// selA    : 出力 A の元チャンネル選択
//
// 選択値の意味（RGBモード）: 0=R,1=G,2=B,3=A,4=Y,5=0,6=1
// 選択値の意味（HSVモード）: 0=H,1=S,2=V,3=A,     5=0,6=1
cbuffer ChannelRemapConstants : register(b0)
{
    int mode;
    int selR;
    int selG;
    int selB;
    int selA;
    int _pad0;
    int _pad1;
    int _pad2;
};

// RGB(0-1) -> HSV(0-1) 変換。S=0やRGBの最大最小が等しい場合(グレー)も含めて安全に処理する。
float3 RgbToHsv(float3 c)
{
    float maxC = max(c.r, max(c.g, c.b));
    float minC = min(c.r, min(c.g, c.b));
    float delta = maxC - minC;

    float h = 0.0;
    if (delta > 1e-6)
    {
        if (maxC == c.r)
        {
            h = ((c.g - c.b) / delta) % 6.0;
        }
        else if (maxC == c.g)
        {
            h = ((c.b - c.r) / delta) + 2.0;
        }
        else
        {
            h = ((c.r - c.g) / delta) + 4.0;
        }
        h = h / 6.0;
        if (h < 0.0)
        {
            h += 1.0;
        }
    }

    float s = (maxC <= 1e-6) ? 0.0 : (delta / maxC);
    float v = maxC;

    return float3(h, s, v);
}

// HSV(0-1) -> RGB(0-1) 変換
float3 HsvToRgb(float3 hsv)
{
    float h = saturate(hsv.x) * 6.0;
    float s = saturate(hsv.y);
    float v = saturate(hsv.z);

    float c = v * s;
    float x = c * (1.0 - abs((h % 2.0) - 1.0));
    float m = v - c;

    float3 rgb;
    if (h < 1.0)      { rgb = float3(c, x, 0.0); }
    else if (h < 2.0) { rgb = float3(x, c, 0.0); }
    else if (h < 3.0) { rgb = float3(0.0, c, x); }
    else if (h < 4.0) { rgb = float3(0.0, x, c); }
    else if (h < 5.0) { rgb = float3(x, 0.0, c); }
    else              { rgb = float3(c, 0.0, x); }

    return rgb + m;
}

// selector に応じて a,b,c,d,e のいずれかを返す
// a,b,c = 3チャンネル分の値（RGBモードはR,G,B / HSVモードはH,S,V）
// d     = A（アルファ）
// e     = Y（RGBモードのみ有効。HSVモードでは未使用=0を渡す）
float SelectValue(int selector, float a, float b, float c, float d, float e)
{
    if (selector == 0) return a;
    if (selector == 1) return b;
    if (selector == 2) return c;
    if (selector == 3) return d;
    if (selector == 4) return e;
    if (selector == 5) return 0.0;
    return 1.0; // selector == 6
}

float4 main(
    float4 pos : SV_POSITION,
    float4 posScene : SCENE_POSITION,
    float4 uv0 : TEXCOORD0) : SV_Target
{
    float4 src = InputTexture.Sample(InputSampler, uv0.xy);

    float alpha = saturate(src.a);
    float3 rgb = (alpha > 1e-6)
        ? saturate(src.rgb / alpha)
        : float3(0.0, 0.0, 0.0);

    float4 result;

    if (mode == 0)
    {
        float y = 0.299 * rgb.r + 0.587 * rgb.g + 0.114 * rgb.b;

        float outR = SelectValue(selR, rgb.r, rgb.g, rgb.b, alpha, y);
        float outG = SelectValue(selG, rgb.r, rgb.g, rgb.b, alpha, y);
        float outB = SelectValue(selB, rgb.r, rgb.g, rgb.b, alpha, y);
        float outA = SelectValue(selA, rgb.r, rgb.g, rgb.b, alpha, y);

        result = float4(outR, outG, outB, outA);
    }
    else
    {
        float3 hsv = RgbToHsv(rgb);

        float outH = SelectValue(selR, hsv.x, hsv.y, hsv.z, alpha, 0.0);
        float outS = SelectValue(selG, hsv.x, hsv.y, hsv.z, alpha, 0.0);
        float outV = SelectValue(selB, hsv.x, hsv.y, hsv.z, alpha, 0.0);
        float outA = SelectValue(selA, hsv.x, hsv.y, hsv.z, alpha, 0.0);

        float3 newRgb = HsvToRgb(float3(outH, outS, outV));
        result = float4(newRgb, outA);
    }

    result.rgb = saturate(result.rgb);
    result.a = saturate(result.a);

    result.rgb *= result.a;

    return result;
}