using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CamEffect : MonoBehaviour
{
    [Header("PPSetting")] [SerializeField] private Volume volume;

    [Header("DashEffect")] [SerializeField]
    private float dashIntensity = -0.9f;

    [SerializeField] private float dashTime = 0.2f;

    [Header("ShakeSetting")] [SerializeField]
    private float shakeForce;

    [SerializeField] private float shakeDuration;
    private Tween shakeTween;

    [Header("CamEffect")] [SerializeField] private float fOV;
    [SerializeField] private float setSpeedValue;
    [SerializeField] private float startFOV;
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    public void DashEffect()
    {
        LensDistortion lens;
        DepthOfField depth;

        if (volume.profile.TryGet(out lens))
        {
            if (volume.profile.TryGet(out depth))
            {
                lens.intensity.value = dashIntensity;
                depth.focalLength.value = 300;
                DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, 0, dashTime);
                DOTween.To(() => depth.focalLength.value, x => depth.focalLength.value = x, 0, dashTime);
            }
        }
    }

    public void CameraShaking()
    {
        if (shakeTween.IsActive()) return;

        shakeTween = _mainCam.DOShakePosition(shakeDuration, shakeForce);
    }

    public void SetFOV(float speed)
    {
        if (speed < setSpeedValue) _mainCam.fieldOfView = startFOV;
        else _mainCam.fieldOfView = speed - setSpeedValue + startFOV;
    }
}