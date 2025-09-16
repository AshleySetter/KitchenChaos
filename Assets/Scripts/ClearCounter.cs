using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private KitchenObject kitchenObject;

    public override void Interact(Player player)
    {

    }

}
