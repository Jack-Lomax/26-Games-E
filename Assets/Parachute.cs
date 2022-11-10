using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Parachute : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    bool isToggled;

    public void EnableParachute()
    {
        if(isToggled) return;
        transform.DOKill(true);
        transform.localPosition = Vector3.zero;
        transform.DOScale(Vector3.one, speed).SetEase(Ease.OutElastic);
        isToggled = true;
    }

    public void DisableParachute()
    {
        if(!isToggled) return;
        transform.DOMove(transform.position + Vector3.up * 10, 0.5f).SetEase(Ease.OutExpo);
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        isToggled = false;
    }
}
