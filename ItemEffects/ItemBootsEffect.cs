using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ItemEffect/Equipment/Boots")]
public class ItemBootsEffect : ItemEffect
{
    public int bootsSpeed;

    public override bool ExecuteRole()
    {
        Equipment.instance.bootsEquipped = true;
        GameManager.instance.player.speed += bootsSpeed;
        GameManager.instance.player.baseSpeed += bootsSpeed;
        GameManager.instance.player.runSpeed += bootsSpeed*2;
        
        return true;
    }

    public void Unequip()
    {
        if (Equipment.instance.bootsEquipped)
        {
            Equipment.instance.bootsEquipped = false;
            GameManager.instance.player.speed -= bootsSpeed;
            GameManager.instance.player.baseSpeed -= bootsSpeed;
            GameManager.instance.player.runSpeed -= bootsSpeed * 2;
            GameObject semi = Instantiate(ItemDatabase.Instance.itemPrefab, new Vector2(1000, 1000), Quaternion.identity);
            semi.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.itemDB[3]);
            FieldItems fieldItem = semi.GetComponent<FieldItems>();
            Inventory.Instance.AddItem(fieldItem.GetItem());
            Destroy(semi);
        }
    }
}
