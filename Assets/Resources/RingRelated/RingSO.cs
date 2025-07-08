using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings/RingSO")]
public class RingSO : ScriptableObject
{
    private bool initialized = false;

    public int Id;
    public string Name;
    public int power;    // for son rings' effect strength

    public List<StatModifier> StatModifiers = new();
    public List<TriggerEffect> TriggerEffects = new();
    public List<BattleActionSO> GrantedActions = new();

    protected virtual void OnEnable()
    {
        if (initialized) return;
        initialized = true;

        InitStatModifiers();
        InitTriggerEffects();
        InitGrantedActions();
    }

    protected virtual void InitStatModifiers() { }
    protected virtual void InitTriggerEffects() { }
    protected virtual void InitGrantedActions() { }

    public void AffectCharacter(Character c)
    {
        foreach (StatModifier mod in StatModifiers)
        {
            c.Stats.AddModifier(mod);
        }

        foreach (TriggerEffect trigFx in TriggerEffects)
        {
            c.TriggerEffects[trigFx.Trigger].Add(trigFx.Effect);
        }

        foreach (BattleActionSO action in GrantedActions)
        {
            c.BattleActions.Add(action);
            int temp = c.BattleActions.Count - 1;
        }
    }
}
