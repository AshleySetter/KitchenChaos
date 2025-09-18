using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object on counter
            if (player.HasKitchenObject())
            {
                // player carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // there is a kitchen object on counter
            if (player.HasKitchenObject())
            {
                // the player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player is not holding plate, player is holding another kitchen object
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
