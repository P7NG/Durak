using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    [Header("Type")]

    public CardCost cardCost;
    public Sprite CardSprite;
    public Sprite CardBack;
    public bool Enemys = false;
    public bool CardFace = false;


    [Header("Common")]
    public GameObject Table;
    public GameObject Hand;
    public CursorController Cursor;
    public GameBehaviour GameBehaviourScript;
    public bool Interactable = true;
    public CardPlace CardPlace;

    private Image _image;

    private void Start()
    {
        Cursor = Camera.main.GetComponent<CursorController>();
        GameBehaviourScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameBehaviour>();
        _image = GetComponent<Image>();

        if (Enemys) Interactable = false;

        GameBehaviourScript.DeckArray.Add(this);
    }

    private void Update()
    {
        if(CardFace && _image.sprite != CardSprite)
        {
            _image.sprite = CardSprite;
        }
        else if(!CardFace && _image.sprite != CardBack)
        {
            _image.sprite = CardBack;
        }
    }

    public void DropCard()
    {
        if (!Interactable) return;
        if (GameBehaviourScript.Owner == GameBehaviour.StepOwner.Player)
        {
            if (GameBehaviourScript.OpenPlaces == 1)
            {
                gameObject.transform.parent = GameBehaviourScript.CurrentPlace.transform;
                gameObject.transform.position = GameBehaviourScript.CurrentPlace.transform.position;
                CardPlace place = GameBehaviourScript.CurrentPlace.GetComponent<CardPlace>();
                place.Card = this;
                Interactable = false;

                GameBehaviourScript.DropCard(this);
            }
            else
            {
                for(int i = 0; i < GameBehaviourScript.AppendCosts.Count; i++)
                {
                    if(GameBehaviourScript.AppendCosts[i] == cardCost.cost[0])
                    {
                        gameObject.transform.parent = GameBehaviourScript.CurrentPlace.transform;
                        gameObject.transform.position = GameBehaviourScript.CurrentPlace.transform.position;
                        CardPlace place = GameBehaviourScript.CurrentPlace.GetComponent<CardPlace>();
                        place.Card = this;
                        Interactable = false;

                        GameBehaviourScript.DropCard(this);
                        break;
                    }
                }
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
