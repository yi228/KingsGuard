using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public Image armorEpuipImage;
    public Image bootsEpuipImage;
    public Image helmetEpuipImage;
    public Image swordEpuipImage;

    public static Equipment instance;

    public bool helmetEquipped;
    public bool armorEquipped;
    public bool bootsEquipped;
    public bool swordEquipped;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        ColorControl();
    }

    void ColorControl()
    {
        if (armorEquipped)
        {
            armorEpuipImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            armorEpuipImage.color = new Color32(255, 255, 255, 100);
        }

        if (bootsEquipped)
        {
            bootsEpuipImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            bootsEpuipImage.color = new Color32(255, 255, 255, 100);
        }

        if (helmetEquipped)
        {
            helmetEpuipImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            helmetEpuipImage.color = new Color32(255, 255, 255, 100);
        }

        if (swordEquipped)
        {
            swordEpuipImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            swordEpuipImage.color = new Color32(255, 255, 255, 100);
        }
    }

}
