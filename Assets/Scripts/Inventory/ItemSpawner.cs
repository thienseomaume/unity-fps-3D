using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> listItems;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnItem(ItemsEnum item, int amount, Vector3 position)
    {
        foreach(GameObject itemObject in listItems)
        {
            PickableItem itemScript = itemObject.GetComponent<PickableItem>();
            if (itemScript != null)
            {
                Debug.Log("checked spawn: "+itemScript.GetItemType().ToString());
                if (itemScript.GetItemType() == item)
                {
                    Instantiate(itemObject, position, Quaternion.identity).GetComponent<PickableItem>()?.SetAmount(amount);
                }
            }
        }
    }
}
