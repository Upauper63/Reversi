using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassButtonScript : MonoBehaviour {
    GameObject genaratePieces;
    GeneratePiecesScript genaratePiecesScript;
    GameObject turnPlayerText;
    public int passCount;

    // Use this for initialization
    void Start () {
        turnPlayerText = GameObject.Find("TurnPlayerText");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PassTurn()
    {
        genaratePieces = GameObject.Find("GeneratePieces");
        genaratePiecesScript = genaratePieces.GetComponent<GeneratePiecesScript>();
        
        int turnPlayer = genaratePiecesScript.turnPlayer;

        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(genaratePiecesScript.CheckCanSetPiece(j, i))
                {
                    return;
                }
            }
        }

        if(turnPlayer == 1)
        {
            genaratePiecesScript.turnPlayer = 2;
            turnPlayerText.GetComponent<Text>().text = "Black Turn";
            passCount++;
            //genaratePiecesScript.Update();
        }
        else
        {
            genaratePiecesScript.turnPlayer = 1;
            turnPlayerText.GetComponent<Text>().text = "White Turn";
            passCount++;
            //genaratePiecesScript.Update();
        }
    }
}
