using UnityEngine;

public class MoveNode {
  public Vector2Int pos;
  public MoveNode parent;

  public MoveNode(Vector2Int pos, MoveNode parent)
   {
      this.pos = pos;
      this.parent = parent;
   }
}
