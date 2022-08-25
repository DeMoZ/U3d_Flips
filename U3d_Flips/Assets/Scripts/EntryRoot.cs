using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EntryRoot : MonoBehaviour
{
    private CompositeDisposable _disposables;
    private void Awake()
    {
        Debug.Log($"[EntryRoot][time] Loading scene start.. {Time.realtimeSinceStartup}");
        
        _disposables = new CompositeDisposable();
        
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
            
        }).AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}