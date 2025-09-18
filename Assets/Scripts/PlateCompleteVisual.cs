using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject_Mapping
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject_Mapping> kitchenObjectSOGameObjectMappingList;
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSO_GameObject_Mapping kitchenObject_GameObject_Mapping in kitchenObjectSOGameObjectMappingList)
        {
            kitchenObject_GameObject_Mapping.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs evnt)
    {
        foreach (KitchenObjectSO_GameObject_Mapping kitchenObject_GameObject_Mapping in kitchenObjectSOGameObjectMappingList)
        {
            if (kitchenObject_GameObject_Mapping.kitchenObjectSO == evnt.kitchenObjectSO)
            {
                kitchenObject_GameObject_Mapping.gameObject.SetActive(true);
            }
        }
    }

}
