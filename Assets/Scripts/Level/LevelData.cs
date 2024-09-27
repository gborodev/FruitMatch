using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    private int levelID;

    private List<Vector3>[] allPositions;

    public LevelData(Level level)
    {
        levelID = level.GetInstanceID();
        allPositions = level.GetPositions();
    }

    public void ClearLevel()
    {

    }

    public void ClearPosition(Vector3 position)
    {
        for (int i = 0; i < allPositions.Length; i++)
        {
            if (allPositions[i].Contains(position))
            {
                allPositions[i].Remove(position);
            }
        }
    }

    public void TriggerUnderItem(int zIndex, ItemSlot slot)
    {
        int z = zIndex - 1;

        if (z < 0) return;

        for (int zPosition = z; zPosition > 0; zPosition--)
        {
            for (int i = 0; i < ControlPosition.sides.Length; i++)
            {
                if (allPositions[zPosition].Contains(slot.Position + ControlPosition.sides[i]))
                {
                    slot.Triggered(GetPositions(z));
                }
            }
        }
    }

    private List<Vector3>[] GetPositions(int zIndex)
    {
        List<List<Vector3>> positions = new List<List<Vector3>>();

        for (int index = zIndex; index < allPositions.Length; index++)
        {
            positions.Add(allPositions[index]);
        }
        return positions.ToArray();
    }
}
public static class ControlPosition
{
    public static Vector3[] sides = new Vector3[]
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
}
