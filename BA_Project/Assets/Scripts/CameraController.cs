using UnityEngine;
 
public class CameraController : MonoBehaviour {
 
    public Transform target;
    public Vector3 targetOffset=Vector3.zero;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
 
    public float distanceMin = .5f;
    public float distanceMax = 15f;
 
    private Rigidbody rigidbody;
 
    float x = 0.0f;
    float y = 0.0f;
 
    public float xOffset;
    public float yOffset;



    public float topdownOffsetY=10f;
    public float topdownOffsetX=0f;
    public float topdownOffsetZ=10f;
    public float topdownInitRotationX=45f;
    // Use this for initialization
    void Start () 
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
        rigidbody = GetComponent<Rigidbody>();
 
        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }
 
    void LateUpdate () 
    {
        if (target) 
        {
            if(target.CompareTag("Player"))
            {
                transform.rotation=Quaternion.Euler(topdownInitRotationX,0,0);
                Vector3 positionCamWantsToReach= new Vector3(target.position.x,0,target.position.z)+ new Vector3(topdownOffsetX,topdownOffsetY,topdownOffsetZ);
                transform.position =   Vector3.MoveTowards(transform.position, positionCamWantsToReach, 50f*Time.deltaTime);
                //target.position+ new Vector3(topdownOffsetX,topdownOffsetY,topdownOffsetZ);

                //transform.position.y += 10.0f;
                //transform.position.z -= 10.0f;
               // transform.LookAt(target.position);
            }

            if(!target.CompareTag("Player")){
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
 
            y = ClampAngle(y, yMinLimit, yMaxLimit);
 
            Quaternion rotation = Quaternion.Euler(y, x, 0);
 
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
 
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + (target.position+targetOffset);
 
            transform.rotation = rotation;
            transform.position = position+transform.up*yOffset+transform.right*xOffset;
            }
        }
    }
 
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}