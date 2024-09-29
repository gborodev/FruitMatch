using Events;
using System.Collections.Generic;
using UnityEngine;

public class SlotCollector : MonoBehaviour
{
    [SerializeField] private Transform slotsContent;
    [SerializeField] private SlotUI slotUIprefab;

    private List<SlotUI> currentSlots = new List<SlotUI>(7);

    private void OnEnable()
    {
        CollectorEvents.OnSlotCollect += CreateSlot;
    }
    private void OnDisable()
    {
        CollectorEvents.OnSlotCollect -= CreateSlot;
    }

    public void CreateSlot(Item item)
    {
        for (int i = 0; i < currentSlots.Count; i++)
        {
            if (item.ID == currentSlots[i].Item.ID)
            {
                int index = i;

                SlotUI slotUI = SlotInstantiate(index);
                slotUI.Item = item;

                if (MatchControl(item))
                {
                    if (LevelManager.Instance.IsLevelFinished)
                    {
                        GameManager.Instance.NextGame();
                    }
                }
                else
                {
                    CheckSlots();
                }

                return;
            }
        }

        SlotUI slot = SlotInstantiate();
        slot.Item = item;

        CheckSlots();
    }

    public bool MatchControl(Item item)
    {
        int counter = 0;

        List<SlotUI> slots = new List<SlotUI>();

        foreach (SlotUI slot in currentSlots)
        {
            if (slot.Item.ID == item.ID)
            {
                counter++;
                slots.Add(slot);

                if (counter > 2)
                {
                    foreach (SlotUI del in slots)
                    {
                        currentSlots.Remove(del);
                        Destroy(del.gameObject);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public void CheckSlots()
    {
        if (currentSlots.Count >= currentSlots.Capacity)
        {
            GameEvents.OnGameOver?.Invoke();
        }
    }

    private SlotUI SlotInstantiate(int index = -1)
    {
        SlotUI slotUI = Instantiate(slotUIprefab, slotsContent);

        if (index != -1)
        {
            slotUI.transform.SetSiblingIndex(index);
            currentSlots.Insert(index, slotUI);
        }
        else
        {
            currentSlots.Add(slotUI);
        }

        return slotUI;
    }
}
