using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DownPlatForm : MonoBehaviour
{
    public float disappearTime = 1f;
    [SerializeField] private LayerMask _whatIsPlayer;
    private bool isSteppedOn = false;

    private Material _myMat;

    private void Awake()
    {
        _myMat = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & _whatIsPlayer) != 0)
        {   
            isSteppedOn = true;
            _myMat.DOFade(0, 0.5f).OnComplete(() => transform.gameObject.SetActive(false));
        }
    }
}
