using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] images;

    private List<HiddenObject> hiddenObjects;
    private Image[] imagesUI;

    [SerializeField] private int maxSizeInventory = 6;
    public int MaxSizeInventory { get { return maxSizeInventory; } }

    void Start()
    {
        hiddenObjects = new List<HiddenObject>();

        imagesUI = new Image[images.Length];

        for (int i = 0; i < images.Length; i++)
        {
            imagesUI[i] = images[i].GetComponent<Image>();
        }
    }

    public void AddObject(HiddenObject hiddenObject)
    {
        if(!IsInventoryFull())
        {
            hiddenObjects.Add(hiddenObject);
            UpdateInventory();
        }

    } 

    public void RemoveObject(HiddenObject hiddenObject)
    {
        hiddenObjects.Remove(hiddenObject);
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        foreach (GameObject image in images)
        {
            image.SetActive(false);
        }

        for (int i = 0;i < hiddenObjects.Count;i++)
        {
            images[i].SetActive(true);
            imagesUI[i].sprite = hiddenObjects[i].uiImage;
        }
    }

    public List<HiddenObject> GetInventoryList() => hiddenObjects;

    public bool IsInventoryFull() => hiddenObjects.Count >= maxSizeInventory;
}
