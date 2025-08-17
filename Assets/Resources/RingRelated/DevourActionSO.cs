using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/DevourActionSO")]
public class DevourActionSO : BattleActionSO
{
    public override BattleAction GetAction(Character actor, Character target)
    {
        var action = base.GetAction(actor, target);

        action.AfterResolve = (ba) =>
        {
            if (ba?.Target == null || ba.Actor == null) return;

            // 若目标被此击杀死
            if (ba.Target.IsDead)
            {
                // 取最高属性（排除当前值会波动的 HP/MP，选 STR/MND/SPD/MHP/MMP）
                StatType[] candidates = {
                    StatType.STR, StatType.MND, StatType.SPD, StatType.MHP, StatType.MMP
                };

                StatType bestType = candidates[0];
                int bestValue = ba.Target.Stats.GetStat(bestType);

                for (int i = 1; i < candidates.Length; i++)
                {
                    int v = ba.Target.Stats.GetStat(candidates[i]);
                    if (v > bestValue)
                    {
                        bestValue = v;
                        bestType = candidates[i];
                    }
                }

                // 将该最高属性值作为永久 Flat 加成吸收到施法者
                ApplyDevourAbsorb(ba.Actor, bestType, bestValue);
            }
        };

        return action;
    }

    private void ApplyDevourAbsorb(Character actor, StatType statType, int value)
    {
        if (actor == null) return;

        // 直接永久提升角色基础属性
        // 只允许提升 STR/MND/SPD/MHP/MMP
        if (actor.Stats == null) return;

        // 获取当前原始值
        int current = actor.Stats.GetStat(statType);

        // 增加吸收的数值
        // 这里假设 stats 字典是 public 或有方法可直接修改
        // 如果 stats 是 private，可在 CharacterStats 添加一个方法用于永久加成
        actor.Stats.ChangeStat(statType, value);

        // 可选：记录日志或提示
        //Debug.Log($"{actor.Name} devoured {statType}, permanently increased by {value}.");
    }
}
