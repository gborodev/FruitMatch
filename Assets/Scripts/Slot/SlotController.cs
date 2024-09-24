using System.Collections.Generic;
using UnityEngine;

public static class SlotController
{
    public static Vector3[] controlPositions = new Vector3[]
    {
        new Vector3(1, 0 ,0),       //Right
        new Vector3(-1, 0, 0),      //Left        
        new Vector3(0, 1, 0),       //Up
        new Vector3(0, -1, 0),      //Down
        new Vector3(1, 1, 0),       //Right-Up
        new Vector3(1, -1, 0),      //Right-Down
        new Vector3(-1, 1, 0),      //Left-Up
        new Vector3(-1, -1, 0),     //Left-Down
    };

    public static void SlotAroundCheck(Vector3 slotPosition, Dictionary<Vector3, ItemSlot> controlSlots)
    {
        for (int i = 0; i < controlPositions.Length; i++)
        {
            if (controlSlots.ContainsKey(slotPosition + controlPositions[i]))
            {
                ItemSlot slot = controlSlots[slotPosition + controlPositions[i]];
                slot.CanPress = false;
            }
        }
    }

    public static List<ItemSlot> GetUnderItems(Vector3 slotPosition, Dictionary<Vector3, ItemSlot> controlSlots)
    {
        List<ItemSlot> slots = new List<ItemSlot>();

        for (int i = 0; i < controlPositions.Length; i++)
        {
            if (controlSlots.ContainsKey(slotPosition + controlPositions[i]))
            {
                ItemSlot slot = controlSlots[slotPosition + controlPositions[i]];
                slots.Add(slot);
            }
        }

        return slots;
    }
}
