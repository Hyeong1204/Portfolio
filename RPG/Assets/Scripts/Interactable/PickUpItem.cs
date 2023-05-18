using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : Interactable
{
    public Item item;

    public override void Interact()
    {
        SelectItem();
    }

    private void SelectItem()
    {
        // 아이템 획득;
        Debug.Log("아이템");
        bool isSelected = Inventory.Instance.Add(item);
        if (isSelected)
        {
            if(item.audioClip != null)
            {
                AudioManager.instance.source.clip = item.audioClip;
                AudioManager.instance.source.Play();
            }

            Destroy(this.gameObject); 
        }
    }
}
