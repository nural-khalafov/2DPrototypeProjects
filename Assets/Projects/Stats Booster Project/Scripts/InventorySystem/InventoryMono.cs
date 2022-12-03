using StatsBoosterPrototype.InventorySystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMono : MonoBehaviour
{
    [SerializeField]
    private ItemSO[] _inventory;
    public GameObject[] InventorySlots;

    public void Start()
    {
        _inventory = GameObject.Find("Game Controller").GetComponent<ItemDatabase>().ItemList;

        InitializeSlots();

        InventorySlots[0].GetComponent<Image>().sprite = _inventory[0].GetSprite();
    }

    private void InitializeSlots() 
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("InventoryItemSlot");

        foreach (var slot in slots)
        {
            InventorySlots = slots;
        }
    }
}
