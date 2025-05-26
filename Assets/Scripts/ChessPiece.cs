using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChessPiece : MonoBehaviour
{
    //References
    public GameObject controller;
    public GameObject movePlate;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //Variables to keep track of the piece
    public string player;

    public bool hasMoved = false;

    //Reference to the GameManager
    public Sprite black_queen, black_knight, black_bishop, black_rook, black_pawn, black_king;
    public Sprite white_queen, white_knight, white_bishop, white_rook, white_pawn, white_king;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "black_queen":
                this.GetComponent<SpriteRenderer>().sprite = black_queen;
                player = "black";
                break;
            case "black_knight":
                this.GetComponent<SpriteRenderer>().sprite = black_knight;
                player = "black";
                break;
            case "black_bishop":
                this.GetComponent<SpriteRenderer>().sprite = black_bishop;
                player = "black";
                break;
            case "black_rook":
                this.GetComponent<SpriteRenderer>().sprite = black_rook;
                player = "black";
                break;
            case "black_pawn":
                this.GetComponent<SpriteRenderer>().sprite = black_pawn;
                player = "black";
                break;
            case "black_king":
                this.GetComponent<SpriteRenderer>().sprite = black_king;
                player = "black";
                break;
            case "white_queen":
                this.GetComponent<SpriteRenderer>().sprite = white_queen;
                player = "white";
                break;
            case "white_knight":
                this.GetComponent<SpriteRenderer>().sprite = white_knight;
                player = "white";
                break;
            case "white_bishop":
                this.GetComponent<SpriteRenderer>().sprite = white_bishop;
                player = "white";
                break;
            case "white_rook":
                this.GetComponent<SpriteRenderer>().sprite = white_rook;
                player = "white";
                break;
            case "white_pawn":
                this.GetComponent<SpriteRenderer>().sprite = white_pawn;
                player = "white";
                break;
            case "white_king":
                this.GetComponent<SpriteRenderer>().sprite = white_king;
                player = "white";
                break;
        }

    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1.205f;
        y *= 1.205f;

        x += -4.2f;
        y += -4.2f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<GameManager>().IsGameOver() && controller.GetComponent<GameManager>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            IntiateMovePlates();
        }
    }

    private void IntiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
            case "black_king":
            case "white_king":
                SuroundMovePlate();
                CheckCastling();
                break;
        }
    }

    private void CheckCastling()
    {
        GameManager sc = controller.GetComponent<GameManager>();

        if (player == "white" && !hasMoved)
        {
            GameObject rook = sc.GetPositiont(7, 0);
            if (rook != null && rook.name == "white_rook" && !rook.GetComponent<ChessPiece>().hasMoved)
            {
                if (sc.GetPositiont(5, 0) == null && sc.GetPositiont(6, 0) == null)
                {
                    MovePlateSpawn(6, 0); 
                }
            }
        }
        if (player == "black" && !hasMoved)
        {
            GameObject rook = sc.GetPositiont(0, 7);
            if (rook != null && rook.name == "black_rook" && !rook.GetComponent<ChessPiece>().hasMoved)
            {
                if (sc.GetPositiont(1, 7) == null && sc.GetPositiont(2, 7) == null)
                {
                    MovePlateSpawn(1, 7);
                }
            }
        }
    }


    public void PawnMovePlate(int x, int y)
    {
        GameManager sc = controller.GetComponent<GameManager>();

        int direction = (player == "white") ? 1 : -1;

        if (sc.PositionOnBoard(x, y) && sc.GetPositiont(x, y) == null)
        {
            if(!hasMoved)
            {
                MovePlateSpawn(x, y+direction);
            }
            MovePlateSpawn(x, y);

        }

        if (sc.PositionOnBoard(x + 1, y) && sc.GetPositiont(x + 1, y) != null)
        {
            if (sc.GetPositiont(x + 1, y).GetComponent<ChessPiece>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }
        }

        if (sc.PositionOnBoard(x - 1, y) && sc.GetPositiont(x - 1, y) != null)
        {
            if (sc.GetPositiont(x - 1, y).GetComponent<ChessPiece>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void SuroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
    }

    private void PointMovePlate(int x, int y)
    {
        GameManager sc = controller.GetComponent<GameManager>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPositiont(x, y);
            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else
            {
                if (cp.GetComponent<ChessPiece>().player != player)
                {
                    MovePlateAttackSpawn(x, y);
                }
            }
        }
    }

    private void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 2);
    }

    private void LineMovePlate(int v1, int v2)
    {
        GameManager gameManager = controller.GetComponent<GameManager>();
        int x = xBoard + v1;
        int y = yBoard + v2;

        while (gameManager.PositionOnBoard(x, y) && gameManager.GetPositiont(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += v1;
            y += v2;
        }

        if (gameManager.PositionOnBoard(x, y) && gameManager.GetPositiont(x, y) != null)
        {
            if (gameManager.GetPositiont(x, y).GetComponent<ChessPiece>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (GameObject movePlate in movePlates)
        {
            Destroy(movePlate);
        }
    }

    private void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;
        x *= 1.205f;
        y *= 1.205f;
        x += -4.2f;
        y += -4.2f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -1.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    private void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;
        x *= 1.205f;
        y *= 1.205f;
        x += -4.2f;
        y += -4.2f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
