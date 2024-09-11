using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public (int, int, int, int)[] genes { get; private set; } = null;
    private int currentGene = 0;
    private Rigidbody2D rb;
    private int velocity = 0;
    private float moveTimer = 0f;
    private GeneticController geneticController;
    private float fitness = 0f;
    private Checkpoint checkpoint;
    public bool isDead = false;
    public bool isFinish = false;
    private CheckpointsManager checkpointsManager;
    
    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
        checkpointsManager = FindAnyObjectByType<CheckpointsManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = geneticController.getVelocity();
    }

    void Update()
    {
        if (rb) 
        {
            if (isFinish || currentGene >= genes.Length) 
            {
                rb.velocity = Vector2.zero;
                return;
            }
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0 && currentGene < genes.Length && !isDead && !isFinish && checkpoint)
            {
                Move(genes[currentGene]);
                fitness = Vector3.Distance(transform.position, checkpoint.transform.position);
            }
        }
    }
    
    public void InitializeGenes()
    {
        genes = new (int, int, int, int)[geneticController.getAdnLength()];
        for (int i = 0; i < geneticController.getAdnLength(); i++){
            int randomNumber = Random.Range(0, 4);
            if (randomNumber == 0) {
                genes[i] = (1, 0, 0, 0);
            } else if (randomNumber == 1) {
                genes[i] = (0, 1, 0, 0);
            } else if (randomNumber == 2) {
                genes[i] = (0, 0, 1, 0);
            } else {
                genes[i] = (0, 0, 0, 1);
            }
        }
    }

    public void InitializeGenes((int, int, int, int)[] newGenes)
    {
        genes = newGenes;
    }

    void Move((int, int, int, int) movement)
    {
        if (movement.Item1 == 1){
            rb.velocity = new Vector2(rb.velocity.x, velocity);
        } else if (movement.Item2 == 1){
            rb.velocity = new Vector2(velocity, rb.velocity.y);
        } else if (movement.Item3 == 1){
            rb.velocity = new Vector2(rb.velocity.x, velocity*-1);
        } else if (movement.Item4 == 1){
            rb.velocity = new Vector2(velocity*-1, rb.velocity.y);
        }
        currentGene++;
        moveTimer = geneticController.getMoveCooldown();
    }

    public float getFitness() => 1 / fitness;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            fitness *= 1.1f;
            isDead = true;
            Destroy(rb);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            fitness *= 0.9f;
            bool isFinalCheckpoint = checkpointsManager.NextCheckpoint();
            Destroy(other.gameObject);

            if (isFinalCheckpoint)
            {
                isFinish = true;
            }
        }
    }

    public void setCheckpoint(Checkpoint checkpoint) => this.checkpoint = checkpoint;
}
