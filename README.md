# EVI – A Mini‑Planetary Expedition

**EVI** is a hidden‑object space exploration game built with Unity (WebGL) and embedded in my portfolio contact page.  
It rewards curiosity with a short, self‑contained journey across five planets, a flag‑planting mission, and a collect‑the‑gems objective.

> 👩‍🚀 *“Want to play a game?”* – Evi, the astronaut

---

## 🎮 Gameplay

- **Goal**  
  Visit 5 planets, plant a flag on each, collect one unique gem per planet, and return all gems to Earth.

- **Core loop**  
  1. Choose a planet  
  2. Fly there using simple 2D physics (thrust, gravity, orbit)  
  3. Land (forgiving, satisfying landing)  
  4. Plant a flag & collect the gem  
  5. Return to Earth  
  6. Repeat for all 5 planets

- **Completion**  
  After all 5 gems are returned, Evi thanks the player and the contact form appears.

- **Persistence**  
  Progress is saved automatically (via PlayerPrefs + localStorage bridge).  
  Completed planets stay completed. Gems stay collected.

---

## 🕹️ Controls

| Action         | Input               |
|----------------|---------------------|
| Thrust          | `W` / `Up Arrow`    |
| Rotate left     | `A` / `Left Arrow`  |
| Rotate right    | `D` / `Right Arrow` |
| Land / Confirm  | `Space` / `Enter`   |

---

## 🧠 Design Philosophy

- **Reward curiosity** – hidden inside the contact page, no forced pop‑ups.
- **Simple but complete** – a small game with a clear win state.
- **Portfolio as playground** – demonstrates Unity WebGL integration, save states, and player‑centric design.

---

## 🛠️ Technical Notes

| Area               | Implementation                                      |
|--------------------|-----------------------------------------------------|
| Engine             | Unity (WebGL build)                                 |
| Embedding          | `<iframe>` / Unity template inside Angular page     |
| Save system        | PlayerPrefs (Unity) + localStorage bridge           |
| Communication      | Unity ↔ JavaScript (`SendMessage` / `ReceiveFromJS`)|
| Performance target | Loads in <5s on average connection, no heavy assets |

---

## 📁 Repository Structure (No License)

```
EVI/
├── Build/            # WebGL build files
├── Assets/           # Unity assets (scenes, scripts, sprites, prefabs)
├── ProjectSettings/  # Unity project settings
└── README.md         # This file
```
---

## 🙏 Credits

| Role          | Name                                     |
|---------------|------------------------------------------|
| Concept, Code, Art, Design | Youssef Amr                    |
| Character     | Evi (astronaut – born from a smart‑assistant robot) |
| Special thanks| Curiosity, the sky, and one more day     |

---

## 🔗 Where to Play

The game is hidden inside the **contact page** of my portfolio.  
Find Evi. She’ll ask if you want to play.

---

## ⚖️ License

**No license – All rights reserved.**

This project is shared for **viewing and portfolio purposes only**.  
You may not copy, modify, distribute, or use any part of this game or its assets without explicit written permission from the author.

> 👤 **Author:** Youssef Amr – *The Solo Dev*
