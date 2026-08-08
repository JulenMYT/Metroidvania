    using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables
    public int FacingDirection { get; private set; } = 1;

    public Transform CurrentTarget { get; set; }

    //Components
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public EnemyConfig Config;
    public Enemy_Senses Senses { get; private set; }
    public Enemy_Combat Combat { get; private set; }
    public StateMachine StateMachine { get; private set; }

    //Persistence
    private WorldState worldState;
    private PersistentGuid guid;
    private bool isDefeated;

    private void Awake()
    {
        guid = GetComponent<PersistentGuid>();

        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        StateMachine = new StateMachine();
        Senses = GetComponent<Enemy_Senses>();
        Combat = GetComponent<Enemy_Combat>();
    }

    private void Start()
    {
        //Persistence
        worldState = ServiceLocator.Get<WorldState>();
        SaveManager.OnSaveDataLoaded += InitializeFromSave; //So we initialize AFTER data has been loaded
        InitializeFromSave(); //So we always initialize when entering a room
        StateMachine.Initialize(new PatrolState(this));
    }


    private void OnDestroy()
    {
        SaveManager.OnSaveDataLoaded -= InitializeFromSave;
    }

    private void InitializeFromSave()
    {
        if (worldState.defeatedEnemies.Contains(guid.Guid))
        {
            Destroy(gameObject);
        }
    }

    private void Update() => StateMachine.CurrentState?.Update();

    private void FixedUpdate() => StateMachine.CurrentState?.FixedUpdate();
    public void OnAnimationFinished() => StateMachine.CurrentState?.OnAnimationFinished();
    public void FaceTarget(Transform target)
    {
        float offset = target.position.x - transform.position.x;

        int direction = offset > 0 ? 1 : -1;

        if (direction != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = FacingDirection;
        transform.localScale = scale;
    }

    public void Die()
    {
        if (isDefeated) //Make it idempotent
        {
            return;
        }

        worldState.defeatedEnemies.Add(guid.Guid);
    }
}
