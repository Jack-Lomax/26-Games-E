using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EggGraphics : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Parachute parachute;
    [SerializeField] private Transform shell;
    [SerializeField] private Transform crackedEgg;
    private Vector3 posLastFrame;

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void SetIsWalkingState(float velMag)
    {
        animator.SetBool("isWalking", velMag > 0.01f);
    }

    public void ToggleParachute(bool toggle)
    {
        if(toggle)
            parachute.EnableParachute();
        else
            parachute.DisableParachute();
    }

    public void Crack()
    {
        shell.GetComponent<Animator>().enabled = false;
        shell.localScale = Vector3.zero;
        crackedEgg.DOScale(Vector3.one * 2, 0.2f).SetEase(Ease.OutSine);
    }
}
