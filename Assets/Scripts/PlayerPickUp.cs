using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private TMP_Text gameTexts;
    [SerializeField] private Canvas textCanvas;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask layer;

    private void Start()
    {
     boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        detect();
    }

    private void detect()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 1, layer);
        if (raycastHit.collider != null)
        {
            gameTexts.text = "Press A to pick up";
            textCanvas.gameObject.SetActive(true);
        }
        else
        {
            textCanvas.gameObject.SetActive(false);
        }

    }
}