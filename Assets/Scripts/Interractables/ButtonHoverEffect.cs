using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color hoverTextColor = Color.yellow;
    [SerializeField] private Color hoverImageColor = new Color(0.9f, 0.9f, 0.9f);

    private Color normalTextColor;
    private Color normalImageColor;

    void Awake()
    {
        // Auto-get references if not set
        if (buttonText == null) buttonText = GetComponentInChildren<TMP_Text>();
        if (buttonImage == null) buttonImage = GetComponent<Image>();
        
        normalTextColor = buttonText.color;
        normalImageColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverTextColor;
        buttonImage.color = hoverImageColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalTextColor;
        buttonImage.color = normalImageColor;
    }
}