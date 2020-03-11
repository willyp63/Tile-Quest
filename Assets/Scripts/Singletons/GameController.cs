using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Unit selectedUnit;
    TargetableAllyAbility selectedAbility;

    List<MoveNode> moveIndicatorNodes = new List<MoveNode>();
    List<Vector2Int> attackIndicatorPoss = new List<Vector2Int>();
    List<Vector2Int> abilityTargetIndicatorPoss = new List<Vector2Int>();

    GameBoard board;
    IndicatorManager indicatorManager;
    UIManager uiManager;

    Color selectIndicatorColor = new Color32(255, 0, 255, 200);
    Color moveIndicatorColor = new Color32(0, 255, 255, 200);
    Color attackIndicatorColor = new Color32(255, 100, 0, 200);
    Color abilityTargetIndicatorColor = new Color32(255, 255, 0, 200);

    bool canMakeMove = true;

    int playerMana = 12;

    private void Start()
    {
        board = FindObjectOfType<GameBoard>();
        indicatorManager = FindObjectOfType<IndicatorManager>();
        uiManager = FindObjectOfType<UIManager>();

        uiManager.Deselect();
        uiManager.SetMana(playerMana);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (!canMakeMove) { return; }

            // ignore if touch already went through a UI item
            if (EventSystem.current.IsPointerOverGameObject() ||
                EventSystem.current.currentSelectedGameObject != null) {
                    return;
            }

            // convert mouse pos to grid pos
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            var gridPos = GridUtils.GetGridPos(worldPos);

            // LEAVE THIS its very helpful for placing things on grid
            Debug.Log(gridPos);

            // if: click was on an ability target indicator
            foreach (var indicatorPos in abilityTargetIndicatorPoss)
            {
                if (indicatorPos == gridPos)
                {
                    StartCoroutine(ActivateAbility(selectedAbility, board.GetUnitAt(gridPos)));
                    return;
                }
            }

            // if: click was on an attack indicator
            foreach (var indicatorPos in attackIndicatorPoss)
            {
                if (indicatorPos == gridPos)
                {
                    StartCoroutine(Attack(board.GetUnitAt(gridPos) as Enemy));
                    return;
                }
            }

            // else if: click was on a move indicator
            foreach (var move in moveIndicatorNodes)
            {
                if (move.pos == gridPos)
                {
                    StartCoroutine(Move(move));
                    return;
                }
            }

            // else if: click was on a unit
            var unit = board.GetUnitAt(gridPos);
            if (unit != null)
            {
                SelectUnit(unit);
                return;
            }

            // else:
            Deselect();
        }
    }

    public void OnAbility(AllyAbility ability)
    {
        var selectedAlly = selectedUnit as Ally;

        var targetableAbility = ability as TargetableAllyAbility;
        if (targetableAbility != null)
        {
            SelectAbility(targetableAbility);
        } else
        {
            StartCoroutine(ActivateAbility(ability, null));
        }
    }

    private IEnumerator ActivateAbility(AllyAbility ability, Unit target)
    {   
        var selectedAlly = selectedUnit as Ally;

        if (selectedAlly) {
            canMakeMove = false;

            yield return StartCoroutine(selectedAlly.UseAbility(ability, target));

            playerMana -= ability.GetCost();
            uiManager.SetMana(playerMana);

            CheckMissionStatus();

            SelectUnit(selectedUnit);

            canMakeMove = true;
        }
    }

    public void OnPlayerEndTurn()
    {
        Deselect();
        StartCoroutine(AfterAllyTurn());
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SelectUnit(Unit unit)
    {
        selectedUnit = unit;

        uiManager.SelectedUnit(selectedUnit, playerMana);

        ClearIndicators();

        // add selection indicator
        Vector2Int[] selectIndicatorPoss = { unit.GetPosition() };
        indicatorManager.AddIndicators(selectIndicatorPoss, selectIndicatorColor, true);

        var selectedAlly = selectedUnit as Ally;
        if (selectedAlly != null)
        {
            // add move indicators
            if (selectedAlly.GetNumMovesLeft() > 0)
            {
                moveIndicatorNodes.Clear();
                var indicatorPoss = new List<Vector2Int>();
                foreach (var move in board.GetWalkablePossInRange(selectedUnit.GetPosition(), selectedUnit.GetMoveRange())) {
                    moveIndicatorNodes.Add(move);
                    indicatorPoss.Add(move.pos);
                }
                indicatorManager.AddIndicators(indicatorPoss, moveIndicatorColor);
            }

            // add attack indicators
            if (selectedAlly.GetNumAttacksLeft() > 0)
            {
                attackIndicatorPoss.Clear();
                foreach (var posNode in board.GetPossWithEnemyInRange(selectedUnit.GetPosition(), selectedUnit.GetAttackRange())) {
                    attackIndicatorPoss.Add(posNode.pos);
                }
                indicatorManager.AddIndicators(attackIndicatorPoss, attackIndicatorColor, true);
            }
        }
    }

    private void SelectAbility(TargetableAllyAbility ability)
    {
        selectedAbility = ability;

        ClearIndicators();

        // add target indicators
        if (ability.CanTargetAllies())
        {
            var targetIndicatorPoss = new List<Vector2Int>();
            foreach (var posNode in board.GetPossWithAllyInRange(selectedUnit.GetPosition(), ability.GetRange())) {
                targetIndicatorPoss.Add(posNode.pos);
            }
            indicatorManager.AddIndicators(targetIndicatorPoss, abilityTargetIndicatorColor, true);

            foreach (var pos in targetIndicatorPoss)
            {
                abilityTargetIndicatorPoss.Add(pos);
            }
        }

        if (ability.CanTargetEnemies())
        {
            var targetIndicatorPoss = new List<Vector2Int>();
            foreach (var posNode in board.GetPossWithEnemyInRange(selectedUnit.GetPosition(), ability.GetRange())) {
                targetIndicatorPoss.Add(posNode.pos);
            }
            indicatorManager.AddIndicators(targetIndicatorPoss, abilityTargetIndicatorColor, true);

            foreach (var pos in targetIndicatorPoss)
            {
                abilityTargetIndicatorPoss.Add(pos);
            }
        }

        if (ability.CanTargetSelf()) {
            var targetIndicatorPoss = new List<Vector2Int>();
            targetIndicatorPoss.Add(selectedUnit.position);
            abilityTargetIndicatorPoss.Add(selectedUnit.position);
            indicatorManager.AddIndicators(targetIndicatorPoss, abilityTargetIndicatorColor, true);
        }
    }

    private void Deselect()
    {
        selectedUnit = null;

        ClearIndicators();
        uiManager.Deselect();
    }

    private void ClearIndicators()
    {
        moveIndicatorNodes.Clear();
        attackIndicatorPoss.Clear();
        abilityTargetIndicatorPoss.Clear();

        indicatorManager.ClearIndicators();
    }

    private IEnumerator Move(MoveNode move)
    {
        canMakeMove = false;

        var selectedAlly = selectedUnit as Ally;
        
        Deselect();

        yield return StartCoroutine(board.MoveUnit(selectedAlly, move));

        // decrement num moves left
        selectedAlly.SetNumMovesLeft(selectedAlly.GetNumMovesLeft() - 1);

        CheckMissionStatus();

        SelectUnit(selectedAlly);

        canMakeMove = true;
    }

    private IEnumerator Attack(Enemy target)
    {
        canMakeMove = false;

        var selectedAlly = selectedUnit as Ally;

        Deselect();

        yield return StartCoroutine(selectedAlly.Attack(target));

        board.RemoveDeadUnits();

        // decrement num attacks left
        selectedAlly.SetNumAttacksLeft(selectedAlly.GetNumAttacksLeft() - 1);

        CheckMissionStatus();

        SelectUnit(selectedAlly);

        canMakeMove = true;
    }

    private IEnumerator AfterAllyTurn()
    {
        canMakeMove = false;

        foreach (var ally in board.GetAllies())
        {
            ally.AfterTurn();
        }

        foreach (var enemy in board.GetEnemies())
        {
            enemy.BeforeTurn();
        }

        yield return StartCoroutine(TakeEnemyTurn());

        foreach (var enemy in board.GetEnemies())
        {
            enemy.AfterTurn();
        }

        foreach (var enemySpawner in board.GetEnemySpawners())
        {
            enemySpawner.AfterTurn();
        }

        if (board.HasMissionFailed()) {
            FindObjectOfType<SceneController>().LoadMissionFailedScene();
        } else if (board.IsMissionComplete()) {
            FindObjectOfType<SceneController>().LoadMissionCompleteScene();
        }

        foreach (var ally in board.GetAllies())
        {
            ally.BeforeTurn();
        }

        canMakeMove = true;
    }

    private IEnumerator TakeEnemyTurn()
    {
        var enemies = board.GetEnemies();

        ListUtils.Shuffle(enemies);

        foreach (var enemy in enemies)
        {
            yield return StartCoroutine(enemy.TakeTurn(board));
        }
    }

    private void CheckMissionStatus() {
        if (board.HasMissionFailed()) {
            FindObjectOfType<SceneController>().LoadMissionFailedScene();
        } else if (board.IsMissionComplete()) {
            FindObjectOfType<SceneController>().LoadMissionCompleteScene();
        }
    }
}
