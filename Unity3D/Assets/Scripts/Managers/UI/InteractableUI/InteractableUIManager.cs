using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUIManager : MonoBehaviour
{
    [SerializeField] private Image image;
    private TextMeshProUGUI textMeshPro;
    [SerializeField] InteractableIcon[] icons;

    private void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SelectIcon(int idx)
    {
        if (idx < 0 || idx >= icons.Length) return;

        image.gameObject.SetActive(true);
        image.sprite = icons[idx].Sprite;
        textMeshPro.text = icons[idx].Text;
    }
    public void DeselectIcon()
    {
        if (image == null) return;
        image.sprite = null;
        textMeshPro.text = "";
        image.gameObject.SetActive(false);
    }
}
