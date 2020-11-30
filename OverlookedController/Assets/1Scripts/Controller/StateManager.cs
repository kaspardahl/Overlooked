using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KD
{
    public class StateManager : MonoBehaviour
    {
        [Header("Info")]
        public GameObject modelPrefab;
        public bool inGame;
        public bool isPlayer;

        [Header("Stats")]
        public float groundDistance = 0.6f;
        public float groundOffset = 0;
        public float distanceToCheckForward = 1.3f;
        public float runSpeed = 5;
        public float walkSpeed = 2;
        public float jumpForce = 4;
        public float airTimeThreshold = 0.8f;
        public float vaultOverHeight = 1.5f;
        public float vaultFloorHeightDifference = 0.3f;


        [Header("Inputs")]
        public float horizontal;
        public float vertical;
        public bool jumpInput;

        [Header("States")]
        public bool obstacleForward;
        public bool groundForward;
        public float groundAngle;
        public bool vaulting;

        #region StateRequests
        [Header("State Requests")]
        public CharStates curState;
        public bool onGround;
        public bool run; 
        public bool walk; 
        public bool onLocomotion;
        public bool inAngle_MoveDir;
        public bool jumping;
        public bool canJump;
        public bool canVault = true;  //sæt til false hvis det ikke skal være muligt at vaulte i spillet.   
        #endregion

        #region References
        GameObject activeModel;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rBody;
        [HideInInspector]
        public Collider controllerCollider; 
        #endregion

        #region Variables
        [HideInInspector]
        public Vector3 moveDirection;
        [HideInInspector]
        public Vector3 aimPosition;
        Transform aimHelper;
        float currentY;
        float currentZ;
        public float airTime;
        [HideInInspector]
        public bool prevGround;
        [HideInInspector]
        public Vector3 targetVaultPosition;
        [HideInInspector]
        public bool skipGroundCheck;

        public enum VaultType
        {
            idle, walk, run, walk_up, climb_up
        }

        [HideInInspector]
        public VaultType curVaultType;
        #endregion

        LayerMask ignoreLayers;

        public enum CharStates
        {
            idle,moving,onAir,hold,vaulting
        }

        #region Init Phase
        public void Init()
        {
            inGame = true;
            CreateModel();
            SetupAnimator();
            AddControllerReferences();
            canJump = true;

            gameObject.layer = 8;
            ignoreLayers = ~(1 << 3 | 1 << 8);

            controllerCollider = GetComponent<Collider>();
            if(controllerCollider == null)
            {
                Debug.Log("no collider found for the controller");
            }
        }

        void CreateModel()
        {
            activeModel = Instantiate(modelPrefab) as GameObject;
            activeModel.transform.parent = this.transform;
            activeModel.transform.localPosition = Vector3.zero;
            activeModel.transform.localEulerAngles = Vector3.zero;
            activeModel.transform.localScale = Vector3.one; 
        }

        void SetupAnimator()
        {
            anim = GetComponent<Animator>();
            Animator childAnim = activeModel.GetComponent<Animator>();
            anim.avatar = childAnim.avatar;
            Destroy(childAnim);
        }


        void AddControllerReferences()
        {
            gameObject.AddComponent<Rigidbody>();
            rBody = GetComponent<Rigidbody>();
            rBody.angularDrag = 999;
            rBody.drag = 4;
            rBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        }
        #endregion

         public void FixedTick()
        {
            // states that don't run the update in the state manager
            if (curState == CharStates.hold
                || curState == CharStates.vaulting)
                return;
            
            obstacleForward = false;
            groundForward = false;
            onGround = OnGround();

            if (onGround)
            {
                Vector3 origin = transform.position;
                //clear forward - checker om der er fri bane forude - hvis ikke stopper vi hvis ikke vinklen er op til 45 grader fra forward
                origin += Vector3.up * 0.75f;
                IsClear(origin, transform.forward, distanceToCheckForward, ref obstacleForward);
                if (!obstacleForward && !vaulting)
                {
                    // is ground forward? - checker om der er ground forude, hvis ja forbliver man OnGround()
                    origin += transform.forward * 0.6f;
                    IsClear(origin, -Vector3.up, groundDistance * 3, ref groundForward);
                }
                else
                {
                    if(Vector3.Angle(transform.forward,moveDirection) > 30) // hvis det objekt man støder på har en vinkel på mere end 30 grader, ses det som et obstacle. 
                    {
                        obstacleForward = false;
                    }
                }
            }

            UpdateState();
            MonitorAirTime();

        }

        public void RegularTick()
        {
            onGround = OnGround();
        }

        void UpdateState()
        {
            if (curState == CharStates.hold)
                return;

            if (vaulting)
            {
                curState = CharStates.vaulting;
                return;
            }

            if (horizontal != 0 || vertical != 0)
            {
                curState = CharStates.moving;
            }
            else
            {
                curState = CharStates.idle;
            }

            if (!onGround)
            {
                curState = CharStates.onAir;
            }
        }

        public bool OnGround()
        {
            if (skipGroundCheck)
                return false;
            
            bool r = false;

            if (curState == CharStates.hold)
                return false;

            Vector3 origin = transform.position + (Vector3.up * 0.55f); // 0.55 er circa samme højde som collideren

            RaycastHit hit = new RaycastHit();
            bool isHit = false;
            FindGround(origin, ref hit, ref isHit);

            if (!isHit)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 newOrigin = origin;

                    switch (i)
                    {
                        case 0: //forward
                            newOrigin += Vector3.forward / 3;
                            break;
                        case 1: //backwards
                            newOrigin -= Vector3.forward / 3;
                            break;
                        case 2: //left
                            newOrigin -= Vector3.right / 3;
                            break;
                        case 3: //right
                            newOrigin += Vector3.right / 3;
                            break;
                    }

                    FindGround(newOrigin, ref hit, ref isHit);

                    if (isHit == true)
                        break;
                }
            }
            r = isHit;

            if (r != false) // keeps the character afloat
            {
                Vector3 targetPosition = transform.position;
                targetPosition.y = hit.point.y + groundOffset;
                transform.position = targetPosition;
            }

            return r;
        }

        void FindGround(Vector3 origin, ref RaycastHit hit, ref bool isHit) // finds the ground by doing a raycast.
        {
            Debug.DrawRay(origin, -Vector3.up * 0.5f, Color.red);
            if (Physics.Raycast(origin, -Vector3.up, out hit, groundDistance, ignoreLayers))
            {
                isHit = true;
            }
        }

        void IsClear(Vector3 origin, Vector3 direction, float distance, ref bool isHit)
        {
            RaycastHit hit = new RaycastHit();
            float targetDistance = distance;
            if (run)
                targetDistance += 0.5f; // når man løber måler Raycastet længere fremme end når man går.

            int numberOfHits = 0;
            for (int i = -1; i < 2; i++) // man laver tre raycasts fremad for at være sikker at man ikke bare rammer kanten af et obstacle.
            {
                Vector3 targetOrigin = origin;
                targetOrigin += transform.right * (i * 0.3f);
                Debug.DrawRay(targetOrigin, direction * targetDistance, Color.green);
                if (Physics.Raycast(targetOrigin, direction, out hit, targetDistance, ignoreLayers))
                {
                    numberOfHits++;
                }
            }

            if (numberOfHits > 2)
            {
                isHit = true;
            }
            else
            {
                isHit = false;
            }

            if(obstacleForward)
            {
                Vector3 incomingVec = hit.point - origin;
                Vector3 reflectVect = Vector3.Reflect(incomingVec, hit.normal);
                float angle = Vector3.Angle(incomingVec, reflectVect);

                if (angle < 70)
                {
                    obstacleForward = false;
                }
                else
                {
                    if (numberOfHits > 2) // antal af raycasts forud der skal ramme et "vaultable object" for at kunne vaulte
                    {
                        bool willVault = false;
                        CanVaultOver(hit, ref willVault);

                        if (willVault)
                        {
                            curVaultType = VaultType.walk;
                            if (run)
                                curVaultType = VaultType.run;
                            obstacleForward = false;
                            return;
                        }
                        else
                        {
                            bool willClimb = false;

                            ClimbOver(hit, ref willClimb);

                            if (!willClimb)
                            {
                                obstacleForward = true;
                                return;
                            }
                        }
                    }
                }
            }

            if (groundForward)
            {
                if(curState == CharStates.moving)
                {
                    Vector3 p1 = transform.position;
                    Vector3 p2 = hit.point;
                    float diffy = p1.y - p2.y;
                    groundAngle = diffy;
                }

                float targetIncline = 0;

                if (Mathf.Abs(groundAngle) > 0.3f)
                {
                    if (groundAngle < 0)
                        targetIncline = 1;
                    else
                        targetIncline = -1;     
                }

                if (groundAngle == 0)
                    targetIncline = 0;

                anim.SetFloat(Statics.incline, targetIncline, 0.3f, Time.deltaTime);
            }
        }

        void CanVaultOver(RaycastHit hit, ref bool willVault)
        {
            if (!onLocomotion || !inAngle_MoveDir)
                return;

            //we hit a wall around knee height.
            //then we need to see if we can vault over it.
            Vector3 wallDirection = -hit.normal * 0.5f;
            //the opossite of the normal, is going to return us the direction.
            //if the whole level is set with box colliders, then tis will work like a charm.
            RaycastHit vHit;

            Vector3 wallOrigin = transform.position + Vector3.up * vaultOverHeight;
            Debug.DrawRay(wallOrigin, wallDirection * Statics.vaultCheckDistance, Color.red);
            
            if (Physics.Raycast(wallOrigin, wallDirection, out vHit, Statics.vaultCheckDistance, ignoreLayers))
            {
                //it's a wall
                willVault = false;
                return;
            }
            else
            {
                //it's not a wall, but can we vault over it?
                if (canVault && !vaulting)
                {
                    Vector3 startOrigin = hit.point;
                    startOrigin.y = transform.position.y;
                    Vector3 vOrigin = startOrigin + Vector3.up * vaultOverHeight;
                    if (!run)
                        vOrigin += wallDirection * Statics.vaultCheckDistance;
                    else
                        vOrigin += wallDirection * Statics.vaultCheckDistance_Run;

                    Debug.DrawRay(vOrigin, -Vector3.up * Statics.vaultCheckDistance);

                    if (Physics.Raycast(vOrigin, -Vector3.up, out vHit, Statics.vaultCheckDistance, ignoreLayers))
                    {
                        float hitY = vHit.point.y;
                        float diff = hitY - transform.position.y;

                        if(Mathf.Abs(diff) < vaultFloorHeightDifference)
                        {
                            vaulting = true;
                            targetVaultPosition = vHit.point;
                            willVault = true;
                            return;
                        }
                    }
                }
            }
        }

        void ClimbOver(RaycastHit hit, ref bool willClimb)
        {
            float targetDistance = distanceToCheckForward +0.1f;
            if (run)
                targetDistance += 0.5f;

            Vector3 ClimbCheckOrigin
                            = transform.position + (Vector3.up * Statics.walkUpHeight);
            RaycastHit climbHit;

            Vector3 wallDirection = -hit.normal * targetDistance;
            Debug.DrawRay(ClimbCheckOrigin, wallDirection, Color.yellow);
            if (Physics.Raycast(ClimbCheckOrigin, wallDirection, out climbHit, 1.2f, ignoreLayers))
            {
                // do nothing
            }
            else
            {
                Vector3 origin2 = hit.point;
                origin2.y = transform.position.y;
                origin2 += Vector3.up * Statics.walkUpHeight;
                origin2 += wallDirection * 0.2f;
                Debug.DrawRay(origin2, -Vector3.up, Color.yellow);
                if (Physics.Raycast(origin2, -Vector3.up, out climbHit, 1, ignoreLayers))
                {
                    float diff = climbHit.point.y - transform.position.y;
                    if (Mathf.Abs(diff) > Statics.walkUpThreshold)
                    {
                        vaulting = true;
                        targetVaultPosition = climbHit.point;
                        curVaultType = VaultType.walk_up;
                        obstacleForward = false;
                        willClimb = true;
                        skipGroundCheck = true;
                        return;
                    }
                }
            }
        }

        void MonitorAirTime()
        {
            if (!jumping)
                anim.SetBool(Statics.onAir, !onGround);

            if (onGround)
            {
                if (prevGround != onGround) // comming from the air towards the ground - landing animation.
                {
                    anim.SetInteger(Statics.jumpType,
                        (airTime > airTimeThreshold) ? // is airtime over the threshold = it is a long fall.
                        (horizontal != 0 || vertical != 0) ? 2 : 1 // 2 is roll 1 is hard landing.
                        : 0);
                }

                airTime = 0;
            }
            else
            {
                airTime += Time.deltaTime;
            }

            prevGround = onGround;
        }

        public void LegFront()
        {
            Vector3 ll = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position; //works only with humanoid characters.
            Vector3 rl = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;
            Vector3 rel_ll = transform.InverseTransformPoint(ll);
            Vector3 rel_rr = transform.InverseTransformPoint(rl);

            bool left = rel_ll.z > rel_rr.z;
            anim.SetBool(Statics.mirrorJump, left);
        }
    }
}
