using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image slotIcon;

    private Item item;
    public Item Item
    {
        get => item;
        set
        {
            item = value;

            SlotInitialize(item);
        }
    }

    private void SlotInitialize(Item item)
    {
        slotIcon.sprite = item.ItemSprite;
        slotIcon.preserveAspect = true;
        gameObject.name = item.ItemName;
        //effect;
    }
}
