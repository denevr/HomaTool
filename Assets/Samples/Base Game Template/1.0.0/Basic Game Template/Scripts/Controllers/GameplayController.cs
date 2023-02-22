using UnityEngine;

namespace HomaGames.Internal.BaseTemplate.Examples.Basic
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField]


        public void ResetScene()
        {

        }

        public void StartGame()
        {

        }

        public void LoseLife()
        {
            GameManagerBase.Instance.Context.AddToInt("Lives", -1);
        }

        public void AddScore()
        {
            GameManagerBase.Instance.Context.AddToInt("Score", 1);
        }
    }
}