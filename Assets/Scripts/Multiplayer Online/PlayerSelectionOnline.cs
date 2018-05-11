﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class PlayerSelectionOnline : Photon.PunBehaviour
{
    PlayerIndex index1, index2, index3, index4;
    GamePadState state1, state2, state3, state4;
    GamePadState prevState1, prevState2, prevState3, prevState4;

    [SerializeField] MainMenu mainMenu;
    [SerializeField] ScriptablePlayer[] scriptablePlayers;
    public Skins skinsReference;
    [SerializeField] GameObject[] skinsPlayers;
    [SerializeField] Text[] txtsToJoin;
    int[] actualSkin;
    bool[] playerJoined;
    bool[] playerReady;
    public static int onlineNumber;
    Vector3[] initialPos;

    public int[] ActualSkin
    {
        get
        {
            return actualSkin;
        }
    }

    void Start()
    {
        index1 = PlayerIndex.One;
        index2 = PlayerIndex.Two;
        index3 = PlayerIndex.Three;
        index4 = PlayerIndex.Four;

        actualSkin = new int[4];
        actualSkin[0] = 0;
        actualSkin[1] = 0;
        actualSkin[2] = 0;
        actualSkin[3] = 0;

        playerReady = new bool[4];
        playerJoined = new bool[4];

        initialPos = new Vector3[4];
        for (int i = 0; i < initialPos.Length; i++)
        {
            initialPos[i] = skinsPlayers[i].transform.position;
        }

        onlineNumber = PhotonNetwork.playerList.Length;
    }

    void Update()
    {
        prevState1 = state1;
        prevState2 = state2;
        prevState3 = state3;
        prevState4 = state4;
        state1 = GamePad.GetState(index1);
        state2 = GamePad.GetState(index2);
        state3 = GamePad.GetState(index3);
        state4 = GamePad.GetState(index4);

        if (mainMenu.skinSelection.alpha == 1)
        {
            //if (prevState1.Buttons.A == ButtonState.Released && state1.Buttons.A == ButtonState.Pressed && !playerJoined[0])
            //{
            //    PlayerJoin(PhotonNetwork.player.ID);
            //}
            //if (prevState2.Buttons.A == ButtonState.Released && state2.Buttons.A == ButtonState.Pressed && !playerJoined[1])
            //{
            //    PlayerJoin(1);
            //}
            //if (prevState3.Buttons.A == ButtonState.Released && state3.Buttons.A == ButtonState.Pressed && !playerJoined[2])
            //{
            //    PlayerJoin(2);
            //}
            //if (prevState4.Buttons.A == ButtonState.Released && state4.Buttons.A == ButtonState.Pressed && !playerJoined[3])
            //{
            //    PlayerJoin(3);
            //}
        }
        #region Inputs Joystick 1
        if (prevState1.ThumbSticks.Left.X != 1 && state1.ThumbSticks.Left.X == 1)
        {
            SkinMovement(true, PhotonNetwork.player.ID-1);
            Debug.Log("Analogo");
        }
        if (prevState1.ThumbSticks.Left.X != -1 && state1.ThumbSticks.Left.X == -1)
        {
            SkinMovement(false, PhotonNetwork.player.ID-1);
        }
        #endregion
        #region Inputs Joystick 2
        if (prevState2.ThumbSticks.Left.X != 1 && state2.ThumbSticks.Left.X == 1)
        {
            SkinMovement(true, 1);
        }
        if (prevState2.ThumbSticks.Left.X != -1 && state2.ThumbSticks.Left.X == -1)
        {
            SkinMovement(false, 1);
        }
        #endregion
        #region Inputs Joystick 3
        if (prevState3.ThumbSticks.Left.X != 1 && state3.ThumbSticks.Left.X == 1)
        {
            SkinMovement(true, 2);
        }
        if (prevState3.ThumbSticks.Left.X != -1 && state3.ThumbSticks.Left.X == -1)
        {
            SkinMovement(false, 2);
        }
        #endregion
        #region Inputs Joystick 4
        if (prevState4.ThumbSticks.Left.X != 1 && state4.ThumbSticks.Left.X == 1)
        {
            SkinMovement(true, 3);
        }
        if (prevState4.ThumbSticks.Left.X != -1 && state4.ThumbSticks.Left.X == -1)
        {
            SkinMovement(false, 3);
        }
        #endregion
        #region Players Are Ready
        if (prevState1.Buttons.A == ButtonState.Released && state1.Buttons.A == ButtonState.Pressed && playerJoined[0])
            PlayersReady(PhotonNetwork.player.ID);
        //if (prevState2.Buttons.A == ButtonState.Released && state2.Buttons.A == ButtonState.Pressed && playerJoined[1])
        //    PlayersReady(1);
        //if (prevState3.Buttons.A == ButtonState.Released && state3.Buttons.A == ButtonState.Pressed && playerJoined[2])
        //    PlayersReady(2);
        //if (prevState4.Buttons.A == ButtonState.Released && state4.Buttons.A == ButtonState.Pressed && playerJoined[3])
        //    PlayersReady(3);
        #endregion
        #region Players Cancel
        if (prevState1.Buttons.B == ButtonState.Released && state1.Buttons.B == ButtonState.Pressed && playerJoined[0])
            PlayersCancel(0);
        if (prevState2.Buttons.B == ButtonState.Released && state2.Buttons.B == ButtonState.Pressed && playerJoined[1])
            PlayersCancel(1);
        if (prevState3.Buttons.B == ButtonState.Released && state3.Buttons.B == ButtonState.Pressed && playerJoined[2])
            PlayersCancel(2);
        if (prevState4.Buttons.B == ButtonState.Released && state4.Buttons.B == ButtonState.Pressed && playerJoined[3])
            PlayersCancel(3);
        #endregion
        if (prevState1.Buttons.Start == ButtonState.Released && state1.Buttons.Start == ButtonState.Pressed)
            SceneManager.LoadScene("MultiplayerLevel");
        if (prevState2.Buttons.Start == ButtonState.Released && state2.Buttons.Start == ButtonState.Pressed)
            SceneManager.LoadScene("MultiplayerLevel");
    }
    [PunRPC]
    public void PlayersReady(int playerNumber)
    {
        scriptablePlayers[playerNumber].actualSkin = skinsReference.skins[ActualSkin[playerNumber]];
        playerReady[playerNumber] = true;
        skinsPlayers[playerNumber].GetComponent<VFX>().Score();
        Debug.Log(PhotonNetwork.player.ID);
    }
    void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.isWriting)
        {
            stream.SendNext(scriptablePlayers[PhotonNetwork.playerList.Length]);
        }
        else
        {
            scriptablePlayers[PhotonNetwork.playerList.Length] = (ScriptablePlayer)stream.ReceiveNext();
        }
    }
    public void PlayersCancel(int playerNumber)
    {
        playerReady[playerNumber] = false;
        txtsToJoin[playerNumber].CrossFadeAlpha(1, 1, true);
        skinsPlayers[playerNumber].SetActive(false);
        playerJoined[playerNumber] = false;
    }
    public void PlayerJoin(int playerNumber)
    {
        switch (playerNumber)
        {
            case 0:
                playerJoined[playerNumber] = true;
                skinsPlayers[0].SetActive(true);
                txtsToJoin[0].CrossFadeAlpha(0, 1, true);
                break;
            case 1:
                playerJoined[playerNumber] = true;
                skinsPlayers[1].SetActive(true);
                txtsToJoin[1].CrossFadeAlpha(0, 1, true);
                break;
            case 2:
                playerJoined[playerNumber] = true;
                skinsPlayers[2].SetActive(true);
                txtsToJoin[2].CrossFadeAlpha(0, 1, true);
                break;
            case 3:
                playerJoined[playerNumber] = true;
                skinsPlayers[3].SetActive(true);
                txtsToJoin[3].CrossFadeAlpha(0, 1, true);
                break;
        }
    }
    public void SkinMovement(bool right, int playerNumber)
    {
        if (playerJoined[playerNumber])
        {
            if (right && ActualSkin[playerNumber] < skinsPlayers.Length - 1)
                ActualSkin[playerNumber]++;
            if (!right && ActualSkin[playerNumber] >= 1)
                ActualSkin[playerNumber]--;

            GameObject lastSkin = skinsPlayers[playerNumber];
            //lastSkin.SetActive(false);
            PhotonNetwork.Destroy(lastSkin);
            skinsPlayers[playerNumber] = PhotonNetwork.Instantiate(skinsReference.skins[ActualSkin[playerNumber]].name, initialPos[playerNumber], Quaternion.identity,0);
            Debug.Log(skinsReference.skins[ActualSkin[playerNumber]].name);

            //skinsPlayers[playerNumber] = Instantiate(skinsReference.skins[ActualSkin[playerNumber]], initialPos[playerNumber], Quaternion.identity);
            skinsPlayers[playerNumber].SetActive(true);
        }
    }
}
