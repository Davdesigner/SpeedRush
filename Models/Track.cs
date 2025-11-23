using System;
using System.Collections.Generic;

namespace CSharpSpeedRush.Models
{
    /// <summary>
    /// Represents a racing track with multiple laps
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Gets the total number of laps in the race
        /// </summary>
        public int TotalLaps { get; }

        /// <summary>
        /// Gets the distance per lap in arbitrary units
        /// </summary>
        public double LapDistance { get; }

        /// <summary>
        /// Gets the current lap number (1-based)
        /// </summary>
        public int CurrentLap { get; internal set; }

        /// <summary>
        /// Gets the current distance completed in the current lap
        /// </summary>
        public double CurrentLapDistance { get; internal set; }

        /// <summary>
        /// Gets the list of completed lap times
        /// </summary>
        public List<TimeSpan> LapTimes { get; }

        /// <summary>
        /// Initializes a new instance of the Track class
        /// </summary>
        /// <param name="totalLaps">The total number of laps</param>
        /// <param name="lapDistance">The distance per lap</param>
        public Track(int totalLaps = 5, double lapDistance = 100.0)
        {
            TotalLaps = totalLaps;
            LapDistance = lapDistance;
            CurrentLap = 1;
            CurrentLapDistance = 0;
            LapTimes = new List<TimeSpan>();
        }

        /// <summary>
        /// Advances the car's position on the track based on speed
        /// </summary>
        /// <param name="speed">The current speed of the car</param>
        /// <returns>True if a lap was completed, false otherwise</returns>
        public bool AdvancePosition(int speed)
        {
            if (IsRaceComplete())
                return false;

            double distanceIncrement = speed * 0.5; // Speed to distance conversion
            CurrentLapDistance += distanceIncrement;

            if (CurrentLapDistance >= LapDistance)
            {
                CompleteLap();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Completes the current lap and advances to the next
        /// </summary>
        private void CompleteLap()
        {
            CurrentLapDistance = 0;
            CurrentLap++;
        }

        /// <summary>
        /// Gets the current lap progress as a percentage
        /// </summary>
        /// <returns>Progress percentage (0.0 to 1.0)</returns>
        public double GetLapProgress()
        {
            if (IsRaceComplete())
                return 1.0;
            
            return CurrentLapDistance / LapDistance;
        }

        /// <summary>
        /// Gets the overall race progress as a percentage
        /// </summary>
        /// <returns>Overall progress percentage (0.0 to 1.0)</returns>
        public double GetOverallProgress()
        {
            if (IsRaceComplete())
                return 1.0;

            double completedLaps = CurrentLap - 1;
            double currentLapProgress = GetLapProgress();
            
            return (completedLaps + currentLapProgress) / TotalLaps;
        }

        /// <summary>
        /// Checks if the race is complete
        /// </summary>
        /// <returns>True if all laps are completed, false otherwise</returns>
        public bool IsRaceComplete() => CurrentLap > TotalLaps;

        /// <summary>
        /// Gets a textual progress indicator
        /// </summary>
        /// <returns>A string showing visual progress</returns>
        public string GetProgressIndicator()
        {
            if (IsRaceComplete())
                return "[FINISH!]";

            int progressChars = (int)(GetLapProgress() * 10);
            string progress = new string('=', progressChars) + ">";
            string remaining = new string('-', 10 - progressChars);
            
            return $"[{progress}{remaining}]";
        }
    }
}