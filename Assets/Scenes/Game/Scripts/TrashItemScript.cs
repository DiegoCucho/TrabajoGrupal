using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashItemScript : MonoBehaviour
{
    [SerializeField]
    float range;
    [SerializeField]
    float maxDistance;

    public float speed = 0.3f;

    private bool pickUpAllowed;

    Vector2 wayPoint;

    void Start()
    {
        SetNewDestination();
        if (PickUpText.Instance != null)
        {
            PickUpText.Instance.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }

        if (MermaidMovement.Instance != null && pickUpAllowed && Input.GetKeyDown(KeyCode.E) && MermaidMovement.Instance.trash == null)
        {
            PickUp();
        }
    }

    void PickUp()
    {
        MermaidMovement.Instance.hasTrash = true;
        MermaidMovement.Instance.trash = this.gameObject;
        this.gameObject.SetActive(false);
    }

    void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MermaidMovement.Instance == null) return;

        if (collision.gameObject.tag == "Mermaid" && !MermaidMovement.Instance.hasTrash)
        {
            if (PickUpText.Instance != null)
            {
                PickUpText.Instance.gameObject.SetActive(true);
                PickUpText.Instance.text.text = "Pulsa E para recoger";
            }
            pickUpAllowed = true;
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
            pickUpAllowed = false;
        }
    }
}