using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    private int leftCorner = 704;
    private int bottomCorner = 289;
    private int distanceBetweenCells = 100;
    [SerializeField] private Color startColor = new Color(0.8f, 0.6f, 0.2f);
    [SerializeField] private Color endColor = new Color(1.0f, 1.0f, 0.4f);  
    private SpriteRenderer spriteRenderer;
    private Cell cell;
    private TextMeshPro text;

    public void Init(Cell newCell)
    {
        cell = newCell;
        cell.OnValueChanged += UpdateValue;
        cell.OnPositionChanged += UpdatePosition;
        cell.OnDeleted += Delete;

        UpdateValue(cell.Value);
        UpdatePosition(cell.Position);
    }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateValue(int newValue)
    {
        if (text != null)
        {
            text.text = Math.Pow(2, newValue).ToString();
        }

        float lerpFactor = Mathf.Clamp01(Mathf.Log(newValue, 2) / 10f);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, lerpFactor);
        }
    }

    private void UpdatePosition(Vector2Int newPosition)
    {
        transform.position = new Vector3(leftCorner + newPosition.x * distanceBetweenCells, bottomCorner + newPosition.y * distanceBetweenCells, 0);
    }

    private void Delete() 
    {
        Destroy(gameObject);
    }
}