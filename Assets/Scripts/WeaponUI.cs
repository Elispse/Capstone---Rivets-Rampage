using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Image iconUI;
    [SerializeField] TextMeshProUGUI magSizeTxt;
    [SerializeField] TextMeshProUGUI magCountTxt;

    private void Start()
    {
        iconUI = GameObject.Find("Icon").GetComponent<Image>();
        magSizeTxt = GameObject.Find("MagSizeTxt").GetComponent<TextMeshProUGUI>();
        magCountTxt = GameObject.Find("MagCountTxt").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateInfo(Sprite weaponIcon, int magSize, int magCount)
    {
        iconUI.sprite = weaponIcon;
        magSizeTxt.text = magSize.ToString();
        magCountTxt.text = magCount.ToString();
    }

    public void UpdateAmmoCount(int magCount)
    {
        magCountTxt.text = magCount.ToString();
    }
}