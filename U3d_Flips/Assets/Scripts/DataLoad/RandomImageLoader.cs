using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class RandomImageLoader : IDisposable
{
    private string _url;

    public RandomImageLoader()
    {
        _url =@"https://picsum.photos/200";
    }
    
    public RandomImageLoader(string url)
    {
        _url = url;
    }

    public async Task<List<Sprite>> Load(int amount = 1)
    {
        var sprites = new List<Sprite>();

        for (int i = 0; i < amount; i++)
        {
            try
            {
                using var webClient = new WebClient();
                var data = await webClient.DownloadDataTaskAsync(_url);
                var tex = new Texture2D(1, 1);
                tex.LoadImage(data);
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                    new Vector2(tex.width / 2, tex.height / 2));
                
                sprites.Add(sprite);
            }
            catch (Exception)
            {
                // ignored
                sprites.Add(null);
            }
        }

        return sprites;
    }
    
    public async Task<List<Texture2D>> LoadTexture(int amount = 1)
    {
        var textures = new List<Texture2D>();

        for (int i = 0; i < amount; i++)
        {
            try
            {
                using var webClient = new WebClient();
                var data = await webClient.DownloadDataTaskAsync(_url);
                var tex = new Texture2D(1, 1);
                tex.LoadImage(data);
               textures.Add(tex);
            }
            catch (Exception)
            {
                // ignored
                textures.Add(null);
            }
        }

        return textures;
    }

    public void Dispose()
    {
    }    
}