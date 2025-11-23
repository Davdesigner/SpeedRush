using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CSharpSpeedRush.Models;

namespace CSharpSpeedRush
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RaceManager _raceManager;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _raceManager = new RaceManager(300, true); // 5 minutes with real-time decay
            
            InitializeEventHandlers();
            LoadAvailableCars();
            UpdateUI();
        }

        /// <summary>
        /// Wire up RaceManager events to UI handlers
        /// </summary>
        private void InitializeEventHandlers()
        {
            _raceManager.GameStateChanged += OnGameStateChanged;
            _raceManager.GameDataUpdated += OnGameDataUpdated;
        }

        /// <summary>
        /// Populate car selection ComboBox from RaceManager available cars
        /// </summary>
        private void LoadAvailableCars()
        {
            CarSelectionComboBox.ItemsSource = _raceManager.AvailableCars;
        }

        /// <summary>
        /// Handles car selection change in the ComboBox
        /// </summary>
        private void CarSelectionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StartRaceButton.IsEnabled = CarSelectionComboBox.SelectedItem != null;
            if (CarSelectionComboBox.SelectedItem is Car selectedCar)
            {
                DisplayCarInfo(selectedCar);
            }
        }

        /// <summary>
        /// Display selected car details in the right panel
        /// </summary>
        private void DisplayCarInfo(Car car)
        {
            CarInfoPanel.Visibility = Visibility.Visible;
            CarNameLabel.Text = car.Name;
            CarTypeLabel.Text = car.Type.ToString();
            MaxSpeedLabel.Text = $"{car.MaxSpeed} mph";
            FuelCapacityLabel.Text = $"{car.MaxFuelCapacity:F1} L";
        }
        /// <summary>
        /// Handles the start race button click
        /// </summary>
        private void StartRaceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CarSelectionComboBox.SelectedItem is Car selectedCar)
                {
                    _raceManager.StartRace(selectedCar);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start race: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the speed up button click
        /// </summary>
        private void SpeedUpButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessPlayerAction(PlayerAction.SpeedUp);
        }

        /// <summary>
        /// Handles the maintain speed button click
        /// </summary>
        private void MaintainSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessPlayerAction(PlayerAction.MaintainSpeed);
        }

        /// <summary>
        /// Handles the pit stop button click
        /// </summary>
        private void PitStopButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessPlayerAction(PlayerAction.PitStop);
        }

        /// <summary>
        /// Processes a player action and handles any exceptions
        /// </summary>
        /// <param name="action">The action to process</param>
        private void ProcessPlayerAction(PlayerAction action)
        {
            try
            {
                _raceManager.ProcessPlayerAction(action);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Action Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Handles the reset game button click
        /// </summary>
        private void ResetGameButton_Click(object sender, RoutedEventArgs e)
        {
            _raceManager.ResetGame();
            CarSelectionComboBox.SelectedItem = null;
            CarInfoPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Handles the exit button click
        /// </summary>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles game state changes
        /// </summary>
        private void OnGameStateChanged(object? sender, GameState gameState)
        {
            Dispatcher.Invoke(() =>
            {
                switch (gameState)
                {
                    case GameState.NotStarted:
                        EnableGameControls(false);
                        GameStatusLabel.Text = "Select a car to start racing!";
                        GameStatusLabel.Foreground = Brushes.DarkGreen;
                        break;
                    
                    case GameState.InProgress:
                        EnableGameControls(true);
                        GameStatusLabel.Text = "Race in progress! Make your moves!";
                        GameStatusLabel.Foreground = Brushes.Blue;
                        break;
                    
                    case GameState.Completed:
                        EnableGameControls(false);
                        GameStatusLabel.Text = "Congratulations! You completed the race!";
                        GameStatusLabel.Foreground = Brushes.Green;
                        ShowRaceResults();
                        break;
                    
                    case GameState.GameOver:
                        EnableGameControls(false);
                        GameStatusLabel.Text = "Game Over! You ran out of fuel or time.";
                        GameStatusLabel.Foreground = Brushes.Red;
                        ShowRaceResults();
                        break;
                }
            });
        }

        /// <summary>
        /// Handles game data updates
        /// </summary>
        private void OnGameDataUpdated(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(UpdateUI);
        }

        /// <summary>
        /// Updates the user interface with current game data
        /// </summary>
        private void UpdateUI()
        {
            if (_raceManager.CurrentCar == null) return;

            var car = _raceManager.CurrentCar;
            var track = _raceManager.Track;

            // Update lap information
            CurrentLapLabel.Text = track.IsRaceComplete() ? 
                $"{track.TotalLaps}/{track.TotalLaps}" : 
                $"{track.CurrentLap}/{track.TotalLaps}";

            // Update fuel information
            double fuelPercentage = car.GetFuelPercentage() * 100;
            FuelLevelLabel.Text = $"{fuelPercentage:F1}%";
            FuelProgressBar.Value = fuelPercentage;
            
            // Change fuel bar color based on level (new theme colors)
            if (fuelPercentage < 20)
                FuelProgressBar.Foreground = Brushes.Crimson;
            else if (fuelPercentage < 50)
                FuelProgressBar.Foreground = Brushes.Goldenrod;
            else
                FuelProgressBar.Foreground = Brushes.SeaGreen;

            // Update time information
            double timePercentage = _raceManager.GetTimeRemainingPercentage() * 100;
            TimeSpan timeRemaining = TimeSpan.FromSeconds(_raceManager.TimeRemaining);
            TimeRemainingLabel.Text = $"{timeRemaining.Minutes}:{timeRemaining.Seconds:D2}";
            TimeProgressBar.Value = timePercentage;
            
            // Change time bar color based on remaining time (new theme colors)
            if (timePercentage < 20)
                TimeProgressBar.Foreground = Brushes.Crimson;
            else if (timePercentage < 50)
                TimeProgressBar.Foreground = Brushes.Goldenrod;
            else
                TimeProgressBar.Foreground = Brushes.MidnightBlue;

            // Update speed
            SpeedLabel.Text = $"{car.CurrentSpeed} mph";

            // Update textual progress indicator from Track
            try
            {
                ProgressIndicatorLabel.Text = track.GetProgressIndicator();
            }
            catch
            {
                // If track or method not available, fall back silently
            }
        }

        /// <summary>
        /// Enables or disables game control buttons
        /// </summary>
        /// <param name="enabled">Whether to enable the controls</param>
        private void EnableGameControls(bool enabled)
        {
            SpeedUpButton.IsEnabled = enabled;
            MaintainSpeedButton.IsEnabled = enabled;
            PitStopButton.IsEnabled = enabled;
            StartRaceButton.IsEnabled = !enabled && CarSelectionComboBox.SelectedItem != null;
            CarSelectionComboBox.IsEnabled = !enabled;
        }

        /// <summary>
        /// Shows race results when the game ends
        /// </summary>
        private void ShowRaceResults()
        {
            if (_raceManager.CurrentCar == null) return;

            var elapsedTime = _raceManager.GetElapsedTime();
            var completedLaps = _raceManager.Track.CurrentLap - 1;
            var finalFuel = _raceManager.CurrentCar.CurrentFuel;
            
            string message = _raceManager.GameState == GameState.Completed ?
                $"Race Completed!\n\nTime: {elapsedTime.Minutes}:{elapsedTime.Seconds:D2}\nLaps: {completedLaps}/{_raceManager.Track.TotalLaps}\nFuel Remaining: {finalFuel:F1}L" :
                $"Race Failed!\n\nTime: {elapsedTime.Minutes}:{elapsedTime.Seconds:D2}\nLaps Completed: {completedLaps}/{_raceManager.Track.TotalLaps}\nFuel Remaining: {finalFuel:F1}L";

            MessageBox.Show(message, "Race Results", MessageBoxButton.OK, 
                _raceManager.GameState == GameState.Completed ? MessageBoxImage.Information : MessageBoxImage.Warning);
        }
    }
}