using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    [Header("Type")]

    [Range(6, 14)]
    public int cost;
    public Suit suit;

    
    [Header("Common")]
    public GameObject Table;
    public GameObject Hand;
    public CursorController Cursor;
    public GameBehaviour GameBehaviourScript;
    public bool Interactable = true;
    public CardPlace cardPlace;

    private bool _drag = false;

    private void Start()
    {
        Cursor = Camera.main.GetComponent<CursorController>();
        GameBehaviourScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameBehaviour>();
    }

    public void Drag()
    {
        if (Interactable)
        {
            if (!_drag)
            {
                transform.parent = Table.transform;
                _drag = true;
                Cursor.HaveCard = true;
                Cursor.Card = this;
            }

            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
        }
    }

    public void Drop()
    {
        if (Interactable)
        {
            if (Cursor.CurrentTarget == Target.Hand)
            {
                transform.parent = Hand.transform;
                _drag = false;
                Cursor.HaveCard = false;
                Cursor.Card = null;
            }
            else if (Cursor.CurrentTarget == Target.Other)
            {
                transform.parent = Cursor.CurrentCardPlace.transform;
                _drag = false;
                Cursor.HaveCard = false;
                Cursor.Card = null;
                Cursor.CurrentCardPlace.GetComponent<CardPlace>().Card = this;
                Interactable = false;
                Cursor.CurrentCardPlace.GetComponent<CardPlace>().IsActive = false;
                gameObject.transform.position = Cursor.CurrentCardPlace.transform.position;
                GameBehaviourScript.CardAtTable(this);
            }
        }
    }
}

public enum Suit
{
    Peaks,
    Diamonds,
    Hearts,
    Cross
}
