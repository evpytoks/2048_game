using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int value = 0;
    private Vector2Int position;
    public event Action<int> OnValueChanged;
    public event Action<double> OnValueChangedDiff;
    public event Action<Vector2Int> OnPositionChanged;
    public event Action OnDeleted;

    public int Value 
    {
        get { return value; }
        set
        {
            if (this.value != value)
            {
                if (this.value == 0) {
                    OnValueChangedDiff?.Invoke(Math.Pow(2, value));
                } else {
                    OnValueChangedDiff?.Invoke(Math.Pow(2, value) - Math.Pow(2, this.value));
                }
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

    }

    public Vector2Int Position
    {
        get { return position; }
        set
        {
            if (position != value)
            {
                position = value;
                OnPositionChanged?.Invoke(value);
            }
        }
    }

    public Cell(int value, Vector2Int position)
    {
        OnValueChangedDiff += Score.Instance.UpdateScore;
        Value = value;
        Position = position;
    }

    public void Delete() 
    {
        OnValueChangedDiff?.Invoke(-Math.Pow(2, Value));
        OnDeleted?.Invoke();
    }
}
