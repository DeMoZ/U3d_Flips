using System;
using UniRx;
using UnityEngine;


public class MenuScenePm : IDisposable
{
    public struct Ctx
    {
    }

    private Ctx _ctx;

    public MenuScenePm(Ctx ctx)
    {
        _ctx = ctx;
    }

    private void OnClickPlay()
    {
        Debug.Log("[MenuScenePm] OnClickPlay");
    }

    private void OnClickNewGame()
    {
        Debug.Log("[MenuScenePm] OnClickNewGame");
    }

    private void OnClickSettings()
    {
        Debug.Log("[MenuScenePm] OnClickSettings");
    }

    public void Dispose()
    {
    }
}