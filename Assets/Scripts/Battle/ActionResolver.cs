using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionResolver : MonoBehaviour // receive choice from panels
{
    public static ActionResolver I;
    [NonSerialized] public bool IsTargetSelectMode = false;
    [NonSerialized] public BattleActionSO BattleActionSO;
    [SerializeField] BattleLogSystem battleLogSystem;
    [SerializeField] RingPanel ringPanel;
    [SerializeField] GameObject actionPanel;
    [SerializeField] Transform focusImage;
    private BattleAction battleAction;
    public Character Actor;
    public Character Target;
    private bool isUnstoppableActionResolving = false;  // only one this kind of action can be resolving at a time

    private void Awake()
    {
        I = this;
        ringPanel.gameObject.SetActive(false);
    }

    public void SelectTarget(Character target)
    {
        if (IsTargetSelectMode)
        {
            Target = target;
            actionPanel.SetActive(false);
            StartCoroutine(StartResolve());
            IsTargetSelectMode = false ;
        }
        else
        {
            ringPanel.LoadRings(target);
            ringPanel.gameObject.SetActive(true);
        }
    }

    public IEnumerator StartResolve()   // from a character's action decided
    {
        focusImage.gameObject.SetActive(false);
        battleAction = BattleActionSO.GetAction(Actor, Target);
        yield return ScResolve(battleAction);
        Actor.OnCharacterTurnEnd();
        BattleSystem.I.LogCount--;  // indicating resolving complete
    }

    public void Resolve(BattleAction battleAction, bool isNoInterrupt = false)
    {
        StartCoroutine(ScResolve(battleAction, isNoInterrupt));
    }

    private IEnumerator ScResolve(BattleAction ba, bool isUnstoppable = false)
    {
        BattleSystem.I.LogCount++;

        if (isUnstoppable)
        {
            while (isUnstoppableActionResolving)
            {
                yield return null;
            }
            isUnstoppableActionResolving = true;
        }

        // actually do the action
        if (ba.IsDoRelatedActionInstead)
        {
            Resolve(ba.RelatedAction);
            yield break; // no need to continue
        }

        if (ba.Actor != null)
        {
            // announce action
            yield return battleLogSystem.ShowBattleAction(ba, true);
        }

        // apply action stat mod
        if (ba.Actor != null && ba.ActorTempStatMod != null)
        {
            ba.Actor.Stats.TempStatMods.Add(ba.ActorTempStatMod);
        }

        // spend mana
        if (ba.ManaCost != 0)
        {
            ba.Actor.Stats.ChangeStat(StatType.MP, -ba.ManaCost);
        }

        // modify temp stats (eg. frontline add temp stats here)
        ba.Actor?.Trigger(TriggerType.OnBeforeAction, ba);
        ba.Trigger(TriggerType.OnBeforeAction);

        // apply stat scaling
        if (ba.Scale != StatType.NON)
        {
            ba.Power = (int)(ba.Power * ba.Actor.Stats.GetStat(ba.Scale) * 0.01f);
        }
        if (ba.IsDamage)
        {
            yield return ResolveDamage(ba);
        }
        if (ba.IsStatus)
        {
            ApplyStatus(ba);
        }

        ba.Trigger(TriggerType.OnAfterResolve);

        ba.Actor?.Stats.TempStatMods.Clear();

        if (isUnstoppable)
        {
            isUnstoppableActionResolving = false;
        }
        BattleSystem.I.State = BattleState.ActionResolved;  // might be useless bc there might be more action to resolve
        BattleSystem.I.LogCount--;
    }

    private IEnumerator ResolveDamage(BattleAction ba)  // as well as healing (negative damage)
    {
        // actor's augments
        if (ba.Actor != null)
            foreach (var dealDamageModifier in ba.Actor.DealDamageModifiers)
            {
                ba.Damage = dealDamageModifier(ba.Damage);
            }

        ba.scaledValue = ba.Damage.Value * ba.Power * 0.01f;

        // middle ground
        // accuracy and evasion check
        yield return ProcessSubStats(ba);
        ba.Damage.Value = Mathf.RoundToInt(ba.scaledValue);

        // target's augments
        ba.Trigger(TriggerType.OnBeforeDealDamage);
        ba.Target.Trigger(TriggerType.OnBeforeTakeDamage, ba);

        foreach (var takeDamageModifier in ba.Target.TakeDamageModifiers)
        {
            ba.Damage = takeDamageModifier(ba.Damage);
        }


        // finalized
        ba.Damage.FinalizeValue();
        if (!ba.IsMissed)
        {
            ba.Target.Stats.ChangeStat(StatType.HP, -ba.Damage.Value);
            yield return battleLogSystem.ShowActionResult(ba, true);
        }

        ba.Actor?.Trigger(TriggerType.OnAfterDealDamage, ba);
        ba.Target?.Trigger(TriggerType.OnAfterTakeDamage, ba);
    }

    private IEnumerator ProcessSubStats(BattleAction ba)
    {
        if (ba.Damage.Range != DamageRange.Indirect)    // indirect damage always hits normally
        {
            if (ba.Damage.Range != DamageRange.Global && !ba.Damage.IsHealing)  // global or healing can't be dodged
            {
                int evs = ba.Target.Stats.GetStat(StatType.EVS);
                ba.Target.Stats.EvsGauge += evs - ba.Actor.Stats.GetStat(StatType.ACC);
                if (ba.Target.Stats.EvsGauge >= evs)
                {
                    // missed
                    ba.IsMissed = true;
                    ba.Target.Stats.EvsGauge = Math.Min(evs, ba.Target.Stats.EvsGauge - evs);
                }
                else
                {
                    // hit
                    ba.Target.Stats.EvsGauge = Math.Max(0, ba.Target.Stats.EvsGauge);
                }
            }

            if (!ba.IsMissed)
            {
                // critical hit and block check
                int crt = ba.Actor.Stats.GetStat(StatType.CRT);
                int blk = ba.Damage.IsHealing ? 100 : ba.Target.Stats.GetStat(StatType.BLK);  // if healing, no block
                ba.Actor.Stats.CrtGauge += crt - blk;
                if (ba.Actor.Stats.CrtGauge < 0 && !ba.Damage.IsHealing)
                {
                    ba.IsBlocked = true;
                    ba.scaledValue /= blk * 0.01f;
                    ba.Actor.Stats.CrtGauge = Math.Max(0, ba.Actor.Stats.CrtGauge + 100); // reset critical gauge
                    ba.Target.Trigger(TriggerType.OnBlocking, ba);
                }
                else if (ba.Actor.Stats.CrtGauge > 100)    // not counting 100 to make it fair for crit and block
                {
                    ba.IsCrit = true;
                    ba.scaledValue *= crt * 0.01f;
                    ba.Actor.Stats.CrtGauge = Math.Min(100, ba.Actor.Stats.CrtGauge - 100);
                }
            }
            yield return BattleLogSystem.I.ShowActionCheck(ba);
        }
    }

    public void ApplyStatusByName(Character applier, Character bearer, String statusName)
    {
        Status status = StatusLibrary.I.GetStatusByName(applier, bearer, statusName);
        ApplyStatus(status);
    }

    private void ApplyStatus(BattleAction ba)   // with scaling
    {
        if (ba.Scale != StatType.NON)
        {
            ba.Status.Stack *= ba.Power * 0.01f;
        }
        ApplyStatus(ba.Status);
    }


    private void ApplyStatus(Status status)
    {
        // "on add status"
        status.Bearer.StatusSystem.AddStatus(status);
        // "after apply status"
    }
}
