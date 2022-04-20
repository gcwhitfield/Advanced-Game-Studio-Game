using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryItemUseEvents
{
    public enum UseEvent
    {
        BONE,
        NONE
    };

    public static void BoneUseEvent()
    {
        // tell the wolf encounter that the father has used the bone
        WolfEncounter.Instance.OnFatherThrowBone();
    }

    // an empty event
    public static void NoneUseEvent() { }

    public static InventoryItem.UseEvent GetEvent(UseEvent e)
    {
        switch(e)
        {
            case UseEvent.BONE:
                return BoneUseEvent;
            default:
                return NoneUseEvent;
        }
    }
}
