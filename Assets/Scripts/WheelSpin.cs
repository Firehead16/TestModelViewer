using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour, IPlayable
{
    [SerializeField]
    private float speed;

    public void Play()
    {
        transform.DOLocalRotate(new Vector3(speed, 0, 0), 1, RotateMode.LocalAxisAdd).SetLoops(-1);
    }

    public void Stop()
    {
        DOTween.Kill(transform);
    }
}
