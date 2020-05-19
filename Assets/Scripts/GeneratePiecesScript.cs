using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePiecesScript : MonoBehaviour {
    //1:white 2:black
    int[,] piecesOnBoard = new int[8, 8];
    public GameObject piecePrefab;
    private Plane plane;
    private Vector3 mousePoint;
    public int turnPlayer;
    private bool canSet;
    GameObject turnPlayerText;
    private int remainPlacesNum;
    GameObject resultText;
    GameObject passButton;
    PassButtonScript passButtonScript;

	// Use this for initialization
	void Start () {
        SetStartPieces();
        plane = new Plane(Vector3.up, Vector3.zero);
        turnPlayer = 1;
        RenderPiecesOnBoard();
        turnPlayerText = GameObject.Find("TurnPlayerText");
        remainPlacesNum = 60;
        resultText = GameObject.Find("ResultText");
        passButton = GameObject.Find("PassButton");
        passButtonScript = passButton.GetComponent<PassButtonScript>();
    }

    // Update is called once per frame
    public void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (Input.GetMouseButtonDown(0) && plane.Raycast(ray, out distance))
        {
            mousePoint = ray.GetPoint(distance);

            //マウスポイントがずれるので補正してある
            int mousePointX = Mathf.FloorToInt(mousePoint.x + 0.5f);
            int mousePointZ = Mathf.FloorToInt(mousePoint.z + 0.5f);

            //マウスポイントがボードの外の場合はreturn
            if(mousePointX < 0 || mousePointX > 7 || mousePointZ < 0 || mousePointZ > 7)
            {
                return;
            }

            if (!CheckCanSetPiece(mousePointX, mousePointZ))
            {
                return;
            }
                
            if (turnPlayer == 1)
            {
                //石を置いたらターンを交代
                piecesOnBoard[mousePointX, mousePointZ] = turnPlayer;
                SetPiece(mousePointX, mousePointZ);
                turnPlayer = 2;
                turnPlayerText.GetComponent<Text>().text = "Black Turn";
                passButtonScript.passCount = 0;
                remainPlacesNum--;
            }
            else
            {
                //石を置いたらターンを交代
                piecesOnBoard[mousePointX, mousePointZ] = turnPlayer;
                SetPiece(mousePointX, mousePointZ);
                turnPlayer =1;
                turnPlayerText.GetComponent<Text>().text = "White Turn";
                passButtonScript.passCount = 0;
                remainPlacesNum--;
            }
            if (remainPlacesNum == 0)
            {
                ShowResult();
            }
        }
    }

    void SetStartPieces()
    {
        piecesOnBoard[3, 3] = 2;
        piecesOnBoard[4, 3] = 1;
        piecesOnBoard[3, 4] = 1;
        piecesOnBoard[4, 4] = 2;
    }


    public bool CheckCanSetPiece(int mousePointX, int mousePointZ)
    {
        canSet = false;

        if(piecesOnBoard[mousePointX, mousePointZ] != 0)
        {
            return canSet;
        }

        //縦方向：x軸+方向 
        if (mousePointX < 6 && piecesOnBoard[mousePointX + 1, mousePointZ] != turnPlayer && piecesOnBoard[mousePointX + 1, mousePointZ] != 0)
        {
            for (int i = mousePointX + 2; i < 8; i++)
            {
                if (piecesOnBoard[i, mousePointZ] == turnPlayer)
                {
                    canSet = true;
                    break;
                }
                if(piecesOnBoard[i, mousePointZ] == 0)
                {
                    break;
                }
            }
        }

        //縦方向：x軸-方向
        if (mousePointX > 1 && piecesOnBoard[mousePointX - 1, mousePointZ] != turnPlayer && piecesOnBoard[mousePointX - 1, mousePointZ] != 0)
        {
            for (int i = mousePointX - 2; i > -1; i--)
            {
                if (piecesOnBoard[i, mousePointZ] == turnPlayer)
                {
                    canSet = true;
                    break;
                }
                if (piecesOnBoard[i, mousePointZ] == 0)
                {
                    break;
                }
            }
        }

        //横方向：z軸+方向
        if (mousePointZ < 6 && piecesOnBoard[mousePointX, mousePointZ + 1] != turnPlayer && piecesOnBoard[mousePointX, mousePointZ + 1] != 0)
        {
            for (int i = mousePointZ + 2; i < 8; i++)
            {
                if (piecesOnBoard[mousePointX, i] == turnPlayer)
                {

                    canSet = true;
                    break;
                }
                if (piecesOnBoard[mousePointX, i] == 0)
                {
                    break;
                }
            }
        }

        //横方向：z軸-方向
        if (mousePointZ > 1 && piecesOnBoard[mousePointX, mousePointZ - 1] != turnPlayer && piecesOnBoard[mousePointX, mousePointZ - 1] != 0)
        {
            for (int i = mousePointZ - 2; i > 0; i--)
            {
                if (piecesOnBoard[mousePointX, i] == turnPlayer)
                {
                    canSet = true;
                    break;
                }
                if (piecesOnBoard[mousePointX, i] == 0)
                {
                    break;
                }
            }
        }


        //斜め方向：x軸+,z軸+方向
        if (mousePointX < 6 && mousePointZ < 6 && piecesOnBoard[mousePointX + 1, mousePointZ + 1] != turnPlayer && piecesOnBoard[mousePointX + 1, mousePointZ + 1] != 0)
        {
            int i = 2;
            while((mousePointX + i) < 8 && (mousePointZ + i) < 8)
            {
                if (piecesOnBoard[mousePointX + i, mousePointZ + i] == turnPlayer)
                {
                    canSet = true;
                    break;
                }

                if (piecesOnBoard[mousePointX + i, mousePointZ + i] == 0)
                {
                    break;
                }
                i++;
            }
        }

        //斜め方向：x軸-,z軸-方向
        if (mousePointX > 1 && mousePointZ > 1 && piecesOnBoard[mousePointX - 1, mousePointZ - 1] != turnPlayer && piecesOnBoard[mousePointX - 1, mousePointZ - 1] != 0)
        {
            int i = 2;
            while ((mousePointX - i) > -1 && (mousePointZ - i) > -1)
            {
                if (piecesOnBoard[mousePointX - i, mousePointZ - i] == turnPlayer)
                {
                    canSet = true;
                    break;
                }
                if (piecesOnBoard[mousePointX - i, mousePointZ - i] == 0)
                {
                    break;
                }
                i++;
            }
        }

        //斜め方向：x軸+,z軸-方向
        if (mousePointX < 6 && mousePointZ > 1 && piecesOnBoard[mousePointX + 1, mousePointZ - 1] != turnPlayer && piecesOnBoard[mousePointX + 1, mousePointZ - 1] != 0)
        {
            int i = 2;
            while ((mousePointX + i) < 8 && (mousePointZ - i) > -1)
            {
                if (piecesOnBoard[mousePointX + i, mousePointZ - i] == turnPlayer)
                {
                    canSet = true;
                    break;
                }
                if (piecesOnBoard[mousePointX + i, mousePointZ - i] == 0)
                {
                    break;
                }
                i++;
            }
        }

        //斜め方向：x軸-,z軸+方向
        if (mousePointX > 1 && mousePointZ < 6 && piecesOnBoard[mousePointX - 1, mousePointZ + 1] != turnPlayer && piecesOnBoard[mousePointX - 1, mousePointZ + 1] != 0)
        {
            int i = 2;
            while ((mousePointX - i) > -1 && (mousePointZ + i) < 8)
            {
                if (piecesOnBoard[mousePointX - i, mousePointZ + i] == turnPlayer)
                {
                    canSet = true;
                    break;
                }

                if (piecesOnBoard[mousePointX - i, mousePointZ + i] == 0)
                {
                    break;
                }
                i++;
            }
        }
        

        return canSet;
    }


    void SetPiece(int mousePointX, int mousePointZ)
    {
        //縦横方向：x軸+方向 
        if (mousePointX < 6 && piecesOnBoard[mousePointX + 1, mousePointZ] != turnPlayer && piecesOnBoard[mousePointX + 1, mousePointZ] != 0)
        {
            for (int i = mousePointX + 2; i < 8; i++)
            {
                if (piecesOnBoard[i, mousePointZ] == turnPlayer)
                {
                    ReversePieces(mousePointX, mousePointZ, i, mousePointZ);
                    break;
                }
                if (piecesOnBoard[i, mousePointZ] == 0)
                {
                    break;
                }
            }
        }

        //縦横方向：x軸-方向
        if (mousePointX > 1 && piecesOnBoard[mousePointX - 1, mousePointZ] != turnPlayer && piecesOnBoard[mousePointX - 1, mousePointZ] != 0)
        {
            for (int i = mousePointX - 2; i > -1; i--)
            {
                if (piecesOnBoard[i, mousePointZ] == turnPlayer)
                {
                    ReversePieces(i, mousePointZ, mousePointX, mousePointZ);
                    break;
                }
                if (piecesOnBoard[i, mousePointZ] == 0)
                {
                    break;
                }
            }
        }

        //縦横方向：z軸+方向
        if (mousePointZ < 6 && piecesOnBoard[mousePointX, mousePointZ + 1] != turnPlayer && piecesOnBoard[mousePointX, mousePointZ + 1] != 0)
        {
            for (int i = mousePointZ + 2; i < 8; i++)
            {
                if (piecesOnBoard[mousePointX, i] == turnPlayer)
                {
                    ReversePieces(mousePointX, mousePointZ, mousePointX, i);
                    break;
                }
                if (piecesOnBoard[mousePointX, i] == 0)
                {
                    break;
                }
            }
        }

        //縦横方向：z軸-方向
        if (mousePointZ > 1 && piecesOnBoard[mousePointX, mousePointZ - 1] != turnPlayer && piecesOnBoard[mousePointX, mousePointZ - 1] != 0)
        {
            for (int i = mousePointZ - 2; i > 0; i--)
            {
                if (piecesOnBoard[mousePointX, i] == turnPlayer)
                {
                    ReversePieces(mousePointX, i, mousePointX, mousePointZ);
                    break;
                }
                if (piecesOnBoard[mousePointX, i] == 0)
                {
                    break;
                }
            }
        }


        //斜め方向：x軸+,z軸+方向
        if (mousePointX < 6 && mousePointZ < 6 && piecesOnBoard[mousePointX + 1, mousePointZ + 1] != turnPlayer && piecesOnBoard[mousePointX + 1, mousePointZ + 1] != 0)
        {
            int i = 2;
            while ((mousePointX + i) < 8 && (mousePointZ + i) < 8)
            {
                if (piecesOnBoard[mousePointX + i, mousePointZ + i] == turnPlayer)
                {
                    ReversePieces(mousePointX, mousePointZ, mousePointX + i, mousePointZ + i);
                    break;
                }
                if (piecesOnBoard[mousePointX + i, mousePointZ + i] == 0)
                {
                    break;
                }
                i++;
            }
        }

        //斜め方向：x軸-,z軸-方向
        if (mousePointX > 1 && mousePointZ > 1 && piecesOnBoard[mousePointX - 1, mousePointZ - 1] != turnPlayer && piecesOnBoard[mousePointX - 1, mousePointZ - 1] != 0)
        {
            int i = 2;
            while ((mousePointX - i) > -1 && (mousePointZ - i) > -1)
            {
                if (piecesOnBoard[mousePointX - i, mousePointZ - i] == turnPlayer)
                {
                    ReversePieces(mousePointX - i, mousePointZ - i, mousePointX, mousePointZ);
                    break;
                }
                if (piecesOnBoard[mousePointX - i, mousePointZ - i] == 0)
                {
                    break;
                }
                i++;
            }
        }
        
        //斜め方向：x軸+,z軸-方向
        if (mousePointX < 6 && mousePointZ > 1 && piecesOnBoard[mousePointX + 1, mousePointZ - 1] != turnPlayer && piecesOnBoard[mousePointX + 1, mousePointZ - 1] != 0)
        {
            int i = 2;
            while ((mousePointX + i) < 8 && (mousePointZ - i) > -1)
            {
                if (piecesOnBoard[mousePointX + i, mousePointZ - i] == turnPlayer)
                {
                    ReversePieces(mousePointX, mousePointZ, mousePointX + i, mousePointZ - i);
                    break;
                }
                if (piecesOnBoard[mousePointX + i, mousePointZ - i] == 0)
                {
                    break;
                }
                i++;
            }
        }

        //斜め方向：x軸-,z軸+方向
        if (mousePointX > 1 && mousePointZ < 6 && piecesOnBoard[mousePointX - 1, mousePointZ + 1] != turnPlayer && piecesOnBoard[mousePointX - 1, mousePointZ + 1] != 0)
        {
            int i = 2;
            while ((mousePointX - i) > -1 && (mousePointZ + i) < 8)
            {
                if (piecesOnBoard[mousePointX - i, mousePointZ + i] == turnPlayer)
                {
                    ReversePieces(mousePointX - i, mousePointZ + i, mousePointX, mousePointZ);
                    break;
                }
                if (piecesOnBoard[mousePointX - i, mousePointZ + i] == 0)
                {
                    break;
                }
                i++;
            }
        }
    }


    void ReversePieces(int startIndexX, int startIndexZ, int endIndexX, int endIndexZ)
    {
        //横方向
        if(startIndexZ == endIndexZ)
        {
            for (int i = 0; i < endIndexX - startIndexX + 1; i++)
            {
                if (turnPlayer == 1)
                {
                    piecesOnBoard[startIndexX + i, startIndexZ] = 1;
                }
                else
                {
                    piecesOnBoard[startIndexX + i, startIndexZ] = 2;
                }
            }
        }

        //縦方向
        if (startIndexX == endIndexX)
        {
            for (int i = 0; i < endIndexZ - startIndexZ + 1; i++)
            {
                if (turnPlayer == 1)
                {
                    piecesOnBoard[startIndexX, startIndexZ + i] = 1;
                }
                else
                {
                    piecesOnBoard[startIndexX, startIndexZ + i] = 2;
                }
            }
        }

        //斜め方向：x+ z+方向
        if (startIndexX < endIndexX && startIndexZ < endIndexZ)
        {
            for (int i = 0; i < endIndexX - startIndexX + 1; i++)
            {
                if (turnPlayer == 1)
                {
                    piecesOnBoard[startIndexX + i, startIndexZ + i] = 1;
                }
                else
                {
                    piecesOnBoard[startIndexX + i, startIndexZ + i] = 2;
                }
            }
        }

        //斜め方向：x+ z-方向
        if (startIndexX < endIndexX && startIndexZ > endIndexZ)
        {
            for (int i = 0; i < endIndexX - startIndexX + 1; i++)
            {
                if (turnPlayer == 1)
                {
                    piecesOnBoard[startIndexX + i, startIndexZ - i] = 1;
                }
                else
                {
                    piecesOnBoard[startIndexX + i, startIndexZ - i] = 2;
                }
            }
        }


        RenderPiecesOnBoard();
    }


    //盤面の描画
    private void RenderPiecesOnBoard()
    {
        //描画する前に一度、全体を削除
        var pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach(var piece in pieces)
        {
            Destroy(piece);
        }

        //配列に格納されたデータに基づいて盤面を描画
        for (int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++)
            {
                if(piecesOnBoard[j, i] == 1)
                {
                    GameObject pieceObj = Instantiate(
                        piecePrefab,
                            new Vector3(j, 0, i),
                                Quaternion.Euler(0, 0, 0)
                        );
                }
                else if(piecesOnBoard[j, i] == 2)
                {
                    GameObject pieceObj = Instantiate(
                        piecePrefab,
                            new Vector3(j, 0.08f, i),
                                Quaternion.Euler(180, 0, 0)
                        );
                }
            }
        }
    }


    private void ShowResult()
    {
        var whiteCount = 0;
        var blackCount = 0;
        var result = "";

        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(piecesOnBoard[j, i] == 1)
                {
                    whiteCount++;
                } else if(piecesOnBoard[j, i] == 2)
                {
                    blackCount++;
                }
            }
        }

        if(whiteCount > blackCount)
        {
            result = "White Win !";
        }else if(whiteCount < blackCount)
        {
            result = "Black Win !";
        }
        else
        {
            result = "Draw !";
        }

        resultText.GetComponent<Text>().text = "White:" + whiteCount + " vs Black:" + blackCount + "\n\n" + result;
    }
}
