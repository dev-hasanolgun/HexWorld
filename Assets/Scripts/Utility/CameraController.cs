using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float LerpValue;
    public bool Lerp;
    public bool FollowRotation;

    [BoxGroup("Position Offset")][OnValueChanged("UpdateOffsetsRelatively"), OnValueChanged("TestCamera")]
    public bool Relative;
    [BoxGroup("Position Offset")][OnValueChanged("UpdateCamera")][PropertyRange(-180f,180f)]
    public float PosX,PosY,PosZ;
    [BoxGroup("Rotation Offset")][OnValueChanged("UpdateCamera")][PropertyRange(-180f,180f)]
    public float RotX,RotY,RotZ;

    [OnValueChanged("TestCamera")]
    public bool TestCam;

    [BoxGroup("Test Object")][OnValueChanged("TestCamera")][ShowIf("TestCam")]
    public bool TestRelative;
    [BoxGroup("Test Object/Position")][OnValueChanged("TestCamera")][PropertyRange(-90f,90f)][ShowIf("TestCam")]
    public float TestPosX,TestPosY,TestPosZ;
    [BoxGroup("Test Object/Rotation")][OnValueChanged("TestCamera")][PropertyRange(-90f,90f)][ShowIf("TestCam")]
    public float TestRotX,TestRotY,TestRotZ;
    [BoxGroup("Test Object/Scale")][OnValueChanged("TestCamera")][PropertyRange(-90f,90f)][ShowIf("TestCam")]
    public float TestSclX = 1,TestSclY = 1,TestSclZ = 1;
    
    [HideInInspector]
    public bool IsFocused = true;
    
    
    [SerializeField][HideInInspector]
    private GameObject _testObj;

    private Vector3 _velocity;

    private void Awake()
    {
        TestCam = false;
        if (_testObj != null) Destroy(_testObj);
    }

    private void Start()
    {
        IsFocused = true;
        FocusPlayer(Target);
    }
    
    private void Update()
    {
        if (IsFocused)
        {
            if (Lerp) LerpFocusPlayer(Target);
            else FocusPlayer(Target);
        }
    }
    
    public void UpdateOffsetsRelatively()
    {
        if (Relative)
        {
            var eq1 = new Vector4(transform.right.x, transform.up.x, transform.forward.x, -PosX);
            var eq2 = new Vector4(transform.right.y, transform.up.y, transform.forward.y, -PosY);
            var eq3 = new Vector4(transform.right.z, transform.up.z, transform.forward.z, -PosZ);
            
            var solution = Solve3x3Equation(eq1, eq2, eq3);

            PosX = Mathf.Round(solution.x * 1000f) / 1000f;
            PosY = Mathf.Round(solution.y * 1000f) / 1000f;
            PosZ = Mathf.Round(solution.z * 1000f) / 1000f;
        } 
        else
        {
            var v = transform.right * PosX + transform.up * PosY + transform.forward * PosZ;
            
            PosX = Mathf.Round(v.x * 1000f) / 1000f;
            PosY = Mathf.Round(v.y * 1000f) / 1000f;
            PosZ = Mathf.Round(v.z * 1000f) / 1000f;
        }
    }

    public void UpdateCamera()
    {
        transform.rotation = Quaternion.LookRotation(Target.transform.forward) * Quaternion.Euler(RotX,RotY,RotZ);

        var camPosOffset = Relative ? transform.right * PosX + transform.up * PosY + transform.forward * PosZ : new Vector3(PosX,PosY,PosZ);
        transform.position = Target.transform.position + camPosOffset;
    }

    public void TestCamera()
    {
        if (TestCam)
        {
            if (_testObj == null)
            {
                var material = new Material(Shader.Find("Standard"))
                {
                    color = new Color(1f, 0.62f, 0f)
                };
                _testObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _testObj.transform.position = Vector3.zero;
                _testObj.GetComponent<MeshRenderer>().sharedMaterial = material;
            }
            
            var testPosOffset = TestRelative ? _testObj.transform.right * PosX + _testObj.transform.up * PosY + _testObj.transform.forward * PosZ : new Vector3(PosX,PosY,PosZ);
            _testObj.transform.position = _testObj.transform.right * TestPosX + _testObj.transform.up * TestPosY + _testObj.transform.forward * TestPosZ;
            _testObj.transform.rotation = Quaternion.Euler(TestRotX,TestRotY,TestRotZ);
            _testObj.transform.localScale = new Vector3(TestSclX,TestSclY,TestSclZ);
            
            transform.rotation = Quaternion.LookRotation(_testObj.transform.forward) * Quaternion.Euler(RotX,RotY,RotZ);

            var camPosOffset = Relative ? transform.right * PosX + transform.up * PosY + transform.forward * PosZ : new Vector3(PosX,PosY,PosZ);
            transform.position = _testObj.transform.position + camPosOffset;
        }
        else
        {
            if (_testObj != null) DestroyImmediate(_testObj);
        }
    }
    
    private void FocusPlayer(Transform player) // Focus camera to the player with offset and rotation set from inspector
    {
        var rot = Quaternion.LookRotation(player.transform.forward, player.transform.up) * Quaternion.Euler(RotX,RotY,RotZ);
        if (FollowRotation) transform.rotation = rot;
        transform.position = player.transform.position +
            (Relative
                ? transform.right * PosX + transform.up * PosY + transform.forward * PosZ
                : new Vector3(PosX, PosY, PosZ));
    }
    
    private void LerpFocusPlayer(Transform player) // Focus camera to the player with offset and rotation set from inspector
    {
        var rot = Quaternion.LookRotation(player.transform.forward, player.transform.up) * Quaternion.Euler(RotX,RotY,RotZ);
        if (FollowRotation) transform.rotation = Quaternion.Lerp(transform.rotation,rot,Time.deltaTime * LerpValue/2f);
        transform.position = Vector3.Lerp(transform.position,player.transform.position +
                                                             (Relative
                                                                 ? transform.right * PosX + transform.up * PosY + transform.forward * PosZ
                                                                 : new Vector3(PosX, PosY, PosZ)), Time.deltaTime * LerpValue);
    }
    
    private Vector3 Solve3x3Equation(Vector4 firstRow, Vector4 secondRow, Vector4 thirdRow)
    {
        var a = firstRow.x;
        var b = firstRow.y;
        var c = firstRow.z;
        var d = firstRow.w;
        
        var p = secondRow.x;
        var q = secondRow.y;
        var r = secondRow.z;
        var s = secondRow.w;
        
        var g = thirdRow.x;
        var h = thirdRow.y;
        var k = thirdRow.z;
        var l = thirdRow.w;

        var U = a * (q * k - r * h) - b * (p * k - r * g) + c * (p * h - q * g);

        var Vx = -b * (r * l - s * k) + c * (q * l - s * h) - d * (q * k - r * h);
        var Vy = a * (r * l - s * k) - c * (p * l - s * g) + d * (p * k - r * g);
        var Vz = -a * (q * l - s * h) + b * (p * l - s * g) - d * (p * h - q * g);

        return new Vector3(Vx / U, Vy / U, Vz / U);
    }
}