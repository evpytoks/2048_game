using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameFiled : MonoBehaviour
{
    private int fieldSize = 4;
    private int leftCorner = 704;
    private int bottomCorner = 289;
    private int distanceBetweenCells = 100;
    [SerializeField] private GameObject cellPrefab;
    private Input input;
    private List<Cell> cells = new();

    private void Start()
    {
        input = FindObjectOfType<Input>();
        input.OnMove += Move;
        CreateCell();
    }

    private bool IsPositionEmpty(Vector2Int position)
    {
        foreach (Cell cell in cells)
        {
            if (cell.Position == position)
                return false;
        }
        return true;
    }

    private Cell GetCellFromPosition(Vector2Int position)
    {
        foreach (Cell cell in cells)
        {
            if (cell.Position == position)
                return cell;
        }
        return null;
    }

    private Vector2Int GetEmptyPosition() {
        List<Vector2Int> places = new List<Vector2Int>();
        for (int x = 0; x < fieldSize; ++x) 
        {
            for (int y = 0; y < fieldSize; ++y) 
            {
                Vector2Int possiblePosition = new Vector2Int(x, y);

                if (IsPositionEmpty(possiblePosition)) 
                {
                    places.Add(possiblePosition);
                }
            }
        }

        if (places.Count == 0) {
            throw new Exception("No place to create a cell.");
        }

        int ind = UnityEngine.Random.Range(0, places.Count);
        return places[ind];
    }

    private void CreateCell() {
        Vector2Int cellPosition = GetEmptyPosition();
        int cellValue = (UnityEngine.Random.value < 0.8f) ? 1 : 2;
        
        Cell newCell = new Cell(cellValue, cellPosition);
        cells.Add(newCell);

        CellView newCellView = Instantiate
        (
            cellPrefab,
            new Vector3(leftCorner + cellPosition.x * distanceBetweenCells, bottomCorner + cellPosition.y * distanceBetweenCells, 0), 
            Quaternion.identity
        ).GetComponent<CellView>();
        newCellView.Init(newCell);
    }

    private bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.x < fieldSize &&
               position.y >= 0 && position.y < fieldSize;
    }

    private void SortByDirection(Vector2Int direction) {
        if (direction == Vector2Int.up) 
        {
            cells.Sort((a, b) => b.Position.y.CompareTo(a.Position.y));
        }
        else if (direction == Vector2Int.down) 
        {
            cells.Sort((a, b) => a.Position.y.CompareTo(b.Position.y));
        }
        else if (direction == Vector2Int.right) 
        {
            cells.Sort((a, b) => b.Position.x.CompareTo(a.Position.x));
        }
        else if (direction == Vector2Int.left)
        {
            cells.Sort((a, b) => a.Position.x.CompareTo(b.Position.x));
        }
    }

    private void TryMove(Vector2Int direction)
    {
        bool[] merged = new bool[fieldSize * fieldSize];
        List<Cell> cellsToRemove = new List<Cell>();
        SortByDirection(direction);
        

        foreach (Cell cell in cells)
        {
            Vector2Int currentPosition = cell.Position;
            Vector2Int newPosition = currentPosition + direction;
            
            while (IsPositionValid(newPosition) && IsPositionEmpty(newPosition))
            {
                currentPosition = newPosition;
                newPosition = currentPosition + direction;
            }
            cell.Position = currentPosition;
        }

        foreach (Cell cell in cells)
        {
            Vector2Int currentPosition = cell.Position;
            Vector2Int newPosition = currentPosition + direction;

            while (IsPositionValid(newPosition) && !merged[newPosition.x + newPosition.y * fieldSize])
            {
                if (IsPositionEmpty(newPosition))
                {
                    currentPosition = newPosition;
                    newPosition = currentPosition + direction;
                    cell.Position = currentPosition;
                } 
                else 
                {
                    Cell otherCell = GetCellFromPosition(newPosition);
                    if (otherCell != null && otherCell.Value == cell.Value)
                    {
                        otherCell.Value += 1;
                        merged[otherCell.Position.x + otherCell.Position.y * fieldSize] = true;
                        cell.Position = newPosition;
                        cellsToRemove.Add(cell);
                    }
                    break;
                }
            }
        }
        

        foreach (Cell cell in cellsToRemove)
        {
            cells.Remove(cell);
            cell.Delete();
        }
    }

    private void Move(Vector2Int direction)
    {
        TryMove(direction);
        CreateCell();
    }
}