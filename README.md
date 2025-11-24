# Speed Rush - Time-Based Racing Game Simulation
🚗 SpeedRush – Time-Based Racing Simulation (WPF .NET)
SpeedRush is a turn-based, time-focused racing simulation game built with C# and WPF. Players select a car, manage fuel, speed, and time, and attempt to complete 5 laps before resources run out.

This project demonstrates key programming concepts:

- ✔ OOP Classes
- ✔ Control structures
- ✔ Data structures
- ✔ Exception handling
- ✔ WPF UI
- ✔ Unit testing
- ✔ XML documentation

## Features
### Core Gameplay
Choose from multiple car types (different speed, fuel capacity, fuel consumption).

- Race on a 5-lap track.
- Turn-based actions:
- Speed Up – increases speed but uses more fuel.
- Maintain Speed – balanced fuel usage.
- Refuel – refills fuel but costs time.
- Game ends when:
- All laps are completed, or
- Fuel runs out, or
- Time reaches zero.

## WPF User Interface
### The UI displays:

- Current lap (x/5)
- Fuel level (ProgressBar)
- Time remaining (ProgressBar)
- Speed / current status
- Action buttons
- Car selection dropdown

🏛 Project Structure
```
SpeedRush/
│── App.xaml
│── MainWindow.xaml
│── MainWindow.xaml.cs
│── Models/
│   ├── Car.cs
│   ├── Track.cs
│   └── RaceManager.cs
│── Tests/
│   ├── FuelTests.cs
│   ├── LapTests.cs
│   └── SpeedTests.cs
│── README.md
└── SpeedRush.csproj
```
## How to Play
### Run the application.

- Choose a car from the dropdown.
- Click Start Race.
- Each turn, choose one action:
- Speed Up: go faster, burn more fuel.
- Maintain Speed: keep steady.
- Pit Stop: refuel but lose time.
- Watch your:
- Fuel bar
- Time bar
- Lap counter

Finish all 5 laps before time/fuel runs out to win.
## Car Specifications

| Car Type | Brand | Max Speed | Fuel Capacity | Fuel Consumption | Strategy |
|----------|-------|-----------|---------------|------------------|----------|
| Sports Car | Ford | 110 mph | 60.0 L | 7.0 L/turn | Balanced approach |
| Eco Car | Chevrolet | 75 mph | 85.0 L | 4.5 L/turn | Endurance racing |
| Race Car | Dodge | 130 mph | 45.0 L | 13.0 L/turn | High-risk, high-reward |
| Sports Car | Benz | 105 mph | 70.0 L | 6.5 L/turn | Premium balance |

## How to Run the Project

### Prerequisites
- .NET 6.0 or later
- Windows operating system (for WPF support)
- Visual Studio 2022 or Visual Studio Code with C# extension

Run Steps

Clone the repository:
```
git clone https://github.com/Davdesigner/SpeedRush.git
```

2. Open SpeedRush.csproj in Visual Studio.

3. Press F5 or click Start to run.

## Unit Tests

Located in the Tests/ folder:

| Test Name         | Purpose                                      |
| ----------------- | -------------------------------------------- |
| **FuelTests.cs**  | Verifies correct fuel consumption per action |
| **LapTests.cs**   | Ensures laps progress correctly              |
| **SpeedTests.cs** | Validates speed increase and status effects  |


Run tests in Visual Studio using Test Explorer.

## Architecture Overview
### Components:

- Car Class: Holds speed, fuel capacity, and consumption.
- RaceManager: Main game logic controller.
- Track: Handles laps and distance.
- WPF UI: Displays all real-time stats.

## Design Choices

- List<Car> used for car selection.
- Enum ActionType { SpeedUp, Maintain, Refuel } for turn actions.
- Exception Handling used for:
- Fuel overfill attempts
- Invalid actions
- Out-of-range speed

## Demo Video

click the link to view the project demonstration: https://drive.google.com/file/d/1BaFnCEFfMRb1PKWJI7m47DGnrKclyg_W/view?usp=sharing 

## License

This project is created for educational purposes under course requirements.

done by David Ubushakebwimana
