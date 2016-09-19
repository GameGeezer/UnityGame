
public class OctreeConstants
{
    public const int X_WEIGHT = 1, Y_WEIGHT = 2, Z_WEIGHT = 4;

    public const int BODY_NODE_BASE_LEVEL = 1;

    public const int NUMBER_OF_CHILDREN = 8;
}

public enum OctreeChild
{
    LEFT_BOTTOM_BACK = 0,
    RIGHT_BOTTOM_BACK = OctreeConstants.X_WEIGHT,
    LEFT_TOP_BACK = OctreeConstants.Y_WEIGHT,
    RIGHT_TOP_BACK = OctreeConstants.X_WEIGHT + OctreeConstants.Y_WEIGHT,
    LEFT_BOTTOM_FRONT = OctreeConstants.Z_WEIGHT,
    RIGHT_BOTTOM_FRONT = OctreeConstants.X_WEIGHT + OctreeConstants.Z_WEIGHT,
    LEFT_TOP_FRONT = OctreeConstants.Y_WEIGHT + OctreeConstants.Z_WEIGHT,
    RIGHT_TOP_FRONT = OctreeConstants.X_WEIGHT + OctreeConstants.Y_WEIGHT + OctreeConstants.Z_WEIGHT,
}