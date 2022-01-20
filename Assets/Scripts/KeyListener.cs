using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace OculusSampleFramework
{

    public class KeyListener : MonoBehaviour
    {
        private const float LERP_TO_OLD_POS_DURATION = 1.0f;
        private const float LOCAL_SIZE_HALVED = 0.5f;

        //[SerializeField] private MeshRenderer _meshRenderer = null;
        [SerializeField] private ButtonController _buttonController = null;

        [SerializeField] private AudioSource _audioSource = null;
        [SerializeField] private AudioClip _actionSoundEffect = null;

        [SerializeField] private Transform _buttonContactTransform = null;
        [SerializeField] private float _contactMaxDisplacementDistance = 0.0141f;

        private Material _buttonMaterial;
        private Color _buttonDefaultColor;
        //private int _materialColorId;

        private bool _buttonInContactOrActionStates = false;

        private Coroutine _lerpToOldPositionCr = null;
        private Vector3 _oldPosition;

        private void Awake()
        {

           //_materialColorId = Shader.PropertyToID("_Color");

            _oldPosition = transform.localPosition;
        }

        private void OnDestroy()
        {
            if (_buttonMaterial != null)
            {
                Destroy(_buttonMaterial);
            }
        }

        private void OnEnable()
        {
            _buttonController.InteractableStateChanged.AddListener(InteractableStateChanged);
            _buttonController.ContactZoneEvent += ActionOrInContactZoneStayEvent;
            _buttonController.ActionZoneEvent += ActionOrInContactZoneStayEvent;
            _buttonInContactOrActionStates = false;
        }

        private void OnDisable()
        {
            if (_buttonController != null)
            {
                _buttonController.InteractableStateChanged.RemoveListener(InteractableStateChanged);
                _buttonController.ContactZoneEvent -= ActionOrInContactZoneStayEvent;
                _buttonController.ActionZoneEvent -= ActionOrInContactZoneStayEvent;
            }
        }

        private void ActionOrInContactZoneStayEvent(ColliderZoneArgs collisionArgs)
        {
            if (!_buttonInContactOrActionStates || collisionArgs.CollidingTool.IsFarFieldTool)
            {
                return;
            }
            
            Vector3 buttonScale = _buttonContactTransform.localScale;
            Vector3 interactionPosition = collisionArgs.CollidingTool.InteractionPosition;
            Vector3 localSpacePosition = _buttonContactTransform.InverseTransformPoint(
              interactionPosition);

            Vector3 offsetVector = localSpacePosition - LOCAL_SIZE_HALVED * Vector3.one;

            float scaledLocalSpaceOffset = offsetVector.y * buttonScale.y;
            

            if (scaledLocalSpaceOffset > -_contactMaxDisplacementDistance && scaledLocalSpaceOffset
                <= 0.0f)
            {
                transform.localPosition = new Vector3(_oldPosition.x, _oldPosition.y + scaledLocalSpaceOffset, _oldPosition.z);
                
            }
            //offsetVector.x = 1 - offsetVector.x;
            //if (offsetVector.x < 0.2)
            //{
            //    transform.localRotation = Quaternion.Euler(0, 0, 0);
            //} else
            //{
            //    transform.localRotation = Quaternion.Euler(offsetVector.x * 30, 0, 0);
            //}
            //print(offsetVector.x);
            //transform.eulerAngles = new Vector3(scaledLocalSpaceOffset * 10, 0, 0);
        }

        private void InteractableStateChanged(InteractableStateArgs obj)
        {
            _buttonInContactOrActionStates = false;
            switch (obj.NewInteractableState)
            {
                case InteractableState.ContactState:
                    StopResetLerping();
                    _buttonInContactOrActionStates = true;
                    break;
                case InteractableState.ProximityState:
                    LerpToOldPosition();
                    break;
                case InteractableState.ActionState:
                    StopResetLerping();
                    PlaySound(_actionSoundEffect);
                    _buttonInContactOrActionStates = true;
                    break;
                default:
                    LerpToOldPosition();
                    break;
            }
        }

        private void PlaySound(AudioClip clip)
        {
            _audioSource.timeSamples = 0;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private void StopResetLerping()
        {
            if (_lerpToOldPositionCr != null)
            {
                StopCoroutine(_lerpToOldPositionCr);
            }
        }

        private void LerpToOldPosition()
        {
            if ((transform.localPosition - _oldPosition).sqrMagnitude < Mathf.Epsilon)
            {
                return;
            }
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            StopResetLerping();
            _lerpToOldPositionCr = StartCoroutine(ResetPosition());
        }

        private IEnumerator ResetPosition()
        {
            var startTime = Time.time;
            var endTime = Time.time + LERP_TO_OLD_POS_DURATION;

            while (Time.time < endTime)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _oldPosition,
                  (Time.time - startTime) / LERP_TO_OLD_POS_DURATION);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localPosition = _oldPosition;
            _lerpToOldPositionCr = null;
        }
    }
}
