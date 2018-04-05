
public class IDFactory {

    public static int EntityID;
    public static int AbilityID;
    public static int ItemID;

    public static int GenerateEntityID() {

        EntityID++;

        return EntityID;
    }

    public static int GenerateAbilityID() {
        AbilityID++;

        return AbilityID;
    }

    public static int GenerateItemID() {
        ItemID++;

        return ItemID;
    }

}
