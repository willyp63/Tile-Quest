using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AOE Damage Ability")]
public class AOEDamageAbility : TargetableAllyAbility
{
    public int damageAmount = 4;
    public int radius = 1;
    public GameObject effectPrefab;
    public float effectDuration = 3f;
    public bool isFriendlyFirePossible = false;

     public override void OnAnimStateActivate(Ally ally, Unit target, int animState)
    {
        if (animState == 2) {
            if (effectPrefab != null) {
                var effect = Instantiate(effectPrefab, GridUtils.GetWorldPos(target.position), Quaternion.identity);
                Destroy(effect, effectDuration);
            }
        } else if (animState == 3) {
            Damage(ally, target);
        }
    }

    private void Damage(Ally ally, Unit target) {
        target.Damage(damageAmount);

        var board = FindObjectOfType<GameBoard>();
        var targetPossInRange = isFriendlyFirePossible
          ? board.GetPossWithUnitInRange(target.GetPosition(), radius)
          : board.GetPossWithEnemyInRange(target.GetPosition(), radius);
        foreach (var posNode in targetPossInRange)
        {
            board.GetUnitAt(posNode.pos).Damage(damageAmount);
        }
    }
}
