using UnityEngine;
using UniRx;
using DG.Tweening;
using Game.EatableObjects;
using Testing.HoleSystem.Scripts.HoleLogic;
using UniRx.Triggers;

namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class HoleFallAreaCollisionDetection : MonoBehaviour
    {
        [SerializeField] private HoleEatLogicController Parent;
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private const float AcceptableDistance = 0.5f;
        private const float TweenDuration = 0.5f; // duration of the tween animation

        private void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.TryGetComponent(out EatableHoleObject holeObject) && holeObject.CanBeEaten)
                    {
                        if (holeObject.Parent.CurrentLevel < Parent.CurrentLevel)
                        {
                            Observable.EveryUpdate()
                                .TakeUntil(this.OnTriggerExitAsObservable().Where(col => col == collider))
                                .Where(_ => Vector3.Distance(holeObject.transform.position, transform.position) < AcceptableDistance)
                                .Take(1) // trigger only once
                                .Subscribe(_ =>
                                {
                                    // Start tween: scale the object to zero with an easing effect
                                    holeObject.transform.DOScale(Vector3.zero, TweenDuration)
                                        .SetEase(Ease.OutBack)
                                        .OnComplete(() =>
                                        {
                                            // After tween ends, proceed with the eat logic
                                            Parent.HoleEatableCounter.EatObject(holeObject);
                                            Destroy(holeObject.Parent.gameObject);
                                        });
                                })
                                .AddTo(disposables);
                        }
                    }
                    else if (collider.TryGetComponent(out EatableObject eatable) && eatable.CanBeEaten && eatable.MinimumLevelRequired <= Parent.CurrentLevel)
                    {
                        eatable.SetIgnoreCollisionWithGround(true);
                    }
                })
                .AddTo(disposables);

            this.OnTriggerExitAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.TryGetComponent(out EatableObject eatable))
                    {
                        eatable.SetIgnoreCollisionWithGround(false);
                    }
                })
                .AddTo(disposables);
        }

        private void OnDestroy()
        {
            disposables.Dispose();
        }
    }
}
