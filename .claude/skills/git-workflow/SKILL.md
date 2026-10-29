---
name: git-workflow
description: Use when starting a feature, making commits, finishing a feature or preparing a pull request in this repository. Defines branch names, commit message format, what must never be committed, and the exact commands.
---

# Git workflow

## Start a feature

```bash
git status                       # must be clean
git checkout main
git checkout -b feature/<short-name>
```

## While working

- Commit after each small working step, not at the end.
- Stage specific files, not everything blindly: `git add Assets/Scripts/Effects/ Assets/Scripts/Effects.meta`
- A `.meta` file always travels with its asset.
- Message format: `type: summary` (feat, fix, refactor, docs, chore), imperative, under 60 chars.
  - `feat: add event-driven effects manager`
  - `fix: unsubscribe spawner on disable`
  - `docs: update systems table`

## Never commit

`Library/`, `Temp/`, `Obj/`, `Logs/`, `UserSettings/`, `Builds/`, `*.csproj`, `*.sln`, secrets.
If `git status` shows any of them, stop and fix `.gitignore` instead.

## Finish a feature

1. Check: no compile errors, scene saved, game plays one minute.
2. Show the user `git log --oneline -5` and a 3-line summary of what changed.
3. Do NOT push. The user pushes with GitHub Desktop (Publish branch), opens the Pull Request on
   GitHub, merges it, deletes the branch, then pulls `main`.

## Pull request description template

```
## What
One or two sentences.

## Why
The player-facing reason.

## How to test
1. Open the Main scene, press Play.
2. ...

## Editor setup needed
Assets or prefabs that must exist (if any).
```

## Forbidden without explicit approval

`git push`, `git push --force`, `git rebase`, `git reset --hard`, deleting branches.
