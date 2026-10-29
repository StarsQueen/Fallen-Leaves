---
name: unity-architecture
description: Use when adding or changing any gameplay script, system, event or ScriptableObject in this Unity 6 project. Explains how systems communicate (GameEvents), where data lives (ScriptableObjects), the folder layout, and gives templates for a new system, event and data asset.
---

# Unity architecture for The Catcher

## Mental model

- A system is a small MonoBehaviour with ONE job. Systems never hold references to each other.
- Systems talk through `GameEvents` (static C# events). A system raises an event when something
  happened. Other systems react.
- Data (item stats, difficulty curves) lives in ScriptableObjects. Runtime state lives in normal
  fields or plain C# classes, never inside a ScriptableObject (the asset is shared and persists).

## Add a new event (3 steps)

1. In `GameEvents.cs`: `public static event Action<int> OnThingHappened;` and
   `public static void ThingHappened(int value) => OnThingHappened?.Invoke(value);`
2. The system that knows it happened calls `GameEvents.ThingHappened(value);`
3. Listeners subscribe in `OnEnable` and unsubscribe in `OnDisable`.

## Template: new system

```csharp
using UnityEngine;

public class ExampleSystem : MonoBehaviour
{
    [SerializeField] private float someValue = 1f;

    private void OnEnable()  => GameEvents.OnGameOver += HandleGameOver;
    private void OnDisable() => GameEvents.OnGameOver -= HandleGameOver;

    private void HandleGameOver() { }
}
```

## Template: ScriptableObject data

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "Catcher/Example", fileName = "NewExample")]
public class ExampleData : ScriptableObject
{
    [SerializeField] private int points = 10;
    public int Points => points;   // read-only from outside
}
```

## Rules

- No `FindObjectOfType`, no singletons, no static mutable state outside `GameEvents`.
- Always unsubscribe in `OnDisable`. Static events outlive scene reloads.
- Private serialized fields, public read-only properties.
- Pool objects spawned more than about once per second (`UnityEngine.Pool.ObjectPool<T>`).
- Scripts go in `Assets/Scripts/{Core,Gameplay,UI,Audio,Effects,Data}`.

## Before finishing

1. Re-read the code for compile errors.
2. Give an **Editor setup** section with exact steps for anything done in the Unity editor.
3. Update the "Current systems" table in `AGENTS.md`.
