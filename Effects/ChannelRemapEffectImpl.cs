using System.Runtime.InteropServices;
using Vortice;
using Vortice.Direct2D1;
using YukkuriMovieMaker.Player.Video;

namespace YMM4ChannelRemap.Effects;

[CustomEffect(1)]
internal sealed class ChannelRemapEffectImpl : D2D1CustomShaderEffectImplBase<ChannelRemapEffectImpl>
{
    private ConstantBuffer constantBuffer;

    public ChannelRemapEffectImpl() : base(ShaderResourceLoader.GetShaderResource("ChannelRemap.cso"))
    {
    }

    [CustomEffectProperty(PropertyType.Int32, (int)Properties.Mode)]
    public int Mode
    {
        get => constantBuffer.Mode;
        set { constantBuffer.Mode = value; UpdateConstants(); }
    }

    [CustomEffectProperty(PropertyType.Int32, (int)Properties.SelR)]
    public int SelR
    {
        get => constantBuffer.SelR;
        set { constantBuffer.SelR = value; UpdateConstants(); }
    }

    [CustomEffectProperty(PropertyType.Int32, (int)Properties.SelG)]
    public int SelG
    {
        get => constantBuffer.SelG;
        set { constantBuffer.SelG = value; UpdateConstants(); }
    }

    [CustomEffectProperty(PropertyType.Int32, (int)Properties.SelB)]
    public int SelB
    {
        get => constantBuffer.SelB;
        set { constantBuffer.SelB = value; UpdateConstants(); }
    }

    [CustomEffectProperty(PropertyType.Int32, (int)Properties.SelA)]
    public int SelA
    {
        get => constantBuffer.SelA;
        set { constantBuffer.SelA = value; UpdateConstants(); }
    }

    protected override void UpdateConstants()
    {
        drawInformation?.SetPixelShaderConstantBuffer(constantBuffer);
    }

    public override void MapInputRectsToOutputRect(
        RawRect[] inputRects,
        RawRect[] inputOpaqueSubRects,
        out RawRect outputRect,
        out RawRect outputOpaqueSubRect)
    {
        outputRect = inputRects[0];
        outputOpaqueSubRect = inputOpaqueSubRects.Length > 0
            ? inputOpaqueSubRects[0]
            : new RawRect(0, 0, 0, 0);
    }

    public override void MapOutputRectToInputRects(RawRect outputRect, RawRect[] inputRects)
    {
        inputRects[0] = outputRect;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ConstantBuffer
    {
        public int Mode;
        public int SelR;
        public int SelG;
        public int SelB;
        public int SelA;
        private int _pad0;
        private int _pad1;
        private int _pad2;
    }

    public enum Properties
    {
        Mode,
        SelR,
        SelG,
        SelB,
        SelA,
    }
}
