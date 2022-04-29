using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class EscapeManager : MonoBehaviour
{
    public NetworkManager networkManager;
    public Canvas canvas;
    private bool showRole = false;
    private bool showInfo = false;

    private const int ROLE_MOTHER = 1;
    private const int ROLE_GIRLFRIEND = 2;
    private const int ROLE_DETECTIVE = 3;
    private const int ROLE_CLASSMATE = 4;

    private const int ITEM_COFFEE = 1;
    private const int ITEM_PROPERTY = 2;
    private const int ITEM_BOTTLE = 3;
    private const int ITEM_PHONE = 4;
    private const int ITEM_DEBT = 5;
    private const int ITEM_PHOTO = 6;

    private int localRole = -1;

    private int currentItemId = -1;
    public InfoBoard board;

    private float timeRemaining = -1.0f;
    private const float timeTotal = 600.0f;
    public Clock clock;

    private bool voted = false;
    private int voteId = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int decision = board.decision;
        if (decision != -1)
        {
            string text = "";
            if (decision == 1)
                text = "Congratulations!\nDetective votes °∞Katerina°± as the murderer, she is the murderer!\nYou guys escape the room successfully!";
            else if (decision == 2)
                text = "Game over!\nDetective votes °∞Parva°± as the murderer, she is not the murderer!\nYou guys fail to escape the room!";
            else if (decision == 3)
                text = "Game over!\nDetective votes °∞Christian°± as the murderer, she is not the murderer!\nYou guys fail to escape the room!";
            else if (decision == 4)
                text = "Game over!\nDetective votes himself as the murderer, he is not the murderer!\nYou guys fail to escape the room!";

            canvas.transform.GetChild(1).GetComponent<Text>().text = text;
            canvas.transform.GetChild(2).gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);
            return;
        }

        timeRemaining = clock.GetTime();

        if (timeRemaining <= 0.0f && timeRemaining != -1.0f)
        {
            string text = "Time's up!\nYou guys fail to escape the room!";
            canvas.transform.GetChild(1).GetComponent<Text>().text = text;
            canvas.transform.GetChild(2).gameObject.SetActive(false);
            canvas.enabled = true;
            return;
        }

        if ((timeRemaining > 0.0f && timeRemaining < timeTotal * 0.4f) || board.GetFoundCount() >= 6)
        {
            string text = "";
            // VOTE RESULT
            if (board.voteTotal >= 4)
            {
                text = "Voting Result:\n";
                text += "°∞Katerina°±: ";
                text += board.voteCountMother + " votes\n";
                text += "°∞Parva°±: ";
                text += board.voteCountGirlFriend + " votes\n";
                text += "°∞Christian°±: ";
                text += board.voteCountClassmate + " votes\n";
                text += "°∞Detective°±: ";
                text += board.voteCountDetective + " votes";

                // FINAL DECISION
                if (localRole == ROLE_DETECTIVE)
                {
                    text += "\n\nAs a detective, it°Øs your responsibility to make the final decision. Who is the murderer?\n";
                    text += "Press A to vote for °∞Katerina°±\nPress B to vote for °∞Parva°±\nPress X to vote for °∞Christian°±\nPress Y to vote for °∞Detective°±";

                    if (OVRInput.GetDown(OVRInput.Button.One))
                    {
                        board.Decide(1);
                    }
                    else if (OVRInput.GetDown(OVRInput.Button.Two))
                    {
                        board.Decide(2);
                    }
                    else if (OVRInput.GetDown(OVRInput.Button.Three))
                    {
                        board.Decide(3);
                    }
                    else if (OVRInput.GetDown(OVRInput.Button.Four))
                    {
                        board.Decide(4);
                    }
                }

                canvas.transform.GetChild(1).GetComponent<Text>().text = text;
                canvas.transform.GetChild(2).gameObject.SetActive(false);
                canvas.gameObject.SetActive(true);

                return;
            }

            // VOTING
            if (voteId == -1)
            {
                text = "Please vote out the murderer:\nPress A to vote for °∞Katerina°±\nPress B to vote for °∞Parva°±\nPress X to vote for °∞Christian°±\nPress Y to vote for °∞Detective°±";

                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    board.VoteMother();
                    voteId = 1;
                }
                else if (OVRInput.GetDown(OVRInput.Button.Two))
                {
                    board.VoteGirlFriend();
                    voteId = 2;
                }
                else if (OVRInput.GetDown(OVRInput.Button.Three))
                {
                    board.VoteClassmate();
                    voteId = 3;
                }
                else if (OVRInput.GetDown(OVRInput.Button.Four))
                {
                    board.VoteDetective();
                    voteId = 4;
                }
            }
            else
            {
                if (voteId == 1)
                    text = "You voted for °∞Katerina°±";
                else if (voteId == 2)
                    text = "You voted for °∞Parva°±";
                else if (voteId == 3)
                    text = "You voted for °∞Christian°±";
                else if (voteId == 4)
                    text = "You voted for °∞Detective°±";
            }

            canvas.transform.GetChild(1).GetComponent<Text>().text = text;
            canvas.transform.GetChild(2).gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);

            return;
        }

        if (localRole == -1)
        {
            if (networkManager.IsInARoom())
            {
                localRole = PhotonNetwork.LocalPlayer.ActorNumber;
                showRole = true;

                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    timeRemaining = timeTotal;
                    clock.SetTime(timeRemaining);
                }
            }
        }

        if (showInfo || showRole)
            canvas.gameObject.SetActive(true);
        else
            canvas.gameObject.SetActive(false);

        if (showRole)
        {
            Sprite sprite = Resources.Load<Sprite>("Katerina");
            string text = "";
            if (localRole == ROLE_MOTHER)
            {
                sprite = Resources.Load<Sprite>("Katerina");
                text += "Katerina\nRole Description: Although you appear to be a good mother to David, few people are aware that you are his stepmother. The main conflict you have with David is that is that he owns all of the family's property and has stated that he would not divide it with you. That's why you decided to murder him for the sake of the family's money. You poisoned everyone's coffee through the coffee machine, and then you put the antidote in the cake when you cut it in the kitchen. Everyone had the cake except David because you know he does not eat any desserts. That was how you conducted the murder successfully.\nSkills: Any clue you find, you can choose to hide it, and the other players will never know who does that. And you are the only player who can lie during the game.";
            }
            else if (localRole == ROLE_GIRLFRIEND)
            {
                sprite = Resources.Load<Sprite>("Parva");
                text += "Parva\nRole Description: You are David's girlfriend. However, you two had recently been having a bad relationship because he wanted to break up with you while you refused. You two had a heated argument last night. You texted him, threatening to kill him if he broke up with you. You were just trying to scare him in this way. On the day of the murder, you offered the coffee to everyone using the coffee machine, and also delivered the cake to all the people except David, because you know he doesn't enjoy any dessert.\nSkills: What happened to David is unknown to you. You have the option of telling the facts you know, but you are not permitted to lie in the game.";
            }
            else if (localRole == ROLE_DETECTIVE)
            {
                sprite = Resources.Load<Sprite>("Detective");
                text += "Detective\nRole Description: You are David's friend, as well as a detective. You are the only one in the game who should not be suspected. Your job is to decide who the murderer is based on your own and other players' opinions. The decision you make will lead to the result if you guys can escape the room.\nSkills: You declare your role at the beginning of the game and make the final decision in the voting part.";
            }
            else if (localRole == ROLE_CLASSMATE)
            {
                sprite = Resources.Load<Sprite>("Christian");
                text += "Christian\nRole Description: You were David's high school classmate and are now a major pharmacy student. You two appear to be excellent friends, but few people are aware that you and David have a financial issue since you borrowed 30,000$ from David two years ago, and your contract with David states that the debt repayment deadline is approaching (2 days later), but you are unable to pay the money. You brought the cake on the day of the murder. You can tell David died of poisoning in the first place since your major is pharmacy.\nSkills: What happened to David is unknown to you. You have the option of telling the facts you know, but you are not permitted to lie in the game.";
            }

            canvas.transform.GetChild(1).GetComponent<Text>().text = text;
            canvas.transform.GetChild(2).GetComponent<Image>().sprite = sprite;

            if (OVRInput.GetDown(OVRInput.Button.One))
                showRole = false;
        }

        if (showInfo)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                // Found
                string boardInfo = "\n";
                if (currentItemId == ITEM_COFFEE)
                {
                    boardInfo += "°ÒPoison 516 residues are discovered in the coffee machine, which indicates the reason for David's death.";
                }
                else if (currentItemId == ITEM_PROPERTY)
                {
                    boardInfo += "°ÒDavid owns all the family°Øs property, and it will not be given to anyone without blood relationship.";
                }
                else if (currentItemId == ITEM_BOTTLE)
                {
                    boardInfo += "°ÒAntidote bottle found in the dustbin can detoxify the poison 516.";
                }
                else if (currentItemId == ITEM_PHONE)
                {
                    boardInfo += "°ÒParva texted David that she would kill him if he broke up with her.";
                }
                else if (currentItemId == ITEM_DEBT)
                {
                    boardInfo += "°ÒChristian owed David $30,000 for his pharmacy degree tuition. Two days later will be the payment deadline";
                }
                else if (currentItemId == ITEM_PHOTO)
                {
                    boardInfo += "°ÒDavid does not eat any dessert. Katerina is his stepmother.";
                }

                board.UpdateInfo(boardInfo);

                showInfo = false;
            }
            else if (localRole == ROLE_MOTHER && OVRInput.GetDown(OVRInput.Button.Four))
            {
                // Hide
                board.UpdateInfo("\nAn item has been hidden by the murderer");

                showInfo = false;
            }
        }
    }

    public void ShowInfo(int itemId)
    {
        Sprite sprite = Resources.Load<Sprite>("coffee");
        string info = "item";
        if (itemId == ITEM_COFFEE)
        {
            sprite = Resources.Load<Sprite>("coffee");
            info = "Coffee machine\nCongratulations! You find some poison 516 residues in the coffee machine, which indicates the reason for David's death.";
        }
        else if (itemId == ITEM_PROPERTY)
        {
            sprite = Resources.Load<Sprite>("contract");
            info = "Property Contract\nCongratulations! You find a property contract in the bookshelf. The information on it shows that David has the full ownership of all the family°Øs property. The statement declares that David will not leave any of it to the person without blood relationship.";
        }
        else if (itemId == ITEM_BOTTLE)
        {
            sprite = Resources.Load<Sprite>("antidote");
            info = "Antidote bottle\nCongratulations! You find an antidote bottle in the dustbin. The label on it shows that it can detoxify the poison found in the coffee machine.";
        }
        else if (itemId == ITEM_PHONE)
        {
            sprite = Resources.Load<Sprite>("phone");
            info = "David°Øs smart phone\nCongratulations! You find a message in David°Øs smart phone.";
        }
        else if (itemId == ITEM_DEBT)
        {
            sprite = Resources.Load<Sprite>("debt");
            info = "Debt contract\nCongratulations! You find a debt contract in the drawer. It shows that Christian has a debt dispute with David, and the payment deadline is two days later.";
        }
        else if (itemId == ITEM_PHOTO)
        {
            sprite = Resources.Load<Sprite>("photo");
            info = "David°Øs childhood photo\nCongratulations! You find a clue in a David°Øs childhood photo. It shows that David does not eat any dessert. Katerina is his stepmother.";
        }

        if (localRole == ROLE_MOTHER)
            info += "\n\nYou can hide this item by pressing Y";
        info += "\nPress B to dismiss.";

        canvas.transform.GetChild(1).GetComponent<Text>().text = info;
        canvas.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
        showInfo = true;
        currentItemId = itemId;
    }
}
