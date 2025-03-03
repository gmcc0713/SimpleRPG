using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteration : InteractionObject
{
    public override void PressInteractionKey()
    {
        UIManager.Instance.LockUnLockMouseCursor(true);
        gameObject.GetComponent<NPCController>().UseInterationKey();
    }
}
