using Godot;

namespace TS2D.Util
{
    public class Entity
    {
        public string Name;
        public int ID;
        public Vector2 position;

        public Entity(string name)
        {
            
            Name = name;
            //ID = id;
            this.position = position;
        }
    }
}