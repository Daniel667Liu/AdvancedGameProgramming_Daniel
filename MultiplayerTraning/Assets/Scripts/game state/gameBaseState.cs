using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class gameBaseState 
{
    public abstract void EnterState(GameStateManager stateManager);
    public abstract void UpdateState(GameStateManager stateManager);
    public abstract void ExitState(GameStateManager stateManager);
}
