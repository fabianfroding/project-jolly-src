using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float zoomDistance = 22.5f;
    [SerializeField] private Vector2 playerBaseOffset = new Vector2(1.5f, 1f);
    [SerializeField] private Vector2 playerWalkOffset = new Vector2(3f, 1f);
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Color backgroundColorDawnAndDusk;
    [SerializeField] private Color backgroundColorMidday;
    [SerializeField] private Color backgroundColorMidnight;

    private CameraState cameraState = CameraState.FollowPlayer;
    private CameraBehaviourZone.CameraBehaviour cameraBehaviour = CameraBehaviourZone.CameraBehaviour.HorizontalLane;

    private CameraBehaviourHorizontalLane behaviourHorizontalLane;
    private CameraBehaviourVerticalLane behaviourVerticalLane;
    private CameraBehaviourCrossLane behaviourCrossLane;
    private CameraBehaviourPointOfInterest behaviourPointOfInterest;

    private Vector2 cameraOffset;
    private Vector2 cameraDestination;
    private Vector2 followVelocity;
    private Vector2 followSmoothTime;
    private bool hasEnteredVertRegion = false;
    private Vector2 defaultFST = new Vector2(0.4f, 0.1f);
    private Vector2 lerpFST = new Vector2(0.01f, 0.01f);

    private Camera cachedCamera;
    private float cachedDawnMid;
    private float cachedDuskMid;

    private GameObject player;
    public GameObject Player
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>().gameObject;
            }
            return player;
        }
    }

    private Player playerScript;
    private Player PlayerScript
    {
        get
        {
            if (playerScript == null)
            {
                if (Player != null)
                {
                    playerScript = Player.GetComponent<Player>();
                }
            }
            return playerScript;
        }
    }

    public GameObject CameraBehaviourObject { get; private set; }
    public CameraBehaviourZone CameraBehaviourZone
    {
        get
        {
            if (CameraBehaviourObject != null)
            {
                return CameraBehaviourObject.GetComponent<CameraBehaviourZone>();
            }
            return null;
        }
    }

    public enum CameraState
    {
        FollowPlayer,
        HorizontalLane,
        VerticalLane,
        CrossLane,
        PointOfInterest
    }

    #region Unity Callback Functions
    private void Awake()
    {
        if (behaviourHorizontalLane == null) behaviourHorizontalLane = gameObject.AddComponent<CameraBehaviourHorizontalLane>();
        if (behaviourVerticalLane == null) behaviourVerticalLane = gameObject.AddComponent<CameraBehaviourVerticalLane>();
        if (behaviourCrossLane == null) behaviourCrossLane = gameObject.AddComponent<CameraBehaviourCrossLane>();
        if (behaviourPointOfInterest == null) behaviourPointOfInterest = gameObject.AddComponent<CameraBehaviourPointOfInterest>();

        cachedCamera = GetComponent<Camera>();

        DaytimeManager.OnDaytimeTick += UpdateBackgroundTint;
    }

    private void Start()
    {
        if (Player != null)
        {
            // Set distance on scene transition.
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -zoomDistance);
        }

        cachedDawnMid = DaytimeManager.Instance.GetDawnMidTime();
        cachedDuskMid = DaytimeManager.Instance.GetDuskMidTime();
    }

    private void LateUpdate()
    {
        if (Player != null)
        {
            CheckCameraBehaviour();
            FollowPlayer();
        }
    }

    private void OnEnable()
    {
        CameraBehaviourZone.OnEnterCameraBehaviourZone += SetCurrentBehaviourObj;
        CameraBehaviourZone.OnExitCameraBehaviourZone += SetCurrentBehaviourObj;
    }

    private void OnDisable()
    {
        CameraBehaviourZone.OnEnterCameraBehaviourZone -= SetCurrentBehaviourObj;
        CameraBehaviourZone.OnExitCameraBehaviourZone -= SetCurrentBehaviourObj;
    }

    private void OnDestroy()
    {
        DaytimeManager.OnDaytimeTick -= UpdateBackgroundTint;
    }
    #endregion

    #region Camera Behaviour Zone Getters
    public bool UsesTransformAsAnchor() => CameraBehaviourZone.UsesTransformAsAnchor();
    public bool GetHorizontalPanCameraUp() => CameraBehaviourZone.GetHorizontalPanCameraUp();
    public bool GetVerticalFollow() => CameraBehaviourZone.GetVerticalFollow();
    public bool GetStopAtLeft() => CameraBehaviourZone.GetStopAtLeft();
    public float GetHorizontalYAdjustment() => CameraBehaviourZone.GetHorizontalYAdjustment();
    #endregion

    public LayerMask GetGroundMask() => groundMask;

    public void SetCurrentBehaviourObj(GameObject obj)
    {
        CameraBehaviourObject = obj;
    }

    private void CheckCameraBehaviour()
    {
        if (CameraBehaviourObject == null)
        {
            behaviourHorizontalLane.ResetHoriAdjPos();
            cameraState = CameraState.FollowPlayer;
        }
        else
        {
            cameraBehaviour = CameraBehaviourObject.GetComponent<CameraBehaviourZone>().GetCameraBehaviour();
            switch (cameraBehaviour)
            {
                case CameraBehaviourZone.CameraBehaviour.PointOfInterest:
                    cameraState = CameraState.PointOfInterest;
                    break;
                case CameraBehaviourZone.CameraBehaviour.HorizontalLane:
                    cameraState = CameraState.HorizontalLane;
                    break;
                case CameraBehaviourZone.CameraBehaviour.VerticalLane:
                    cameraState = CameraState.VerticalLane;
                    break;
                case CameraBehaviourZone.CameraBehaviour.CrossLane:
                    cameraState = CameraState.CrossLane;
                    break;
                default:
                    cameraState = CameraState.FollowPlayer;
                    break;
            }
        }
    }

    private void FollowPlayer()
    {
        switch (cameraState)
        {
            case CameraState.FollowPlayer:
                cameraOffset = GetPlayerOffset();
                cameraDestination = GetCameraDestination(cameraOffset);
                break;
            case CameraState.HorizontalLane:
                cameraOffset = GetPlayerOffset();
                cameraDestination = behaviourHorizontalLane.GetCameraDestination(cameraOffset);
                break;
            case CameraState.VerticalLane:
                cameraOffset = GetPlayerOffset();
                cameraDestination = behaviourVerticalLane.GetCameraDestination(cameraOffset);
                break;
            case CameraState.CrossLane:
                cameraOffset = GetPlayerOffset();
                cameraDestination = behaviourCrossLane.GetCameraDestination(cameraOffset);
                break;
            case CameraState.PointOfInterest:
                cameraDestination = behaviourPointOfInterest.GetCameraDestination();
                break;
        }

        // 3. FST determines how fast camera reaches target
        CalcFollowSmoothTimeX();
        CalcFollowSmoothTimeY();

        // 4. Calculate camera position for next frame
        Vector2 newPos = GetNewCameraPosition(cameraDestination);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    public Vector2 GetPlayerOffset()
    {
        Vector2 newOffset = cameraOffset;

        if (Player != null)
        {
            if (PlayerScript.HasXMovementInput() && !PlayerScript.InLockedState())
            {
                newOffset = playerWalkOffset;
            }
            else
            {
                newOffset = playerBaseOffset;
            }

            if (PlayerScript.GetFacingDirection() != 1)
            {
                newOffset.x = -newOffset.x;
            }
        }

        return newOffset;
    }

    public Vector2 GetNewCameraPosition(Vector2 target)
    {
        // smoothdamp adjustments - if player is moving move the target farther away, making it spring faster
        // effect - camera sticks closer to the player on x axis
        Vector2 finalPos;

        if (cameraState == CameraState.PointOfInterest)
        {
            finalPos.x = Mathf.Lerp(transform.position.x, target.x, behaviourPointOfInterest.GetPoiTValue());
            finalPos.y = Mathf.Lerp(transform.position.y, target.y, behaviourPointOfInterest.GetPoiTValue());
        }
        else
        {
            finalPos.x = Mathf.SmoothDamp(transform.position.x, target.x, ref followVelocity.x, followSmoothTime.x);
            finalPos.y = Mathf.SmoothDamp(transform.position.y, target.y, ref followVelocity.y, followSmoothTime.y);
        }

        return finalPos;
    }

    private void CalcFollowSmoothTimeX()
    {
        // increase FSTX right after leaving or entering so it doesnt snap
        // but we dont want this increase while players in lane either so we need a flag to know if the regionfstx has been applied 

        // i dont like this. Me neither.
        switch (IsInVertRegionX(), hasEnteredVertRegion)
        {
            // reach player slower when player exits poi
            case (false, true):  // leaving region
                followSmoothTime.x = behaviourHorizontalLane.GetRegionFST().x;
                hasEnteredVertRegion = false;
                break;
            case (true, false):  // entering region
                followSmoothTime.x = behaviourHorizontalLane.GetRegionFST().x;
                hasEnteredVertRegion = true;
                break;
        }

        // still applies regardless of state
        if (followSmoothTime.x > GetDefaultFST().x)
        {
            followSmoothTime.x = Mathf.Lerp(followSmoothTime.x, GetDefaultFST().x, lerpFST.x);
        }
        else
        {
            followSmoothTime.x = GetDefaultFST().x;
        }
    }

    private void CalcFollowSmoothTimeY()
    {
        // lerp smooth time when exiting region so camera doesnt instasnap back to player - 
        // gradually increases rubber band strength

        if (IsInVertRegionY())
        {
            // increase smooth time when entering specifically horizontal or pois to not instasnap to region
            followSmoothTime.y = behaviourHorizontalLane.GetRegionFST().y;
            return;
        }

        if (followSmoothTime.y > GetDefaultFST().y)
        {
            if (cameraState != CameraState.HorizontalLane)
            {
                // tighten band when exiting regions
                followSmoothTime.y = Mathf.Lerp(followSmoothTime.y, GetDefaultFST().y, lerpFST.y);
            }
            return;
        }


        // but, if falling overwrite above - tighten the band even more
        // if (player.GetComponent<PlayerController>().IsFalling()) {
        //     // smooths both entering and leaving falls
        //     if (followSmoothTimeY > fallingFSTimeY) {
        //         followSmoothTimeY = Mathf.Lerp(followSmoothTimeY, fallingFSTimeY, lerpFallingFSTY);
        //     }
        //     if (followSmoothTimeY < fallingFSTimeY) {
        //         followSmoothTimeY = Mathf.Lerp(followSmoothTimeY, fallingFSTimeY, lerpFallingFSTY);
        //     }
        // }

        followSmoothTime.y = GetDefaultFST().y;
    }

    public Vector2 GetCameraDestination(Vector2 newOffset)
    {
        Vector2 target = newOffset;

        if (Player != null)
        {
            target.x = Player.transform.position.x + newOffset.x;
            target.y = Player.transform.position.y + newOffset.y;
        }

        return target;
    }

    private bool IsInVertRegionX()
    {
        switch (cameraState)
        {
            case CameraState.VerticalLane:
            case CameraState.PointOfInterest:
                return true;
        }
        return false;
    }

    private bool IsInVertRegionY()
    {
        switch (cameraState)
        {
            case CameraState.VerticalLane:
            case CameraState.FollowPlayer:
                return false;
        }
        return true;
    }

    public Vector2 GetDefaultFST()
    {
        return defaultFST;
    }

    private void UpdateBackgroundTint(float timeOfDay)
    {
        if (!cachedCamera) return;

        float dawnStart = DaytimeManager.Instance.GetDawnStartTime();
        float dawnEnd = DaytimeManager.Instance.GetDawnEndTime();
        float duskStart = DaytimeManager.Instance.GetDuskStartTime();
        float duskEnd = DaytimeManager.Instance.GetDuskEndTime();

        if (timeOfDay >= dawnStart && timeOfDay < cachedDawnMid)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - dawnStart) / (cachedDawnMid - dawnStart));
            cachedCamera.backgroundColor = Color.Lerp(backgroundColorMidnight, backgroundColorDawnAndDusk, lerpFactor);
        }
        else if (timeOfDay >= cachedDawnMid && timeOfDay < dawnEnd)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - cachedDawnMid) / (dawnEnd - cachedDawnMid));
            cachedCamera.backgroundColor = Color.Lerp(backgroundColorDawnAndDusk, backgroundColorMidday, lerpFactor);
        }
        else if (timeOfDay >= duskStart && timeOfDay < cachedDuskMid)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - duskStart) / (cachedDuskMid - duskStart));
            cachedCamera.backgroundColor = Color.Lerp(backgroundColorMidday, backgroundColorDawnAndDusk, lerpFactor);
        }
        else if (timeOfDay >= cachedDuskMid && timeOfDay < duskEnd)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - cachedDuskMid) / (duskEnd - cachedDuskMid));
            cachedCamera.backgroundColor = Color.Lerp(backgroundColorDawnAndDusk, backgroundColorMidnight, lerpFactor);
        }
        else
        {
            cachedCamera.backgroundColor = (timeOfDay < dawnStart || timeOfDay >= duskEnd) ? backgroundColorMidnight : backgroundColorMidday;
        }
    }
}
