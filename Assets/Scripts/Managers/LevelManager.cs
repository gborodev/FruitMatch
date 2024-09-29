using Events;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("References")]
    [SerializeField] private GameDatabase database;
    [SerializeField] private ItemSlot slotPrefab;

    private List<GameObject> parentList = new List<GameObject>();

    public bool IsLevelFinished => slotItemsDictionary.Count <= 0;
    private Dictionary<Vector3, ItemSlot> slotItemsDictionary;
    private List<Vector3>[] slotPositions;

    private LevelData currentData;

    public void SetLevel(Level level)
    {
        slotItemsDictionary?.Clear();

        foreach (var parent in parentList)
        {
            Destroy(parent.gameObject);
        }
        parentList?.Clear();

        LevelInitialized(level);
    }

    private void LevelInitialized(Level level)
    {
        if (level is null) Debug.Log("Level is not found");

        currentData = new LevelData(level);

        slotItemsDictionary = new Dictionary<Vector3, ItemSlot>();

        slotPositions = currentData.GetPositions(0);

        //Generate Items for slots
        int slotCount = 0;
        foreach (var slot in slotPositions)
        {
            slotCount += slot.Count;
        }
        List<Item> items = GetItems(slotCount);

        //Generate Slot Z Position 
        for (int i = 0; i < slotPositions.Length; i++)
        {
            GameObject indexerParent = new GameObject();
            indexerParent.name = $"Slots {i}";

            List<Vector3> indexPositions = slotPositions[i];

            //Generate Slot Local Positions
            for (int index = 0; index < indexPositions.Count; index++)
            {
                Vector3 position = indexPositions[index];
                Quaternion rotation = Quaternion.identity;

                ItemSlot slot = Instantiate(slotPrefab, position, rotation, indexerParent.transform);
                Item item = items[Random.Range(0, items.Count)];

                slot.Item = item;
                slot.Position = position;
                slot.name = $"{item.ItemName} Slot";

                slot.OnClickEvent += SlotCollect;

                List<Vector3> underSides = UnderPositions(slot);

                for (int u = 0; u < underSides.Count; u++)
                {
                    ItemSlot controlSlot = slotItemsDictionary[underSides[u]];

                    controlSlot.CanPress = false;
                }

                //Control Lists
                items.Remove(item);
                slotItemsDictionary.Add(position, slot);
            }
        }

        GameEvents.OnLoadLevel?.Invoke(level.LevelValue);
    }

    private void SlotCollect(ItemSlot slot)
    {
        List<Vector3> underSides = UnderPositions(slot);

        slotItemsDictionary.Remove(slot.Position);

        CollectorEvents.OnSlotCollect?.Invoke(slot.Item);

        Destroy(slot.gameObject);


        for (int i = 0; i < underSides.Count; i++)
        {
            ItemSlot controlSlot = slotItemsDictionary[underSides[i]];
            controlSlot.CanPress = true;

            for (int zIndex = controlSlot.Layer + 1; zIndex < slotPositions.Length; zIndex++)
            {
                Vector3 upperPosition = new Vector3(controlSlot.Position.x, controlSlot.Position.y, zIndex);

                if (slotItemsDictionary.ContainsKey(upperPosition))
                {
                    controlSlot.CanPress = false;
                    break;
                }

                for (int sideIndex = 0; sideIndex < Sides.GetSides().Length; sideIndex++)
                {
                    Vector3 sidePosition = upperPosition + Sides.GetSides()[sideIndex];

                    if (slotItemsDictionary.ContainsKey(sidePosition))
                    {
                        controlSlot.CanPress = false;
                        break;
                    }

                }
            }
        }
    }

    private List<Vector3> UnderPositions(ItemSlot slot)
    {
        List<Vector3> underSides = new List<Vector3>();

        Vector3 pos = slot.Position;

        for (int z = (int)pos.z - 1; z >= 0; z--)
        {
            Vector3 down = new Vector3(pos.x, pos.y, z);

            if (slotItemsDictionary.ContainsKey(down)) underSides.Add(down);

            for (int sideIndex = 0; sideIndex < Sides.GetSides().Length; sideIndex++)
            {
                Vector3 controlPosition = down + Sides.GetSides()[sideIndex];
                if (slotItemsDictionary.ContainsKey(controlPosition))
                {
                    underSides.Add(controlPosition);
                }
            }
        }
        return underSides;
    }

    private List<Item> GetItems(int slotCount, int eachCount = 6)
    {
        int diffItemCount = slotCount / eachCount;

        Item[] items = database.GetItemsFromDatabase();

        List<Item> output = new List<Item>();
        for (int diff = 0; diff < diffItemCount; diff++)
        {
            for (int e = 0; e < eachCount; e++)
            {
                output.Add(items[diff]);
            }
        }

        return output;
    }
}
