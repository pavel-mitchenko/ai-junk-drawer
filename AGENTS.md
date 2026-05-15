# AI Junk Drawer

A loose collection of small standalone web tools, toys, and experiments built with help from LLMs. Not a product — no shared build, no framework, no test suite.

## Important: this repo is public

It's a junk drawer, but it lives on a public GitHub repo. Be careful what gets committed — no secrets, no API keys, no internal links, no personal data. Junk ≠ careless.

## Hosting

The repo is published as a **GitHub Pages** site. The entry point is `index.html` in the repo root — that's the landing page Pages serves at the site URL. Anything you put in the root or subfolders is reachable as a public URL.

## Layout

- `index.html` — landing page (Pages entry point).
- `Experiments/` — sandbox for quick spike pages (e.g. `cors-fetch.html`).
- `SoftmaxLab/` — a finished sub-toy.

## Finished toys

- **SoftmaxLab** (`SoftmaxLab/`) — softmax visualization sub-toy.

## Conventions

- Plain HTML + vanilla JS + inline `<style>`. No build step, no npm install required.
- Keep new toys self-contained in a single HTML file when possible.
- For experiments, prefer `Experiments/<name>.html`.
- Code comments are always written in English, regardless of the language used in chat.

## Code style

- Always brace control-flow bodies, even for a single statement. Applies to `if`, `else`, `for`, `foreach`, `while`, `do`, `using` — in any language with C-style braces (C#, JS, TS, etc.).

  ```csharp
  // good
  if (orphan is not null)
  {
      throw new InvalidOperationException(...);
  }

  // bad
  if (orphan is not null)
      throw new InvalidOperationException(...);
  ```

- When a method declaration or a call is too long to read comfortably on one line, put **each parameter / argument on its own line**, indented one level from the method name. Same rule for both declaration and call sites. Don't half-wrap (two args on one line, the rest below).

  ```csharp
  // good — declaration
  public static ObfuscatedChatStats Obfuscate(
      ChatStats stats,
      RawChatInfo rawChat,
      double? timeJitterSeconds,
      double? durationJitterSeconds)
  {
      ...
  }

  // good — call
  var obfuscated = StatsObfuscator.Obfuscate(
      stats,
      rawChat,
      timeJitterSeconds,
      durationJitterSeconds);

  // bad
  public static ObfuscatedChatStats Obfuscate(ChatStats stats, RawChatInfo rawChat, double? timeJitterSeconds, double? durationJitterSeconds)
  ```

## Working style

- This is a junk drawer — prioritize getting things working over polish, abstraction, or test coverage.
- Don't add frameworks or build tooling without asking.
- Don't refactor unrelated files.

## Writing docs

When asked to write or edit docs (READMEs, AGENTS.md, comments), keep it short and plain.

- Use simple English. Non-native speakers read these files too — short sentences, common words, no idioms.
- Don't pad. Skip throat-clearing intros, recaps, and trailing "in summary" lines. If a sentence doesn't add information, delete it.
- Don't restate what's already documented elsewhere. Link or point to it.
- No emoji, no decorative headings, no marketing tone. Plain Markdown.
- Lists and short paragraphs over long prose. One idea per bullet.

## Subprojects

Subprojects (like `SoftmaxLab/`) can — and should — have their own instruction files when they need them. If a toy has its own conventions, dependencies, quirks, or a more detailed description of its own, document them **inside the subproject**, not here. Keep this central file lean.

The reverse also holds: anything cross-cutting — code style, formatting rules, language conventions that apply to more than one toy — belongs **here**, not in a subproject `AGENTS.md`. If you catch yourself adding the same note to two subproject files, pull it up to this one.

## `AGENTS.md` vs `CLAUDE.md`

This file (`AGENTS.md`) is the canonical instruction file for AI coding agents — a growing cross-tool convention (Codex, Cursor, Aider, etc. read it).

Claude Code itself looks for `CLAUDE.md`. To avoid duplication, `.claude/CLAUDE.md` is a one-line pointer that imports this file via `@../AGENTS.md`.

Subprojects follow the same pattern: a subproject-level `AGENTS.md` for the content, and a thin `CLAUDE.md` pointer that imports it. The pointer **must** live at `<subproject>/.claude/CLAUDE.md` (not at the subproject root). It must contain **only the import line and nothing else** — no `# CLAUDE.md` header, no boilerplate paragraph like "This file provides guidance to Claude Code…". The pointer is exactly:

```
@../AGENTS.md
```

Edit the `AGENTS.md`; leave the pointer alone.
