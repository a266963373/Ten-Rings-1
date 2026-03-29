# Ten Rings 1 🌌⚔️

A full-scale Unity game system featuring a modular combat engine, ring-based progression, and extensible gameplay architecture.

Gameplay: 
https://youtu.be/EbSCILgqq7E
---

## 📌 Overview

Ten Rings 1 is a complete game system built in Unity, integrating combat, progression, saving/loading, and system-driven mechanics.

The project focuses on **designing interacting systems**, where mechanics combine to produce emergent gameplay rather than isolated features.

---

## 🎮 Core Systems

### 🔄 Game System (Global State Manager)
- Centralized singleton (`GameSystem`) controlling game state, save data, and runtime session
- Persistent across scenes using `DontDestroyOnLoad`
- Handles:
  - Save/Load system integration
  - Currency (gold) updates with event-driven notifications
  - Scene transitions

### ⚔️ Combat System
- Turn-based combat architecture
- Action resolution system (`BattleActionLibrary`, `TriggerEffect`)
- Supports:
  - Modular actions
  - Trigger-based effects
  - Extensible combat logic

### 💍 Ring System
- Core progression mechanic
- Each ring grants abilities or modifies combat behavior
- Implemented via:
  - `Ring.cs`
  - `RingLibrary.cs`
- Scales to large numbers of rings (e.g., 75+ implemented)

### 🧬 Status & Effect System
- Dynamic status effects (`Status.cs`, `StatusLibrary.cs`)
- Supports:
  - Buffs / debuffs
  - Interaction between effects
  - Trigger-based updates

### 🧍 Character System
- ScriptableObject-based character definitions (`CharacterSO`)
- Runtime stats handling (`CharacterStats.cs`)
- Supports flexible character configuration and scaling

### 🗺️ Level & Encounter System
- Encounter assets defining battle scenarios
- Scene-based progression
- Modular level design structure

### 💾 Save System
- Persistent save/load system (`SaveSystem`, `SaveData`)
- Integrated with global game state
- Supports multiple save slots

---

## 🧠 Architecture Highlights

- **Singleton Core System**
  - Global game state managed through `GameSystem`

- **Event-Driven Design**
  - Example: gold updates trigger UI refresh via `OnGoldChange`

- **Library Pattern**
  - Centralized registries for Rings, Statuses, and Actions

- **Modular Systems**
  - Combat, progression, and effects are decoupled but interoperable

---

## 🛠️ Tech Stack

- Unity (C#)
- ScriptableObjects for data-driven design
- Scene-based architecture
- Localization support (Unity Localization)

---

## 📂 Project Structure

```
Assets/
│── Scripts/
│   ├── Battle/            # Combat logic and action resolution
│   ├── CharacterSO/       # Character definitions
│   ├── Level/             # Level and encounter systems
│   ├── Main/              # Core game logic (GameSystem, managers)
│   ├── Save/              # Save/load system
│   ├── Ring.cs            # Ring mechanics
│   ├── Status.cs          # Status system
│   └── TriggerEffect.cs   # Effect triggers
│
│── Scenes/                # Game scenes
│── Prefabs/               # Reusable game objects
│── Resources/             # Assets and data
```

---

## 🔥 Key Features

- Fully functional combat system with extensible mechanics  
- Scalable ring-based progression (75+ rings implemented)  
- Modular and reusable architecture for rapid feature expansion  
- Event-driven updates and clean system separation  
- Integration of gameplay systems with persistence and UI  

---

## 🚀 Future Improvements

- Advanced AI behavior for enemies  
- Expanded interaction between status effects  
- UI polish and visualization improvements  
- Additional worlds and gameplay systems  

---

## ✨ Design Philosophy

Ten Rings 1 is built on the idea that:

> Complex gameplay emerges from simple, interacting systems.

Instead of scripting isolated features, the project defines **rules and interactions**, allowing systems to combine dynamically.

---

## 🐾 Summary

This project demonstrates:
- System design thinking
- Scalable architecture in Unity
- Integration of multiple gameplay systems
- Ability to design and implement complex mechanics

Ten Rings 1 is both a technical implementation and a design-driven exploration of game systems.

