using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoveCounter : BaseCounter
{
    public event EventHandler<OnStateChangedEventArgs> onStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
        onStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state,
        });
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // KitchenObject is Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burningRecipeSO = GetBurningRecipeForInput(fryingRecipeSO.output);
                        state = State.Fried;
                        onStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state,
                        });
                        burningTimer = 0f;
                    }
                    break;
                case State.Fried:
                    if (burningRecipeSO)
                    {
                        burningTimer += Time.deltaTime;
                        if (burningTimer > burningRecipeSO.burningTimerMax)
                        {
                            // KitchenObject is Burned
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                            state = State.Burned;
                            onStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = state,
                            });
                        }
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object on counter
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // player carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
                state = State.Frying;
                onStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state,
                });
                fryingTimer = 0f;
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
                state = State.Idle;
                onStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state,
                });
            }
        }
    }

    private FryingRecipeSO GetFryingRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {

        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {

        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipeForInput(inputKitchenObjectSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipeForInput(inputKitchenObjectSO)?.output;
    }
}
