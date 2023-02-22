using UnityEngine;
using System.Collections;
using DG.Tweening;
using HomaGames.Internal.BaseTemplate;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    new Rigidbody rigidbody;
    [SerializeField]
    float moveExtents = 3;
    [SerializeField]
    float runSpeed = 10;
    [SerializeField]
    float sideSpeed = 2;

    int currLane = 0;
    Animator animator;

    private void Awake()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
        //animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        OnCharacterChanged(Store.Instance.StoreItems[0]);
        Store.Instance.OnItemSelected += OnCharacterChanged;
    }

    private void OnDestroy()
    {
        if (Store.Instance != null)
        {
            Store.Instance.OnItemSelected -= OnCharacterChanged;        
        }
    }

    public void OnCharacterChanged(StoreItem characterItem)
    {
        if (animator != null) {
            Destroy(animator.gameObject);
        }
        GameObject characterInstance = Instantiate(characterItem.Prefab, transform);
        animator = characterInstance.GetComponent<Animator>();
    }

    public void StartMoving()
    {       
        DOTween.To(
                () => rigidbody.velocity,
                value => rigidbody.velocity = value,
                new Vector3(0, 0, runSpeed),
                3);
    }

    public void StopMoving()
    {
        DOTween.To(
                () => rigidbody.velocity,
                value => rigidbody.velocity = value,
                new Vector3(0, 0, 0),
                0f);
    }

    public void Update()
    {
        animator.SetFloat("MoveSpeed", rigidbody.velocity.magnitude);
        GameManagerBase.Instance.Context.SetValue("Distance", transform.position.z);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        if (h != 0)
        {
            MoveLane(Mathf.CeilToInt(Mathf.Sign(h)));
        }
    }

    public void MoveLane(int sign)
    {
        currLane = Mathf.Clamp(currLane + sign, -1, 1);
        rigidbody.DOMoveX(1.5f * currLane, 0.5f, false);

            //= Vector3.MoveTowards(transform.position, new Vector3(moveExtents * , transform.position.y, transform.position.z), sideSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Die();
    }

    public void Die()
    {
        animator.SetBool("Dead", true);
        GameManagerBase.Instance.Context.AddToInt("Lives", -1);
        DOTween.CompleteAll();
        rigidbody.velocity = Vector3.zero;
    }

    public void ResetPosition()
    {
        DOTween.CompleteAll();
        currLane = 0;
        rigidbody.DOMoveX(0, 0.5f, false);
        animator.SetBool("Dead", false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rigidbody.velocity = Vector3.zero;
    }
}
