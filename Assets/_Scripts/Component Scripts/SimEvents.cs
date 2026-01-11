public static class SimEvents
{
    public const byte AR_STATUS_UPDATE = 10; // bool atStart, bool ready
    public const byte FIRE_SELECTION = 22; // int[]
    public const byte HANDSHAKE_DONE = 30; // int houseIndex
    public const byte FIRE_EXTINGUISHED = 40; // int fireId
    public const byte SIMULATION_RESET = 50;
    public const byte SIMULATION_END = 60;
    public const byte EVENT_FIRE_EXTINGUISHED = 2;
    public const byte POSITION_EVENT = 15;


}
