using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float glideDrag;
    [SerializeField] private float timeBetweenDashes;
    [SerializeField] private EggGraphics graphics;
    private Rigidbody rb;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private float sensitivity;

    [SerializeField] private Transform cam;

    [SerializeField] private TMPro.TMP_Text timerText;
    [SerializeField] private TMPro.TMP_Text timer2Text;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private AudioSource trampoline;
    float timer;


    float yRot;

    float xInput, yInput;
    public bool canDash {get; set;} = true;
    public bool canGlide {get; set;} = true;

    float timeOfLastDash;

    bool IsGrounded => Physics.CheckSphere(groundCheck.position, groundCheckDistance, LayerMask.GetMask("GROUND", "ICE", "BOUNCE"));

    bool isGliding;

    float maxHeight;
    float jumpHeight;

    bool canMove = true;

    bool onIce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(!canMove) return;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        timer += Time.deltaTime;
        timerText.text = timer.ToString("F2");
        timer2Text.text = timer.ToString("F2");


        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            Jump();


        yRot += sensitivity * Input.GetAxis("Mouse X");

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, yRot, transform.eulerAngles.z);

        //if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time - timeOfLastDash > timeBetweenDashes)
        //    StartCoroutine(Dash());

        graphics.SetIsWalkingState(IsGrounded ? new Vector2(rb.velocity.x, rb.velocity.z).magnitude : 0);

        if(!IsGrounded && transform.position.y > maxHeight)
            maxHeight = transform.position.y;
        else if(IsGrounded && maxHeight != transform.position.y)
        {   
            float difference = maxHeight - transform.position.y;
            if(difference > 3)
                Crack();
            maxHeight = transform.position.y;
        }



        if(isGliding)
            maxHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        if(!canMove) return;

        Vector3 camNoY = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;

        float y = rb.velocity.y;
        rb.velocity = (camNoY * yInput + cam.right * xInput).normalized * speed * (onIce ? 0.3f : 1);
        rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);

        isGliding = false;
        if(canGlide && !IsGrounded && Input.GetKey(KeyCode.Space) && rb.velocity.y < 0)
            Glide();

        graphics.ToggleParachute(isGliding);
            
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity += Vector3.up * jumpForce * (onIce ? 0.75f : 1);
        graphics.Jump();
    }

    IEnumerator Dash()
    {
        speed *= 10;
        yield return new WaitForSeconds(0.2f);
        speed /= 10;
        timeOfLastDash = Time.time;
    }

    void Glide()
    {
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -1, Mathf.Infinity), rb.velocity.z);
        isGliding = true;
    }

    void Crack()
    {
        canMove = false;
        graphics.Crack();
        Invoke(nameof(ReturnToMenu), 4f);
        GetComponent<AudioSource>().Play();
    }

    void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("ICE"))
            onIce = true;
        else if(col.gameObject.layer == LayerMask.NameToLayer("DEATH"))
            Crack();
        else if (col.gameObject.layer == LayerMask.NameToLayer("BOUNCE"))
        {
            trampoline.Play();
        }
        else if(col.gameObject.layer == LayerMask.NameToLayer("FINISH"))
        {
            endScreen.SetActive(true);
            Crack();
        }
    }

    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("ICE"))
            onIce = false;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
    }



}
