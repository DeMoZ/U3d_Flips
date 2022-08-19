using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class OperationButtonSets : ScriptableObject
{
    public Button buttonPrefab = default;
    public List<OperationButtonSet> interactableSets = default;

    public string GetBtnDescription(InteractionTypes operation)
    {
        var a =  interactableSets.FirstOrDefault( d  => d.operation == operation);
        return a?.description;
    }
}

[System.Serializable]
public class OperationButtonSet
{
    public InteractionTypes operation;
    public string description;
    //public Sprite sprite;
}