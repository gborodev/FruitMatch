using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("References")]
    [SerializeField] private GameDatabase database;
    [SerializeField] private ItemSlot slotPrefab;

    private List<GameObject> parentList = new List<GameObject>();
    private Dictionary<Vector3, ItemSlot> slotItemsDictionary;

    private LevelData currentData;

    public void SetLevel(int level)
    {
        foreach (var parent in parentList)
        {
            Destroy(parent.gameObject);
        }
        parentList?.Clear();

        LevelInitialized(level);
    }

    public void LevelInitialized(int currentLevel)
    {
        int levelIndex = currentLevel - 1;
        Level[] levels = database.GetLevelsFromDatabase();

        if (levelIndex < 0 || levelIndex > levels.Length - 1)
        {
            Debug.LogWarning("Level is not found");
            return;
        }

        Level level = database.GetLevelsFromDatabase()[currentLevel - 1]?.GetInstance();

        if (level is null) Debug.Log("Level is not found");

        LevelData levelData = new LevelData(level);
        currentData = levelData;
        slotItemsDictionary = new Dictionary<Vector3, ItemSlot>();
    }

}
