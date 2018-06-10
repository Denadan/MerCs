namespace Mercs.Tactical.States
{
    public enum TacticalState
    {
        NotReady = 0,
        DeployInit = 10,
        DeploySelectUnit = 11,
        DeployPlaceUnit = 12,
        DeployRotation = 13,

        TurnPrepare = 20,
        PhasePrepare = 21,
        PhaseSelectFaction = 22,
        AIPrepare = 23,
        PlayerPrepare = 24,
        AIEndTurn = 25,

        SelectUnit = 30,
        SelectMovement = 40,
        SelectRun = 41,
        SelectJump = 42,
        SelectRotation = 43,
        ConfirmGuard = 44,
        PlayerEndTurn = 45,
        WaitMovementComplete = 46,
    }
}