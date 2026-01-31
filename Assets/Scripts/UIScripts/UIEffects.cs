using UnityEngine;

namespace UIScripts
{
    public static class UIEffects
    {
        public static readonly RotateShake RotateShakeEffect = new();
        public static readonly PulseInOut PulseInOutEffect = new();
        public static readonly HoverFloat HoverFloatEffect = new();
        public static readonly MoveBy MoveByEffect = new();
        public static readonly AppearDisappear AppearDisappearEffect = new();
        
        public class AppearDisappear
        {
            public LTDescr StartAppearing(CanvasGroup cg, float duration)
            {
                cg.alpha = 0f;
                return LeanTween.alphaCanvas(cg, 1f, duration);
            }

            public LTDescr StartDisappearing(CanvasGroup cg, float duration)
            {
                cg.alpha = 1f;
                return LeanTween.alphaCanvas(cg, 0f, duration);
            }
        }
        
        public class MoveBy
        {
            public LTDescr StartMoveBy(GameObject moveObject, MoveByInfo moveByInfo)
            {
                var startLocalPos = moveObject.transform.localPosition;
                var target = startLocalPos + new Vector3(moveByInfo.MoveX, moveByInfo.MoveY, 0f);

                var tween = LeanTween.moveLocal(moveObject, target, moveByInfo.Duration)
                    .setEaseInOutBounce();
                return tween;
            }
        }
        
        public class HoverFloat
        {
            public LTDescr StartHoverFloat(GameObject hoverFloatObject, HoverFloatInfo hoverFloatInfo)
            {
                var baseY = hoverFloatObject.transform.localPosition.y;
                var minHeight = hoverFloatInfo.MinHoverHeight;
                var maxHeight = hoverFloatInfo.MaxHoverHeight;
                var cycleTime = hoverFloatInfo.HoverUpDuration + hoverFloatInfo.HoverDownDuration;

                var tween = LeanTween.value(hoverFloatObject, 0f, cycleTime, cycleTime)
                    .setLoopClamp()
                    .setEaseLinear()
                    .setOnUpdate(t =>
                    {
                        float yOffset;
                        // Moving UP (min -> max)
                        if (t <= hoverFloatInfo.HoverUpDuration)
                        {
                            var n = t / hoverFloatInfo.HoverUpDuration;
                            yOffset = Mathf.Lerp(minHeight, maxHeight, EaseInOut(n));
                        }
                        // Moving DOWN (max -> min)
                        else
                        {
                            var n = (t - hoverFloatInfo.HoverUpDuration) / hoverFloatInfo.HoverDownDuration;
                            yOffset = Mathf.Lerp(maxHeight, minHeight, EaseInOut(n));
                        }

                        var localPos = hoverFloatObject.transform.localPosition;
                        localPos.y = baseY + yOffset;
                        hoverFloatObject.transform.localPosition = localPos;
                    });
                return tween;
            }

            // Soft hover feel
            private float EaseInOut(float t)
            {
                return Mathf.SmoothStep(0f, 1f, t);
            }
        }
        
        public class PulseInOut
        {
            // Growing in size slowly and returning back to original size like a pulse
            public LTDescr StartPulsingInOut(GameObject pulseObject, PulseInOutInfo pulseInfo)
            {
                var baseScale = pulseObject.transform.localScale;
                var cycleTime = pulseInfo.PulseInTime + pulseInfo.PulseOutTime;

                var tween = LeanTween.value(pulseObject, 0f, 1f, cycleTime)
                    .setLoopClamp()
                    .setRepeat(pulseInfo.PulseCount)
                    .setOnUpdate(t =>
                    {
                        // IN phase
                        if (t <= pulseInfo.PulseInTime / cycleTime)
                        {
                            var n = t / (pulseInfo.PulseInTime / cycleTime);
                            pulseObject.transform.localScale = Vector3.Lerp(baseScale, baseScale * pulseInfo.PulseOutScale, n);
                        }
                        // OUT phase
                        else
                        {
                            var n = (t - pulseInfo.PulseInTime / cycleTime) / (pulseInfo.PulseOutTime / cycleTime);
                            pulseObject.transform.localScale = Vector3.Lerp(baseScale * pulseInfo.PulseOutScale, baseScale, n);
                        }
                    })
                    .setOnComplete(() =>
                    {
                        pulseObject.transform.localScale = baseScale;
                    });
                return tween;
            }
        }
        
        //Gives a small shake like effect (rattle)
        public class RotateShake
        {
            public LTDescr StartRotateShake(GameObject shakeObject, RotateShakeInfo shakeInfo)
            {
                var baseZ = shakeObject.transform.eulerAngles.z;
                var cycleTime = shakeInfo.Duration + shakeInfo.Gap;

                var tween = LeanTween.value(shakeObject, 0f, cycleTime, cycleTime)
                    .setDelay(shakeInfo.Gap)
                    .setLoopClamp()
                    .setLoopPingPong()
                    .setOnUpdate(t =>
                    {
                        if (t <= shakeInfo.Duration)
                        {
                            // Normalize motion phase to 0..1
                            var n = t / shakeInfo.Duration;

                            // 0..1 -> left->right->center
                            var wave = Mathf.Sin(n * Mathf.PI * 2f);
                            shakeObject.transform.rotation = Quaternion.Euler(0, 0, baseZ + wave * shakeInfo.Angle);
                        }
                        else
                        {
                            // Pause phase → stay centered
                            shakeObject.transform.rotation = Quaternion.Euler(0, 0, baseZ);
                        }
                    })
                    .setOnComplete(() =>
                    {
                        var rot = shakeObject.transform.rotation;
                        shakeObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, baseZ);
                    });

                return tween;
            }
        }
        
        public struct MoveByInfo
        {
            public readonly float MoveX;
            public readonly float MoveY;
            public readonly float Duration;

            public MoveByInfo(float moveX, float moveY, float duration)
            {
                MoveX = moveX;
                MoveY = moveY;
                Duration = duration;
            }
        }
        
        public struct HoverFloatInfo
        {
            public readonly float HoverUpDuration;
            public readonly float HoverDownDuration;

            public readonly float MinHoverHeight;
            public readonly float MaxHoverHeight;

            public HoverFloatInfo(float  hoverUpDuration, float hoverDownDuration, float minHoverHeight, float maxHoverHeight)
            {
                HoverDownDuration = hoverDownDuration;
                HoverUpDuration = hoverUpDuration;
                MinHoverHeight = minHoverHeight;
                MaxHoverHeight = maxHoverHeight;
            }
        }

        public struct RotateShakeInfo
        {
            public readonly float Angle; //shake angle
            public readonly float Duration; //full shake duration, left->right
            public readonly float Gap; //gap between shakes

            public RotateShakeInfo(float angle, float duration, float gap)
            {
                Angle = angle;
                Duration = duration;
                Gap = gap;
            }
        }
        
        public struct PulseInOutInfo
        {
            public readonly float PulseInTime;
            public readonly float PulseOutTime;
            public readonly float PulseOutScale;
            public readonly int PulseCount;

            public PulseInOutInfo(float pulseInTime, float pulseOutTime, float pulseOutScale, int pulseCount)
            {
                PulseInTime = pulseInTime;
                PulseOutTime = pulseOutTime;
                PulseOutScale = pulseOutScale;
                PulseCount = pulseCount;
            }
        }
    }
}