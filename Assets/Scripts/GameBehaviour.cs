using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public List<CardBehaviour> Cards = new List<CardBehaviour>();
    public List<CardBehaviour> PlayerStack = new List<CardBehaviour>();
    public List<CardBehaviour> EnemyStack = new List<CardBehaviour>();
    public List<CardBehaviour> DeckArray = new List<CardBehaviour>();
    public List<int> AppendCosts = new List<int>();
    public GameObject[] Places;
    public GameObject EnemyHand;
    public GameObject PlayerHand;
    public GameObject CurrentPlace;
    public GameObject Deck;
    public GameObject Bito;
    public StepOwner Owner;

    public int OpenPlaces = 0;

    [SerializeField] private GameObject _cardPlace;

    private void Start()
    {
        Owner = StepOwner.Player;

        OpenPlaces = 1;
        CurrentPlace = Places[0];
        TakeCardInHand();       
    }

    public void PlayerStep()
    {

    }

    public void EnemyStep()
    {
        CardCost needCardCost = Cards[Cards.Count - 1].cardCost;
        int findedCost = 15;
        CardBehaviour findedCard = null;

        for (int i = 0; i < EnemyStack.Count; i++)
        {
            if (EnemyStack[i].cardCost.suit == needCardCost.suit)
            {
                if (findedCost > EnemyStack[i].cardCost.cost[0] && EnemyStack[i].cardCost.cost[0] > needCardCost.cost[0])
                {
                    findedCost = EnemyStack[i].cardCost.cost[0];
                    findedCard = EnemyStack[i];
                }
            }
        }

        if (findedCard != null)
        {
            Cards.Add(findedCard);
            findedCard.transform.position = Cards[Cards.Count - 2].CardPlace.gameObject.transform.position;
            findedCard.transform.parent = Cards[Cards.Count - 2].transform.parent;
            findedCard.CardFace = true;
            AppendCosts.Add(findedCost);
            AppendCosts.Add(needCardCost.cost[0]);
            findedCard.Interactable = false;
            EnemyStack.Remove(findedCard);
        }
        else
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].transform.parent = this.gameObject.transform;
                Cards[i].transform.parent = EnemyHand.transform;
                EnemyStack.Add(Cards[i]);
                ClearTable();
            }
        }
    }

    public void ClearTable()
    {
        AppendCosts.Clear();
        OpenPlaces = 1;
        CurrentPlace = Places[OpenPlaces - 1];
    }

    public void DropCard(CardBehaviour card)
    {
        OpenPlaces++;
        CurrentPlace = Places[OpenPlaces - 1];
        Cards.Add(card);

        if (Owner == StepOwner.Player)
        {
            EnemyStep();
        }
    }

    public void SendInBito()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].transform.parent = this.gameObject.transform;
        }

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].transform.parent = Bito.transform;
        }

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards.Remove(Cards[i]);
        }
    }

    private void PlayerTakeCards()
    {
        if (PlayerStack.Count < 6)
        {
            int lessCardsCount = 6 - PlayerStack.Count;

            for (int i = 0; i < lessCardsCount; i++)
            {
                int randCard = Random.RandomRange(0, DeckArray.Count);

                if (DeckArray.Count > 0)
                {
                    CardBehaviour randomCard = DeckArray[randCard];
                    DeckArray.Remove(randomCard);
                    randomCard.transform.parent = PlayerHand.transform;
                    PlayerStack.Add(randomCard);
                    randomCard.Interactable = true;
                    randomCard.CardFace = true;                   
                }
                else
                {
                    //TakeKozur
                    break;
                }
            }
        }
    }

    private void EnemyTakeCards()
    {
        if (EnemyStack.Count < 6)
        {
            int lessCardsCount = 6 - EnemyStack.Count;
            Debug.Log(lessCardsCount);

            for (int i = 0; i < lessCardsCount; i++)
            {
                //Debug.Log(lessCardsCount);
                Debug.Log(i);
                int randCard = Random.RandomRange(0, DeckArray.Count);

                if (DeckArray.Count > 0)
                {
                    CardBehaviour randomCard = DeckArray[randCard];                    
                    DeckArray.Remove(DeckArray[randCard]);                    
                    randomCard.transform.parent = EnemyHand.transform;
                    EnemyStack.Add(randomCard);
                    randomCard.Interactable = true;
                    randomCard.CardFace = true;
                    
                }
                else
                {
                    //TakeKozur
                    break;
                }
            }
        }
    }

    private void TakeCardInHand()
    {
        if(Owner == StepOwner.Enemy)
        {
            PlayerTakeCards();
            EnemyTakeCards();
        }
        else
        {
            EnemyTakeCards();
            //PlayerTakeCards();
        }        
    } 

    public void BitoButton()
    {
        if (Owner == StepOwner.Player)
        {
            Owner = StepOwner.Enemy;

            SendInBito();
        }
    }

    public enum StepOwner
    {
        Player,
        Enemy
    }
}
