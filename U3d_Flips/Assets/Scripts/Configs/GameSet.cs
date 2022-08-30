using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu]
    public class GameSet : ScriptableObject
    {
        public GameObject table = default;
        public OperationButton buttonPrefab = default;
        public List<InteractableSet> interactableSets = default;
    }

    [System.Serializable]
    public class InteractableSet
    {
        public InteractableTypes type;
        public List<OperationTypes> operations;
        public int amount;
        public InteractableView prefab;
    }
}