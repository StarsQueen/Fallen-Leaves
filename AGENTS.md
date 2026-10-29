# AGENTS.md: The Catcher

Instructions for AI coding agents (Claude Code, Codex, Cursor...) working in this repository.
Read this whole file before changing anything.

## Project

"The Catcher" is a 2D game made in Unity 6 (URP 2D Renderer). The player moves a hand-drawn
cardboard box left and right and catches falling kitties for points. Frogs act as bombs and cost one
life (three paws). The whole game is displayed on the LCD screen of a hand-drawn handheld console.

- Unity 6 (6000.x), URP 2D, TextMeshPro, legacy Input Manager (Active Input Handling = Both).
- All art is hand drawn by the author. Never replace it with generated or placeholder art.
- Art direction: cute, soft, pastel kitties, retro handheld console, LCD screen with scanlines.
  New effects must fit that look (soft, rounded, slightly grainy) and must not look like default
  Unity assets (no default white squares, no neon colours, no stock UI sprites).

## Architecture rules (must follow)

1. One script, one responsibility. Systems are small MonoBehaviours.
2. Systems never reference each other. They communicate through `GameEvents`
   (static C# events, the Observer pattern). Subscribe in `OnEnable`, unsubscribe in `OnDisable`.
3. Game data (items, difficulty) lives in ScriptableObjects. Never mutate a ScriptableObject at
   runtime. Runtime state lives in normal fields or plain C# classes.
4. No magic numbers in code. Use `[SerializeField] private` fields or ScriptableObjects.
5. No singletons, no `FindObjectOfType`, no `GameObject.Find`, no public static mutable state
   (except the events in `GameEvents`).
6. A new feature is a new system that listens to existing events. Do not edit unrelated systems.
7. Pool anything spawned often (`UnityEngine.Pool.ObjectPool<T>`): items, particles, popups.
8. Naming: PascalCase for types and methods, camelCase for private fields, events are
   `OnSomethingHappened`, the method that raises them is `SomethingHappened()`.
9. Comments explain WHY, not what. No emoji, no leftover TODOs, no dead code.
10. Folder layout under `Assets/`: `Scripts/{Core,Gameplay,UI,Audio,Effects,Data}`, `Art`, `Audio`,
    `Prefabs`, `Shaders`, `ScriptableObjects`, `Scenes`.

## How to work (workflow rules)

- One feature at a time, one branch per feature. Never one-shot several features.
- Start every feature in plan mode: read the code, show a short plan, wait for approval.
- You cannot click in the Unity editor. When a feature needs editor work (assets, prefabs, scene
  wiring, Inspector values), finish the code, then end with a section called **Editor setup** with
  exact numbered steps and exact values.
- Never hand-edit `.unity`, `.prefab`, `.asset`, `.meta` files or `ProjectSettings/`.
- Do not add packages or change project settings without asking.
- Before saying a feature is done: re-read the code for compile errors (Unity 6 APIs), then give a
  short Play mode test checklist.
- Update the "Current systems" table below whenever systems or events change.
- Use the skills in `.claude/skills/` when they apply.

## GitHub rules

- `main` must always be playable. Do not commit directly to `main` (the first setup commit is the
  only exception).
- Branches: `feature/<short-name>`, `fix/<short-name>`, `chore/<short-name>`.
- Small, frequent commits. Message format `type: summary`, imperative, under 60 characters.
  Types: `feat`, `fix`, `refactor`, `docs`, `chore`. Example: `feat: add pooled catch particles`.
- Always commit a `.meta` file together with its asset. Never commit `Library/`, `Temp/`, `Obj/`,
  `Logs/`, `UserSettings/`, `Builds/`, `*.csproj` or `*.sln` (the Unity `.gitignore` covers them).
- Before every commit: no compile errors in Unity, the scene is saved (the user presses Cmd+S),
  and the game plays for one minute without errors.
- Features are merged into `main` with a Pull Request on GitHub, then the branch is deleted.
- Agents may run `git status`, `git diff`, `git add`, `git commit`, `git branch`, `git checkout`.
  Agents must NOT push, force-push, rebase, `reset --hard` or delete branches without explicit
  approval from the user.
- Never commit secrets or API keys.

## Current systems (keep up to date)

| Script | Responsibility | Raises | Listens to |
|---|---|---|---|
| `GameEvents` | Static event hub and the `ItemType` enum | (holds all events) | |
| `PlayerController` | Moves the box with the horizontal axis, clamped to `xLimit` | | |
| `FallingItem` | Falls, picks a random kitty sprite, detects the box, destroyed at `killY` | `OnItemCaught` | |
| `Spawner` | Spawns kitties and frogs on a timer (`bombChance`) | | `OnGameOver` |
| `ScoreSystem` | +10 points per kitty | `OnScoreChanged` | `OnItemCaught` |
| `HealthSystem` | Three lives, one lost per frog | `OnHealthChanged`, `OnGameOver` | `OnItemCaught` |
| `UIManager` | Score text, paw icons, game over panel | | score, health, game over |
| `AudioManager` | Catch/hurt/game over sounds, stops music at the end | | `OnItemCaught`, `OnGameOver` |
| `GameRestarter` | Press R after game over to reload the scene | | `OnGameOver` |

The game camera renders into a RenderTexture that is shown on a RawImage inside the console frame
(ConsoleCanvas). The HUD lives on a second canvas that renders to the game camera.

## Quality bar ("does not look like AI slop")

- Every effect has a purpose (feedback for a catch, a hit, a game over) and fits the art direction.
- Effects are consistent: same palette, same softness, same timing language.
- No leftover default Unity content (SampleScene name, default sprites, placeholder text).
- The README is written in the author's own words and explains the architecture honestly.
