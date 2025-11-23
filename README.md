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

- * Race on a 5-lap track.
- * Turn-based actions:
- * Speed Up – increases speed but uses more fuel.
- * Maintain Speed – balanced fuel usage.
- * Refuel – refills fuel but costs time.
- * Game ends when:
- * All laps are completed, or
- * Fuel runs out, or
- * Time reaches zero.

## WPF User Interface
### The UI displays:

- * Current lap (x/5)
- * Fuel level (ProgressBar)
- * Time remaining (ProgressBar)
- * Speed / current status
- * Action buttons
- * Car selection dropdown

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

### Running the Game
1. **Clone or Download** the project files
2. **Open Terminal/Command Prompt** in the project directory
3. **Build the Project**:
   ```bash
   dotnet build
   ```
4. **Run the Game**:
   ```bash
   dotnet run
   ```

### Running Tests
To run the unit tests:
```bash
cd UnitTests
dotnet test
```

### Alternative: Using Visual Studio
1. Open `CSharpSpeedRush.csproj` in Visual Studio
2. Press F5 to build and run the application
3. Use Test Explorer to run unit tests

## Project Structure

```
CSharpSpeedRush/
├── Models/
│   ├── Car.cs              # Car class with attributes and methods
│   ├── Track.cs            # Track management and lap progression
│   ├── RaceManager.cs      # Game state and logic management
│   └── GameEnums.cs        # Enums for car types, game states, and actions
├── UnitTests/
│   ├── CarTests.cs         # Unit tests for Car class
│   ├── TrackTests.cs       # Unit tests for Track class
│   └── RaceManagerTests.cs # Unit tests for RaceManager class
├── MainWindow.xaml         # Main UI layout
├── MainWindow.xaml.cs      # UI logic and event handling
├── App.xaml               # Application configuration
├── App.xaml.cs            # Application startup logic
└── README.md              # This file
```

## Technical Features

### Object-Oriented Design
- **Car Class**: Encapsulates car properties and behaviors
- **Track Class**: Manages race progress and lap completion
- **RaceManager Class**: Coordinates game state and logic
- **Proper Encapsulation**: Private fields with public properties and methods

### Data Structures Used
- **List<Car>**: Stores available car options
- **List<TimeSpan>**: Tracks lap completion times
- **Enums**: CarType, GameState, PlayerAction for type safety
- **Events**: For UI updates and state change notifications

### Exception Handling
- **InvalidOperationException**: For invalid game actions (exceeding max speed, refueling full tank)
- **ArgumentNullException**: For null parameter validation
- **Try-Catch Blocks**: In UI event handlers for graceful error handling

### WPF Features
- **Data Binding**: Real-time UI updates
- **Progress Bars**: Visual fuel and time indicators
- **Event-Driven Architecture**: Responsive user interface
- **XAML Layout**: Professional UI design

### Optional Enhancements Implemented
- **Real-Time Decay**: Optional DispatcherTimer for gradual fuel/time consumption
- **Car-Specific Strategies**: Different performance characteristics per car type
- **Enhanced Visuals**: Progress indicators and color-coded status bars

## Game Strategy Tips

1. **Choose Your Car Wisely**: 
   - Eco Car for beginners (forgiving fuel consumption)
   - Sports Car for balanced gameplay
   - Race Car for experienced players seeking challenge

2. **Fuel Management**:
   - Monitor fuel levels constantly
   - Plan pit stops strategically
   - Don't wait until fuel is critically low

3. **Speed vs. Efficiency**:
   - Higher speeds complete laps faster but consume more fuel
   - Sometimes maintaining speed is better than speeding up
   - Use pit stops to reset your strategy

4. **Time Management**:
   - You have 5 minutes total
   - Each action consumes time based on current speed
   - Balance speed with fuel efficiency for optimal time usage

## Development Notes

This project demonstrates:
- **Clean Architecture**: Separation of concerns between UI, business logic, and data models
- **SOLID Principles**: Single responsibility, open/closed, and dependency inversion
- **Test-Driven Development**: Comprehensive unit test coverage
- **Exception Safety**: Robust error handling throughout the application
- **Documentation**: XML documentation for all public methods and classes

## Future Enhancements

Potential improvements could include:
- Multiple track types with different characteristics
- Weather conditions affecting performance
- Multiplayer racing capabilities
- Save/load game functionality
- Achievement system
- Sound effects and animations

---

**Developed as part of a .NET WPF learning project focusing on object-oriented programming, data structures, exception handling, and user interface design.**
