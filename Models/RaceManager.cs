using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace CSharpSpeedRush.Models
{
    /// <summary>
    /// Manages the overall race simulation and game state
    /// </summary>
    public class RaceManager
    {
        private readonly DispatcherTimer? _gameTimer;
        private DateTime _raceStartTime;

        /// <summary>
        /// Gets the current car being used in the race
        /// </summary>
        public Car? CurrentCar { get; private set; }

        /// <summary>
        /// Gets the race track
        /// </summary>
        public Track Track { get; }

        /// <summary>
        /// Gets the current game state
        /// </summary>
        public GameState GameState { get; private set; }

        /// <summary>
        /// Gets the time remaining in the race (in seconds)
        /// </summary>
        public double TimeRemaining { get; internal set; }

        /// <summary>
        /// Gets the maximum time allowed for the race
        /// </summary>
        public double MaxRaceTime { get; }

        /// <summary>
        /// Gets the available car types
        /// </summary>
        public List<Car> AvailableCars { get; }

        /// <summary>
        /// Event raised when the game state changes
        /// </summary>
        public event EventHandler<GameState>? GameStateChanged;

        /// <summary>
        /// Event raised when game data is updated
        /// </summary>
        public event EventHandler? GameDataUpdated;

        /// <summary>
        /// Initializes a new instance of the RaceManager class
        /// </summary>
        /// <param name="maxRaceTime">Maximum time allowed for the race in seconds</param>
        /// <param name="enableRealTimeDecay">Whether to enable real-time fuel/time decay</param>
        public RaceManager(double maxRaceTime = 300, bool enableRealTimeDecay = false)
        {
            Track = new Track();
            GameState = GameState.NotStarted;
            MaxRaceTime = maxRaceTime;
            TimeRemaining = maxRaceTime;
            
            AvailableCars = CreateAvailableCars();

            if (enableRealTimeDecay)
            {
                _gameTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                _gameTimer.Tick += OnGameTimerTick;
            }
        }

        /// <summary>
        /// Creates the list of available cars with different characteristics
        /// </summary>
        /// <returns>List of available cars</returns>
        private List<Car> CreateAvailableCars()
        {
            return new List<Car>
            {
                // Four available brands requested: Ford, Chevrolet, Dodge, Benz
                // Max speed set to 100 mph for all cars per request
                new Car(CarType.SportsCar, "Ford", 100, 7.0, 60.0),
                new Car(CarType.EcoCar, "Chevrolet", 100, 4.5, 85.0),
                new Car(CarType.RaceCar, "Dodge", 100, 13.0, 45.0),
                new Car(CarType.SportsCar, "Benz", 100, 6.5, 70.0)
            };
        }

        /// <summary>
        /// Starts a new race with the selected car
        /// </summary>
        /// <param name="selectedCar">The car to use for the race</param>
        /// <exception cref="ArgumentNullException">Thrown when selectedCar is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when game is already in progress</exception>
        public void StartRace(Car selectedCar)
        {
            if (selectedCar == null)
                throw new ArgumentNullException(nameof(selectedCar));

            if (GameState == GameState.InProgress)
                throw new InvalidOperationException("Race is already in progress");

            CurrentCar = selectedCar;
            CurrentCar.CurrentFuel = CurrentCar.MaxFuelCapacity;
            CurrentCar.CurrentSpeed = 0;
            
            GameState = GameState.InProgress;
            TimeRemaining = MaxRaceTime;
            _raceStartTime = DateTime.Now;
            
            _gameTimer?.Start();
            
            GameStateChanged?.Invoke(this, GameState);
            GameDataUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Processes a player action during their turn
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <exception cref="InvalidOperationException">Thrown when game is not in progress or car is null</exception>
        public void ProcessPlayerAction(PlayerAction action)
        {
            if (GameState != GameState.InProgress)
                throw new InvalidOperationException("Game is not in progress");

            if (CurrentCar == null)
                throw new InvalidOperationException("No car selected");

            try
            {
                switch (action)
                {
                    case PlayerAction.SpeedUp:
                        CurrentCar.SpeedUp(5);
                        break;
                    case PlayerAction.MaintainSpeed:
                        // Speed remains the same
                        break;
                    case PlayerAction.PitStop:
                        CurrentCar.Refuel();
                        CurrentCar.CurrentSpeed = Math.Max(0, CurrentCar.CurrentSpeed - 20); // Slow down during pit stop
                        break;
                }

                // Consume fuel and advance position
                CurrentCar.ConsumeFuel();
                Track.AdvancePosition(CurrentCar.CurrentSpeed);
                
                // Consume time (each action takes 5-10 seconds based on speed)
                double timeConsumed = 10 - (CurrentCar.CurrentSpeed / 20.0);
                TimeRemaining = Math.Max(0, TimeRemaining - timeConsumed);

                CheckGameEndConditions();
                GameDataUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Action failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the game should end based on current conditions
        /// </summary>
        private void CheckGameEndConditions()
        {
            if (CurrentCar == null) return;

            if (Track.IsRaceComplete())
            {
                EndGame(GameState.Completed);
            }
            else if (!CurrentCar.HasFuel() || TimeRemaining <= 0)
            {
                EndGame(GameState.GameOver);
            }
        }

        /// <summary>
        /// Ends the current game
        /// </summary>
        /// <param name="endState">The final game state</param>
        private void EndGame(GameState endState)
        {
            GameState = endState;
            _gameTimer?.Stop();
            GameStateChanged?.Invoke(this, GameState);
        }

        /// <summary>
        /// Resets the game to initial state
        /// </summary>
        public void ResetGame()
        {
            _gameTimer?.Stop();
            GameState = GameState.NotStarted;
            CurrentCar = null;
            TimeRemaining = MaxRaceTime;
            
            // Reset track
            var newTrack = new Track();
            typeof(Track).GetProperty(nameof(Track.CurrentLap))?.SetValue(Track, 1);
            
            GameStateChanged?.Invoke(this, GameState);
            GameDataUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the game timer tick for real-time decay
        /// </summary>
        private void OnGameTimerTick(object? sender, EventArgs e)
        {
            if (GameState != GameState.InProgress || CurrentCar == null)
                return;

            // Gradual fuel decay over time
            CurrentCar.CurrentFuel = Math.Max(0, CurrentCar.CurrentFuel - 0.1);
            TimeRemaining = Math.Max(0, TimeRemaining - 1);

            CheckGameEndConditions();
            GameDataUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the elapsed race time
        /// </summary>
        /// <returns>Elapsed time as TimeSpan</returns>
        public TimeSpan GetElapsedTime()
        {
            if (GameState == GameState.NotStarted)
                return TimeSpan.Zero;
            
            return DateTime.Now - _raceStartTime;
        }

        /// <summary>
        /// Gets the time remaining as a percentage
        /// </summary>
        /// <returns>Time remaining percentage (0.0 to 1.0)</returns>
        public double GetTimeRemainingPercentage() => TimeRemaining / MaxRaceTime;
    }
}