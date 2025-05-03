using System.Collections;
using UnityEngine;
public class Tinkerller : MonoBehaviour
{
    [SerializeField] GameObject kart;
    Movement kartic;
    MPack.MArray posxs, posys, poszs;
    MPack.MArray rotws, rotxs, rotys, rotzs;
    Animator anim;
    int i;
    
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private float stuckTimer = 0f;
    private const float STUCK_THRESHOLD = 5.0f;
    private const float POSITION_THRESHOLD = 0.1f;
    
    void Start()
    {
        kartic = kart.GetComponent<Movement>();
        posxs = kartic.dict["posxs"] as MPack.MArray;
        posys = kartic.dict["posys"] as MPack.MArray;
        poszs = kartic.dict["poszs"] as MPack.MArray;
        rotws = kartic.dict["rotws"] as MPack.MArray;
        rotxs = kartic.dict["rotxs"] as MPack.MArray;
        rotys = kartic.dict["rotys"] as MPack.MArray;
        rotzs = kartic.dict["rotzs"] as MPack.MArray;
        anim = GetComponent<Animator>();
        
        lastPosition = transform.position;
        currentPosition = transform.position;
    }
    
    void SetGhost()
    {
        lastPosition = currentPosition;
        
        currentPosition = new Vector3(
            posxs[i].To<float>(),
            posys[i].To<float>(),
            poszs[i].To<float>()
        );
        
        transform.SetPositionAndRotation(
            currentPosition,
            new Quaternion(
                rotxs[i].To<float>(),
                rotys[i].To<float>(),
                rotzs[i].To<float>(),
                rotws[i].To<float>()
            )
        );
        
        transform.Rotate(-90f, 0, 0);
        
        if (++i >= posxs.Count)
        {
            anim.enabled = false;
            i = 0;
        }
        
        CheckSamePosition();
    }
    
    private void CheckSamePosition()
    {
        if (Vector3.Distance(currentPosition, lastPosition) < POSITION_THRESHOLD && i < posxs.Count)
        {
            Debug.Log("Position similar to previous one. Jumping to next position.");
            SetGhost();
        }
    }
    
    public void JumpStart()
    {
        anim.enabled = true;
        i = 0;
        stuckTimer = 0f;
        lastPosition = transform.position;
        currentPosition = transform.position;
        SetGhost();
    }
    
    private void FixedUpdate()
    {
        if (anim.enabled)
        {
            CheckIfTrapped();
            SetGhost();
        }
    }
    
    private void CheckIfTrapped()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        
        if (distanceMoved < POSITION_THRESHOLD)
        {
            stuckTimer += Time.fixedDeltaTime;
            
            if (stuckTimer >= STUCK_THRESHOLD)
            {
                Debug.Log("Tinkerller was trapped for " + stuckTimer + " seconds. Moving to next position.");
                stuckTimer = 0f;
                SetGhost();
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }
}
