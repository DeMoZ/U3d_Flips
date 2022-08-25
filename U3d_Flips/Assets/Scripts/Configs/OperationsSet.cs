using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu]
    public class OperationsSet : ScriptableObject
    {
        [TableList] public List<Operation> interactableSets = default;

        public Operation GetOperation(OperationTypes operation)
        {
            var a = interactableSets.FirstOrDefault(d => d.operation == operation);
            return a;
        }
    }

    [System.Serializable]
    public class Operation
    {
        [TableColumnWidth(70, false)] public OperationTypes operation;
        [TableColumnWidth(50, false)] public bool enabled;
        [TableColumnWidth(70, false)] public bool hasButton;
        [TableColumnWidth(70, false)] public string description;
        [TableColumnWidth(70, false)] public float duration;

        [TableColumnWidth(60, false)] [PreviewField]
        public Sprite sprite;
    }
}