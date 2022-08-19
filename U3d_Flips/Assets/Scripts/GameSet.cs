using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class GameSet : ScriptableObject
{
    public GameObject table = default;
    public List<InteractableSet> interactableSets = default;
}

[System.Serializable]
public class InteractableSet
{
    public InteractableTypes type;
    public int amount;
    public InteractableView prefab;
}