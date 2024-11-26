using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TitleStart : MonoBehaviour
{
    [SerializeField] private int sceneNumber;

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
