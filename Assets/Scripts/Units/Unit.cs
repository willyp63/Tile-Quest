using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    public string unitName = "Unit X";

    public int maxHealth = 12;
    public int attackDamage = 4;
    public int attackRange = 1;
    public int moveRange = 4;

    public Vector2Int position;

    public GameObject damageIndicatorPrefab;
    public GameObject healIndicatorPrefab;

    public Transform statusIndicatorPos;

    public float moveSpeed = 4f;

    int health;

    // TODO: make status manager class
    List<Status> statuses = new List<Status>();
    List<int> statusRemainingDurations = new List<int>();
    List<GameObject> activeStatusIndicators = new List<GameObject>();

    int numMovesLeft = 1;
    int numAttacksLeft = 1;

    protected Animator anim;

    int attackAnimState = 0;

    bool isMoving = false;
    MoveNode nextMove;

    public string GetName()
    {
        return unitName;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetAttackDamage()
    {
        return attackDamage;
    }
    public int GetAttackRange()
    {
        return attackRange;
    }
    public int GetMoveRange()
    {
        return moveRange;
    }
    public Vector2Int GetPosition()
    {
        return position;
    }
    public List<Status> GetStatuses()
    {
        return statuses;
    }
    public List<int> GetStatusRemainingDurations()
    {
        return statusRemainingDurations;
    }

    public int GetNumMovesLeft()
    {
        return numMovesLeft;
    }
    public int GetNumAttacksLeft()
    {
        return numAttacksLeft;
    }

    public void SetNumAttacksLeft(int numAttacks)
    {
        numAttacksLeft = numAttacks;
    }
    public void SetNumMovesLeft(int numMoves)
    {
        numMovesLeft = numMoves;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
 
        var randomIdleStart = Random.Range(0,anim.GetCurrentAnimatorStateInfo(0).length); //Set a random part of the animation to start from
        anim.Play("Idle", 0, randomIdleStart);

        transform.position = GridUtils.GetWorldPos(position);

        health = maxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (isMoving) {
            float step =  moveSpeed * Time.deltaTime; // calculate distance to move
            var targetPos = GridUtils.GetWorldPos(nextMove.pos);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            float dist = Vector3.Distance(transform.position, targetPos);
            if (dist < Mathf.Epsilon) {
                nextMove = nextMove.parent;

                if (nextMove == null) {
                    isMoving = false;
                }
            }
        }
    }

    public IEnumerator Move(MoveNode moveNode)
    {
        // reverse the linked list :O
        MoveNode prev = null, curr = moveNode, next = null;
        while (curr != null) {
            next = curr.parent;

            curr.parent = prev;

            prev = curr;
            curr = next;
        }

        nextMove = prev; // skip first node cause that is where we currently are
        isMoving = true;

        yield return new WaitUntil(() => isMoving == false);

        this.position = moveNode.pos;
    }

    public IEnumerator Attack(Unit target)
    {
        if (anim) {
            anim.SetTrigger("onAttack");
            attackAnimState = 1;

            yield return new WaitUntil(() => attackAnimState == 2);

            target.Damage(attackDamage);

            yield return new WaitUntil(() => attackAnimState == 0);
        } else {
            target.Damage(attackDamage);
        }
    }

    public void OnAttackHitFrame() {
        attackAnimState = 2;
    }

    public void OnAttackEndFrame() {
        attackAnimState = 0;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        UpdateHealthBar();

        var damageIndicator = Instantiate(damageIndicatorPrefab, transform.position, Quaternion.identity);
        damageIndicator.GetComponentInChildren<Text>().text = damageAmount.ToString();
        Destroy(damageIndicator, 0.5f);

        if (health <= 0)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    public void Heal(int healingAmount)
    {
        var previousHealth = health;

        health += healingAmount;

        // dont let health go over max
        health = Mathf.Min(maxHealth, health);

        var healIndicator = Instantiate(healIndicatorPrefab, transform.position, Quaternion.identity);
        healIndicator.GetComponentInChildren<Text>().text = (health - previousHealth).ToString();
        Destroy(healIndicator, 0.5f);

        UpdateHealthBar();
    }

    public void AddStatus(Status status)
    {
        statuses.Add(status);
        statusRemainingDurations.Add(status.GetDuration());

        if (status.indicatorPrefab != null) {
            var statusIndicator = Instantiate(status.indicatorPrefab, statusIndicatorPos.position, Quaternion.identity);
            statusIndicator.transform.parent = statusIndicatorPos;
            activeStatusIndicators.Add(statusIndicator);
        } else {
            activeStatusIndicators.Add(null);
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    private void UpdateHealthBar()
    {
        var healthPercent = Mathf.Max(0, health / (float)maxHealth);
        transform.Find("Unit Base").Find("Health").localScale = new Vector3(healthPercent, healthPercent, 1);
    }

    public virtual void BeforeTurn() {
        // update statuses
        for (int i = 0; i < statuses.Count; i++)
        {
            statuses[i].BeforeTurn(this);
        }
    }

    public virtual void AfterTurn()
    {
        numMovesLeft = 1;
        numAttacksLeft = 1;

        // update statuses
        for (int i = 0; i < statuses.Count; i++)
        {
            statuses[i].AfterTurn(this);

            if (!statuses[i].HasInfiniteDuration())
            {
                statusRemainingDurations[i]--;

                if (statusRemainingDurations[i] <= 0) {
                    if (activeStatusIndicators[i] != null) { Destroy(activeStatusIndicators[i]); }

                    statuses.RemoveAt(i);
                    statusRemainingDurations.RemoveAt(i);
                    activeStatusIndicators.RemoveAt(i);

                    i--;
                }
            }
        }
    }
}
