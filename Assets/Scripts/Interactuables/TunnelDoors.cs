using UnityEngine;

public class TunnelDoors : MonoBehaviour, IInteractable, IOutlined
{
    private SceneChanger _sceneChanger;
    private GameScenes _gameScene;

    private void Awake()
    {
        _sceneChanger = GetComponent<SceneChanger>();
        _gameScene = _sceneChanger.escenaDestino;
    }

    public void Interact(GameObject interactor)
    {
        _sceneChanger.ChangeScene(_gameScene);
    }
}
