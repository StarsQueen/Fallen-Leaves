---
name: unity-particles-vfx
description: Use when adding particle effects or visual feedback (catch sparkles, hit splats, ambient dust, game over bursts, screen shake, score popups) to this Unity 6 project. Gives the prefab recipe, the event-driven EffectsManager pattern with pooling, and style rules.
---

# Particles and VFX

## Pattern: events in, effects out

- Gameplay scripts NEVER call effects directly.
- An `EffectsManager` (in `Assets/Scripts/Effects/`) subscribes to `GameEvents`, picks the right
  particle prefab and plays it at the event position from an object pool.
- Particle prefabs live in `Assets/Prefabs/Effects/`, fields are `[SerializeField]` on the manager.

## Pooled particle component

```csharp
using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticle : MonoBehaviour
{
    public Action<PooledParticle> Release;
    public ParticleSystem System { get; private set; }

    private void Awake()
    {
        System = GetComponent<ParticleSystem>();
        var main = System.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped() => Release?.Invoke(this);
}
```

Use `UnityEngine.Pool.ObjectPool<PooledParticle>`: create, `OnGet` activates, `OnRelease` deactivates.

## One-shot burst recipe (editor values the agent must give the user)

- Main: Duration `0.6`, Looping off, Start Lifetime `0.4 to 0.8`, Start Speed `2 to 4`,
  Start Size `0.15 to 0.35`, Start Color = gradient from the palette, Gravity Modifier `0.2`,
  Play On Awake off, Stop Action `Callback`.
- Emission: Rate over Time `0`, one Burst at time `0`, count `10 to 16`.
- Shape: Circle, radius `0.1`.
- Color over Lifetime: fade alpha to `0`. Size over Lifetime: shrink to `0`.
- Renderer: a soft circle, heart or sparkle sprite from `Assets/Art` (hand drawn if possible),
  Sorting Layer above the items, Order in Layer `10`.

## Ambient effect

A looping, very low-emission particle system (dust or twinkles) inside the LCD screen area,
Start Alpha `0.2 to 0.4`, slow speed. It must never distract from the falling items.

## Screen shake and popups

- Shake: a `CameraShake` component listening to `OnItemCaught` (frogs only), decaying offset with
  `Mathf.PerlinNoise`, `0.15 s`, small amplitude. Reset the position in `OnDisable`.
- Popups: pooled world-space TextMeshPro "+10" that floats up and fades in `0.6 s`.

## Style rules

- Colours from the game palette (soft pink, mint green, cream). No pure red, no neon.
- Kitty catch: warm, soft, rewarding. Frog hit: short, greenish, slightly harsh.
- Never more than about 20 particles per burst. Always pool.
