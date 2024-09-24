using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Level")]
public class Level : ScriptableObject
{
    [Header("Level attributes")]
    [SerializeField] private Sprite[] levelSprites;

    private Dictionary<int, List<Vector3>> levelPositions;

    private void Awake()
    {
        levelPositions = new Dictionary<int, List<Vector3>>();

        if (levelSprites == null) return;

        for (int i = 0; i < levelSprites.Length; i++)
        {
            int index = i;

            Sprite sprite = levelSprites[index];

            float xSize = sprite.rect.size.x;
            float ySize = sprite.rect.size.y;

            int pointCount = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Color point = sprite.texture.GetPixel(x + (int)sprite.rect.xMin, y + (int)sprite.rect.yMin);

                    if (point.a > 0)
                    {
                        float xPos = (x - xSize / 2) + 0.5f;
                        float yPos = (y - ySize / 2) + 0.5f;

                        Vector3 position = new Vector3(xPos, yPos, index);

                        if (levelPositions.ContainsKey(index))
                        {
                            levelPositions[index].Add(position);
                        }
                        else
                        {
                            levelPositions.Add(index, new List<Vector3>()
                            {
                                position
                            });
                        }

                        pointCount++;
                    }
                }
            }
        }
    }

    public Level GetInstance() => Instantiate(this);
    public int GetLevelPositionListCount() => levelPositions.Count;
    public List<Vector3> GetPositions(int index) => levelPositions[index];
}

[Serializable]
public class LevelItem
{
    public Item item;
    public int stackAmount;
}
