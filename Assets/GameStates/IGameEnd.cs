namespace DefaultNamespace.GameStates
{
    public enum GameEnd
    {
        Back,
        Restart,
        Win,
        Loose
    }
    public interface IGameEnd
    {
        public void EndGame(GameEnd gameEnd);
    }
    
}