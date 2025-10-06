using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image btnImage; // �ٲ� �̹���
    public Sprite normalImage; // ���� ��������Ʈ
    public Sprite hoverImage; // Ŀ���� �÷����� �� �ٲ�� ��������Ʈ

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
