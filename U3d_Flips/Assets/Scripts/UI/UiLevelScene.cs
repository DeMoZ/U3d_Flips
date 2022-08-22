using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiLevelScene : MonoBehaviour
    {
        public struct Ctx
        {
            public GameSet gameSet;
            public OperationsSet operationsSet;
            public ReactiveCommand<List<OperationTypes>> onSelectInteractable;
            public ReactiveCommand<OperationTypes> onInteractionButtonClick;
            public Pool pool;
        }

        private const float FADE_TIME = 0.3f;

        [SerializeField] private Transform interactionsBtnParent = default;

        private Ctx _ctx;
        private List<Button> _currentOperations = new();
        private CanvasGroup _interactionBtnsCanvasGroup;

        public void SetCtx(Ctx ctx)
        {
            _interactionBtnsCanvasGroup = interactionsBtnParent.GetComponent<CanvasGroup>();
            _ctx = ctx;
            List<IDisposable> a = new List<IDisposable>();
            _ctx.onSelectInteractable.Subscribe(OnSelectInteractable);
        }

        private async void OnSelectInteractable(List<OperationTypes> operationsTypes)
        {
            if (_currentOperations.Count > 0)
                await HideOperations();

            foreach (var operationType in operationsTypes)
            {
                var btnGo = _ctx.pool.Get(_ctx.gameSet.buttonPrefab.gameObject);
                var btn = btnGo.GetComponent<Button>();
                btn.transform.SetParent(interactionsBtnParent);
                var operation = _ctx.operationsSet.GetOperation(operationType);
                
                btn.GetComponentInChildren<TextMeshProUGUI>().text = operation.description;
                btn.GetComponentInChildren<Image>().sprite = operation.sprite;
                
                btn.onClick.AddListener(() =>
                {
                    _ctx.onInteractionButtonClick.Execute(operationType);
                });

                _currentOperations.Add(btn);
                btnGo.SetActive(true);
            }

            ShowOperations();
        }

        private void ShowOperations()
        {
            _interactionBtnsCanvasGroup.alpha = 0;
            _interactionBtnsCanvasGroup.DOFade(1, FADE_TIME);
        }

        private async Task HideOperations()
        {
            _interactionBtnsCanvasGroup.DOFade(0, FADE_TIME);

            await Task.Delay((int) (FADE_TIME * 1000));

            foreach (var btn in _currentOperations)
            {
                _ctx.pool.Return(btn.gameObject);
                btn.onClick.RemoveAllListeners();
            }

            _currentOperations.Clear();
        }
    }
}