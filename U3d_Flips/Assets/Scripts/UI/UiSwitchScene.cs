using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class UiSwitchScene : MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveProperty<string> onLoadingProcess;
        }
    
        [SerializeField] private TextMeshProUGUI loadingValue = default;

        public void SetCtx(Ctx ctx)
        {
            OnLoadingProcess("0");
            ctx.onLoadingProcess.Subscribe(OnLoadingProcess);
        }

        private void OnLoadingProcess(string value)
        {
            loadingValue.text = value;
        }
    }
}
