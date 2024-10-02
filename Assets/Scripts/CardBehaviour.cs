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
    public bool NeedAddToDeck = true;

    private Image _image;

    private void Start()
    {
        Cursor = Camera.main.GetComponent<CursorController>();
        GameBehaviourScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameBehaviour>();
        _image = GetComponent<Image>();

        if (Enemys) Interactable = false;

        if (NeedAddToDeck)
        {
            GameBehaviourScript.DeckArray.Add(this);
        }
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
                gameObject.transform.parent = GameBehaviourScript.CurrentPlace.GetComponent<CardPlace>().CardPlaceFact;
                CardPlace place = GameBehaviourScript.CurrentPlace.GetComponent<CardPlace>();
                place.Card = this;
                Interactable = false;

                GameBehaviourScript.DropCard(this);
                GameBehaviourScript.PlayerStack.Remove(this);
            }
            else
            {
                for(int i = 0; i < GameBehaviourScript.AppendCosts.Count; i++)
                {
                    if(GameBehaviourScript.AppendCosts[i] == cardCost.cost[0])
                    {
                        gameObject.transform.parent = GameBehaviourScript.CurrentPlace.GetComponent<CardPlace>().CardPlaceFact;
                        CardPlace place = GameBehaviourScript.CurrentPlace.GetComponent<CardPlace>();
                        place.Card = this;
                        Interactable = false;

                        GameBehaviourScript.DropCard(this);
                        GameBehaviourScript.PlayerStack.Remove(this);
                        break;
                    }
                }
            }
        }
        else
        {
            if (GameBehaviourScript.Cards.Count % 2 == 0) return;
            CardBehaviour enemysCard = GameBehaviourScript.Cards[GameBehaviourScript.Cards.Count - 1];

            if ((cardCost.cost[0] > enemysCard.cardCost.cost[0] && cardCost.suit == enemysCard.cardCost.suit) || (enemysCard.cardCost.suit != GameBehaviourScript.KozyrSuit && cardCost.suit == GameBehaviourScript.KozyrSuit))
            {
                gameObject.transform.parent = enemysCard.CardPlace.CardPlaceFact;
                Interactable = false;
                GameBehaviourScript.Cards.Add(this);
                GameBehaviourScript.PlayerStack.Remove(this);
                GameBehaviourScript.AppendCosts.Add(cardCost.cost[0]);
                StartCoroutine(GameBehaviourScript.EnemyWaitAttack());
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
