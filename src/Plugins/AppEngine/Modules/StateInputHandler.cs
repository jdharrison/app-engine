using UnityEngine;
using System.Collections;

public class StateInputHandler : Core.StateModule
{
    public Core.eState state;

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            controller.ChangeState(state);
        }
    }
}