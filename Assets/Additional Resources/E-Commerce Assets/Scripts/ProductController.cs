using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductController : MonoBehaviour
{
    [SerializeField] private Catogeries[] catogeries;
    [SerializeField] private ToggleGroup[] productSelection;
    [SerializeField] private ToggleGroup categorySelection;
    [SerializeField] private CustomPalmMenu customPalmMenu;
    private int catogetyPage;
    private int productNumber;

    private void Start()
    {
        Toggle[] catogeryToggles = categorySelection.GetComponentsInChildren<Toggle>();
        Debug.Log(catogeryToggles.Length + "<<< number  =");

        for (int pageIndex = 0; pageIndex < catogeryToggles.Length; pageIndex++)
        {
            int catogeryIndex = pageIndex;
            catogeryToggles[pageIndex].onValueChanged.AddListener((isCatgogeryToggled) =>
            {
                if (!isCatgogeryToggled)
                {
                    Debug.Log("<<< index= " + catogeryIndex);
                    productSelection[catogeryIndex].SetAllTogglesOff();
                }
            });

            // Get the ToggleGroup component of the current page
            ToggleGroup toggleGroup = productSelection[pageIndex].GetComponent<ToggleGroup>();
            if (toggleGroup == null)
            {
                Debug.LogWarning("ToggleGroup not found on page " + pageIndex);
                continue;
            }

            // Get all Toggle components in the current page
            Toggle[] toggles = productSelection[pageIndex].GetComponentsInChildren<Toggle>();

            // Loop through each toggle and add a listener to its onValueChanged event
            for (int toggleIndex = 0; toggleIndex < toggles.Length; toggleIndex++)
            {
                int currentPageIndex = pageIndex;
                int currentToggleIndex = toggleIndex;

                toggles[toggleIndex].onValueChanged.AddListener((isProductToggled) =>
                {
                    if (isProductToggled)
                    {
                        DisplayToggleInfo(currentPageIndex, currentToggleIndex);
                        DisplayProduct(currentPageIndex, currentToggleIndex);
                    }
                    else
                    {
                        DisplayToggleInfo(currentPageIndex, currentToggleIndex);
                        HideProduct(currentPageIndex, currentToggleIndex);
                    }
                });
            }
        }
    }

    private void DisplayProduct(int pageIndex, int toggleIndex)
    {
        catogeries[pageIndex].products[toggleIndex].SetActive(true);
        customPalmMenu.SetProductRenderer(catogeries[pageIndex].products[toggleIndex]);
    }

    private void HideProduct(int pageIndex, int toggleIndex)
    {
        catogeries[pageIndex].products[toggleIndex].SetActive(false);
    }

    private void DisplayToggleInfo(int pageIndex, int toggleIndex)
    {
        Debug.Log("Page: " + (pageIndex + 1) + ", Toggle Index: " + (toggleIndex + 1));
    }
}

[System.Serializable]
public struct Catogeries
{
    public string CatogeryName;
    public GameObject[] products;
}
