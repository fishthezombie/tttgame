﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameVisualController : MonoBehaviour {

    public GameObject gameEnvironment;
    public GameObject winBackground;
    public Text topTextMessage;
    public Text midTextMessage;
    public string firstMessage;

    public Sprite playerXSprite;
    public Sprite playerOSprite;

    //Initialization
    void Start () {

        //Initialize game visual
        GameObject environment = Instantiate(gameEnvironment, this.transform.position, Quaternion.identity) as GameObject;
        environment.transform.SetParent(this.transform);
        winBackground.SetActive(false);
        topTextMessage.text = firstMessage;
        midTextMessage.text = "";
    }

    void LateUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            if (getTopTextMessage() == firstMessage)
                setTopTextMessage("");
        }
    }

    //Insert the player mark in the interacted grid
    public void InsertMark (GameObject grid, bool xTurn) {
        if (xTurn)
            grid.GetComponent<SpriteRenderer>().sprite = playerXSprite;
        else
            grid.GetComponent<SpriteRenderer>().sprite = playerOSprite;
    }

    public void UpdateScoreBoard() {
        ScoreMenuDisplay scoreDisplay = gameObject.GetComponent<ScoreMenuDisplay>();
        scoreDisplay.UpdateScoreBoard();
    }

    public void WinGameVisual (string winMessage) {
        winBackground.SetActive(true);
        setMidTextMessage(winMessage);
    }

    public void setTopTextMessage(string inputText) {
        topTextMessage.text = inputText;
    }

    public string getTopTextMessage() {
        return topTextMessage.text;
    }

    public void setMidTextMessage(string inputText) {
        midTextMessage.text = inputText;
    }

    public string getMidTextMessage() {
        return midTextMessage.text;
    }
}
