using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Cromosoma */
    private (int, int, int, int)[] chromosome;
    private int currentGene = 0;
    private float fitness = 0f;
    private float distanceToTarget = 0; 
    private bool isDead = false;
    private bool isFinish = false;

    /* Variables */
    private int velocity = 0;
    private float moveCooldown = 0f;

    /* Algoritmo */
    private GeneticController geneticController;

    /* Otros */
    private Rigidbody2D rb;
    private CheckpointsManager checkpointsManager;
    private HashSet<Checkpoint> visitedCheckpoints = new HashSet<Checkpoint>();
    
    /* Referencias a objetos */
    void Awake()
    {
        geneticController = FindAnyObjectByType<GeneticController>();
        checkpointsManager = FindAnyObjectByType<CheckpointsManager>();
    }

    /* Inicializa variables */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = geneticController.GetVelocity();
        moveCooldown = geneticController.GetMoveCooldown();
    }

    /* Manejador del movimiento del individuo */
    void FixedUpdate()
    {
        moveCooldown -= Time.deltaTime;
        if (moveCooldown <= 0 && currentGene == chromosome.Length - 1)
        {
            isDead = true;
        }
        if (moveCooldown <= 0 && !isDead && !isFinish)
        {
            Move(chromosome[currentGene]);
        }
    }
    
    /* Inicializa los genes de la primera generacion */
    public void InitializeGenes()
    {
        chromosome = new (int, int, int, int)[geneticController.GetChromosomeLength()];
        for (int i = 0; i < geneticController.GetChromosomeLength(); i++){
            int randomNumber = Random.Range(0, 4);
            if (randomNumber == 0) {
                chromosome[i] = (1, 0, 0, 0);
            } else if (randomNumber == 1) {
                chromosome[i] = (0, 1, 0, 0);
            } else if (randomNumber == 2) {
                chromosome[i] = (0, 0, 1, 0);
            } else {
                chromosome[i] = (0, 0, 0, 1);
            }
        }
    }

    /* Movimiento del individuo */
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
        moveCooldown = geneticController.GetMoveCooldown();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        /* Interaccion del individuo con los obstaculos */
        if (other.gameObject.CompareTag("Obstacle"))
        {
            isDead = true;
            Destroy(rb);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /* Interaccion del individuo con el checkpoint */
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            Checkpoint checkpoint = other.gameObject.GetComponent<Checkpoint>();
            if (checkpointsManager.IsTheEnd(checkpoint))
            {
                isFinish = true;
            }
            else if (!visitedCheckpoints.Contains(checkpoint))
            {
                visitedCheckpoints.Add(checkpoint);
            }
        }
    }

    public (int, int, int, int)[] GetChromosome() => chromosome;
    public void SetChromosome((int, int, int, int)[] newGenes) => chromosome = newGenes;
    public bool GetIsDead() => isDead;
    public bool GetIsFinish() => isFinish;
    public void SetDistanceToTarget(float distance) => distanceToTarget = distance;
    public float GetDistanceToTarget() => distanceToTarget;
    public void SetFitness(float value) => fitness = value;
    public float GetFitness() => fitness;
    public int GetVisitedCheckpoints() => visitedCheckpoints.Count;
}
