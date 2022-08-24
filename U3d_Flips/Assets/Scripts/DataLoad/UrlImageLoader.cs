using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DataLoad
{
    public class UrlImageLoader : IImageLoader
    {
        private readonly string _apiAddress;
        private readonly string _endPoint;


        public UrlImageLoader(string apiAddress, string endPoint)
        {
            _apiAddress = apiAddress;
            _endPoint = endPoint;
        }

        public async Task<List<Sprite>> LoadImages()
        {
            var urls = await GetUrls();
            var sprites = new List<Sprite>();

            foreach (var url in urls)
            {
                sprites.Add(await Load(url));
            }

            return sprites;
        }
        private async Task<List<string>> GetUrls()
        {
            var urls = await RestApiGetImagesUrls();
            return urls;
        }

        private async Task<List<string>> RestApiGetImagesUrls()
        {
            await Task.Yield();
            return new List<string>();
        }
    
        private async Task<Sprite> Load(string url)
        {
            // await download image from url, return sprite
            await Task.Yield();
            return null;
        }

        public void Dispose()
        {
        }
    }
}