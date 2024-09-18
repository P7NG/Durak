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
        StartCoroutine(AddCardToHand());
    }

    public void EnemyAttack()
    {
        if (OpenPlaces == 1)
        {
            CardBehaviour findedCard = DeckArray[0];
            findedCard.cardCost.cost[0] = 15;
            Debug.Log("1");
            for(int i = 0; i < EnemyStack.Count; i++)
            {
                if(EnemyStack[i].cardCost.cost[0] < findedCard.cardCost.cost[0])
                {
                    findedCard = EnemyStack[i];
                }
            }
            Cards.Add(findedCard);
            findedCard.transform.position = CurrentPlace.transform.position;
            findedCard.transform.parent = CurrentPlace.transform;
            findedCard.CardFace = true;
            AppendCosts.Add(findedCard.cardCost.cost[0]);
            findedCard.Interactable = false;
            EnemyStack.Remove(findedCard);

            OpenPlaces++;
            CurrentPlace = Places[OpenPlaces - 1];
        }
        else
        {
            CardBehaviour findedCard = null;

            for (int i = 0; i < EnemyStack.Count; i++)
            {
                for(int j = 0; j < AppendCosts.Count; j++)
                {
                    if (EnemyStack[i].cardCost.cost[0] == AppendCosts[j])
                    {
                        findedCard = EnemyStack[i];
                        Cards.Add(findedCard);
                        findedCard.transform.position = CurrentPlace.transform.position;
                        findedCard.transform.parent = CurrentPlace.transform;
                        findedCard.CardFace = true;
                        findedCard.Interactable = false;
                        EnemyStack.Remove(findedCard);
                    }
                }
            }

            if (findedCard == null)
            {
                SendInBito();
                ClearTable();
                Owner = StepOwner.Player;
                TakeCardInHand();
            }
        }
    }

    private void EnemyTake()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].transform.parent = this.gameObject.transform;
            Cards[i].transform.parent = EnemyHand.transform;
            Cards[i].CardFace = false;
            EnemyStack.Add(Cards[i]);
            Cards.Remove(Cards[i]);
            ClearTable();
        }
        TakeCardInHand();
    }

    private void EnemyDropDefendCard(CardBehaviour findedCard, int findedCost, CardCost needCardCost)
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
    
    public void EnemyDefend()
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
            EnemyDropDefendCard(findedCard, findedCost, needCardCost);
        }
        else
        {
            EnemyTake();
            EnemyTake();
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
            EnemyDefend();
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
            while (PlayerStack.Count < 6) 
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
            while (EnemyStack.Count < 6)
            {
                int randCard = Random.RandomRange(0, DeckArray.Count);

                if (DeckArray.Count > 0)
                {
                    CardBehaviour randomCard = DeckArray[randCard];                    
                    DeckArray.Remove(DeckArray[randCard]);                    
                    randomCard.transform.parent = EnemyHand.transform;
                    EnemyStack.Add(randomCard);
                    randomCard.Interactable = true;
                    randomCard.CardFace = false;
                    
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
            PlayerTakeCards();
        }        
    } 

    public void BitoButton()
    {
        if (Owner == StepOwner.Player)
        {
            Owner = StepOwner.Enemy;

            SendInBito();
            StartCoroutine(AddCardToHand());
            ClearTable();
            EnemyAttack();
        }
        else
        {
            PlayerTake();
            PlayerTake();
            StartCoroutine(AddCardToHand());
            ClearTable();
            EnemyAttack();
        }
    }

    private void PlayerTake()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].transform.parent = this.gameObject.transform;
            Cards[i].transform.parent = PlayerHand.transform;
            Cards[i].CardFace = true;
            Cards[i].Interactable = true;
            PlayerStack.Add(Cards[i]);
            Cards.Remove(Cards[i]);
            ClearTable();
        }
        TakeCardInHand();
    }

    public IEnumerator AddCardToHand()
    {
        yield return new WaitForSeconds(0.1f);
        TakeCardInHand();
    }

    public enum StepOwner
    {
        Player,
        Enemy
    }
}
