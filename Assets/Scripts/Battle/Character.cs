using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.FullSerializer;

public class Character
{
    public string Name;
    public CharacterStats Stats;
    public List<Status> Statuses => StatusSystem.Statuses;
    public Ring[] Rings = new Ring[10];
    public Ring[] EffectiveRings => Rings.Where(ring => ring != null && ring.IRF(this)).ToArray();
    public Ring[] ERings => EffectiveRings;
    public List<Func<Damage, Damage>> DealDamageModifiers =>
        ERings.Where(ring => ring.DealDamageModifier != null)
             .Select(ring => ring.DealDamageModifier)
             .Concat(Statuses.Where(status => status != null && status.DealDamageModifier != null)
                             .Select(status => status.DealDamageModifier))
             .ToList();
    public List<Func<Damage, Damage>> TakeDamageModifiers =>
        ERings.Where(ring => ring.TakeDamageModifier != null)
             .Select(ring => ring.TakeDamageModifier)
             .Concat(Statuses.Where(status => status != null && status.TakeDamageModifier != null)
                             .Select(status => status.TakeDamageModifier))
             .ToList();

    private List<BattleActionSO> battleActions = new();
    public List<BattleActionSO> BattleActions
    {
        get
        {
            var newBattleActions = new List<BattleActionSO>(battleActions);

            for (int i = 0; i < ERings.Length; i++)
            {
                var ring = ERings[i];

                foreach (var action in ring.GrantedActions)
                {
                    newBattleActions.Add(action);
                    if (action == ActivatedWeaponActionSO)
                    {
                        //ActivateWeaponActionSO activateWeaponActionSO = action as ActivateWeaponActionSO;
                        //newBattleActions.AddRange(activateWeaponActionSO.GrantedActions);
                        newBattleActions.AddRange(ActivatedWeaponActionSO.GrantedActions);
                    }
                }

                if (ring.Enchantment != null)
                {
                    for (int j = i - 1; j >= 0; --j)
                    {
                        if (ERings[j].Type == RingType.Weapon)
                        {
                            ActivateWeaponActionSO activateWeaponActionSO = ERings[j].GrantedActions[0] as ActivateWeaponActionSO;
                            ring.Enchantment.Empowerment.Target = activateWeaponActionSO.GrantedActions[0];
                            break;
                        }
                    }
                }

                if (ring.Empowerment != null)
                    ring.Empowerment.Target = newBattleActions.LastOrDefault();
            }
            return newBattleActions;
        }

        set { battleActions = value; }
    }
    public StatusSystem StatusSystem = new();
    public List<TriggerEffect> TriggerEffects = new();  // for timing effects, like on world/character turn
    List<TriggerEffect> AllTriggerEffects => ERings
            .SelectMany(r => r.TriggerEffects)
            .Concat(Statuses.Where(s => s != null)
            .SelectMany(s => s.TriggerEffects))
            .Concat(TriggerEffects).ToList();
    public event Action<Character> OnApplierTurnBegin;

    public bool IsPlayerSide = false;
    public bool IsPlayerControlled
    {
        get { return IsPlayerSide && !ERings.Any(r => r.Id == 49); }
    }
    public bool IsDead = false;
    public ActivateWeaponActionSO ActivatedWeaponActionSO;
    public List<Character> Allies   // with self
    {
        get
        {
            return BattleSystem.I.Characters
                .Where(c => c.IsPlayerSide == this.IsPlayerSide)
                .ToList();
        }
    }
    public List<Character> Teammates   // without self
    {
        get
        {
            return BattleSystem.I.Characters
                .Where(c => c != this && c.IsPlayerSide == this.IsPlayerSide)
                .ToList();
        }
    }
    public List<Character> Enemies
    {
        get
        {
            return BattleSystem.I.Characters
                .Where(c => c.IsPlayerSide != this.IsPlayerSide)
                .ToList();
        }
    }
    public Character TauntedApplier => Statuses.Where(s => s.Id == 4)
                                               .Select(s => s.Applier)
                                               .FirstOrDefault();

    private float actionGauge = 0f;
    public float ActionGauge
    {
        get { return actionGauge; }
        set
        {
            actionGauge = value;
            OnActionGaugeChanged?.Invoke();
        }
    }
    public event Action OnActionGaugeChanged;
    public Character LastTarget;
    public Character RandomOther
    {
        get
        {
            var selectableCharacters = BattleSystem.I.Characters
                .Where(c => c != this && !c.IsDead)
                .ToList();

            if (selectableCharacters.Count == 0)
                return null;

            return selectableCharacters[UnityEngine.Random.Range(0, selectableCharacters.Count)];
        }
    }

    public Character(CharacterSO so)
    {
        Name = so.CharacterName;
        Stats = so.GetStats(); // 运行时副本
        Stats.Owner = this; // 设置 Stats 的所有者为当前 Character 实例
        Rings = so.GetRings();

        // Load Attack Action
        LoadBasicActions();

        Stats.InitBeforeBattle();
        StatusSystem.Init(this); // 设置 StatusSystem 的所有者为当前 Character 实例

        InitTriggerEffects();
    }

    public Character(Character original, float statModifier = 1f, RingInheritType ringInheritType = RingInheritType.None)
    {
        Name = original.Name;
        // 深拷贝 Stats，Owner 指向新角色
        Stats = new(original.Stats, statModifier)
        {
            Owner = this
        };

        // 初始化Rings为长度10的数组
        Rings = new Ring[10];
        if (ringInheritType == RingInheritType.All)
        {
            for (int i = 0; i < 10; i++)
                Rings[i] = original.Rings[i];
        }
        else if (ringInheritType == RingInheritType.LeftHand)
        {
            for (int i = 0; i < 10; i += 2)
                Rings[i] = original.Rings[i];
            // 其他位置为null
        }
        else if (ringInheritType == RingInheritType.RightHand)
        {
            for (int i = 1; i < 10; i += 2)
                Rings[i] = original.Rings[i];
            // 其他位置为null
        }
        // None: 全部为空，已初始化

        LoadBasicActions();

        // 新建 StatusSystem，复制已有状态
        StatusSystem.Init(this);
        foreach (var status in original.StatusSystem.Statuses)
        {
            StatusSystem.ApplyStatus(status);
        }

        IsPlayerSide = original.IsPlayerSide;
        //IsDead = original.IsDead;
        ActionGauge = original.ActionGauge;
        Stats.InitBeforeBattle();
        InitTriggerEffects();
    }

    public int GetStat(StatType type)
    {
        return Stats.GetStat(type);
    }

    public void Trigger(TriggerType type, BattleAction context=null, Status status=null)
    {
        foreach (var effect in AllTriggerEffects)
        {
            if (effect.Trigger == type)
            {
                if (effect.Effect != null)
                {
                    effect.Effect(context);
                }
                else
                {
                    effect.EffectOnStatus?.Invoke(status);
                }
            }
        }
    }

    public void Trigger(TimingType timing)
    {
        foreach (var triggerEffect in AllTriggerEffects)
        {
            if (triggerEffect.Timing == timing)
            {
                triggerEffect.EffectOnTime?.Invoke();
            }
        }
    }

    public void UpdateDynamicStatMods(StatType statType)    // for beginner's luck & chased status
    {
        foreach (var ring in ERings)
        {
            foreach (var dynamicStatMod in ring.DynamicStatMods)
            {
                if (dynamicStatMod.TypeEquals(statType))
                {
                    dynamicStatMod.Updator(this);
                }
            }
        }

        foreach (var status in StatusSystem.Statuses)
        {
            foreach (var dynamicStatMod in status.DynamicStatMods)
            {
                if (dynamicStatMod.TypeEquals(statType))
                {
                    dynamicStatMod.Updator();
                }
            }
        }
    }

    private void InitTriggerEffects()
    {
        TriggerEffects.Add(new TriggerEffect()
        {
            Trigger = TriggerType.OnAfterAction,
            Effect = (ba) =>
            {
                LastTarget = ba.Target;
            }
        });

        TriggerEffects.Add(new TriggerEffect()
        {
            Timing = TimingType.OnWorldTurn,
            EffectOnTime = () => {
                StatusSystem.OnTurn(TimingType.OnWorldTurn);
                // 修改这里：HPR 除以 10
                Stats.ChangeStat(StatType.HP, Stats.GetStat(StatType.HPR) / 10);
                Stats.ChangeStat(StatType.MP, Stats.GetStat(StatType.MPR) / 10);
            }
        });
        TriggerEffects.Add(new TriggerEffect()
        {
            Timing = TimingType.OnSelfTurnBegin,
            EffectOnTime = () =>
            {
                StatusSystem.OnTurn(TimingType.OnSelfTurnBegin);
                OnApplierTurnBegin?.Invoke(this);
            }
        });
        TriggerEffects.Add(new TriggerEffect()
        {
            Timing = TimingType.OnSelfTurnEnd,
            EffectOnTime = () => {
                StatusSystem.OnTurn(TimingType.OnSelfTurnEnd);
            }
        });
    }

    private void LoadBasicActions()
    {
        // Load Attack Action
        BattleActionSO basicAttackAction = Resources.Load<BattleActionSO>("RingRelated/AttackActionSO");
        battleActions.Add(basicAttackAction);
        BattleActionSO basicDefendAction = Resources.Load<BattleActionSO>("RingRelated/DefendActionSO");
        battleActions.Add(basicDefendAction);
    }
}

public enum RingInheritType
{
    None,
    All,
    LeftHand,
    RightHand,
}