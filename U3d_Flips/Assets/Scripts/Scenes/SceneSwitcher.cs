using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : IDisposable
{
    public struct Ctx
    {
        public ReactiveCommand<GameScenes> onSwitchScene;
    }

    private const string MENU_SCENE = "MenuScene";
    private const string SWITCH_SCENE = "2_SwitchScene";
    private const string LEVEL_SCENE = "LevelScene";

    private Ctx _ctx;
    private List<IDisposable> _diposables;

    private IGameScene _currentScene;

    public SceneSwitcher(Ctx ctx)
    {
        _ctx = ctx;
        _diposables = new List<IDisposable>();
        _ctx.onSwitchScene.Subscribe(OnSwitchScene).AddTo(_diposables);
    }

    private void OnSwitchScene(GameScenes scene)
    {
        // load switch scene Additive (with UI over all)
        _diposables.Add(SceneManager.LoadSceneAsync(SWITCH_SCENE) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[RootEntity][SwitchScenes] Async load scene {SWITCH_SCENE} progress: " +
                          x.progress); // show progress
            }).Subscribe(_ =>
            {
                Debug.Log($"[RootEntity][SwitchScenes] Async load scene {SWITCH_SCENE} done");
                OnSwitchSceneLoaded(scene);
            }));
    }

    private async void OnSwitchSceneLoaded(GameScenes scene)
    {
        _currentScene?.Exit();
        var onLoadingProcess = new ReactiveProperty<string>();
        var switchSceneEntity = new LoadingSceneEntity(new LoadingSceneEntity.Ctx
        {
            onLoadingProcess = onLoadingProcess,
        });

        Debug.Log($"[RootEntity][OnSwitchSceneLoaded] Start load scene {scene}");
        
        _currentScene = await SceneEntity(scene);

        _diposables.Add(SceneManager.LoadSceneAsync(GetSceneName(scene)) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[RootEntity][OnSwitchSceneLoaded] Async load scene {scene} progress: " +
                          x.progress); // show progress
                onLoadingProcess.Value = x.progress.ToString();
            }).Subscribe( _ =>
            {
                switchSceneEntity.Exit();
                switchSceneEntity.Dispose();

                _currentScene.Enter();
            }));
    }

    private string GetSceneName(GameScenes scene)
    {
        return scene switch
        {
            GameScenes.Menu => MENU_SCENE,
            GameScenes.Level1 => LEVEL_SCENE,
            _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
        };
    }

    private async Task<IGameScene> SceneEntity(GameScenes scene)
    {
        IGameScene newScene = scene switch
        {
            GameScenes.Menu => LoadMenu(),
            GameScenes.Level1 => await LoadLevel1(),
            _ => await LoadLevel1()
        };

        return newScene;
    }

    private IGameScene LoadMenu()
    {
        return new MenuSceneEntity(new MenuSceneEntity.Ctx
        {
            scene = GameScenes.Menu,
            //onPlayClicked = onPlayClicked,
        });
    }


    private async Task<IGameScene> LoadLevel1()
    {
        var constructorTask = new Container<Task>();
        var sceneEntity = new LevelSceneEntity(new LevelSceneEntity.Ctx
        {
            constructorTask = constructorTask,
        });

        await constructorTask.Value;
        return sceneEntity;
    }

    public void Dispose()
    {
        foreach (var disposable in _diposables)
            disposable.Dispose();

        _currentScene.Dispose();
    }
}