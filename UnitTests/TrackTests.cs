using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpSpeedRush.Models;

namespace CSharpSpeedRush.Tests
{
    /// <summary>
    /// Unit tests for the Track class
    /// </summary>
    [TestClass]
    public class TrackTests
    {
        private Track _testTrack = null!;

        /// <summary>
        /// Initializes test data before each test
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _testTrack = new Track(5, 100.0);
        }

        /// <summary>
        /// Tests that track initialization sets correct values
        /// </summary>
        [TestMethod]
        public void Track_Initialization_SetsCorrectValues()
        {
            // Arrange & Act
            var track = new Track(3, 150.0);

            // Assert
            Assert.AreEqual(3, track.TotalLaps);
            Assert.AreEqual(150.0, track.LapDistance);
            Assert.AreEqual(1, track.CurrentLap);
            Assert.AreEqual(0.0, track.CurrentLapDistance);
            Assert.IsFalse(track.IsRaceComplete());
        }

        /// <summary>
        /// Tests that AdvancePosition works correctly
        /// </summary>
        [TestMethod]
        public void AdvancePosition_UpdatesDistanceCorrectly()
        {
            // Arrange
            int speed = 40;
            double expectedDistance = speed * 0.5; // 20.0

            // Act
            bool lapCompleted = _testTrack.AdvancePosition(speed);

            // Assert
            Assert.IsFalse(lapCompleted);
            Assert.AreEqual(expectedDistance, _testTrack.CurrentLapDistance);
            Assert.AreEqual(1, _testTrack.CurrentLap);
        }

        /// <summary>
        /// Tests lap completion when distance exceeds lap distance
        /// </summary>
        [TestMethod]
        public void AdvancePosition_CompletesLap_WhenDistanceExceedsLapDistance()
        {
            // Arrange - advance to near completion
            _testTrack.AdvancePosition(180); // 90.0 distance
            int speed = 40; // This will add 20.0, totaling 110.0

            // Act
            bool lapCompleted = _testTrack.AdvancePosition(speed);

            // Assert
            Assert.IsTrue(lapCompleted);
            Assert.AreEqual(2, _testTrack.CurrentLap);
            Assert.AreEqual(0.0, _testTrack.CurrentLapDistance);
        }

        /// <summary>
        /// Tests GetLapProgress returns correct percentage
        /// </summary>
        [TestMethod]
        public void GetLapProgress_ReturnsCorrectPercentage()
        {
            // Test at start
            Assert.AreEqual(0.0, _testTrack.GetLapProgress(), 0.001);

            // Test at 50% completion
            _testTrack.AdvancePosition(100); // 50.0 distance
            Assert.AreEqual(0.5, _testTrack.GetLapProgress(), 0.001);
        }

        /// <summary>
        /// Tests GetOverallProgress returns correct percentage
        /// </summary>
        [TestMethod]
        public void GetOverallProgress_ReturnsCorrectPercentage()
        {
            // Test at start
            Assert.AreEqual(0.0, _testTrack.GetOverallProgress(), 0.001);

            // Test after completing 2 laps and 50% of lap 3
            // Complete 2 full laps
            _testTrack.AdvancePosition(200); // Complete lap 1
            _testTrack.AdvancePosition(200); // Complete lap 2
            _testTrack.AdvancePosition(100); // 50% of lap 3
            double expectedProgress = (2.0 + 0.5) / 5.0; // 0.5
            Assert.AreEqual(expectedProgress, _testTrack.GetOverallProgress(), 0.001);
        }

        /// <summary>
        /// Tests IsRaceComplete method
        /// </summary>
        [TestMethod]
        public void IsRaceComplete_ReturnsCorrectValue()
        {
            // Test during race
            Assert.IsFalse(_testTrack.IsRaceComplete());

            // Complete all 5 laps
            for (int i = 0; i < 5; i++)
            {
                _testTrack.AdvancePosition(200); // Complete each lap
            }
            
            // Test after completion
            Assert.IsTrue(_testTrack.IsRaceComplete());
        }

        /// <summary>
        /// Tests GetProgressIndicator returns correct visual representation
        /// </summary>
        [TestMethod]
        public void GetProgressIndicator_ReturnsCorrectVisualRepresentation()
        {
            // Test at start
            string startIndicator = _testTrack.GetProgressIndicator();
            Assert.IsTrue(startIndicator.Contains(">"));
            Assert.IsTrue(startIndicator.Contains("-"));

            // Test at completion - complete all laps
            for (int i = 0; i < 5; i++)
            {
                _testTrack.AdvancePosition(200);
            }
            string finishIndicator = _testTrack.GetProgressIndicator();
            Assert.AreEqual("[FINISH!]", finishIndicator);
        }

        /// <summary>
        /// Tests multiple lap completions
        /// </summary>
        [TestMethod]
        public void AdvancePosition_HandlesMultipleLaps()
        {
            // Complete first lap
            _testTrack.AdvancePosition(200); // Completes lap 1
            Assert.AreEqual(2, _testTrack.CurrentLap);

            // Complete second lap
            _testTrack.AdvancePosition(200); // Completes lap 2
            Assert.AreEqual(3, _testTrack.CurrentLap);

            // Verify race is not complete yet
            Assert.IsFalse(_testTrack.IsRaceComplete());
        }

        /// <summary>
        /// Tests that advancing position after race completion does nothing
        /// </summary>
        [TestMethod]
        public void AdvancePosition_DoesNothing_WhenRaceComplete()
        {
            // Arrange - Complete the race
            for (int i = 0; i < 5; i++)
            {
                _testTrack.AdvancePosition(200);
            }

            // Act
            bool result = _testTrack.AdvancePosition(50);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(_testTrack.CurrentLap > 5);
        }
    }
}