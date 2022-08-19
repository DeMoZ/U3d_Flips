using UniRx;
using UnityEngine;

namespace UI
{
    public class MenuScenePm
    {
        public struct Ctx
        {
            public ReactiveCommand onClickPlay;
            public ReactiveCommand onClickNewGame;
            public ReactiveCommand onClickSettings;
        }
        
        private Ctx _ctx;

        public MenuScenePm(Ctx ctx)
        {
            _ctx = ctx;
            
            _ctx.onClickPlay.Subscribe(_=> OnClickPlay());
            _ctx.onClickNewGame.Subscribe(_=> OnClickNewGame());
            _ctx.onClickSettings.Subscribe(_=> OnClickSettings());
        }
        
        private void OnClickPlay()
        { 
            Debug.Log("[MenuScenePm] OnClickPlay");
        }
        private void OnClickNewGame()
        { 
            Debug.Log("[MenuScenePm] OnClickNewGame");
        }
        private void OnClickSettings()
        { 
            Debug.Log("[MenuScenePm] OnClickSettings");
        }
    }
}