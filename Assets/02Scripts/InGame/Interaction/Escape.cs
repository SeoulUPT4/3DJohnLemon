using UnityEngine;

public class Escape : InteractionComponent
{
    [SerializeField]
    private SceneType m_nextSceneType;
    public override bool Interact(PlayerInventory inventory)
    {
        OnExcape();
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            OnExcape();
        }
    }
    private void OnExcape()
    {
        SceneLoader.LoadScene(m_nextSceneType);
        AudioManager.Instance.ChangeBGM(m_nextSceneType);
    }
}
