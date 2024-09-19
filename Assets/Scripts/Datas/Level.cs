using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Level")]
public class Level : ScriptableObject
{
    [Header("Level attributes")]
    [SerializeField] private Sprite levelSprite;

    [Header("Level items")]
    [SerializeField] private List<LevelItem> levelItems;

    private List<Vector2> levelPositions;

    public Level GetInstance() => Instantiate(this);

    private void OnEnable()
    {
        levelPositions = new List<Vector2>();

        if (levelSprite == null) return;

        float xSize = levelSprite.texture.width;
        float ySize = levelSprite.texture.height;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Color point = levelSprite.texture.GetPixel(x, y);

                if (point.a > 0)
                {
                    float xPos = (x - xSize / 2) + 0.5f;
                    float yPos = (y - ySize / 2) + 0.5f;

                    levelPositions.Add(new Vector2(xPos, yPos));
                }
            }
        }
    }

    public Vector2[] GetPositions() => levelPositions.ToArray();
    public List<LevelItem> GetLevelItems() => levelItems;
}

[Serializable]
public class LevelItem
{
    public Item item;
    public int stackAmount;
}
