namespace RootGame;

/**
 * A bit of a hack to let the tree Area2D alert rigid bodies they have entered or exited.
 */
public interface ITreeEnterable
{
    public void OnTreeEnter()
    {
    }

    public void OnTreeExit()
    {
    }
}