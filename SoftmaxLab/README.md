# Softmax Lab
 
An interactive playground for building intuition about how softmax behaves in language models. Move a slider, watch probabilities respond.

## Screenshots
<img src="screenshots/01.png" alt="Default view" width="600">
 
## What you can do
 
- Edit any logit by clicking its bar
- Adjust temperature on a log-scaled slider (0.01 to 50)
- Toggle top-k and top-p sampling
- Fill logits with random values, an arithmetic progression ("steps"), or a constant
- Lock the probabilities scale to 0–100% to compare distributions across runs
- Switch between English and Russian
## How to run
 
Open `index.html` in a browser. That's it. No build, no install, no server.
 
---
 
## For future contributors (or future me)
 
Single-file vanilla HTML/CSS/JS. Chart.js 4 from CDN. No frameworks, no bundler, no state persistence — reload is a clean slate by design.
 
### Math
 
Standard numerically-stable softmax: `p_i = exp(l_i/T - max) / Σ exp(l_k/T - max)`. Top-k and top-p are applied to the resulting probabilities, then the surviving subset is re-normalized to sum to 1.
 
A useful identity the **Steps** preset is built around: at T=1, two logits differing by Δ produce probabilities whose ratio is `exp(Δ)`. So Δ = ln(2) ≈ 0.693 → 2× ratio.
 
### Design decisions worth preserving
 
These are deliberate, not accidents — please don't "improve" them without thinking:
 
- **Light theme only.** No dark mode, no gradients, no neon. Accent `#8b5cf6`. SVG icons, no emoji.
- **System font stack** with tabular numerals so digits don't jitter on update.
- **Bar thickness capped at 28px.** Without this, 2-logit charts become two huge blocks.
- **Phantom slots when n < 15.** Real bars stay grouped left at consistent thickness; tooltips and clicks on phantoms are filtered out.
- **Temperature is log-scaled** (0.01–50). Equal slider movement = equal *ratio* of temperatures. T=1 sits near the middle.
- **Precision: logits 3 decimals, temperature 2, top-p 2, probabilities as percent with 2 decimals.** Logits get 3 decimals so the Steps preset (Δ = ln(2) = 0.693…) doesn't round badly.
- **Number-input spinners hidden** in the fill section (manual entry only).
- **Sidebar reading order: parameter, then action.** `from / to` above `Random`, `step` next to `Steps`, `value` next to `Fill`.
### i18n
 
Two-language dictionary (`I18N.ru`, `I18N.en`) at the top of the script. Translatable elements use `data-i18n`, `data-i18n-title`, or `data-i18n-aria` attributes. Add a key to both languages, attach the attribute, done. No library.
 
 
### License
 
MIT.