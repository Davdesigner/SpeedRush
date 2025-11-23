using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpSpeedRush.Models;
using System;

namespace CSharpSpeedRush.Tests
{
    /// <summary>
    /// Unit tests for the Car class
    /// </summary>
    [TestClass]
    public class CarTests
    {
        private Car _testCar = null!;

        /// <summary>
        /// Initializes test data before each test
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _testCar = new Car(CarType.SportsCar, "Test Car", 100, 5.0, 50.0);
        }

        /// <summary>
        /// Tests that car initialization sets correct values
        /// </summary>
        [TestMethod]
        public void Car_Initialization_SetsCorrectValues()
        {
            // Arrange & Act
            var car = new Car(CarType.EcoCar, "Eco Test", 80, 3.0, 60.0);

            // Assert
            Assert.AreEqual(CarType.EcoCar, car.Type);
            Assert.AreEqual("Eco Test", car.Name);
            Assert.AreEqual(80, car.MaxSpeed);
            Assert.AreEqual(3.0, car.FuelConsumption);
            Assert.AreEqual(60.0, car.MaxFuelCapacity);
            Assert.AreEqual(60.0, car.CurrentFuel);
            Assert.AreEqual(0, car.CurrentSpeed);
        }

        /// <summary>
        /// Tests that SpeedUp increases speed correctly
        /// </summary>
        [TestMethod]
        public void SpeedUp_IncreasesSpeedCorrectly()
        {
            // Arrange
            int initialSpeed = _testCar.CurrentSpeed;

            // Act
            _testCar.SpeedUp(20);

            // Assert
            Assert.AreEqual(initialSpeed + 20, _testCar.CurrentSpeed);
        }

        /// <summary>
        /// Tests that SpeedUp throws exception when exceeding max speed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SpeedUp_ThrowsException_WhenExceedingMaxSpeed()
        {
            // Arrange
            _testCar.CurrentSpeed = 95;

            // Act
            _testCar.SpeedUp(10); // This should exceed max speed of 100

            // Assert - Exception expected
        }

        /// <summary>
        /// Tests that fuel consumption works correctly
        /// </summary>
        [TestMethod]
        public void ConsumeFuel_ReducesFuelCorrectly()
        {
            // Arrange
            _testCar.CurrentSpeed = 50;
            double initialFuel = _testCar.CurrentFuel;

            // Act
            double consumedFuel = _testCar.ConsumeFuel();

            // Assert
            Assert.IsTrue(consumedFuel > 0);
            Assert.IsTrue(_testCar.CurrentFuel < initialFuel);
            Assert.AreEqual(initialFuel - consumedFuel, _testCar.CurrentFuel, 0.001);
        }

        /// <summary>
        /// Tests that fuel consumption increases with speed
        /// </summary>
        [TestMethod]
        public void ConsumeFuel_IncreasesWithSpeed()
        {
            // Arrange
            var slowCar = new Car(CarType.EcoCar, "Slow", 100, 5.0, 50.0);
            var fastCar = new Car(CarType.EcoCar, "Fast", 100, 5.0, 50.0);
            
            slowCar.CurrentSpeed = 20;
            fastCar.CurrentSpeed = 80;

            // Act
            double slowConsumption = slowCar.ConsumeFuel();
            double fastConsumption = fastCar.ConsumeFuel();

            // Assert
            Assert.IsTrue(fastConsumption > slowConsumption);
        }

        /// <summary>
        /// Tests that refuel works correctly
        /// </summary>
        [TestMethod]
        public void Refuel_RestoresFuelToMaximum()
        {
            // Arrange
            _testCar.CurrentFuel = 20.0;

            // Act
            _testCar.Refuel();

            // Assert
            Assert.AreEqual(_testCar.MaxFuelCapacity, _testCar.CurrentFuel);
        }

        /// <summary>
        /// Tests that refuel throws exception when tank is full
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Refuel_ThrowsException_WhenTankIsFull()
        {
            // Arrange
            _testCar.CurrentFuel = _testCar.MaxFuelCapacity;

            // Act
            _testCar.Refuel();

            // Assert - Exception expected
        }

        /// <summary>
        /// Tests HasFuel method
        /// </summary>
        [TestMethod]
        public void HasFuel_ReturnsCorrectValue()
        {
            // Test with fuel
            Assert.IsTrue(_testCar.HasFuel());

            // Test without fuel
            _testCar.CurrentFuel = 0;
            Assert.IsFalse(_testCar.HasFuel());
        }

        /// <summary>
        /// Tests GetFuelPercentage method
        /// </summary>
        [TestMethod]
        public void GetFuelPercentage_ReturnsCorrectPercentage()
        {
            // Test full tank
            Assert.AreEqual(1.0, _testCar.GetFuelPercentage(), 0.001);

            // Test half tank
            _testCar.CurrentFuel = 25.0;
            Assert.AreEqual(0.5, _testCar.GetFuelPercentage(), 0.001);

            // Test empty tank
            _testCar.CurrentFuel = 0;
            Assert.AreEqual(0.0, _testCar.GetFuelPercentage(), 0.001);
        }
    }
}