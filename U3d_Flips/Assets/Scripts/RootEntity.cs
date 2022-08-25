using System;
using UniRx;
using UnityEngine;

public class RootEntity : IDisposable
{
    public struct Ctx
    {
    }

    private Ctx _ctx;
    
    private ReactiveCommand<bool> _onBoo;

    private CompositeDisposable _diposables;

    public RootEntity(Ctx ctx)
    {
        Debug.Log($"[RootEntity][time] Loading scene start.. {Time.realtimeSinceStartup}");
        _ctx = ctx;
        _diposables = new CompositeDisposable();
        
        var onSwitchScene = new ReactiveCommand<GameScenes>();
        
        //_diposables.Add(_onPlayClick.Subscribe(x => OnPlayScene()));

        var sceneSwitcher = new SceneSwitcher(new SceneSwitcher.Ctx
        {
            onSwitchScene = onSwitchScene,
        });

        onSwitchScene.Execute(GameScenes.Level1);
    }

    public void Dispose()
    {
        _diposables.Dispose();
    }
}