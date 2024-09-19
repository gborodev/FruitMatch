using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] SpriteRenderer sp;

    private Item item;
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            if (item != null)
            {
                sp.sprite = item.ItemSprite;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click " + gameObject.name);
    }
}
