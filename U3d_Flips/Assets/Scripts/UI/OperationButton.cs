using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OperationButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;
        
        public Button Button => button;
        public void SetText(string str) => text.text = str;
        public void SetImage(Sprite sprite) => image.sprite = sprite;
    }
}