using UI;
using UniRx;
using UnityEngine;

public class MenuSceneEntity : IGameScene
{
    public struct Ctx
    {
        public GameScenes scene;
        public ReactiveCommand onPlayClicked { get; set; }
    }

    private Ctx _ctx;
    private UiMenuScene _ui;

    private ReactiveCommand _onClickPlay;
    private ReactiveCommand _onClickNewGame;
    private ReactiveCommand _onClickSettings;

    public MenuSceneEntity(Ctx ctx)
    {
        _ctx = ctx;

        _onClickPlay = new();
        _onClickNewGame = new();
        _onClickSettings = new();
    }

    public void Enter()
    {
        var menuScenePm = new MenuScenePm(new MenuScenePm.Ctx
        {
            onClickPlay = _onClickPlay,
            onClickNewGame = _onClickNewGame,
            onClickSettings = _onClickSettings,
        });
        // Find UI
        _ui = UnityEngine.GameObject.FindObjectOfType<UiMenuScene>();
        
        _ui.SetCtx(new UiMenuScene.Ctx
        {
            onClickPlay = _onClickPlay,
            onClickNewGame = _onClickNewGame,
            onClickSettings = _onClickSettings,
        });
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Dispose()
    {
    }
}