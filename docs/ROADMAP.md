# Roadmap

Work on ONE feature per session and per branch. Start each one in plan mode. Paste the prompt,
read the plan, approve it, then let the agent write the code. After each feature: do the
"Editor setup" steps in Unity, press Play, then commit.

Hard rule: if a feature is not working after 15 minutes, discard the branch and move on.

## F0: Audit (no changes yet)  [15 min]

Branch: none (read-only), then `fix/audit-fixes`

Prompt:
> Plan mode. Read AGENTS.md and every script in Assets/Scripts. Do NOT change anything yet.
> Report: 1) what each system does, 2) where the code breaks the architecture rules in AGENTS.md,
> 3) bugs and risks (for example static event leaks, null references, hard-coded numbers),
> 4) a short list of small fixes in priority order. Then wait for my approval and apply only the
> fixes I approve.

## F1: Data-driven items (ScriptableObjects)  [30 min]  MUST

Branch: `feature/data-driven-items`

Prompt:
> Plan mode. Use the unity-architecture skill. Feature: make items data-driven.
> Create a ScriptableObject `ItemDefinition` (menu Catcher/Item) with: ItemType, Sprite[] sprites,
> int points, Vector2 fallSpeedRange, float spawnWeight, optional AudioClip catchSfx.
> Create a ScriptableObject `DifficultyProfile` with AnimationCurves over elapsed time for the
> spawn interval, the frog chance and a fall speed multiplier.
> The Spawner picks an ItemDefinition by weight and applies the DifficultyProfile. FallingItem
> initialises itself from its ItemDefinition (ideally one Item prefab for everything).
> Change the event to `OnItemCaught(ItemDefinition item, Vector3 worldPosition)` and update
> ScoreSystem (use item.Points), HealthSystem and AudioManager. At the start of the game the
> behaviour must feel the same as now. End with an Editor setup section.

## F2: Event-driven particles  [35 min]  MUST

Branch: `feature/particles`

Prompt:
> Plan mode. Use the unity-particles-vfx skill. Feature: particle feedback driven by events.
> Create `EffectsManager` and `PooledParticle` in Assets/Scripts/Effects. It listens to
> GameEvents and plays pooled prefabs at the caught item's position: a soft pink heart/sparkle
> burst for kitties and a green splat for frogs. Add a game over burst and an ambient low-emission
> dust particle system inside the LCD screen. Gameplay scripts must not reference the
> EffectsManager. Give me exact Inspector values for every particle prefab, module by module.

## F3: Shader on sprites (flash / glow)  [35 min]  MUST (at least one shader)

Branch: `feature/sprite-shader`

Prompt:
> Plan mode. Use the urp-2d-shaders skill. Feature: sprite shader `Catcher/SpriteFX` with a hit
> flash and a soft pulsing outline/glow. Create a `HitFlash` component for the box that listens to
> OnItemCaught and flashes pink for kitties and red for frogs using a MaterialPropertyBlock.
> Kitties get a gentle idle glow. Give me the material setup steps. If the shader does not compile
> I will paste the Console error.

## F4: Retro screen shader from ShaderToy  [30 min]  NICE

Branch: `feature/lcd-shader`

Prompt:
> Plan mode. Use the urp-2d-shaders skill. I will paste a ShaderToy shader. Convert it to a Unity
> UI shader for the RawImage that shows the game on the console screen. Combine it with scanlines,
> slight curvature, vignette and a `_Glitch` property. Add a `ScreenFX` script that pulses
> `_Glitch` briefly when OnHealthChanged fires. Explain what I must change on the material.

## F5: Juice (shake + score popups)  [20 min]  NICE

Branch: `feature/juice`

Prompt:
> Plan mode. Use the unity-particles-vfx skill. Feature: `CameraShake` (frogs only, short, soft)
> and pooled floating "+10" score popups at the catch position. Both are driven by events.
> Give me the Editor setup steps.

## F6: Menus and progression  [25 min]  OPTIONAL

Branch: `feature/menus`

Prompt:
> Plan mode. Feature: title screen with Start, pause with Esc, best score saved with PlayerPrefs,
> game over panel showing score and best. Use the hand-drawn coloured button sprites. Keep it
> event-driven and follow AGENTS.md. Give me the Editor setup steps.

## F7: README and cleanup  [15 min]  MUST

Branch: `docs/readme`

Prompt:
> Plan mode. Use the polish-and-submission skill. Create README.md following the skill, with a
> Mermaid diagram of the event flow. Leave clearly marked placeholders for the screenshot and for
> sentences I will write myself. Update the systems table in AGENTS.md. Do not invent features.
