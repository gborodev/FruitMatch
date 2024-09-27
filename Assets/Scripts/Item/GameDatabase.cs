using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Database")]
public class GameDatabase : ScriptableObject
{
    Item[] allItems;
    Level[] allLevels;

    public Item[] GetItemsFromDatabase() => allItems;
    public Level[] GetLevelsFromDatabase() => allLevels;

#if UNITY_EDITOR

    private void OnEnable()
    {
        LoadItems();
        EditorApplication.projectChanged += LoadItems;
    }
    private void OnDisable()
    {
        EditorApplication.projectChanged -= LoadItems;
    }

    private void LoadItems()
    {
        allItems = FindAssetsByType<Item>("Assets/Database/ItemsData");
        allLevels = FindAssetsByType<Level>("Assets/Database/LevelsData");
    }

    public static T[] FindAssetsByType<T>(params string[] folders) where T : Object
    {
        string type = typeof(T).ToString().Replace("UnityEngine.", "");

        string[] guids;

        if (folders == null || folders.Length == 0)
        {
            guids = AssetDatabase.FindAssets(string.Format("t:" + type));
        }
        else
        {
            guids = AssetDatabase.FindAssets(string.Format("t:" + type), folders);
        }

        T[] assets = new T[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);

        }
        return assets;
    }
#endif
}
