using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private Level testLevel;
    [Space]

    [SerializeField] private ItemSlot slotPrefab;

    private List<Vector3[]> slotPositionsList;

    private void Start()
    {
        Level level = testLevel.GetInstance();
        List<Item> levelItems = level.GetLevelItems();

        //Create Dictionary for Positions
        slotPositionsList = new List<Vector3[]>();
        for (int i = 0; i < level.GetLevelPositionListCount(); i++)
        {
            slotPositionsList.Add(level.GetPositions(i));
        }

        int slotcount = 0;
        slotPositionsList.ForEach((x) => slotcount += x.Length);


        //Collect All Items
        List<Item> allItems = new List<Item>();
        for (int index = 0; index < levelItems.Count; index++)
        {
            int remain = slotcount / 3;
            int amount = 3 + 3 * remain;

            Debug.Log(amount);

            for (int i = 0; i < amount; i++)
            {
                allItems.Add(levelItems[index]);
            }
        }

        for (int i = 0; i < slotPositionsList.Count; i++)
        {
            GameObject parent = new GameObject($"Slots {i}");

            for (int positionIndex = 0; positionIndex < slotPositionsList[i].Length; positionIndex++)
            {
                if (allItems.Count <= 0) break;

                int randomItemIndex = Random.Range(0, allItems.Count);
                Item item = allItems[randomItemIndex];

                Vector3 position = slotPositionsList[i][positionIndex];

                ItemSlot slot = Instantiate(slotPrefab, position, Quaternion.identity, parent.transform);
                slot.Item = item;

                allItems.Remove(item);
            }
        }

        Debug.Log($"Slots: {slotcount} / Items: {allItems.Count}");
    }
}
