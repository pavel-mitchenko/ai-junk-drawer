# AI Junk Drawer

A loose collection of small standalone web tools, toys, and experiments built with help from LLMs. Not a product — no shared build, no framework, no test suite.

## Important: this repo is public

It's a junk drawer, but it lives on a public GitHub repo. Be careful what gets committed — no secrets, no API keys, no internal links, no personal data. Junk ≠ careless.

## Hosting

The repo is published as a **GitHub Pages** site. The entry point is `index.html` in the repo root — that's the landing page Pages serves at the site URL. Anything you put in the root or subfolders is reachable as a public URL.

## Layout

- `index.html` — landing page (Pages entry point).
- `experiments/` — sandbox for quick spike pages (e.g. `cors-fetch.html`).
- `SoftmaxLab/` — a finished sub-toy.

## Finished toys

- **SoftmaxLab** (`SoftmaxLab/`) — softmax visualization sub-toy.

## Conventions

- Plain HTML + vanilla JS + inline `<style>`. No build step, no npm install required.
- Keep new toys self-contained in a single HTML file when possible.
- For experiments, prefer `experiments/<name>.html`.
- Code comments are always written in English, regardless of the language used in chat.

## Working style

- This is a junk drawer — prioritize getting things working over polish, abstraction, or test coverage.
- Don't add frameworks or build tooling without asking.
- Don't refactor unrelated files.

## Subprojects

Subprojects (like `SoftmaxLab/`) can — and should — have their own instruction files when they need them. If a toy has its own conventions, dependencies, quirks, or a more detailed description of its own, document them **inside the subproject**, not here. Keep this central file lean.

## `AGENTS.md` vs `CLAUDE.md`

This file (`AGENTS.md`) is the canonical instruction file for AI coding agents — a growing cross-tool convention (Codex, Cursor, Aider, etc. read it).

Claude Code itself looks for `CLAUDE.md`. To avoid duplication, `.claude/CLAUDE.md` is a one-line pointer that imports this file via `@../AGENTS.md`.

Subprojects should follow the same pattern: a subproject-level `AGENTS.md` for the content, and a thin `CLAUDE.md` (either at the subproject root or in `.claude/`) that imports it. The pointer file must contain **only the import line and nothing else** — no `# CLAUDE.md` header, no boilerplate paragraph like "This file provides guidance to Claude Code…". For example, a subproject `CLAUDE.md` placed next to its `AGENTS.md` is exactly:

```
@AGENTS.md
```

Edit the `AGENTS.md`; leave the pointer alone.
