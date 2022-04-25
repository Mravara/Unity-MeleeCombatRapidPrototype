using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateType
{
    Idle,
    Walk,
    PrepareToSwing,
    HoldSwing,
    Swing,
    RecoverFromSwing,
    Staggered,
    PrepareToThrust,
    HoldPrepareToThrust,
    Thrust,
    HoldThrust,
    RecoverFromThrust
}

public enum PlayerStateType
{
    Idle,
    Walk,
    StartBlock,
    HoldBlock,
    EndBlock,
    Swing,
    StartHeavy,
    HoldHeavy,
    ReleaseHeavy,
    HoldReleaseHeavy
}
