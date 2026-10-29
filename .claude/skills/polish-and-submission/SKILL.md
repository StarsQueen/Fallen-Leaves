---
name: polish-and-submission
description: Use when polishing the game, cleaning the repository, writing the README or preparing the final submission (public GitHub repo and a short screen recording). Contains the juice checklist, the anti-slop checklist and the final submission checklist.
---

# Polish and submission

## Juice checklist (do the cheap ones first)

- [ ] Catch feedback: particles + sound + score popup
- [ ] Hit feedback: frog particles + screen shake + box flash
- [ ] Idle life: items wobble or glow, ambient particles, LCD flicker
- [ ] Game over has weight: slow-down or fade, then the panel
- [ ] Music loops, sound effects sit under the music, nothing clips
- [ ] Difficulty ramps over time (spawn rate, fall speed, frog chance)

## Anti-slop checklist

- [ ] No default Unity look left: custom font, hand drawn buttons, no stock grey UI
- [ ] Scene named "Main", not "SampleScene". Product name and company set in Player Settings
- [ ] One palette, one font, one animation speed language
- [ ] No unused scripts, no commented-out code, no debug logs
- [ ] Every script has one clear job and the systems table in AGENTS.md is correct

## README (the author writes the text, the agent only structures it)

Sections: what the game is, controls, a screenshot or GIF, how to run (Unity 6, open the
project, open `Main` scene, press Play), architecture (events diagram with Mermaid, ScriptableObjects),
shaders and particles used, how AI was used (skills, AGENTS.md, plan mode, one feature at a time),
credits.

## Final submission checklist

- [ ] Repository visibility is Public
- [ ] `main` contains the finished game and it opens without errors on a fresh clone
- [ ] No `Library/` folder in the repo, `.gitignore` is the Unity one
- [ ] `AGENTS.md`, `CLAUDE.md` and `.claude/skills/` are in the repo
- [ ] Feature branches were merged with Pull Requests
- [ ] Short screen recording (30 to 60 seconds) is ready
