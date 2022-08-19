public class LevelSceneEntity : IGameScene
{
    public struct Ctx
    {
        public GameScenes scene;
    }

    private Ctx _ctx;

    public LevelSceneEntity(Ctx ctx)
    {
        _ctx = ctx;

        // SwipeCatcher swipeCatcher = default;//_context.ResourceLoader.Get<SwipeCatcher>(_context.ResourceLoader.UIPrefabs.SwipeCatcher, _context.UIParent); 
        //
        //
        // IReactiveCommand<Swipe> onSwipe = new ReactiveCommand<Swipe>();
        //
        // SwipeCatcher.Context swipeCatcherCtx = new SwipeCatcher.Context
        // {
        //     OnSwipe = onSwipe
        // };
        //
        // swipeCatcher.SetContext(swipeCatcherCtx);
        //
        // PlayerSwipeInput psi = new PlayerSwipeInput(new PlayerSwipeInput.Context
        // {
        //     OnSwipe = onSwipe
        // });
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