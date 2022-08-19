using System;
using System.Collections.Generic;
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
        _diposables = new();
        _ctx.onSwitchScene.Subscribe(OnSwitchScene);
    }

    private void OnSwitchScene(GameScenes scene)
    {
        // load switch scene Additive (with UI over all)
        _diposables.Add(SceneManager.LoadSceneAsync(SWITCH_SCENE) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[RootEntity][SwitchScenes] Async load scene {SWITCH_SCENE} progress: " + x.progress); // show progress
            }).Subscribe(_ =>
            {
                Debug.Log($"[RootEntity][SwitchScenes] Async load scene {SWITCH_SCENE} done");
                OnSwitchSceneLoaded(scene);
            }));
    }
    
    private void OnSwitchSceneLoaded(GameScenes scene)
    {
        _currentScene?.Exit();
        var onLoadingProcess = new ReactiveProperty<string>();
        var switchSceneEntity = new LoadingSceneEntity(new LoadingSceneEntity.Ctx{
            onLoadingProcess = onLoadingProcess,
        });

        Debug.Log($"[RootEntity][OnSwitchSceneLoaded] Start load scene {scene}");

        _currentScene = SceneEntity(scene);
        _diposables.Add(SceneManager.LoadSceneAsync(GetSceneName(scene)) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[RootEntity][OnSwitchSceneLoaded] Async load scene {scene} progress: " + x.progress); // show progress
                onLoadingProcess.Value = x.progress.ToString();
            }).Subscribe(_ =>
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
    
    private IGameScene SceneEntity(GameScenes scene)
    {
        IGameScene newScene = scene switch
        {
            GameScenes.Menu => LoadMenu(),
            GameScenes.Level1 => LoadLevel1(),
            _ => LoadLevel1()
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

    private IGameScene LoadLevel1()
    {
        return new LevelSceneEntity(new LevelSceneEntity.Ctx
        {
        });
    }
    
    public void Dispose()
    {
        foreach (var disposable in _diposables)
            disposable.Dispose();
        
        _currentScene.Dispose();
    }
}