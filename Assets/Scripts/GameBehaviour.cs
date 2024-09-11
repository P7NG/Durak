using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public StepOwner step;
    public Status status;
    public List<CardBehaviour> Cards = new List<CardBehaviour>();
    public List<CardBehaviour> ActiveCards = new List<CardBehaviour>();
    public List<CardBehaviour> PlayerStack = new List<CardBehaviour>();
    public List<CardBehaviour> EnemyStack = new List<CardBehaviour>();
    public GameObject[] Places;

    private int _openPlaces = 0;

    [SerializeField] private GameObject _cardPlace;

    private void Start()
    {
        PlayerStep();
    }

    public void PlayerStep()
    {
        if (step == StepOwner.Player && status == Status.Attack)
        {
            _openPlaces++;
            Places[_openPlaces - 1].SetActive(true);
        }
    }

    public void EnemyStep()
    {
        Debug.Log("0");
        if (step == StepOwner.Enemy && status == Status.Defend)
        {
            for (int i = 0; ActiveCards.Count > 0; i += 0)
            {
                int needCost = ActiveCards[0].cost + 1;
                Suit needSuit = ActiveCards[0].suit;
                CardBehaviour agreeCard = null;

                Debug.Log("1");

                for(int k = 0; k < EnemyStack.Count; k++)
                {
                    Debug.Log("2");
                    if (EnemyStack[k].suit == needSuit && EnemyStack[k].cost > needCost)
                    {
                        Debug.Log("3");
                        if (agreeCard == null || agreeCard.cost > EnemyStack[k].cost)
                        {
                            agreeCard = EnemyStack[k];
                        }
                        else
                        {
                            //Nothing actions
                        }
                    }                   
                }

                
                if (agreeCard != null)
                {
                    Debug.Log("4");
                    CardPlace cardPlace = ActiveCards[0].cardPlace;
                    cardPlace.gameObject.SetActive(true);
                    cardPlace.Card = agreeCard;
                    agreeCard.transform.parent = cardPlace.gameObject.transform;
                    agreeCard.transform.position = cardPlace.transform.position;
                    cardPlace.IsActive = false;
                }
                else
                {
                    Debug.Log("Take");
                }
                ActiveCards.Remove(ActiveCards[0]);
            }
        }
    }

    public void CardAtTable(CardBehaviour Card)
    {
        Cards.Add(Card);
        ActiveCards.Add(Card);
        PlayerStep();
    }

    public void NextButton()
    {
        if(step == StepOwner.Player)
        {
            if(status == Status.Attack)
            {
                step = StepOwner.Enemy;
                status = Status.Defend;
                EnemyStep();
            }
        }
        
        //many actions
    }
}

public enum StepOwner
{
    Player,
    Enemy
}

public enum Status
{
    Defend,
    Attack
}
