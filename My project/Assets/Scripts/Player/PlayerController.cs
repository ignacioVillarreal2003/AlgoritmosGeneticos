using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public (int, int, int, int)[] genes { get; private set; } = null;
    private int currentGene = 0;
    private Rigidbody2D rb;
    [SerializeField] private int velocity = 5;
    private float moveTimer = 0f;
    private GeneticController geneticController;
    private float fitness = 0f;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    private int currentCheckpoint = 0;
    public bool isDead = false;
    
    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
        foreach(Checkpoint c in FindObjectsOfType<Checkpoint>())
        {
            checkpoints.Add(c);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkpoints.Sort((a, b) => a.getOrder().CompareTo(b.getOrder()));
    }

    void Update()
    {
        if (isDead || currentGene >= genes.Length) 
        {
            rb.velocity = Vector2.zero;
            return;
        }
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            if (currentGene < genes.Length && !isDead) {
                Move(genes[currentGene]);
                fitness = Vector3.Distance(transform.position, checkpoints[currentCheckpoint].transform.position);
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
        }

        if (other.gameObject.CompareTag("Checkpoint") && other.gameObject.GetComponent<Checkpoint>().getOrder() == currentCheckpoint)
        {
            if (currentCheckpoint < checkpoints.Count - 1)
            {
                currentCheckpoint ++;
                fitness *= 0.9f;
            }
            if (currentCheckpoint == checkpoints.Count - 1)
            {
                isDead = true;
            }
        }
    }

}