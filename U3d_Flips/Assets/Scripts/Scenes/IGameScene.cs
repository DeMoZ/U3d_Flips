using System;

public interface IGameScene : IDisposable
{
    public void Enter();
    public void Exit();
}