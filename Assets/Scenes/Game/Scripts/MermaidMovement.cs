using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermaidMovement : MonoBehaviour
{
    public static MermaidMovement Instance;

    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Animator animator;

    Vector2 movement;

    [SerializeField] private GameObject _trashIcon;
    public GameObject trash;
    public bool hasTrash;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hasTrash = false;
        trash = null;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (hasTrash)
        {
            _trashIcon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ThrowTrash();
            }
        }
        else
        {
            _trashIcon.SetActive(false);
        }

    }

    public void ThrowTrash()
    {
        trash.gameObject.SetActive(true);
        trash.gameObject.transform.position = transform.position + Vector3.left;
        hasTrash = false;
        trash = null;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
