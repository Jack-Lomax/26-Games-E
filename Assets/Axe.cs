using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private float speed;

    float rotation = 0;

    AudioSource swoosh;

    void Awake() => swoosh = GetComponent<AudioSource>();

    void Update()
    {
        rotation += Mathf.PI * 2 * Time.deltaTime * speed;

        Quaternion startRot = Quaternion.Euler(0,0,-80);
        Quaternion endRot = Quaternion.Euler(0,0,80);

        transform.localRotation = Quaternion.Lerp(startRot, endRot, (Mathf.Sin(rotation) + 1) / 2);

        //if(((((Mathf.Sin(rotation) + 1) / 2)) >= 0.9f || ((Mathf.Sin(rotation) + 1) / 2) <= 0.5f) && !swoosh.isPlaying)
        //    swoosh.Play();

    }
}
