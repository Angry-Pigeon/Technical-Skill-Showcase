using UnityEngine;
namespace Game.GameLogic.Managers
{
    public class BootStrapperContext : MonoBehaviour
    {
        [field: SerializeField]
        public Joystick Joystick { get; private set; }
    }
}