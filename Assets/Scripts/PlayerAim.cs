using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControlls controlls;

    [Header("Aim controll")]
    [SerializeField] private Transform aim;

    [Header("Camera controll")]
    [SerializeField] private Transform cameraTarget;
    [Range(.5f,1)]
    [SerializeField] private float minCameraDistance =1.5f;

    [Range(1,3f)]
    [SerializeField] private float maxCameraDistance =4f;

    [Range(3f,5f)]
    [SerializeField] private float cameraSensetivity=5f;

    [SerializeField] private LayerMask aimLayerMask;
    
    private Vector2 aimInput;

    private RaycastHit lastKnowMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
        
    }

    private void Update()
    {
        aim.position = GetMouseHitInfo().point;
        aim.position = new Vector3(aim.position.x, transform.position.y +1, aim.position.z);

        cameraTarget.position= Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(),cameraSensetivity * Time.deltaTime);
    }

    private Vector3 DesiredCameraPosition()
    {
        //float actualMaxCameraDistance;

        //bool movingDownwards = player.movement.moveInput.y < -.5f;
        //if (movingDownwards)
        //{
        //    actualMaxCameraDistance = minCameraDistance;
        //}
        //else
        //{
        //    actualMaxCameraDistance= maxCameraDistance;
        //}

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition= GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition -transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);

       float clampedDistance= Mathf.Clamp(distanceToDesiredPosition, minCameraDistance,maxCameraDistance);
            desiredCameraPosition = transform.position + aimDirection * clampedDistance;
            desiredCameraPosition.y = transform.position.y + 1; 

        return desiredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray=Camera.main.ScreenPointToRay(aimInput);

        if(Physics.Raycast(ray, out var hitInfo,Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }
        return lastKnowMouseHit;
    }

    private void AssignInputEvents()
    {

        controlls = player.controlls;
        controlls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controlls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
