using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ring : MonoBehaviour
{
    private Material _myMat;
    public float disappearTime = 0.3f;
    [SerializeField] private LayerMask _whatIsPlayer;

    private void Awake()
    {
        _myMat = GetComponent<MeshRenderer>().material;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & _whatIsPlayer) != 0)
        {
            Sequence seq = DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0, -2160, 0), 2.5f))
                .Append(transform.DOMove(new Vector3(0, 5, 0), 1f))
                .Append(_myMat.DOFade(0, disappearTime).SetEase(Ease.Linear).OnComplete(() => gameObject.SetActive(false)));
        }
    }
}
