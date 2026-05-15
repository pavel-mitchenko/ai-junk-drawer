# ChatLab

This is a subproject. Put any cross-cutting rule (code style, doc style, working style, public-repo warnings) in `../AGENTS.md`, not here — this file only carries ChatLab-specific notes.

See `README.md` for the one-line description.

## Layout

- `index.html` — the viewer. Renders activity charts (message volume over time, top posters, busiest hours) from a pre-shaped JSON file. Currently a placeholder.
- `src/ChatLab.Cli/` — small .NET CLI. Takes raw Telegram chat exports and reshapes them into the JSON format the viewer expects. Lets the same viewer work on any exported chat.

## Conventions

- Viewer: vanilla HTML + JS, inline `<style>`, no build step. Pull chart libs via CDN when needed.
- `ChatLab.Cli`: single-purpose .NET CLI. Input is a raw export, output is viewer-ready JSON. Keep it boring and scriptable.
