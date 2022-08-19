using UI;
using UniRx;

public class LoadingSceneEntity : IGameScene
{
    public struct Ctx
    {
        public ReactiveProperty<string> onLoadingProcess;
    }

    private Ctx _ctx;

    public LoadingSceneEntity(Ctx ctx)
    {
        _ctx = ctx;

        var ui = UnityEngine.GameObject.FindObjectOfType<UiSwitchScene>();
        ui.SetCtx(new UiSwitchScene.Ctx
        {
            onLoadingProcess = _ctx.onLoadingProcess,
        });
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
    }
}