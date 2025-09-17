using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOsArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object on counter
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // player carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;
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
            }
            else
            {
                // player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a KitchenObject on cutting counter and it has a recipe
            cuttingProgress++;
            CuttingRecipeSO recipe = GetCuttingRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingProgress >= recipe.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private CuttingRecipeSO GetCuttingRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {

        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOsArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipeForInput(inputKitchenObjectSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipeForInput(inputKitchenObjectSO)?.output;
    }
}
