using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YMM4ChannelRemap.Effects;

internal static class ShaderResourceLoader
{
    public static byte[] GetShaderResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(resource => resource.EndsWith("." + name, StringComparison.Ordinal))
            ?? throw new InvalidOperationException($"埋め込みシェーダー '{name}' が見つかりません。");

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"埋め込みシェーダー '{resourceName}' を読み込めません。");
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }
}
