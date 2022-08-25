using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace DataLoad
{
    public class StrAssetImageLoader : IImageLoader
    {
        private string _path;
        private string[] _filters = {"jpg", "jpeg", "png", "bmp"};

        public StrAssetImageLoader(string folder)
        {
            _path = Path.Combine(Application.streamingAssetsPath, folder);
        }

        public async Task<List<Texture2D>> LoadImages()
        {
            var allFiles = new List<string>();
            foreach (var filter in _filters)
            {
                var found = Directory.GetFiles(_path, $"*.{filter}").ToArray();
                allFiles.AddRange(found);
            }

            var textures = new List<Texture2D>();
            foreach (var file in allFiles)
            {
                Debug.Log($"[StrAssetImageLoader] load texture by path: {file}");
                textures.Add(await Load(file));
            }

            await Task.Yield();
            return textures;
        }

        private async Task<Texture2D> Load(string path)
        {
            byte[] bytes = await File.ReadAllBytesAsync(path);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            // return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            return tex;
        }

        public void Dispose()
        {
        }
    }
}