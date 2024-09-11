using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlace : MonoBehaviour
{
    public bool IsActive = true;
    public bool AnyCard = true;
    public GameBehaviour GameBehaviourScript;
    public Suit ThisSuit;
    [Range(6, 14)]
    public int ThisCost;
    
    [SerializeField] private float _distance;
    [SerializeField] private CursorController _cursor;
    public CardBehaviour Card;

    private void Start()
    {
        _cursor = Camera.main.gameObject.GetComponent<CursorController>();
        GameBehaviourScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameBehaviour>();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y)) < _distance && _cursor.HaveCard && IsActive)
        {
            if(_cursor.CurrentCardPlace != this.gameObject && ((_cursor.Card.cost > ThisCost && _cursor.Card.suit == ThisSuit)||AnyCard))
            {
                _cursor.CurrentCardPlace = this.gameObject;
                _cursor.CurrentTarget = Target.Other;
            }
        }
        else
        {
            if(_cursor.CurrentCardPlace == this.gameObject)
            {
                _cursor.CurrentCardPlace = null;
                _cursor.CurrentTarget = Target.Hand;
            }
        }
    }
}
