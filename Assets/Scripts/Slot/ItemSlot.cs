using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item")]
    [SerializeField] SpriteRenderer iconSp;

    [Header("Slot")]
    [SerializeField] SpriteRenderer sp;
    [SerializeField] Collider2D col;

    [SerializeField] Color normalColor;
    [SerializeField] Color disableColor;

    public event Action<ItemSlot> OnClickEvent;

    private bool activating = false;
    public bool CanPress
    {
        get => activating;
        set
        {
            activating = value;

            if (activating is false)
            {
                sp.color = disableColor;
                col.enabled = false;
            }
            else
            {
                sp.color = normalColor;
                col.enabled = true;
            }
        }
    }

    private Item item;
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            if (item != null)
            {
                iconSp.sprite = item.ItemSprite;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent?.Invoke(this);
    }
}
