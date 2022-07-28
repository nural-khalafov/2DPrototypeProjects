using UnityEngine;

namespace GravityDefiedPrototype 
{
    public class VehicleController : MonoBehaviour
    {
        public enum VehicleType
        {
            RearWheelDrive,
            FrontWheelDrive,
            AllWheelDrive
        }

        private WheelJoint2D _rearWheel;

        [SerializeField] private float _speed;
        private float _force = 0f;

        private Rigidbody2D _vehicleRigidbody;

        [field: SerializeField] public float Acceleration { get; set; }
        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = _speed * Acceleration;
            }
        }
        [field: SerializeField] public float LeanForce { get; set; }



        private void Start()
        {
            _rearWheel = GameObject.Find("Rear_Wheel").GetComponent<WheelJoint2D>();
            _vehicleRigidbody = GameObject.Find("Vehicle_Body").GetComponent<Rigidbody2D>();

            Speed = Speed * Acceleration;
        }

        private void FixedUpdate()
        {
            _force = Input.GetAxis("Vertical") * Speed;
            float lean = -Input.GetAxis("Horizontal");

            JointMotor2D motor = new JointMotor2D
            {
                motorSpeed = _force,
                maxMotorTorque = _rearWheel.motor.maxMotorTorque
            };
            _rearWheel.motor = motor;

            _vehicleRigidbody.AddTorque(lean * LeanForce);

            if (_force == 0f)
            {
                _rearWheel.useMotor = false;
            }
            else
            {
                _rearWheel.useMotor = true;
            }
        }
    }
}
