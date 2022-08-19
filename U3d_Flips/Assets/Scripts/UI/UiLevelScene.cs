using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiLevelScene : MonoBehaviour
    {
        public struct Ctx
        {
            public OperationButtonSets operationButtonSets;
            public ReactiveCommand<List<InteractionTypes>> onSelectInteractable;
            public ReactiveCommand<InteractionTypes> onInteractionButtonClick;
        }

        [SerializeField] private Transform interactionsBtnsParent = default;
        private Ctx _ctx;
        private List<Button> _currentOperations;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            List<IDisposable> a = new List<IDisposable>();
            _ctx.onSelectInteractable.Subscribe(OnSelectInteractable);
        }

        private async void OnSelectInteractable(List<InteractionTypes> operations)
        {
            if (_currentOperations != null && _currentOperations.Count > 0)
                await HideOperations();

            foreach (var operation in operations)
            {
                var btn = Instantiate(_ctx.operationButtonSets.buttonPrefab, interactionsBtnsParent);

                // var operationSet = new OperationButton(operation);
                btn.GetComponent<Text>().text = _ctx.operationButtonSets.GetBtnDescription(operation);
                btn.onClick.AddListener(() => { _ctx.onInteractionButtonClick.Execute(operation); });

                _currentOperations.Add(btn);
            }

            ShowOperations();
        }

        private void ShowOperations()
        {
            throw new System.NotImplementedException();
        }

        private async Task HideOperations()
        {
            throw new System.NotImplementedException();
        }
    }
}