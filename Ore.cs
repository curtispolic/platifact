namespace platifact;

public class Ore : StaticPlatifactObject
{

    public int oreAmount;

    public Ore()
    {
        isNothing = false;
        isBlocking = true;
        oreAmount = 100;
    }
}
