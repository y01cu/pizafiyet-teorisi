using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using TopDownShooter;

public class SlicingPizza : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pizza;

    private Vector2 _pizzaCenter;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _pizzaCenter = pizza.transform.position;

    }

    [Command]
    private void SlicePizzaIntoGivenEqualNumber(int sliceAmount)
    {
        Debug.Log($"pizza is sliced into {sliceAmount} equal pieces");

        Debug.Log($"pizza mid position: {pizza.sprite.rect.position}");

        _canPizzaBeSliced = true;
    }

    private bool _canPizzaBeSliced;

    private void OnDrawGizmos()
    {
        if (!_canPizzaBeSliced)
        {
            return;
        }
        
        Gizmos.DrawLine(new Vector3(_pizzaCenter.x, _pizzaCenter.y, 0), new Vector3(_pizzaCenter.x + 3.6f, _pizzaCenter.y + 3.6f, 0));
        Gizmos.DrawLine(new Vector3(_pizzaCenter.x, _pizzaCenter.y, 0), new Vector3(_pizzaCenter.x - 3.6f, _pizzaCenter.y - 3.6f, 0));
    }
}