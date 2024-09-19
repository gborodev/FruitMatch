using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string ID { get; private set; }

    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;

    //Gets
    public Sprite ItemSprite => itemIcon;

    private void OnEnable()
    {
        ID = Guid.NewGuid().ToString();
    }
}
