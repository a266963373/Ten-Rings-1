using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
        if (IsTargetSelectMode) // player decided action
        {
            Target = target;
            actionPanel.SetActive(false);
            StartCoroutine(StartResolve());
            IsTargetSelectMode = false ;
        }
        else  // not player deciding, then open character panel
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
        Actor.Trigger(timing: TimingType.OnSelfTurnEnd);
        BattleSystem.I.LogCount--;  // indicating resolving complete
    }

    public void Resolve(BattleAction battleAction, bool isNoInterrupt = true)
    {
        // triggered damage, usually unstoppable
        StartCoroutine(ScResolve(battleAction, isNoInterrupt));
    }

    private IEnumerator ScResolve(BattleAction ba, bool isUnstoppable = false)
    {
        BattleSystem.I.LogCount++;

        if (ba.Range == RangeType.Indirect) isUnstoppable = true; // indirect action can't be interrupted
        if (isUnstoppableActionResolving)
        {
            while (isUnstoppableActionResolving)
            {
                yield return null;
            }
        }
        if (isUnstoppable)
            isUnstoppableActionResolving = true;

        ba.ApplyEmpowerment();

        // announce action
        if (ba.Actor != null && !ba.IsFollowUp)
        {
            yield return battleLogSystem.ShowBattleAction(ba, true);
        }

        // spend mana
        if (ba.ManaCost != 0 && !ba.IsFollowUp)
        {
            ba.Actor.Stats.ChangeStat(StatType.MP, -ba.ManaCost);
        }

        // related action instead
        if (ba.IsDoRelatedActionInstead)
        {
            yield return ScResolve(ba.RelatedAction, isUnstoppable);
            BattleSystem.I.LogCount--;
            yield break; // no need to continue
        }

        // convert area action to single actions
        if (ba.Area == AreaType.Area)
        {
            var targets = ba.Target.Allies.ToList(); // ¸´ÖÆÒ»·Ý
            foreach (var target in targets)
            {
                BattleAction singleAction = new(ba)
                {
                    Target = target,
                    Area = AreaType.Single,
                    IsFollowUp = true
                };
                yield return ScResolve(singleAction, isUnstoppable);
            }
            BattleSystem.I.LogCount--;
            yield break;
        }

        //--- actually do the action ---//
        // apply action stat mod
        if (ba.Actor != null && ba.ActorTempStatMod != null)
        {
            ba.Actor.Stats.TempStatMods.Add(ba.ActorTempStatMod);
        }

        // modify temp stats (eg. frontline add temp stats here)
        ba.Actor?.Trigger(TriggerType.OnBeforeAction, ba);
        ba.Target?.Trigger(TriggerType.OnBeforeTargeted, ba);
        ba.Trigger(TriggerType.OnBeforeAction);
        FieldSystem.I.Trigger(TriggerType.OnBeforeAction, ba);

        // all temp stat mods should be applied before this point

        // apply stat scaling
        if (ba.Scale != StatType.NON)
        {
            ba.Power = Mathf.RoundToInt(ba.Power * ba.Actor.Stats.GetStat(ba.Scale) * 0.01f);
        }

        // accuracy and evasion check
        yield return ProcessSubStats(ba);

        if (!ba.IsMissed)
        {
            if (ba.IsDamage)
            {
                yield return ResolveDamage(ba);
            }
            if (ba.Statuses.Count != 0)
            {
                if (ba.IsRemovingStatuses)
                {
                    RemoveStatus(ba);
                }
                else
                {
                    ApplyStatus(ba);
                }
            }
            if (ba.Field != null)
            {
                ApplyField(ba);
            }
            ba.Trigger(TriggerType.OnExtraEffect);
        }

        if (ba.Range == RangeType.Melee)
        {
            ba.Actor?.Trigger(TriggerType.OnAfterMeleeAction, ba);
            ba.Target?.Trigger(TriggerType.OnAfterMeleeTargeted, ba);
            if (!ba.IsMissed)
            {
                ba.Actor?.Trigger(TriggerType.OnAfterMeleeContact, ba);
                ba.Target?.Trigger(TriggerType.OnAfterMeleeContact, ba);
            }
        }

        ba.Actor?.Trigger(TriggerType.OnAfterAction, ba);
        FieldSystem.I.Trigger(TriggerType.OnAfterAction, ba);

        // Follow Up Action
        if (ba.FollowUpAction != null)
        {
            yield return ScResolve(ba.FollowUpAction);
        }

        if (ba.Actor != null && !ba.IsFollowUp)
        {
            ba.Actor.ActionGauge -= ba.RecoveryTime;
        }

        ba.Actor?.Stats.TempStatMods.Clear();
        ba.Target?.Stats.TempStatMods.Clear();

        ba.Trigger(TriggerType.OnAfterResolve);
        if (!ba.IsMissed) ba.Trigger(TriggerType.OnAfterResolveNoMiss);

        if (isUnstoppable)
        {
            isUnstoppableActionResolving = false;
        }

        BattleSystem.I.State = BattleState.ActionResolved;  // might be useless bc there might be more action to resolve
        BattleSystem.I.LogCount--;
    }

    private IEnumerator ResolveDamage(BattleAction ba)  // as well as healing (negative damage)
    {
        if (ba.Target == null)
        {
            Debug.LogError("Damage target is null!");
            yield break;
        }

        // actor's augments
        if (ba.Actor != null)
            foreach (var dealDamageModifier in ba.Actor.DealDamageModifiers)
            {
                ba.Damage = dealDamageModifier(ba.Damage);
            }

        ba.Damage.Value = Mathf.RoundToInt(ba.Damage.Value * ba.Power * 0.01f);

        // target's augments
        ba.Trigger(TriggerType.OnBeforeDealDamage);
        ba.Target.Trigger(TriggerType.OnBeforeTakeDamage, ba);

        foreach (var takeDamageModifier in ba.Target.TakeDamageModifiers)
        {
            ba.Damage = takeDamageModifier(ba.Damage);
        }

        // finalized
        ba.Damage.FinalizeValue();

        if (ba.Damage.Property == StatType.NON) ba.Damage.Property = StatType.HP;
        ba.Target.Stats.ChangeStat(ba.Damage.Property, -ba.Damage.Value);
        if (ba.Damage.Property == StatType.HP)
        {
            yield return battleLogSystem.ShowActionResult(ba, true);
        }
        StartCoroutine(CheckDeath(ba.Target));

        ba.Actor?.Trigger(TriggerType.OnAfterDealDamage, ba);
        ba.Trigger(TriggerType.OnAfterDealDamage);
        ba.Target?.Trigger(TriggerType.OnAfterTakeDamage, ba);
    }

    private IEnumerator ProcessSubStats(BattleAction ba)
    {
        if (ba.Actor == null || ba.Target == null || ba.Range == RangeType.Indirect || 
            ba.IsMustSelf || (ba.Damage == null && ba.Statuses.Count == 0))    // indirect damage always hits normally
        {
            yield break;
        }

        if (ba.Range != RangeType.Global && !ba.Damage.IsHealing)  // global or healing can't be dodged
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
                ba.Power = Mathf.RoundToInt(ba.Power * 100f / blk);
                ba.Actor.Stats.CrtGauge = Math.Max(0, ba.Actor.Stats.CrtGauge + 100); // reset critical gauge
                ba.Target.Trigger(TriggerType.OnBlocking, ba);
            }
            else if (ba.Actor.Stats.CrtGauge > 100)    // not counting 100 to make it fair for crit and block
            {
                ba.IsCrit = true;
                ba.Power = Mathf.RoundToInt(ba.Power * crt * 0.01f);
                ba.Actor.Stats.CrtGauge = Math.Min(100, ba.Actor.Stats.CrtGauge - 100);
            }
        }
        yield return BattleLogSystem.I.ShowActionCheck(ba);
    }

    public void ApplyStatusByName(Character applier, Character bearer, String statusName, float stack=-1f)
    {
        Status status = StatusLibrary.I.GetStatusByName(applier, bearer, statusName, stack);
        ApplyStatus(status);
    }

    private void ApplyStatus(BattleAction ba)   // with scaling
    {
        for (int i = 0; i < ba.Statuses.Count; i++)
        {
            Status status = ba.Statuses[i];
            status.Stack *= ba.Power * 0.01f;
            ApplyStatus(status);
        }
    }

    private void RemoveStatus(BattleAction ba)   // with scaling
    {
        for (int i = 0; i < ba.Statuses.Count; i++)
        {
            Status status = ba.Statuses[i];
            status.Bearer.StatusSystem.RemoveSameClassStatus(status);
        }
    }

    private void ApplyStatus(Status status)
    {
        status.Bearer.StatusSystem.ApplyStatus(status);
    }

    private void ApplyField(BattleAction ba)
    {
        if (ba.Scale != StatType.NON)
        {
            ba.Field.Duration *= ba.Power * 0.01f;
            FieldSystem.I.AddField(ba.Field);
        }
    }

    private IEnumerator CheckDeath(Character c)
    {
        if (c.Stats.GetStat(StatType.HP) <= 0)
        {
            BattleAction ba = new()
            {
                Actor = c
            };
            c.Trigger(TriggerType.OnBeforeDeath, ba);

            if (isUnstoppableActionResolving)
            {
                while (isUnstoppableActionResolving)
                {
                    yield return null;
                }
            }
            c.IsDead = true;
            BattleLoader.I.DestroyCharacter(c);
            c.Trigger(TriggerType.OnAfterDeath, ba);
            BattleSystem.I.CheckBattleEnd();
        }
    }

}
