using Godot;

namespace TS2D.Util
{
    public static class MathHelper
    {
        public static Vector2 snap(Vector2 pos, int v) {
            float x = pos.x;
            float y = pos.y;
           
            x = Mathf.FloorToInt(x / v) * v;
            y = Mathf.FloorToInt(y / v) * v;
           
            return new Vector2(x, y);
        }

        public static int snap(int pos, int v) {
            float x = pos;
            return Mathf.FloorToInt(x / v) * v;
        }

        public static float snap(float pos, float v) {
            float x = pos;
            return Mathf.FloorToInt(x / v) * v;
        }
    }
}