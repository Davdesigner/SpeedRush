# Copilot / AI Agent Instructions

This project is a small .NET WPF turn-based racing simulation. The goal of these instructions is to make an AI coding agent productive quickly by highlighting the project's architecture, workflows, conventions, and important code locations to reference.

Quick facts
- Language: C# (WPF, .NET 6+/8 support)
- UI: WPF XAML (`MainWindow.xaml`, `MainWindow.xaml.cs`)
- Domain: `Models/` contains core game logic (`Car.cs`, `Track.cs`, `RaceManager.cs`, `GameEnums.cs`)
- Tests: `UnitTests/` (MSTest - `Microsoft.VisualStudio.TestTools.UnitTesting`)

Primary design/architecture to know
- `RaceManager` (Models/RaceManager.cs): central orchestrator for game state. It exposes `CurrentCar`, `Track`, `AvailableCars`, `TimeRemaining`, `GameState`, and two key events: `GameStateChanged` and `GameDataUpdated`. The UI subscribes to these events.
- `Car` (Models/Car.cs): domain model for vehicle behavior (speed changes, fuel consumption, refuel). Methods commonly throw `InvalidOperationException` for invalid operations — callers often catch and display errors.
- `Track` (Models/Track.cs): lap and progress management. `RaceManager` advances the track after each action.
- UI (MainWindow.xaml.cs): subscribes to `RaceManager` events and always uses `Dispatcher.Invoke` to update UI elements. This means that event handlers may be called from background timers; ensure UI updates are marshalled to the UI thread.

Notable implementation details and gotchas
- Real-time decay: `RaceManager` optionally uses a `DispatcherTimer` when constructed with `enableRealTimeDecay=true`. Tests and the UI expect both deterministic (turn-based) and optional real-time modes.
- ResetGame uses reflection to reset track lap state (`typeof(Track).GetProperty(nameof(Track.CurrentLap))?.SetValue(Track, 1);`) — be careful when refactoring `Track` properties: ResetGame assumes `CurrentLap` exists and is writable.
- Time consumption formula: `TimeRemaining` is decremented in `ProcessPlayerAction` using `10 - (CurrentCar.CurrentSpeed / 20.0)`. If you change speed/turn semantics, update related tests.
- Error handling: Model methods throw `InvalidOperationException` on invalid actions. The UI displays these messages (see MainWindow's `try/catch` around `_raceManager` calls). Keep that contract stable.

Build / test / debug workflows
- Build solution (recommended):
  - `dotnet build CSharpSpeedRush.sln`
- Run (WPF; Windows only):
  - Preferred: open the solution in Visual Studio 2022/2023 and press F5 (WPF requires Windows desktop environment)
  - CLI (works on Windows with appropriate SDKs): `dotnet run --project CSharpSpeedRush.csproj`
- Tests (MSTest):
  - From repo root: `dotnet test ./UnitTests/CSharpSpeedRush.Tests.csproj`
  - To target a framework explicitly: `dotnet test ./UnitTests/CSharpSpeedRush.Tests.csproj -f net6.0-windows`

Conventions and patterns to follow
- Small, focused classes in `Models/` — respect Single Responsibility when adding features.
- Events-based UI updates: make changes to `RaceManager` emit `GameDataUpdated` after state mutations so the UI remains in sync.
- Exceptions: use `InvalidOperationException` for invalid game actions (existing tests expect this). Avoid changing exception types without updating tests.
- Tests are authoritative: modify or add tests in `UnitTests/` when changing logic. Tests use MSTest attributes (`[TestClass]`, `[TestMethod]`, `[TestInitialize]`).

Files to read first (high ROI)
- `Models/RaceManager.cs` — game loop, eventing, time/fuel logic
- `Models/Car.cs` — speed/fuel/refuel behavior and exception cases
- `Models/Track.cs` — lap progression and progress indicator helpers
- `MainWindow.xaml.cs` — how UI subscribes and displays model data
- `UnitTests/*` — unit tests demonstrate expected behavior and edge cases

When making changes, checklist for PRs
- Update or add unit tests in `UnitTests/` covering the changed behavior
- Ensure `RaceManager` still raises `GameStateChanged` and `GameDataUpdated` at the same meaningful points
- Keep public method contracts and exception types stable or clearly document changes
- Run `dotnet build` and `dotnet test` locally on Windows before submitting changes

Example prompts for an AI agent working in this repo
- "Add a unit test in `UnitTests` that asserts `TimeRemaining` decreases when `ProcessPlayerAction` is called at `CurrentSpeed=40`. Use MSTest." 
- "Refactor `RaceManager.ResetGame` to avoid reflection; update usages and tests accordingly." 
- "Fix a UI bug where `FuelProgressBar` color doesn't update when fuel hits exactly 20%. Trace events from `RaceManager` to `MainWindow` handlers."

If anything here is unclear or you want more detail (e.g., list of public methods in `Car.cs` or exact test coverage gaps), tell me which area to expand and I'll update this file.
