using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private Level testLevel;
    [Space]

    [SerializeField] private ItemSlot slotPrefab;

    private Dictionary<Vector3, ItemSlot> slotDictionary;

    private void Start()
    {
        slotDictionary = new Dictionary<Vector3, ItemSlot>();

        Level level = testLevel.GetInstance();

        Vector2[] positions = level.GetPositions();
        List<LevelItem> levelItems = level.GetLevelItems();

        for (int i = 0; i < positions.Length; i++)
        {
            if (levelItems is null || levelItems.Count <= 0) return;

            LevelItem levelItem = levelItems[Random.Range(0, levelItems.Count)];

            for (int j = 0; j < levelItem.stackAmount * 3; j++)
            {
                Vector3 spawnPosition = positions[Random.Range(0, positions.Length)];

                while (true)
                {
                    if (slotDictionary.ContainsKey(spawnPosition))
                    {
                        spawnPosition.z += 1;
                    }
                    else break;
                }

                ItemSlot slot = Instantiate(slotPrefab, spawnPosition, Quaternion.identity);
                slotDictionary.Add(spawnPosition, slot);

                slot.Item = levelItem.item;

                float opX = spawnPosition.z % 2 == 0 ? -1 : 1;

                Vector3 direction = new Vector2(opX, 0).normalized;
                Vector3 newPosition = spawnPosition + direction * Random.Range(-0.3f, 0.3f);

                slot.transform.position = new Vector3(newPosition.x, spawnPosition.y, spawnPosition.z);
            }

            levelItems.Remove(levelItem);
        }
    }
}
