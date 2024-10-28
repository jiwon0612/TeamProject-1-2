using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DownPlatForm : MonoBehaviour
{
    public float disappearTime = 1f;
    [SerializeField] private LayerMask _whatIsPlayer;
    private Material _myMat;

    private void Awake()
    {
        _myMat = GetComponent<MeshRenderer>().material;
    }
    private void OnCollisionEnter(Collision other)
    {
        if ((1 << other.gameObject.layer & _whatIsPlayer) != 0)
        {
            _myMat.DOFade(0, disappearTime).OnComplete(() => gameObject.SetActive(false)).SetEase(Ease.Linear);
        }
    }
}
