using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject inventory, description, baitMenu, baitDesc;

    [Header("Bucket Settings")]
    [SerializeField] private bool isBucket;
    [SerializeField] private GameObject particle;
    [SerializeField] private Transform point;

    private Animator anim;
    private AudioSource noise;
    private BoxCollider2D box;
    private World world;

    private bool canPlayHoverEffect = true;

    void Start()
    {
        // Cache the World script once. Finding it every single click is terrible for performance!
        world = FindAnyObjectByType<World>();
        box = GetComponent<BoxCollider2D>();

        if (isBucket)
        {
            anim = GetComponent<Animator>();
            noise = GetComponent<AudioSource>();
            if (point == null) point = transform;
        }
    }

    void Update()
    {
        if (isBucket)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (box.OverlapPoint(mousePos))
            {
                // FIXED: Your old "good" boolean was instantly setting itself back to false, 
                // causing the animation, particles, and sound to spam every single frame the mouse hovered.
                if (canPlayHoverEffect)
                {
                    canPlayHoverEffect = false;
                    anim.SetTrigger("moved");

                    if (particle != null) Instantiate(particle, point.position, Quaternion.identity);
                    if (noise != null) noise.Play();
                }
            }
            else
            {
                // Reset the effect only when the mouse completely leaves the object
                canPlayHoverEffect = true;
            }
        }
    }

    private void OnMouseDown()
    {
        bool isInventory = gameObject.CompareTag("Inventory");
        bool isBaitMenu = gameObject.CompareTag("BaitMenu");

        // Check if the menu this specific object controls is currently open
        bool menuIsAlreadyOpen = (isInventory && inventory.activeSelf) || (isBaitMenu && baitMenu.activeSelf);

        // Block the click IF the mouse is over a UI element AND the menu isn't already open
        if (!menuIsAlreadyOpen && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Pass the target menu first, and the opposite menu second so it knows what to close
        if (isInventory)
        {
            ToggleMenu(inventory, description, baitMenu, baitDesc);
        }
        else if (isBaitMenu)
        {
            ToggleMenu(baitMenu, baitDesc, inventory, description);
        }
    }

    // Updated to accept the "other" menu and description
    private void ToggleMenu(GameObject menuToToggle, GameObject descToClose, GameObject otherMenu, GameObject otherDesc)
    {
        bool isOpening = !menuToToggle.activeSelf;

        if (isOpening)
        {
            // Force close the opposite menu so they cannot be open at the same time
            otherMenu.SetActive(false);
            otherDesc.SetActive(false);
        }

        menuToToggle.SetActive(isOpening);
        descToClose.SetActive(false);

        world.menued = isOpening;
        world.inGame = isOpening;
    }

    // Helper method to keep your code clean and prevent writing the exact same logic twice
    private void ToggleMenu(GameObject menuToToggle, GameObject descToClose)
    {
        bool isOpening = !menuToToggle.activeSelf;

        menuToToggle.SetActive(isOpening);
        descToClose.SetActive(false);

        world.menued = isOpening;
        world.inGame = isOpening;
    }
}