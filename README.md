## Laser Defense Game

### Overview

Laser Defender is an exciting wave-based shooter game developed using Unity. The game involves strategic dodging and shooting as players navigate through waves of laser-firing enemies. The game is built with several design patterns and principles to ensure maintainability, scalability, and efficiency.

### Features
- Wave-based gameplay: Survive through increasingly challenging waves of enemies.
- Power-ups: Collect power-ups to gain temporary abilities like shields and speed boosts.
- Laser mechanics: Avoid or destroy enemy lasers while strategically positioning yourself.
- Win and Lose Conditions: Survive all waves to win the game or get hit by a laser to lose.


### Controls

- **Movement:** Arrow keys or WASD
- **Jump:** Spacebar
- **Run:** Shift Key

### Design Principles

#### Object Pooling

Object Pooling is used to efficiently manage laser objects. Instead of creating and destroying laser instances frequently, which can be resource-intensive, the game reuses a pool of laser objects. This significantly enhances performance, especially during intense gameplay.

#### Event Service

The Event Service pattern is employed to decouple various parts of the game. It allows different game components to communicate without needing direct references to each other. For example, when the player dies, an event is triggered that other systems, like the UI Manager and Wave Manager, can respond to appropriately.

#### MVC Pattern

The game architecture follows the Model-View-Controller pattern:
- **Model:** Defines data structures such as player stats, wave data, and power-up properties.
- **View:** Visual representation of game elements like lasers, enemies, and UI.
- **Controller:** Handles game logic, user input, and orchestrates interactions between models and views.

#### Service Locator

The Service Locator pattern is used to provide a global point of access to various game services. It helps in managing dependencies and decouples the service consumers from the service implementations.

#### Singleton Design Pattern

Singletons are used for classes that need to be accessed globally and should only have one instance, such as GameManager, WaveManager, and PowerUpSpawnManager. This ensures a controlled access point to these critical game systems.

#### Single Responsibility Principle (SRP)

Each class in the game has a single responsibility. For instance, WaveManager handles wave logic, PowerUpSpawnManager deals with power-up spawning, and LaserController controls laser behavior. This makes the codebase easier to understand, maintain, and extend.

#### Open-Closed Principle for Power-ups

The game follows the Open/Closed Principle, particularly for power-ups. New power-ups can be added without modifying existing code. This is achieved through the use of the IPowerUp interface and a flexible factory pattern that allows easy integration of new power-up types.

### Credits

- Laser and Power-up assets by Unity Asset Store


