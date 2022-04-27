public class ParryPlayerState : AbstractPlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.SpendStamina(staminaCost);
    }
}
