using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private string clickSoundName = "cursor_Select";
    [SerializeField] private string hoverSoundName = "cursor_Hover";

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance?.PlaySound2D(clickSoundName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance?.PlaySound2D(hoverSoundName);
    }
}