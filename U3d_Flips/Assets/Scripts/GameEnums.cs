public enum SwipeStates
{
    None = 0,
    Start = 1,
    Change = 2,
    End = 3
}

public enum SwipeDirections
{
    None = 0,
    ToRight = 1,
    ToLeft = 2,
    ToUp = 3,
    ToDown = 4,
    Thrust = 5, // Lunge // колящий удар
}

public enum FightStates
{
    None,
    Idle,
    StartPosition,
    Hold,
    Hit,
    End
}