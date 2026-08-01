using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private List<CollectibleSO> lootTable = new List<CollectibleSO>();
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private float launchForce = 4;

    private PlayerInput playerInput;
    private bool isOpened;

    //Persistence
    private WorldState worldState;
    private PersistentGuid guid;

    private void Awake() => guid = GetComponent<PersistentGuid>();

    private void Start()
    {
        //Persistence
        worldState = ServiceLocator.Get<WorldState>();
        if (worldState.openedChests.Contains(guid.Guid))
        {
            isOpened = true;
            anim.Play("ChestOpenIdle");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInput>(out var input))
        {
            playerInput = input;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInput>(out var input))
        {
            if (input == playerInput)
            {
                playerInput = null;
            }
        }
    }

    private void Update()
    {
        if (isOpened || playerInput == null)
        {
            return;
        }

        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        if (moveInput.y > 0.1f)
        {
            StartCoroutine(OpenChestRoutine());
        }

        if (playerInput.actions["Interact"].WasPressedThisFrame())
        {
            StartCoroutine(OpenChestRoutine());
        }
    }

    private IEnumerator OpenChestRoutine()
    {
        if (isOpened)
        {
            yield break;
        }

        isOpened = true;
        worldState.openedChests.Add(guid.Guid);
        anim.Play("ChestOpen");

        yield return new WaitForSeconds(spawnDelay);

        foreach (CollectibleSO loot in lootTable)
        {
            Loot newLoot = Instantiate(lootPrefab, transform.position, Quaternion.identity).GetComponent<Loot>();
            newLoot.Initialize(loot);


            Rigidbody2D rb = newLoot.GetComponent<Rigidbody2D>();

            Vector2 direction = new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(0.5f, 1)).normalized;
            rb.AddForce(direction * launchForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
