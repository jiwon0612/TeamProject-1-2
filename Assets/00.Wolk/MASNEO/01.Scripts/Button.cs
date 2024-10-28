using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject[] walls;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ShowWallsSequentially());
        }
    }
    public void ClickButton()
    {
        StartCoroutine(ShowWallsSequentially());
    }

    private IEnumerator ShowWallsSequentially()
    {
        foreach (GameObject wall in walls)
        {
            if (wall != null)
            {
                wall.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                wall.SetActive(false);
            }
        }
    }
}
