using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ItemEffect/Equipment/Armor")]
public class ItemArmorEffect : ItemEffect
{
    public int armorProtection;

    public override bool ExecuteRole()
    {
        Equipment.instance.armorEquipped = true;
        GameManager.instance.player.armorOn = true;
        
        return true;
    }

    public void Unequip()
    {
        if (Equipment.instance.armorEquipped)
        {
            Equipment.instance.armorEquipped = false;
            GameManager.instance.player.armorOn = false;
            GameObject semi = Instantiate(ItemDatabase.Instance.itemPrefab, new Vector2(1000, 1000), Quaternion.identity);
            semi.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.itemDB[1]);
            FieldItems fieldItem = semi.GetComponent<FieldItems>();
            Inventory.Instance.AddItem(fieldItem.GetItem());
            Destroy(semi);
        }
    }
}
