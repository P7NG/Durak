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
    public Suit KozyrSuit;
    public GameObject KozyrCard;
    public GameObject KozyrPlaceEmpty;
    public CardBehaviour EmptyCard;
    public VisualComponent visualComponent;

    public int OpenPlaces = 0;

    [SerializeField] private GameObject _cardPlace;
    private bool TakedKozyr = false;

    private void Start()
    {
        Owner = StepOwner.Player;

        OpenPlaces = 1;
        CurrentPlace = Places[0];
        StartCoroutine(AddCardToHand());
        CreateKozyr();
    }

    private void Update()
    {
        if ((EnemyStack.Count == 0 || PlayerStack.Count == 0) && Owner != StepOwner.Stop)
        {
            EndRool();
        }
    }

    public void CreateKozyr()
    {
        int randCard = Random.RandomRange(0, DeckArray.Count);

        CardBehaviour randomCard = DeckArray[randCard];
        DeckArray.Remove(randomCard);
        randomCard.transform.parent = KozyrCard.transform;
        randomCard.transform.position = KozyrCard.transform.position;
        randomCard.transform.rotation = KozyrCard.transform.rotation;
        randomCard.Interactable = false;
        randomCard.CardFace = true;
        KozyrSuit = randomCard.cardCost.suit;
        KozyrCard = randomCard.gameObject;
        KozyrPlaceEmpty.GetComponent<Image>().sprite = KozyrCard.GetComponent<CardBehaviour>().CardSprite;
    }

    public void EnemyAttack()
    {
        if (OpenPlaces == 1)
        {
            CardBehaviour findedCard = EmptyCard;
            bool finded = false;

            findedCard.cardCost.cost[0] = 15;
            if (KozyrSuit == Suit.Peaks) findedCard.cardCost.suit = Suit.Hearts;
            else findedCard.cardCost.suit = Suit.Peaks;

            for (int i = 0; i < EnemyStack.Count; i++)
            {
                if (EnemyStack[i].cardCost.cost[0] < findedCard.cardCost.cost[0])
                {
                    if (FilterCard(findedCard, EnemyStack[i]))
                    {
                        findedCard = EnemyStack[i];
                        finded = true;
                    }
                }
            }

            if (!finded)
            {
                for (int i = 0; i < EnemyStack.Count; i++)
                {
                    if (EnemyStack[i].cardCost.cost[0] < findedCard.cardCost.cost[0])
                    {
                            findedCard = EnemyStack[i];
                    }
                }
            }

            Cards.Add(findedCard);
            findedCard.transform.parent = CurrentPlace.GetComponent<CardPlace>().CardPlaceFact;
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
                for (int j = 0; j < AppendCosts.Count; j++)
                {
                    if (EnemyStack[i].cardCost.cost[0] == AppendCosts[j])
                    {
                        if (FilterCard(findedCard, EnemyStack[i]))
                        {
                            findedCard = EnemyStack[i];
                        }
                    }
                }
            }
            if (findedCard != null)
            {
                Cards.Add(findedCard);
                findedCard.transform.parent = CurrentPlace.GetComponent<CardPlace>().CardPlaceFact;
                findedCard.CardFace = true;
                findedCard.Interactable = false;
                EnemyStack.Remove(findedCard);
                OpenPlaces++;
                CurrentPlace = Places[OpenPlaces-1];
            }
            if (findedCard == null)
            {
                for (int i = 0; i < EnemyStack.Count; i++)
                {
                    for (int j = 0; j < AppendCosts.Count; j++)
                    {
                        if (EnemyStack[i].cardCost.cost[0] == AppendCosts[j])
                        {
                                findedCard = EnemyStack[i];
                        }
                    }
                }
                if (findedCard == null)
                {

                    visualComponent.WriteMessage("Бито");
                    SendInBito();
                    ClearTable();
                    Owner = StepOwner.Player;
                    TakeCardInHand();
                }
            }
        }
    }

    private bool FilterCard(CardBehaviour lastFindedCard, CardBehaviour currentCard)
    {
        if (lastFindedCard != null && lastFindedCard.cardCost.suit != KozyrSuit && currentCard.cardCost.suit == KozyrSuit)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    private void EnemyTake()
    {
        int CountRepeats = Cards.Count;

        for (int i = 0; i < CountRepeats; i++)
        {
            Cards[0].transform.parent = this.gameObject.transform;
            Cards[0].transform.parent = EnemyHand.transform;
            Cards[0].CardFace = false;
            EnemyStack.Add(Cards[0]);
            Cards.Remove(Cards[0]);
            ClearTable();
        }
        TakeCardInHand();
    }

    private void EnemyDropDefendCard(CardBehaviour findedCard, int findedCost, CardCost needCardCost)
    {
        Cards.Add(findedCard);
        findedCard.transform.parent = Cards[Cards.Count - 2].CardPlace.CardPlaceFact;
        findedCard.CardFace = true;
        AppendCosts.Add(findedCost);
        AppendCosts.Add(needCardCost.cost[0]);
        findedCard.Interactable = false;
        EnemyStack.Remove(findedCard);
    }

    public void EnemyDefend()
    {
        bool finded = false;
        CardCost needCardCost = Cards[Cards.Count - 1].cardCost;
        int findedCost = 15;
        CardBehaviour findedCard = null;

        for (int i = 0; i < EnemyStack.Count; i++)
        {
            if ((EnemyStack[i].cardCost.suit == needCardCost.suit) || (EnemyStack[i].cardCost.suit == KozyrSuit && needCardCost.suit != KozyrSuit))
            {
                if (findedCost > EnemyStack[i].cardCost.cost[0] && EnemyStack[i].cardCost.cost[0] > needCardCost.cost[0])
                {
                    if ((findedCard != null && findedCard.cardCost.suit != KozyrSuit) && EnemyStack[i].cardCost.suit == KozyrSuit)
                    {

                    }
                    else
                    {
                        findedCost = EnemyStack[i].cardCost.cost[0];
                        findedCard = EnemyStack[i];
                        finded = true;
                    }
                }
                else if (findedCost <= EnemyStack[i].cardCost.cost[0] && EnemyStack[i].cardCost.cost[0] > needCardCost.cost[0] && findedCard != null && findedCard.cardCost.suit == KozyrSuit && EnemyStack[i].cardCost.suit != KozyrSuit)
                {
                    findedCost = EnemyStack[i].cardCost.cost[0];
                    findedCard = EnemyStack[i];
                    finded = true;
                }
            }
        }
        if (finded)
        {
            EnemyDropDefendCard(findedCard, findedCost, needCardCost);
        }
        else
        {
            visualComponent.WriteMessage("Беру");
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
        PlayerStack.Remove(card);

        if (Owner == StepOwner.Player)
        {
            StartCoroutine(EnemyWaitDefend());
        }
    }

    public IEnumerator EnemyWaitDefend()
    {
        yield return new WaitForSeconds(0.7f);
        EnemyDefend();
    }

    public IEnumerator EnemyWaitAttack()
    {
        yield return new WaitForSeconds(0.7f);
        EnemyAttack();
    }

    public void SendInBito()
    {
        int CountRepeats = Cards.Count;


        for (int i = 0; i < CountRepeats; i++)
        {
            Cards[0].transform.parent = this.gameObject.transform;
        }

        for (int i = 0; i < CountRepeats; i++)
        {
            var x = Cards[0];
            Cards[0].transform.parent = Bito.transform;
            Cards.Remove(x);
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
                else if (!TakedKozyr)
                {
                    KozyrCard.transform.parent = PlayerHand.transform;
                    CardBehaviour CardComponent = KozyrCard.GetComponent<CardBehaviour>();
                    PlayerStack.Add(CardComponent);
                    CardComponent.Interactable = true;
                    CardComponent.CardFace = true;
                    KozyrCard.transform.localEulerAngles = Vector3.zero;
                    TakedKozyr = true;
                    break;
                }
                else
                {
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
                else if (!TakedKozyr)
                {
                    KozyrCard.transform.parent = EnemyHand.transform;
                    CardBehaviour CardComponent = KozyrCard.GetComponent<CardBehaviour>();
                    EnemyStack.Add(CardComponent);
                    CardComponent.Interactable = true;
                    CardComponent.CardFace = false;
                    KozyrCard.transform.localEulerAngles = Vector3.zero;
                    TakedKozyr = true;
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }

    private void TakeCardInHand()
    {
        if (Owner == StepOwner.Enemy)
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
            StartCoroutine(EnemyWaitAttack());
        }
        else
        {
            PlayerTake();
            StartCoroutine(AddCardToHand());
            ClearTable();
            StartCoroutine(EnemyWaitAttack());
        }
    }

    private void PlayerTake()
    {
        int CountRepeats = Cards.Count;

        for (int i = 0; i < CountRepeats; i++)
        {
            Cards[0].transform.parent = this.gameObject.transform;
            Cards[0].transform.parent = PlayerHand.transform;
            Cards[0].CardFace = true;
            Cards[0].Interactable = true;
            PlayerStack.Add(Cards[0]);
            Cards.Remove(Cards[0]);
            ClearTable();
        }
        TakeCardInHand();
    }

    public IEnumerator AddCardToHand()
    {
        yield return new WaitForSeconds(0.1f);
        TakeCardInHand();
    }

    private void EndRool()
    {
        if (EnemyStack.Count == 0 && PlayerStack.Count > 0)
        {
            Debug.Log("Lose");
            Owner = StepOwner.Stop;
        }
        else if (EnemyStack.Count > 0 && PlayerStack.Count == 0)
        {
            Debug.Log("Win");
            Owner = StepOwner.Stop;
        }
    }

    public enum StepOwner
    {
        Player,
        Enemy,
        Stop
    }
}
