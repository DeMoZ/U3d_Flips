using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiLevelScene : MonoBehaviour, IDisposable
    {
        public struct Ctx
        {
            public GameSet gameSet;
            public OperationsSet operationsSet;
            public ReactiveCommand<List<OperationTypes>> onSelectInteractable;
            public ReactiveCommand<OperationTypes> onInteractionButtonClick;
            public ReactiveCommand<GameScenes> onSwitchScene;
            public Pool pool;
        }

        private const float FADE_TIME = 0.3f;

        [SerializeField] private Camera _camera = default;
        [SerializeField] private Button menuButton = default;
        [SerializeField] private Transform interactionsBtnParent = default;
        [SerializeField] private CanvasGroup interactionBtnsCanvasGroup;

        private Ctx _ctx;
        private CompositeDisposable _disposables;
        private List<OperationButton> _currentOperations = new();

        public Camera Camera =>
            _camera;
        
        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            _disposables = new CompositeDisposable();
            List<IDisposable> a = new List<IDisposable>();
            _ctx.onSelectInteractable.Subscribe(OnSelectInteractable).AddTo(_disposables);
            menuButton.onClick.AddListener(() => _ctx.onSwitchScene.Execute(GameScenes.Menu));
        }

        private async void OnSelectInteractable(List<OperationTypes> operationsTypes)
        {
            if (_currentOperations.Count > 0)
                await HideOperations();

            foreach (var operationType in operationsTypes)
            {
                var operation = _ctx.operationsSet.GetOperation(operationType);

                if (!operation.hasButton || !operation.enabled)
                    continue;

                var btnGo = _ctx.pool.Get(_ctx.gameSet.buttonPrefab.gameObject);
                var btn = btnGo.GetComponent<OperationButton>();
                btn.transform.SetParent(interactionsBtnParent);

                btn.SetText(operation.description);
                btn.SetImage(operation.sprite);

                btn.Button.onClick.AddListener(() => { _ctx.onInteractionButtonClick.Execute(operationType); });

                _currentOperations.Add(btn);
                btnGo.SetActive(true);
            }

            ShowOperations();
        }

        private void ShowOperations()
        {
            interactionBtnsCanvasGroup.alpha = 0;
            interactionBtnsCanvasGroup.DOFade(1, FADE_TIME);
        }

        private async Task HideOperations()
        {
            interactionBtnsCanvasGroup.DOFade(0, FADE_TIME);

            await Task.Delay((int) (FADE_TIME * 1000));

            foreach (var btn in _currentOperations)
            {
                _ctx.pool.Return(btn.gameObject);
                btn.Button.onClick.RemoveAllListeners();
            }

            _currentOperations.Clear();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}