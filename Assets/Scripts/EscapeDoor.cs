using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay;

    public GameSession gameSession;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Consts.PLAYER_LAYER_ID) { gameSession.LevelEnd(); }
    }
}
