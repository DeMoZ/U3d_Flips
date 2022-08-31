using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu]
    public class GameSet : ScriptableObject
    {
        public AssetReference tableRef = default;
        public AssetReference buttonRef = default;
        public List<InteractableSet> interactableSets = default;
    }

    [System.Serializable]
    public class InteractableSet
    {
        public InteractableTypes type;
        public List<OperationTypes> operations;
        public int amount;
        public AssetReference interactableRef = default;
    }
}