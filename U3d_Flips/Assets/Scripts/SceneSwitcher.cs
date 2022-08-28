using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : IDisposable
{
    public struct Ctx
    {
        public ReactiveCommand<GameScenes> onSwitchScene;
        public ScenesHandler scenesHandler;
    }

    private Ctx _ctx;
    private CompositeDisposable _diposables;

    private IGameScene _currentScene;

    public SceneSwitcher(Ctx ctx)
    {
        _ctx = ctx;
        _diposables = new CompositeDisposable();
        _ctx.onSwitchScene.Subscribe(OnSwitchScene).AddTo(_diposables);
    }
    
    private void OnSwitchScene(GameScenes scene)
    {
        // load switch scene Additive (with UI over all)
        _diposables.Add(SceneManager.LoadSceneAsync(_ctx.scenesHandler.SwitchScene) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[{this}][OnSwitchScene] Async load scene {_ctx.scenesHandler.SwitchScene} progress: " +
                          x.progress); // show progress
            }).Subscribe(_ =>
            {
                Debug.Log($"[{this}][OnSwitchScene] Async load scene {_ctx.scenesHandler.SwitchScene} done");
                _currentScene?.Exit();
                _currentScene?.Dispose();
                
                OnSwitchSceneLoaded(scene);
            }));
    }

    private async void OnSwitchSceneLoaded(GameScenes scene)
    {
        var onLoadingProcess = new ReactiveProperty<string>().AddTo(_diposables);
        var switchSceneEntity = new LoadingSceneEntity(new LoadingSceneEntity.Ctx
        {
            onLoadingProcess = onLoadingProcess,
        });

        Debug.Log($"[{this}][OnSwitchSceneLoaded] Start load scene {scene}");

        _currentScene = await _ctx.scenesHandler.SceneEntity(scene);

        _diposables.Add(SceneManager.LoadSceneAsync(_ctx.scenesHandler.GetSceneName(scene)) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[{this}][OnSwitchSceneLoaded] Async load scene {scene} progress: " +
                          x.progress); // show progress
                onLoadingProcess.Value = x.progress.ToString();
            }).Subscribe(_ =>
            {
                Debug.Log($"[{this}][OnSwitchSceneLoaded] Async load scene {scene} done");

                switchSceneEntity.Exit();
                switchSceneEntity.Dispose();

                _currentScene.Enter();
            }));
    }

    public void Dispose()
    {
        _diposables.Dispose();
        _currentScene.Dispose();
    }
}