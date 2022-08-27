using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootEntity : IDisposable
{
    public struct Ctx
    {
    }

    private Ctx _ctx;
    private CompositeDisposable _diposables;

    public RootEntity(Ctx ctx)
    {
        Debug.Log($"[RootEntity][time] Loading scene start.. {Time.realtimeSinceStartup}");
        _ctx = ctx;
        _diposables = new CompositeDisposable();
        
        var startApplicationSceneName = SceneManager.GetActiveScene().name;
        var onSwitchScene = new ReactiveCommand<GameScenes>();

        var sceneSwitcher = new SceneSwitcher(new SceneSwitcher.Ctx
        {
            startApplicationSceneName = startApplicationSceneName,
            onSwitchScene = onSwitchScene,
        }).AddTo(_diposables);
    }

    public void Dispose()
    {
        _diposables.Dispose();
    }
}