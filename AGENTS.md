# AI Junk Drawer

A loose collection of small standalone web tools, toys, and experiments built with help from LLMs. Not a product — no shared build, no framework, no test suite.

## Layout

- Root contains independent static HTML files (`index.html`, `2.html`, `3.html`, etc.) and standalone toys.
- `experiments/` — sandbox for quick spike pages (e.g. `cors-fetch.html`).
- `SoftmaxLab/` — a separate sub-toy.
- `test.json` — fixture used by CORS experiments.

Each file is intended to work on its own when opened directly in a browser. There is no bundler.

## Conventions

- Plain HTML + vanilla JS + inline `<style>`. No build step, no npm install required.
- Keep new toys self-contained in a single file when possible.
- For experiments, prefer `experiments/<name>.html`.
- UI text defaults to English unless the toy is clearly Russian-language.

## Working style

- This is a junk drawer — prioritize getting things working over polish, abstraction, or test coverage.
- Don't add frameworks or build tooling without asking.
- Don't refactor unrelated files.
