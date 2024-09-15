using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardPlace : MonoBehaviour
{
    public bool IsActive = true;
    public GameBehaviour GameBehaviourScript;
    public CardCost cardCost;
    public CardBehaviour Card;

    private void Start()
    {
        GameBehaviourScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameBehaviour>();
    }

    private void Update()
    {
        
    }

    
}

[Serializable]
public class CardCost
{
    [Range(6, 14)]
    public List<int> cost = new List<int>();
    public Suit suit;
    public bool AnyCards = false;
}


