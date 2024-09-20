using System.Collections.Generic;
using UnityEngine;

public static class SlotController
{
    private static Vector3[] controlPositions = new Vector3[]
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

    public static bool SlotAroundCheck(Vector3 slotPosition, Dictionary<Vector3, ItemSlot> controlSlots)
    {
        for (int i = 0; i < controlPositions.Length; i++)
        {
            if (controlSlots.ContainsKey(controlPositions[i]))
            {
                return false;
            }
        }
        return true;
    }
}
