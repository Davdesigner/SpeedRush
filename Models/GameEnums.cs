namespace CSharpSpeedRush.Models
{
    /// <summary>
    /// Represents the different types of cars available in the game
    /// </summary>
    public enum CarType
    {
        SportsCar,
        EcoCar,
        RaceCar
    }

    /// <summary>
    /// Represents the current state of the game
    /// </summary>
    public enum GameState
    {
        NotStarted,
        InProgress,
        Completed,
        GameOver
    }

    /// <summary>
    /// Represents the different actions a player can take during their turn
    /// </summary>
    public enum PlayerAction
    {
        SpeedUp,
        MaintainSpeed,
        PitStop
    }
}