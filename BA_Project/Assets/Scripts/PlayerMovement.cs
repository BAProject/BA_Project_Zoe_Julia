
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;
    public MeshRenderer meshRenderer;
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    public Transform characterGfxTransform;

    public Animator Player_Idle_Run;

    private void Awake()
    {
        if (!cam)
            cam = Camera.main.transform;

        if (!meshRenderer)
            meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);

        //Player_Idle_Run = GetComponent<Animator>();
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        //_inputs = cam.TransformDirection(_inputs).normalized;
        _inputs.y = _body.velocity.y;
        _body.velocity = _inputs * speed;

        if (Input.GetAxis("Horizontal") != 0)
        {
            //Play run animation
            Player_Idle_Run.Play("Run");
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            //Play run animation
            Player_Idle_Run.Play("Run");
        }
        //If the player is moving vertically (forward and backward) or diagonally

        else
        {
            //stop animation
            Player_Idle_Run.Play("Stop");
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        //ROTATE CHARACTER GFX TO MOVEDIRECTION
        Vector3 moveDirectionToLook = new Vector3((_inputs).normalized.x, 0, (_inputs).normalized.z);
        Vector3 lookingDirection = Vector3.RotateTowards(characterGfxTransform.forward, moveDirectionToLook, 8 * Time.deltaTime, 0.0F);
        characterGfxTransform.rotation = Quaternion.LookRotation(lookingDirection);
    }

    public void SetEnabled(bool isEnabled)
    {
        _body.velocity = Vector3.zero;
        meshRenderer.enabled = isEnabled;
        enabled = isEnabled;
    }
}
