using System;
using Vortice.Direct2D1;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;

namespace YMM4ChannelRemap.Effects;

internal sealed class ChannelRemapCustomEffect : D2D1CustomShaderEffectBase
{
    public ChannelRemapCustomEffect(IGraphicsDevicesAndContext devices)
        : base(Create<ChannelRemapEffectImpl>(devices))
    {
    }

    public void SetParameters(RemapMode mode, int r, int g, int b, int a)
    {
        SetValue((int)ChannelRemapEffectImpl.Properties.Mode, mode == RemapMode.HSV ? 1 : 0);
        SetValue((int)ChannelRemapEffectImpl.Properties.SelR, r);
        SetValue((int)ChannelRemapEffectImpl.Properties.SelG, g);
        SetValue((int)ChannelRemapEffectImpl.Properties.SelB, b);
        SetValue((int)ChannelRemapEffectImpl.Properties.SelA, a);
    }

    public void SetInput(ID2D1Image? input)
        => base.SetInput(0, input, true);
}
