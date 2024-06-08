using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOutfit : MonoBehaviour
{
    [SerializeField] private GameObject buttonConintainer;

    private Button[] outfitButtons;

    private int currentOutfit = 0;

    public void OnAvatarLoad()
    {
        outfitButtons = buttonConintainer.GetComponentsInChildren<Button>();
    }

    public void SelectNextOutfit()
    {
        // Invoke the current button's click method
        outfitButtons[currentOutfit].onClick.Invoke();

        // Move to the next button in the array
        currentOutfit++;

        // Loop back to the start if we reach the end
        if (currentOutfit >= outfitButtons.Length)
        {
            currentOutfit = 0;
        }
    }

    public void SelectPreviousOutfit()
    {
        // Move to the previous button in the array
        currentOutfit--;

        // Loop back to the end if we reach the start
        if (currentOutfit < 0)
        {
            currentOutfit = outfitButtons.Length - 1;
        }

        // Invoke the current button's click method
        outfitButtons[currentOutfit].onClick.Invoke();
    }
}
