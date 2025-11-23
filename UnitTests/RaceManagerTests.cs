using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpSpeedRush.Models;
using System;
using System.Linq;

namespace CSharpSpeedRush.Tests
{
    /// <summary>
    /// Unit tests for the RaceManager class
    /// </summary>
    [TestClass]
    public class RaceManagerTests
    {
        private RaceManager _raceManager = null!;
        private Car _testCar = null!;

        /// <summary>
        /// Initializes test data before each test
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _raceManager = new RaceManager(300, false); // 5 minutes, no real-time decay for testing
            _testCar = _raceManager.AvailableCars.First();
        }

        /// <summary>
        /// Tests that RaceManager initialization sets correct values
        /// </summary>
        [TestMethod]
        public void RaceManager_Initialization_SetsCorrectValues()
        {
            // Arrange & Act
            var manager = new RaceManager(600, false);

            // Assert
            Assert.AreEqual(GameState.NotStarted, manager.GameState);
            Assert.AreEqual(600, manager.MaxRaceTime);
            Assert.AreEqual(600, manager.TimeRemaining);
            Assert.IsNull(manager.CurrentCar);
            Assert.IsNotNull(manager.Track);
            Assert.IsNotNull(manager.AvailableCars);
            Assert.IsTrue(manager.AvailableCars.Count > 0);
        }

        /// <summary>
        /// Tests that available cars are created with different characteristics
        /// </summary>
        [TestMethod]
        public void AvailableCars_ContainsDifferentCarTypes()
        {
            // Act
            var cars = _raceManager.AvailableCars;

            // Assert
            Assert.AreEqual(4, cars.Count);
            Assert.IsTrue(cars.Any(c => c.Type == CarType.SportsCar));
            Assert.IsTrue(cars.Any(c => c.Type == CarType.EcoCar));
            Assert.IsTrue(cars.Any(c => c.Type == CarType.RaceCar));
        }

        /// <summary>
        /// Tests that StartRace initializes game correctly
        /// </summary>
        [TestMethod]
        public void StartRace_InitializesGameCorrectly()
        {
            // Arrange
            bool gameStateChanged = false;
            _raceManager.GameStateChanged += (s, state) => gameStateChanged = true;

            // Act
            _raceManager.StartRace(_testCar);

            // Assert
            Assert.AreEqual(GameState.InProgress, _raceManager.GameState);
            Assert.AreEqual(_testCar, _raceManager.CurrentCar);
            Assert.AreEqual(_testCar.MaxFuelCapacity, _testCar.CurrentFuel);
            Assert.AreEqual(0, _testCar.CurrentSpeed);
            Assert.IsTrue(gameStateChanged);
        }

        /// <summary>
        /// Tests that StartRace throws exception with null car
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartRace_ThrowsException_WithNullCar()
        {
            // Act
            _raceManager.StartRace(null!);

            // Assert - Exception expected
        }

        /// <summary>
        /// Tests that StartRace throws exception when game is already in progress
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartRace_ThrowsException_WhenGameInProgress()
        {
            // Arrange
            _raceManager.StartRace(_testCar);

            // Act
            _raceManager.StartRace(_testCar); // Try to start again

            // Assert - Exception expected
        }

        /// <summary>
        /// Tests ProcessPlayerAction with SpeedUp action
        /// </summary>
        [TestMethod]
        public void ProcessPlayerAction_SpeedUp_IncreasesSpeed()
        {
            // Arrange
            _raceManager.StartRace(_testCar);
            int initialSpeed = _testCar.CurrentSpeed;

            // Act
            _raceManager.ProcessPlayerAction(PlayerAction.SpeedUp);

            // Assert
            Assert.IsTrue(_testCar.CurrentSpeed > initialSpeed);
        }

        /// <summary>
        /// Tests ProcessPlayerAction with MaintainSpeed action
        /// </summary>
        [TestMethod]
        public void ProcessPlayerAction_MaintainSpeed_KeepsSpeed()
        {
            // Arrange
            _raceManager.StartRace(_testCar);
            _testCar.CurrentSpeed = 50;
            int initialSpeed = _testCar.CurrentSpeed;

            // Act
            _raceManager.ProcessPlayerAction(PlayerAction.MaintainSpeed);

            // Assert
            Assert.AreEqual(initialSpeed, _testCar.CurrentSpeed);
        }

        /// <summary>
        /// Tests ProcessPlayerAction with PitStop action
        /// </summary>
        [TestMethod]
        public void ProcessPlayerAction_PitStop_RefuelsCar()
        {
            // Arrange
            _raceManager.StartRace(_testCar);
            _testCar.CurrentFuel = 20.0; // Reduce fuel
            _testCar.CurrentSpeed = 60;
            double initialFuel = _testCar.CurrentFuel;

            // Act
            _raceManager.ProcessPlayerAction(PlayerAction.PitStop);

            // Assert - Fuel should be higher than initial (refueled but then consumed)
            Assert.IsTrue(_testCar.CurrentFuel > initialFuel);
            Assert.IsTrue(_testCar.CurrentSpeed < 60); // Speed should be reduced
        }

        /// <summary>
        /// Tests that ProcessPlayerAction throws exception when game not in progress
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ProcessPlayerAction_ThrowsException_WhenGameNotInProgress()
        {
            // Act
            _raceManager.ProcessPlayerAction(PlayerAction.SpeedUp);

            // Assert - Exception expected
        }

        /// <summary>
        /// Tests that game ends when race is completed
        /// </summary>
        [TestMethod]
        public void ProcessPlayerAction_EndsGame_WhenRaceCompleted()
        {
            // Arrange
            _raceManager.StartRace(_testCar);
            bool gameStateChanged = false;
            GameState finalState = GameState.NotStarted;
            
            _raceManager.GameStateChanged += (s, state) => 
            {
                if (state != GameState.InProgress)
                {
                    gameStateChanged = true;
                    finalState = state;
                }
            };

            // Simulate completing all laps by advancing track position
            for (int i = 0; i < 50; i++) // Multiple actions to complete race
            {
                if (_raceManager.GameState != GameState.InProgress) break;
                _raceManager.ProcessPlayerAction(PlayerAction.SpeedUp);
            }

            // Assert
            Assert.IsTrue(gameStateChanged);
            Assert.IsTrue(finalState == GameState.Completed || finalState == GameState.GameOver);
        }

        /// <summary>
        /// Tests that game ends when fuel runs out
        /// </summary>
        [TestMethod]
        public void ProcessPlayerAction_EndsGame_WhenFuelRunsOut()
        {
            // Arrange
            _raceManager.StartRace(_testCar);
            _testCar.CurrentFuel = 1.0; // Very low fuel
            
            bool gameStateChanged = false;
            GameState finalState = GameState.NotStarted;
            
            _raceManager.GameStateChanged += (s, state) => 
            {
                if (state == GameState.GameOver)
                {
                    gameStateChanged = true;
                    finalState = state;
                }
            };

            // Act - Keep consuming fuel until it runs out
            for (int i = 0; i < 10; i++)
            {
                if (_raceManager.GameState != GameState.InProgress) break;
                _raceManager.ProcessPlayerAction(PlayerAction.SpeedUp);
            }

            // Assert
            Assert.IsTrue(gameStateChanged);
            Assert.AreEqual(GameState.GameOver, finalState);
        }

        /// <summary>
        /// Tests ResetGame functionality
        /// </summary>
        [TestMethod]
        public void ResetGame_ResetsAllValues()
        {
            // Arrange
            _raceManager.StartRace(_testCar);
            _raceManager.ProcessPlayerAction(PlayerAction.SpeedUp);
            
            bool gameStateChanged = false;
            _raceManager.GameStateChanged += (s, state) => 
            {
                if (state == GameState.NotStarted)
                    gameStateChanged = true;
            };

            // Act
            _raceManager.ResetGame();

            // Assert
            Assert.AreEqual(GameState.NotStarted, _raceManager.GameState);
            Assert.IsNull(_raceManager.CurrentCar);
            Assert.AreEqual(_raceManager.MaxRaceTime, _raceManager.TimeRemaining);
            Assert.IsTrue(gameStateChanged);
        }

        /// <summary>
        /// Tests GetTimeRemainingPercentage method
        /// </summary>
        [TestMethod]
        public void GetTimeRemainingPercentage_ReturnsCorrectValue()
        {
            // Test at start
            Assert.AreEqual(1.0, _raceManager.GetTimeRemainingPercentage(), 0.001);

            // Test time consumption through actions
            _raceManager.StartRace(_testCar);
            double initialPercentage = _raceManager.GetTimeRemainingPercentage();
            
            // Perform actions to consume time
            for (int i = 0; i < 10; i++)
            {
                if (_raceManager.GameState == GameState.InProgress)
                    _raceManager.ProcessPlayerAction(PlayerAction.MaintainSpeed);
            }
            
            Assert.IsTrue(_raceManager.GetTimeRemainingPercentage() < initialPercentage);
        }

        /// <summary>
        /// Tests GetElapsedTime method
        /// </summary>
        [TestMethod]
        public void GetElapsedTime_ReturnsZero_WhenGameNotStarted()
        {
            // Act
            var elapsedTime = _raceManager.GetElapsedTime();

            // Assert
            Assert.AreEqual(TimeSpan.Zero, elapsedTime);
        }

        /// <summary>
        /// Tests that actions consume time
        /// </summary>
        [TestMethod]
        public void ProcessPlayerAction_ConsumesTime()
        {
            // Arrange
            _raceManager.StartRace(_testCar);
            double initialTime = _raceManager.TimeRemaining;

            // Act
            _raceManager.ProcessPlayerAction(PlayerAction.SpeedUp);

            // Assert
            Assert.IsTrue(_raceManager.TimeRemaining < initialTime);
        }
    }
}