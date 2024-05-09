using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownRoomItem : MonoBehaviour
{
    public Vector2[] pos;

    private void Start()
    {
        for(int i=0; i < pos.Length; i++)
        {
            GameObject dropItem = Instantiate(ItemDatabase.Instance.itemPrefab, pos[i], Quaternion.identity);
            dropItem.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.itemDB[Random.Range(0, 6)]);
        }
    }
}
