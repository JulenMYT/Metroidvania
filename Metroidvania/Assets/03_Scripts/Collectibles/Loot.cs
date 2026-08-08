using System.Collections;
using TMPro;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private Player player;
    [SerializeField] private CollectibleSO collectibleSO;
    [SerializeField] private SpriteRenderer sr;

    public Animator anim;
    public TMP_Text itemMessage;

    [SerializeField] private bool canBeCollected = false;
    [SerializeField] private float collectDelay;

    //Persistence
    private PersistentGuid guid;
    private WorldState worldState;
    private bool isCollected = false;
    [SerializeField] private bool isPersistentLoot;

    private void Awake()
    {
        if (isPersistentLoot)
            guid = GetComponent<PersistentGuid>();
    }

    private void Start()
    {
        //Persistent Data
        worldState = ServiceLocator.Get<WorldState>();

        if (isPersistentLoot)
            SaveManager.OnSaveDataLoaded += InitializeFromSave;
        InitializeFromSave();
    }

    private void OnDestroy()
    {
        SaveManager.OnSaveDataLoaded -= InitializeFromSave;
    }

    private void InitializeFromSave()
    {
        if (isPersistentLoot)
        {
            if (worldState.collectedLoot.Contains(guid.Guid))
            {
                Destroy(gameObject);
            }
        }
    }

    public void Initialize(CollectibleSO collectibleSO)
    {
        this.collectibleSO = collectibleSO;
        sr.sprite = collectibleSO.itemSprite;

        StartCoroutine(EnableCollection());
    }

    private IEnumerator EnableCollection()
    {
        yield return new WaitForSeconds(collectDelay);

        canBeCollected = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();

        if (player == null || !canBeCollected)
            return;

        CollectItem();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void CollectItem()
    {
        if (isCollected)
            return;

        isCollected = true;
        if (isPersistentLoot)
            worldState.collectedLoot.Add(guid.Guid);
        itemMessage.text = "Found " + collectibleSO.itemName;
        anim.Play("CollectLoot");
        collectibleSO.Collect(player);
        Destroy(gameObject, 1);
    }
}
