using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Animator))]
public class AnimationGameObjectFixTransforms : Editor
{
    private Animator _currentAnimator;

    public override void OnInspectorGUI()
    {
        _currentAnimator = (Animator)target;
        
        DrawDefaultInspector();

        // ADD BUTTON IN GUI
        GUILayout.Space(10f);

        if (GUILayout.Button("APPLY TRANSFORMS AND PARAMETRS"))
        {            
            ApplyParametrsOfSelectedAnimation();
        }
    }

    void ApplyParametrsOfSelectedAnimation()
    {
        if (_currentAnimator == null) return;

        AnimationClip animationClip = _currentAnimator.runtimeAnimatorController.animationClips[0]; // GET FIRST ANIMATION

        foreach (AnimationClip clip in _currentAnimator.runtimeAnimatorController.animationClips) // TRY TO FIND IDLE
        {
            if (clip.name.ToLower().Contains("idle"))
            {
                animationClip = clip;
                break;
            }
        }

        GameObject parent = _currentAnimator.gameObject;

        UpdateTransformsRecursivetly(animationClip, parent);
    }

    void UpdateTransformsRecursivetly(AnimationClip clip, GameObject parentGO)
    {
        clip.SampleAnimation(parentGO, 0f);

        for (int i = 0; i < parentGO.transform.childCount; i++) {
            GameObject childrenGO = parentGO.transform.GetChild(i).gameObject;
            clip.SampleAnimation(childrenGO, 0f);
            UpdateTransformsRecursivetly(clip, childrenGO);
        }

    }
}