using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    private List<HiddenObject> hiddenObjects;
    [SerializeField] private GameObject[] images;
    private Image[] imagesUI;

    // Start is called before the first frame update
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
        hiddenObjects.Add(hiddenObject);
        UpdateInventory();
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
}
