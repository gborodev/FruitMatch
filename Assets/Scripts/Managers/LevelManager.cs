using Events;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("References")]
    [SerializeField] private ItemDatabase database;
    [SerializeField] private ItemSlot slotPrefab;

    private Dictionary<int, List<Vector3>> slotPositionsDictionary;
    private Dictionary<Vector3, ItemSlot> slotItemsDictionary;

    private List<GameObject> parentList = new List<GameObject>();

    public void SetLevel(int level)
    {
        slotPositionsDictionary?.Clear();
        slotItemsDictionary?.Clear();

        foreach (var parent in parentList)
        {
            Destroy(parent.gameObject);
        }
        parentList?.Clear();

        LevelInitialized(level);
    }

    public void LevelInitialized(int currentLevel)
    {
        slotPositionsDictionary = new Dictionary<int, List<Vector3>>();
        slotItemsDictionary = new Dictionary<Vector3, ItemSlot>();

        //Positions
        SlotPositionsInitialize(currentLevel);

        //Initialize Slots
        int slotCount = 0;
        foreach (var value in slotPositionsDictionary.Values)
        {
            slotCount += value.Count;
        }
        SlotItemsInitialize(GenerateItems(slotCount));
        Debug.Log($"Slot Count: {slotCount}");
    }

    private void SlotPositionsInitialize(int currentLevel)
    {
        Level level = null;
        if (currentLevel <= database.GetLevelsFromDatabase().Length)
        {
            level = database.GetLevelsFromDatabase()[currentLevel - 1]?.GetInstance();
        }

        if (level != null)
        {
            GameEvents.OnLoadLevel?.Invoke(currentLevel);
        }
        else
        {
            GameEvents.OnGameFinish?.Invoke();
            return;
        }

        //Create Dictionary for Positions
        for (int i = 0; i < level.GetLevelPositionListCount(); i++)
        {
            slotPositionsDictionary.Add(i, level.GetPositions(i));
        }
    }
    private List<Item> GenerateItems(int slotCount)
    {
        List<Item> allItems = new List<Item>();

        //Formula: slotCount = countPerItem'9' * itemCount;
        //Formula: itemCount = slotCount / countPerItem'9'

        int requiredItemCount = slotCount / 6;

        for (int index = 0; index < requiredItemCount; index++)
        {
            for (int i = 0; i < 6; i++)
            {
                allItems.Add(database.GetItemsFromDatabase()[index]);
            }
        }
        Debug.Log($"All items count: {allItems.Count}");
        return allItems;
    }
    private void SlotItemsInitialize(List<Item> allItems)
    {
        for (int zIndex = 0; zIndex < slotPositionsDictionary.Count; zIndex++)
        {
            GameObject parent = new GameObject($"Slots {zIndex}");
            parentList.Add(parent);

            for (int positionIndex = 0; positionIndex < slotPositionsDictionary[zIndex].Count; positionIndex++)
            {
                if (allItems.Count <= 0) break;

                Item item = allItems[Random.Range(0, allItems.Count)];

                Vector3 position = slotPositionsDictionary[zIndex][positionIndex];

                if (zIndex > 0)
                {
                    for (int i = zIndex - 1; i >= 0; i--)
                    {
                        int z = i;
                        SlotController.SlotAroundCheck(new Vector3(position.x, position.y, i), slotItemsDictionary);
                    }
                }

                ItemSlot slot = Instantiate(slotPrefab, position, Quaternion.identity, parent.transform);
                slotItemsDictionary.Add(position, slot);

                slot.Item = item;
                slot.OnClickEvent += SlotClicked;

                allItems.Remove(item);
            }
        }
    }

    private void SlotClicked(ItemSlot slot)
    {
        Vector3 position = slot.transform.position;
        slotItemsDictionary.Remove(position);

        for (int underZ = (int)position.z - 1; underZ >= 0; underZ--)
        {
            List<ItemSlot> slots = SlotController.GetUnderItems(new Vector3(position.x, position.y, underZ), slotItemsDictionary);

            foreach (ItemSlot underSlot in slots)
            {
                Vector3 undPos = underSlot.transform.position;

                bool check = true;
                for (int i = 0; i < SlotController.controlPositions.Length; i++)
                {
                    for (int z = (int)undPos.z; z < slotPositionsDictionary.Count; z++)
                    {
                        if (slotItemsDictionary.ContainsKey(new Vector3(undPos.x, undPos.y, z) + SlotController.controlPositions[i]))
                        {
                            check = false;
                            break;
                        }
                    }
                }

                underSlot.CanPress = check;
            }

        }


        CollectorEvents.OnSlotCollect?.Invoke(slot.Item);

        //Slots control
        if (slotItemsDictionary.Count == 0)
        {
            GameEvents.OnNextGame?.Invoke();
        }

        Destroy(slot.gameObject);
    }
}
