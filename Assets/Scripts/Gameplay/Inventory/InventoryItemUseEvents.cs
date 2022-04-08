using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryItemUseEvents
{
    public enum UseEvent
    {
        BONE
    };

    public static void BoneUseEvent()
    {
        // play the bone throw sound
    }

    public static InventoryItem.UseEvent GetEvent(UseEvent e)
    {
        switch(e)
        {
            case UseEvent.BONE:
                return BoneUseEvent;
        }
        return BoneUseEvent;
    }
}
