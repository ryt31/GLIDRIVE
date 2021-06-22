using System.Collections;
using System.Collections.Generic;
using Glidrive.Player;
using UnityEngine;
using UniRx;

namespace Glidrive.GlidingDisk
{
    public class GlidingDiskAnimationController : BaseGlidingDisk
    {
        private Animator animator;

        protected override void OnInitialize()
        {
            animator = GetComponent<Animator>();
        }

        public void SpinOnAnimation()
        {
            animator.SetBool("IsSpin",true);
        }
        
        public void SpinOffAnimation()
        {
            animator.SetBool("IsSpin",false);
        }
        
    }
}