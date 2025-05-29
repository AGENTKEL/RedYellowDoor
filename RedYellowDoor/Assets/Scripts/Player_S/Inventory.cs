using System.Collections;
using System.Collections.Generic;
using CameraDoorScript;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItemData
{
    public Sprite itemSprite;
    public ItemType itemType;
}

public class Inventory : MonoBehaviour
{
    [Header("Inventory Slots")]
    public Image[] slotImages = new Image[3]; // UI slots
    private InventoryItemData[] items = new InventoryItemData[3];
    public CameraOpenDoor playerCameraInteract;

    public bool AddItem(Interactable item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                // Store only necessary data
                items[i] = new InventoryItemData
                {
                    itemSprite = item.itemSprite,
                    itemType = item.itemType
                };

                if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
                {
                    slotImages[i].sprite = item.itemSprite;
                    slotImages[i].enabled = true;
                }

                Destroy(item.gameObject); // Remove object from scene
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public bool HasItem(ItemType type)
    {
        foreach (var item in items)
        {
            if (item != null && item.itemType == type)
                return true;
        }
        return false;
    }

    public bool UseItem(ItemType type)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemType == type)
            {
                items[i] = null;

                if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
                {
                    slotImages[i].enabled = false;
                    slotImages[i].sprite = null;
                }

                return true;
            }
        }

        return false;
    }
}

