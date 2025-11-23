using System;

namespace CSharpSpeedRush.Models
{
    /// <summary>
    /// Represents a racing car with specific attributes and capabilities
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Gets the type of the car
        /// </summary>
        public CarType Type { get; }

        /// <summary>
        /// Gets the name of the car
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the maximum speed of the car
        /// </summary>
        public int MaxSpeed { get; }

        /// <summary>
        /// Gets the fuel consumption rate per turn
        /// </summary>
        public double FuelConsumption { get; }

        /// <summary>
        /// Gets the maximum fuel capacity
        /// </summary>
        public double MaxFuelCapacity { get; }

        /// <summary>
        /// Gets or sets the current fuel level
        /// </summary>
        public double CurrentFuel { get; set; }

        /// <summary>
        /// Gets or sets the current speed
        /// </summary>
        public int CurrentSpeed { get; set; }

        /// <summary>
        /// Initializes a new instance of the Car class
        /// </summary>
        /// <param name="type">The type of car</param>
        /// <param name="name">The name of the car</param>
        /// <param name="maxSpeed">The maximum speed</param>
        /// <param name="fuelConsumption">The fuel consumption rate</param>
        /// <param name="maxFuelCapacity">The maximum fuel capacity</param>
        public Car(CarType type, string name, int maxSpeed, double fuelConsumption, double maxFuelCapacity)
        {
            Type = type;
            Name = name;
            MaxSpeed = maxSpeed;
            FuelConsumption = fuelConsumption;
            MaxFuelCapacity = maxFuelCapacity;
            CurrentFuel = maxFuelCapacity;
            CurrentSpeed = 0;
        }

        /// <summary>
        /// Increases the car's speed
        /// </summary>
        /// <param name="increment">The amount to increase speed by</param>
        /// <exception cref="InvalidOperationException">Thrown when trying to exceed max speed</exception>
        public void SpeedUp(int increment = 10)
        {
            if (CurrentSpeed + increment > MaxSpeed)
                throw new InvalidOperationException($"Cannot exceed maximum speed of {MaxSpeed}");
            
            CurrentSpeed = Math.Min(CurrentSpeed + increment, MaxSpeed);
        }

        /// <summary>
        /// Consumes fuel based on current speed and consumption rate
        /// </summary>
        /// <returns>The amount of fuel consumed</returns>
        public double ConsumeFuel()
        {
            double consumption = FuelConsumption * (1 + CurrentSpeed / 100.0);
            CurrentFuel = Math.Max(0, CurrentFuel - consumption);
            return consumption;
        }

        /// <summary>
        /// Refuels the car to maximum capacity
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when fuel is already at maximum</exception>
        public void Refuel()
        {
            if (CurrentFuel >= MaxFuelCapacity)
                throw new InvalidOperationException("Fuel tank is already full");
            
            CurrentFuel = MaxFuelCapacity;
        }

        /// <summary>
        /// Checks if the car has enough fuel to continue
        /// </summary>
        /// <returns>True if fuel is available, false otherwise</returns>
        public bool HasFuel() => CurrentFuel > 0;

        /// <summary>
        /// Gets the fuel percentage remaining
        /// </summary>
        /// <returns>Fuel percentage as a value between 0 and 1</returns>
        public double GetFuelPercentage() => CurrentFuel / MaxFuelCapacity;
    }
}