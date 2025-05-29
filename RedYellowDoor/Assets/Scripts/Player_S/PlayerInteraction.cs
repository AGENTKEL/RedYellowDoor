using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public GameObject interactionText;
    private Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            var interactable = hit.transform.GetComponent<Interactable>();

            if (interactable)
            {
                interactionText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    inventory.AddItem(interactable);
                }
            }
            else
            {
                interactionText.SetActive(false);
            }
        }
        else
        {
            interactionText.SetActive(false);
        }
    }
}
