using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TrashFishBehaviour : MonoBehaviour
{
    [SerializeField]
    float range;
    [SerializeField]
    float maxDistance;
    // Ya no se usa (captcha eliminado). Se deja para no romper la referencia del Inspector.
    [SerializeField] private GameObject _captcha;

    public float speed = 2f;

    public Animator animator;

    Vector2 wayPoint;
    private Vector2 origPos, targetPos;
    private bool isEating;
    private bool canEat;
    private bool canMove;

    private string CurrentAnimaton;

    private float maxDistanceAudio = 11f;

    private AudioSource audioSource;

    public static TrashFishBehaviour Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        origPos = transform.position;
        SetNewDestination();
        targetPos = wayPoint;
        if (targetPos.x > origPos.x)
        {
            ChangeAnimation("TrashFishMoveRight");
        }
        if (targetPos.x < origPos.x)
        {
            ChangeAnimation("TrashFishMoveLeft");
        }
        isEating = false;
        canEat = false;
        canMove = true;
    }

    void Update()
    {
        // Evita errores al cambiar de escena
        if (MermaidMovement.Instance == null) return;

        float distance = Vector3.Distance(transform.position, MermaidMovement.Instance.gameObject.transform.position);
        float volume = 0f;

        if (distance <= maxDistanceAudio)
        {
            volume = Mathf.InverseLerp(maxDistanceAudio, 0f, distance);
        }

        audioSource.volume = volume;

        // F: dar la basura al pez directamente (sin captcha)
        if (Input.GetKeyDown(KeyCode.F) && !isEating && canEat && MermaidMovement.Instance.hasTrash)
        {
            canEat = false;
            canMove = false;
            if (PickUpText.Instance != null)
            {
                PickUpText.Instance.gameObject.SetActive(false);
            }
            StartEating();
        }

        if (isEating == false && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, wayPoint) < range)
            {
                origPos = wayPoint;
                SetNewDestination();
                targetPos = wayPoint;
                if (targetPos.x > origPos.x)
                {
                    ChangeAnimation("TrashFishMoveRight");
                }
                if (targetPos.x < origPos.x)
                {
                    ChangeAnimation("TrashFishMoveLeft");
                }
            }
        }

        if (isEating == true)
        {
            if (CurrentAnimaton == "TrashFishMoveRight")
            {
                ChangeAnimation("TrashFishEatRight");
            }
            if (CurrentAnimaton == "TrashFishMoveLeft")
            {
                ChangeAnimation("TrashFishEatLeft");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MermaidMovement.Instance == null) return;

        if (collision.gameObject.tag == "Mermaid" && MermaidMovement.Instance.hasTrash && !isEating)
        {
            if (PickUpText.Instance != null)
            {
                PickUpText.Instance.gameObject.SetActive(true);
                PickUpText.Instance.text.text = "Pulsa F para dar la basura al pez";
            }
            canEat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mermaid")
        {
            if (PickUpText.Instance != null)
            {
                PickUpText.Instance.gameObject.SetActive(false);
            }
            canEat = false;
        }
    }

    public void StartEating()
    {
        StartCoroutine(Eating());
    }
    public IEnumerator Eating()
    {
        isEating = true;
        Destroy(MermaidMovement.Instance.trash.gameObject);
        MermaidMovement.Instance.hasTrash = false;
        MermaidMovement.Instance.trash = null;
        yield return new WaitForSeconds(10f);
        isEating = false;
        wayPoint = transform.position;
        TrashTextScript.TrashItemCount++;
        // Recompensa: alimentar al pez cura al buzo
        if (PlayerHealth.Instance != null) PlayerHealth.Instance.Heal(15f);
        canMove = true;
    }

    void ChangeAnimation(string animation)
    {
        if (CurrentAnimaton == animation) return;

        animator.Play(animation);
        CurrentAnimaton = animation;
    }

    void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }
}