namespace ISDevTemplate.Data
{
    [System.Serializable]
    public struct SaveData
    {
        public string SceneName;
        
        /// <summary>このシーンが何番目であるか</summary>
        public int SceneIndex;

        public int HighScore;

        public SaveData(string sceneName, int sceneIndex, int highScore)
        {
            SceneName = sceneName;
            SceneIndex = sceneIndex;
            HighScore = highScore;
        }
    }
}
