using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class FloorUI : MonoBehaviour
{
    [Header("UISetting")] 
    [SerializeField] private Transform maxFloor;
    [SerializeField] private Transform minFloor;
    
    private Image _floorMaxImage;
    private Image _floorMinImage;
    private Image _floorImage;

    private void Awake()
    {
        _floorImage = transform.Find("CurrentPlayer").GetComponent<Image>();
        _floorMaxImage = transform.Find("FloorInfoMax").GetComponent<Image>();
        _floorMinImage = transform.Find("FloorInfoMin").GetComponent<Image>();
    }

    private void Update()
    {
        if (GameManager.Instance.player != null)
        {
            float floorMin = minFloor.position.y;
            float floorMax = maxFloor.position.y;
            float playerY = GameManager.Instance.player.transform.position.y;
            
            float value = (playerY - floorMin) / (floorMax - floorMin);
            
            Vector3 lerpPoint = Vector3.Lerp(_floorMinImage.transform.position
                , _floorMaxImage.transform.position, value);
            _floorImage.transform.position = new Vector3(_floorImage.transform.position.x , lerpPoint.y);
        }
    }
}
