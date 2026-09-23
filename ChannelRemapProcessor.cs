using Vortice.Direct2D1;
using YMM4ChannelRemap.Effects;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;

namespace YMM4ChannelRemap;

internal class ChannelRemapProcessor : IVideoEffectProcessor
{
    private readonly ChannelRemapEffect item;
    private readonly ChannelRemapCustomEffect customEffect;

    public ChannelRemapProcessor(IGraphicsDevicesAndContext devices, ChannelRemapEffect item)
    {
        this.item = item;
        customEffect = new ChannelRemapCustomEffect(devices);
    }

    public ID2D1Image Output => customEffect.Output;

    public void ClearInput()
    {
        customEffect.SetInput(null);
    }

    public void SetInput(ID2D1Image? input)
    {
        customEffect.SetInput(input);
    }

    public DrawDescription Update(EffectDescription effectDescription)
    {
        if (item.Mode == RemapMode.RGB)
        {
            customEffect.SetParameters(
                RemapMode.RGB,
                (int)item.RgbOutputR,
                (int)item.RgbOutputG,
                (int)item.RgbOutputB,
                (int)item.RgbOutputA);
        }
        else
        {
            customEffect.SetParameters(
                RemapMode.HSV,
                (int)item.HsvOutputH,
                (int)item.HsvOutputS,
                (int)item.HsvOutputV,
                (int)item.HsvOutputA);
        }

        // 描画位置・拡大率などはこのエフェクトでは変更しない
        return effectDescription.DrawDescription;
    }

    public void Dispose()
    {
        customEffect.Dispose();
    }
}
