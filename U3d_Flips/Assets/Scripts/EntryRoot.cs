using System;
using System.Collections.Generic;
using UnityEngine;

public class EntryRoot : MonoBehaviour
{
    private List<IDisposable> _disposables;
    private void Awake()
    {
        Debug.Log($"[EntryRoot][time] Loading scene start.. {Time.realtimeSinceStartup}");
        
        _disposables = new List<IDisposable>();
        
        CreateAppSettings();
        CreateRootEntity();
    }

    private void CreateAppSettings()
    {
    }

    private void CreateRootEntity()
    {
        var rootEntity = new RootEntity(new RootEntity.Ctx
        {
            
        });
    }
}