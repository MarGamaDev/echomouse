using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpPower = 1f;
    private Vector2 inputVector = new Vector2();
    private Vector3 moveForce;

    [Space]
    [SerializeField] private Transform groundRayTransform;
    [SerializeField] private float groundRayLength = 1f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded = true;
    bool isJumping = false;

    [Space]
    [SerializeField] private Transform camTransform;
    [SerializeField] private Vector2 mouseSensitivity = new(1f,1f);
    [SerializeField] private float maxUpDownAngle = 90f;
    private Vector2 mouseInput;
    private float camXRotation = 0f;

    [Space]
    [SerializeField] private Transform squeekRayTransform;
    [SerializeField] private LayerMask soundMask;
    [SerializeField] private float squeekRayLength = 2f;
    [SerializeField] private float squeekCooldownInSeconds = 1f;
    private float squeekTimer = 0f;

    private Rigidbody rb;

    [Space]
    [SerializeField] private AudioSource squeekSource, stepSource;
    [SerializeField] private List<AudioClip> squeekClipList, stepClipList;
    [SerializeField] private AudioClip scaredSqueek;
    [SerializeField] private float catScareDistance = 3f;
    private Transform catTransform;

    private void Start()
    {
        catTransform = GameObject.FindGameObjectWithTag("Cat").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        inputVector = inputVector.normalized;

        Vector3 rightForce = transform.right * inputVector.x;
        Vector3 forwardForce = transform.forward * inputVector.y;
        moveForce = (rightForce + forwardForce) * moveSpeed;

        isGrounded = Physics.Raycast(groundRayTransform.position, -groundRayTransform.up, groundRayLength, groundMask);
        mouseInput = new(Input.GetAxis("Mouse X") * mouseSensitivity.x, Input.GetAxis("Mouse Y") * mouseSensitivity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }

        if (Input.GetMouseButtonDown(0) && squeekTimer >= squeekCooldownInSeconds && Time.timeScale != 0)
        {
            AudioClip clip = scaredSqueek;
            if ((catTransform.position - transform.position).magnitude >= catScareDistance)
                clip = squeekClipList[Random.Range(0, squeekClipList.Count)];
            squeekSource.PlayOneShot(clip);
            //cast ray forward
            RaycastHit hit;
            Vector3 pos = squeekRayTransform.forward * squeekRayLength;
            if (Physics.Raycast(squeekRayTransform.position, squeekRayTransform.forward, out hit, squeekRayLength, soundMask))
            {
                pos = hit.point;
            }
            EchoPointManager.instance.GetPulse(pos);
            //if it hits, make soundwave
            squeekTimer = 0f;
        }

        squeekTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            moveForce *= Time.fixedDeltaTime;
            moveForce.y = rb.velocity.y;
            rb.velocity = moveForce;
            if (inputVector.magnitude != 0 && !stepSource.isPlaying)
            {
                stepSource.PlayOneShot(stepClipList[Random.Range(0, stepClipList.Count)]);
            }
        }

        if (isJumping)
        {
            rb.AddForce(Vector3.up * (jumpPower * Time.deltaTime), ForceMode.Impulse);
            isJumping = false;
        }

        //rotate cam
        float rotX = camXRotation - mouseInput.y * Time.fixedDeltaTime;
        rotX = Mathf.Clamp(rotX, -maxUpDownAngle, maxUpDownAngle);
        camXRotation = rotX;
        camTransform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.Rotate(0, mouseInput.x * Time.fixedDeltaTime, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundRayTransform.position, groundRayTransform.position -  groundRayTransform.up * groundRayLength);
        Gizmos.DrawLine(squeekRayTransform.position, squeekRayTransform.position + squeekRayTransform.forward * squeekRayLength);
    }
}
