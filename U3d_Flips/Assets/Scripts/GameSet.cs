using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class GameSet : ScriptableObject
{
    public GameObject table = default;
    public Button buttonPrefab = default;
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