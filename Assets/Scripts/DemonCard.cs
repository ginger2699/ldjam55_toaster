using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonCard : MonoBehaviour
{
    public int cardID; // Unique ID for each card
    public Sprite cardBack; // The sprite for the back of the card
    public Sprite cardFront; // The sprite for the front of the card
    public DemonProfile d_profile;

    public GameObject matchedDemon;
    private static List<Color> coloredPairs = new List<Color>() { Color.yellow, Color.magenta, Color.cyan, Color.blue};
    private int indexColor= 0;
    private bool isSelected = false;
    private bool isPaired = false;
    private Image image;
    private static List<DemonCard> selectedCards = new List<DemonCard>();

    void Start()
    {
        matchedDemon = null;
        image = GetComponent<Image>();
        image.sprite = cardBack; // Set the initial sprite to the card back
    }

    public void onClick()
    {
        if (!isSelected && !isPaired && selectedCards.Count < 2)
        {
            isSelected = true;
            selectedCards.Add(this);
            //graphic effect to highlight
            //image.sprite = cardFront; // Show the front of the card
            gameObject.GetComponent<Image>().color = Color.green;

        }
        else if (isSelected && selectedCards.Contains(this))
        {
            isSelected = false;
            selectedCards.Remove(this);
            //graphic effect to remove highlight
            //image.sprite = cardBack; // Show the back of the card
            gameObject.GetComponent<Image>().color = Color.white;

        }
        //remove couple
        else if (isPaired && matchedDemon != null)
        {
            matchedDemon.GetComponent<DemonCard>().matchedDemon = null;
            matchedDemon.GetComponent<DemonCard>().isPaired = false;
            matchedDemon.GetComponent<DemonCard>().isSelected = false;
            matchedDemon.gameObject.GetComponent<Image>().color = Color.white;

            matchedDemon = null;
            isPaired = false;
            isSelected = false;
            //add color back into the list
            coloredPairs.Add(gameObject.GetComponent<Image>().color);
            
            gameObject.GetComponent<Image>().color = Color.white;

            //graphic effect
        }

        // Check for a pair if two cards are selected
        if (selectedCards.Count == 2 && matchedDemon == null)
        {
            if (selectedCards[0] != this)
            {
                // Cards form a pair
                isPaired = true;
                selectedCards[0].isPaired = true;
                matchedDemon = selectedCards[0].gameObject;
                selectedCards[0].matchedDemon = this.gameObject;
                gameObject.GetComponent<Image>().color = coloredPairs[0];
                matchedDemon.GetComponent<Image>().color = coloredPairs[0];
                coloredPairs.RemoveAt(0);
                selectedCards.Clear(); // Clear selected cards
            }
        }
    }

    private void CheckForPair()
    {
        if (selectedCards[0] != this)
        {
            // Cards form a pair
            isPaired = true;
            selectedCards[0].isPaired = true;
            selectedCards.Clear(); // Clear selected cards
        }
        else
        {
            // Cards do not form a pair
            foreach (DemonCard card in selectedCards)
            {
                card.isSelected = false;
                card.image.sprite = card.cardBack;
            }
            selectedCards.Clear(); // Clear selected cards
        }
    }
}
