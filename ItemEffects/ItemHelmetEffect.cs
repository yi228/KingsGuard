using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ItemEffect/Equipment/Helmet")]
public class ItemHelmetEffect : ItemEffect
{
    public int helmetProtection;

    public override bool ExecuteRole()
    {
        Equipment.instance.helmetEquipped = true;
        GameManager.instance.player.helmetOn = true;
        
        return true;
    }

    public void Unequip()
    {
        if (Equipment.instance.helmetEquipped)
        {
            Equipment.instance.helmetEquipped = false;
            GameManager.instance.player.helmetOn = false;
            GameObject semi = Instantiate(ItemDatabase.Instance.itemPrefab, new Vector2(1000, 1000), Quaternion.identity);
            semi.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.itemDB[0]);
            FieldItems fieldItem = semi.GetComponent<FieldItems>();
            Inventory.Instance.AddItem(fieldItem.GetItem());
            Destroy(semi);
        }
    }
}
