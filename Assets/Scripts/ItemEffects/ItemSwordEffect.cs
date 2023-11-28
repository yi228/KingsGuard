using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ItemEffect/Equipment/Sword")]
public class ItemSwordEffect : ItemEffect
{
    public int swordAttack;
    public Sprite itemImage;
    public List<ItemEffect> effects;

    public override bool ExecuteRole()
    {
        Equipment.instance.swordEquipped = true;
        GameManager.instance.player.attack += swordAttack;
        
        return true;
    }

    public void Unequip()
    {
        if (Equipment.instance.swordEquipped)
        {
            Equipment.instance.swordEquipped = false;
            GameManager.instance.player.attack -= swordAttack;
            GameObject semi = Instantiate(ItemDatabase.Instance.itemPrefab, new Vector2(1000, 1000), Quaternion.identity);
            semi.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.itemDB[2]);
            FieldItems fieldItem = semi.GetComponent<FieldItems>();
            Inventory.Instance.AddItem(fieldItem.GetItem());
            Destroy(semi);
        }
    }
}
