using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    private void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<GameManager>().GetPositiont(matrixX, matrixY);

            if (cp.name == "white_king") controller.GetComponent<GameManager>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<GameManager>().Winner("white");

            Destroy(cp);
        }

        controller.GetComponent<GameManager>().SetPositionEmpty(reference.GetComponent<ChessPiece>().GetXBoard(),
            reference.GetComponent<ChessPiece>().GetYBoard());

        ChessPiece piece = reference.GetComponent<ChessPiece>();

        if (piece.name == "white_king" && piece.GetXBoard() == 4 && matrixX == 6 && matrixY == 0)
        {
            GameObject rook = controller.GetComponent<GameManager>().GetPositiont(7, 0);
            rook.GetComponent<ChessPiece>().SetXBoard(5);
            rook.GetComponent<ChessPiece>().SetCoords();
            controller.GetComponent<GameManager>().SetPositionEmpty(7, 0);
            controller.GetComponent<GameManager>().SetPosition(rook);
        }

        if (piece.name == "black_king" && piece.GetXBoard() == 3 && matrixX == 1 && matrixY == 7)
        {
            GameObject rook = controller.GetComponent<GameManager>().GetPositiont(0, 7);
            rook.GetComponent<ChessPiece>().SetXBoard(2);
            rook.GetComponent<ChessPiece>().SetCoords();
            controller.GetComponent<GameManager>().SetPositionEmpty(0, 7);
            controller.GetComponent<GameManager>().SetPosition(rook);
        }

        controller.GetComponent<GameManager>().SetPositionEmpty(piece.GetXBoard(), piece.GetYBoard());
        piece.SetXBoard(matrixX);
        piece.SetYBoard(matrixY);
        piece.SetCoords();


        controller.GetComponent<GameManager>().SetPosition(reference);

        controller.GetComponent<GameManager>().NextTurn();

        TryPromotePawn(reference);

        reference.GetComponent<ChessPiece>().DestroyMovePlates();
        reference.GetComponent<ChessPiece>().hasMoved = true;

        if (reference.name.Contains("_king"))
        {
            reference.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }


    public void TryPromotePawn(GameObject piece)
    {
        ChessPiece cp = piece.GetComponent<ChessPiece>();

        if (cp.name == "white_pawn" && cp.GetYBoard() == 7)
        {
            cp.name = "white_queen";
            cp.GetComponent<SpriteRenderer>().sprite = cp.white_queen;
        }
        else if (cp.name == "black_pawn" && cp.GetYBoard() == 0)
        {
            cp.name = "black_queen";
            cp.GetComponent<SpriteRenderer>().sprite = cp.black_queen;
        }
    }


    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}