using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager Instance;
    public Inventory currInventory;

    public const int numItems = 16;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            GameObject.Destroy(gameObject);
    }
    void Start()
    {
    }

}
//Class for inventory objects
[CreateAssetMenu(fileName = "BlankInventory",menuName="Inventory",order = 3)]
public class Inventory:ScriptableObject 
{
    public Item[] items;

    //Builds an empty iventory
    public Inventory()
    {
        items = new Item[InventoryManager.numItems];
        for (int i = 0; i < InventoryManager.numItems; i++)
        {
            items[i] = null;
        }
        Debug.Log("Inventory Constructed of " + InventoryManager.numItems + " items");
    }
    //Adds the inventory to the array 
    public void AddItem(Item addItem)
    {
        for(int i =0;i < InventoryManager.numItems; i++)
        {
            if (items[i] == null)
            {
                items[i] = addItem;
                Debug.Log("Added item:" + addItem.name + " to inventory");
                return;
            }
        }
        
            Debug.Log("ERROR:Tried to add item:" + addItem.name + " to a full inventory");
            return;
    }
    //Removes the first instance of an item from the inventory 
    public void RemoveItem(Item removeItem)
    {
        for (int i = 0; i < InventoryManager.numItems; i++)
        {
            if (items[i] == removeItem)
            {
                items[i] = null;
            }
        }
        return;
    }
    //sorts the inventory by ID TODO
    public void SortInventory()
    {
        return;
    }

}
