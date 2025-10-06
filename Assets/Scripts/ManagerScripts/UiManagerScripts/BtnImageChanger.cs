using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image btnImage; // 바꿀 이미지
    public Sprite normalImage; // 기존 스프라이트
    public Sprite hoverImage; // 커서를 올려놨을 시 바뀌는 스프라이트

    void Start ()
    {
        if (btnImage == null)
        {
            btnImage = GetComponent<Image>();
        }
    } // start ed

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btnImage != null && hoverImage != null)
        {
            btnImage.sprite = hoverImage;
        }
    } // OPEnter ed

    public void OnPointerExit(PointerEventData eventData)
    {
        if (btnImage != null && hoverImage != null)
        {
            btnImage.sprite = normalImage;
        }
    } // OPExit ed
} // classs ed 
