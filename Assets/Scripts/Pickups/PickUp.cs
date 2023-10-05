using System;
using System.Collections;
using System.Collections.Generic;
using Pickups;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] 
    private ItemEffectSO effectToActivate;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<CarHandler>();
        if (player)
        {
            effectToActivate.ActivateEffect(player);
        }
    }
}
