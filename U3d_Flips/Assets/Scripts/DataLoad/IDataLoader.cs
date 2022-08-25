using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DataLoad
{
    public interface IImageLoader : IDisposable
    {
        Task<List<Texture2D>> LoadImages();
    }
}