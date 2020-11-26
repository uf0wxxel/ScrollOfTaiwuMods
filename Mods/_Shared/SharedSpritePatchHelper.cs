using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UnityModManagerNet
{
    internal static class SharedSpritePatchHelper
    {
        private static IDictionary<string, Sprite> _key2SpriteMappings = new Dictionary<string, Sprite>();

        public static IDictionary<string, (string Path, string Group)> TextureKey2InfoMappings { get; } = LoadTextureAssets();

        public static UnityModManager.ModEntry.ModLogger Logger { get; set; }

        public static IDictionary<string, (string Path, string Group)> LoadTextureAssets(string root = "Texture")
        {
            var fullRoot = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), root);
            Logger?.Debug($"Texture root: {fullRoot}");
            var results = Directory
                .EnumerateFiles(fullRoot, "*.png", SearchOption.AllDirectories)
                .Select(path =>
                {
                    var relative = path.Substring(path.IndexOf(root)).TrimStart(Path.DirectorySeparatorChar);
                    var relativeParts = relative.Split(Path.DirectorySeparatorChar);
                    return new
                    {
                        Key = Path.GetFileNameWithoutExtension(path),
                        Value = (path, relativeParts.Length > 1 ? relativeParts[0] : string.Empty),
                    };
                })
                .ToDictionary(i => i.Key, i => i.Value);

            Logger?.Log($"{results.Count} texture files loaded.");
            return results;
        }

        public static bool Patch(Image image, string spriteKey)
        {
            if (string.IsNullOrEmpty(spriteKey))
            {
                return true;
            }

            if (!TextureKey2InfoMappings.TryGetValue(spriteKey, out var info))
            {
                return true;
            }

            // TODO: Filter with group names

            Sprite sprite;
            if (!_key2SpriteMappings.TryGetValue(spriteKey, out sprite))
            {
                Logger?.Debug($"Patching {spriteKey} from file {info.Path}");
                sprite = CreateSpriteFromPath(info.Path);
                _key2SpriteMappings.Add(spriteKey, sprite);
            }

            if (sprite == null)
            {
                return true;
            }

            image.sprite = sprite;
            image.enabled = true;

            return false;
        }

        public static Sprite CreateSpriteFromPath(string path)
        {
            if (!File.Exists(path))
            {
                Logger?.Log($"[Texture] Texture file {path}  NOT found.");
                return null;
            }

            Logger?.Debug($"Loading texture file: {path}");
            var toload = new Texture2D(2, 2);
            toload.LoadImage(File.ReadAllBytes(path));
            return Sprite.Create(toload, new Rect(0, 0, toload.width, toload.height), new Vector2(0, 0), 100);
        }
    }
}
