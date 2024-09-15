using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public List<CardBehaviour> Cards = new List<CardBehaviour>();
    public List<CardBehaviour> PlayerStack = new List<CardBehaviour>();
    public List<CardBehaviour> EnemyStack = new List<CardBehaviour>();
    public List<int> AppendCosts = new List<int>();
    public GameObject[] Places;
    public GameObject EnemyHand;
    public GameObject CurrentPlace;
    public StepOwner Owner;

    public int OpenPlaces = 0;

    [SerializeField] private GameObject _cardPlace;

    private void Start()
    {
        Owner = StepOwner.Player;

        OpenPlaces = 1;
        CurrentPlace = Places[0];
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

    public enum StepOwner
    {
        Player,
        Enemy
    }
}
