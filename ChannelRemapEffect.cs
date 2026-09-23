using System.ComponentModel.DataAnnotations;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin.Effects;

namespace YMM4ChannelRemap;

[VideoEffect("チャンネルリマップ", ["加工"], ["カラーリマップ", "AlightMotion風", "HSV"])]
internal class ChannelRemapEffect : VideoEffectBase
{
    public override string Label => "チャンネルリマップ";

    [Display(GroupName = "基本設定", Name = "モード", Description = "RGBまたはHSVでのチャンネルリマップを選択します")]
    [EnumComboBox]
    public RemapMode Mode { get; set => Set(ref field, value); } = RemapMode.RGB;

    // ---- RGBモード設定 ----

    [Display(GroupName = "RGBモード", Name = "出力R", Description = "出力する赤チャンネルの元ネタ（RGBモード時のみ使用）")]
    [EnumComboBox]
    public RgbChannelSource RgbOutputR { get; set => Set(ref field, value); } = RgbChannelSource.R;

    [Display(GroupName = "RGBモード", Name = "出力G", Description = "出力する緑チャンネルの元ネタ（RGBモード時のみ使用）")]
    [EnumComboBox]
    public RgbChannelSource RgbOutputG { get; set => Set(ref field, value); } = RgbChannelSource.G;

    [Display(GroupName = "RGBモード", Name = "出力B", Description = "出力する青チャンネルの元ネタ（RGBモード時のみ使用）")]
    [EnumComboBox]
    public RgbChannelSource RgbOutputB { get; set => Set(ref field, value); } = RgbChannelSource.B;

    [Display(GroupName = "RGBモード", Name = "出力A", Description = "出力する不透明度チャンネルの元ネタ（RGBモード時のみ使用）")]
    [EnumComboBox]
    public RgbChannelSource RgbOutputA { get; set => Set(ref field, value); } = RgbChannelSource.A;

    // ---- HSVモード設定 ----

    [Display(GroupName = "HSVモード", Name = "出力H", Description = "出力する色相チャンネルの元ネタ（HSVモード時のみ使用）")]
    [EnumComboBox]
    public HsvChannelSource HsvOutputH { get; set => Set(ref field, value); } = HsvChannelSource.H;

    [Display(GroupName = "HSVモード", Name = "出力S", Description = "出力する彩度チャンネルの元ネタ（HSVモード時のみ使用）")]
    [EnumComboBox]
    public HsvChannelSource HsvOutputS { get; set => Set(ref field, value); } = HsvChannelSource.S;

    [Display(GroupName = "HSVモード", Name = "出力V", Description = "出力する明度チャンネルの元ネタ（HSVモード時のみ使用）")]
    [EnumComboBox]
    public HsvChannelSource HsvOutputV { get; set => Set(ref field, value); } = HsvChannelSource.V;

    [Display(GroupName = "HSVモード", Name = "出力A", Description = "出力する不透明度チャンネルの元ネタ（HSVモード時のみ使用）")]
    [EnumComboBox]
    public HsvChannelSource HsvOutputA { get; set => Set(ref field, value); } = HsvChannelSource.A;

    public override IEnumerable<string> CreateExoVideoFilters(int keyFrameIndex, ExoOutputDescription exoOutputDescription)
    {
        // カスタムGPUシェーダーを使用するため、AviUtl(.exo)出力には対応しない
        return [];
    }

    public override IVideoEffectProcessor CreateVideoEffect(IGraphicsDevicesAndContext devices)
    {
        return new ChannelRemapProcessor(devices, this);
    }

    protected override IEnumerable<IAnimatable> GetAnimatables() => [];
}
