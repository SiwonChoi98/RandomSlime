using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class STweenText : STweenBase<string>
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public
    
    public override void Restore ()
    {
        base.Restore ();
        _target.text = string.Empty;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // overrides STweenBase 

    protected override void PlayTween ()
    {
        _target.text = this.start;
        base.PlayTween ();

        base.tweenValue = this.tweener.CreateTween (this.end);
    }

    protected override void UpdateValue (string value)
    {
        base.UpdateValue (value);
        _target.text = value;
    }

    private Text _target;
    private void Awake()
    {
        _target = GetComponent<Text>();
    }
}
