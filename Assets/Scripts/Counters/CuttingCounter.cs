using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

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
                CuttingRecipeSO recipe = GetCuttingRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
                cuttingProgress = 0;

                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / (float)recipe.cuttingProgressMax
                });
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
            CuttingRecipeSO recipe = GetCuttingRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / (float)recipe.cuttingProgressMax
            });
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

        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
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
