using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiMenuScene: MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveCommand onClickNewGame;
            public ReactiveCommand onClickSettings;
            public ReactiveCommand<GameScenes> onSwitchScene { get; set; }
        }

        [SerializeField] private Button playBtn = default;
        [SerializeField] private Button newGameBtn = default;
        [SerializeField] private Button settingsBtn = default;
       
        private Ctx _ctx;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            playBtn.onClick.AddListener(OnClickPlay);
            newGameBtn.onClick.AddListener(OnClickNewGame);
            settingsBtn.onClick.AddListener(OnClickSettings);
        }

        private void OnClickPlay()
        { 
            Debug.Log("[UiMenuScene] OnClickPlay");
            _ctx.onSwitchScene.Execute(GameScenes.Level1);
        }
        private void OnClickNewGame()
        { 
            Debug.Log("[UiMenuScene] OnClickNewGame");
            _ctx.onClickNewGame.Execute();
        }
        private void OnClickSettings()
        { 
            Debug.Log("[UiMenuScene] OnClickSettings");
            _ctx.onClickSettings.Execute();
        }
    }

}