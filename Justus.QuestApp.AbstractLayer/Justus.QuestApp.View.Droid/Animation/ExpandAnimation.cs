using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Justus.QuestApp.View.Droid.Animation
{
    public class ExpandAnimation : Android.Views.Animations.Animation
    {
        private readonly Android.Views.View _animatedView;
        private readonly LinearLayout.LayoutParams _layoutParams;
        private readonly int _marginStart;
        private readonly int _marginEnd;
        private readonly bool _isVisible;
        private bool _wasEnded;

        public ExpandAnimation(Android.Views.View view, int duration)
        {
            Duration = duration;
            _animatedView = view;
            _layoutParams = view.LayoutParameters as LinearLayout.LayoutParams;

            _isVisible = _animatedView.Visibility == ViewStates.Visible;

            _marginStart = _layoutParams.BottomMargin;
            _marginEnd = _marginStart == 0 ? (0 - _animatedView.Height) : (0);

            _animatedView.Visibility = ViewStates.Visible;

        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            base.ApplyTransformation(interpolatedTime, t);
            if (interpolatedTime < 1.0f)
            {
                _layoutParams.BottomMargin = _marginStart + (int) ((_marginEnd - _marginStart)*interpolatedTime);
                _animatedView.RequestLayout();
            }
            else if(!_wasEnded)
            {
                _layoutParams.BottomMargin = _marginEnd;
                _animatedView.RequestLayout();

                if (_isVisible)
                {
                    _animatedView.Visibility = ViewStates.Gone;
                }
                _wasEnded = true;
            }
        }
    }
}