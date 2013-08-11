//
// Copyright ©2006, 2007, Martin R. Gagné (martingagne@gmail.com)
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//   - Redistributions of source code must retain the above copyright notice, 
//     this list of conditions and the following disclaimer.
//
//   - Redistributions in binary form must reproduce the above copyright notice, 
//     this list of conditions and the following disclaimer in the documentation 
//     and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.
//

namespace WayGUI.CustomControls {
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal partial class LoadingCircle : Control {
        // Constants =========================================================
        private const double NumberOfDegreesInCircle = 360;
        private const double NumberOfDegreesInHalfCircle = NumberOfDegreesInCircle / 2;
        private const int DefaultInnerCircleRadius = 8;
        private const int DefaultOuterCircleRadius = 10;
        private const int DefaultNumberOfSpoke = 10;
        private const int DefaultSpokeThickness = 4;
        private readonly Color _defaultColor = Color.DarkGray;

        // Attributes ========================================================
        private readonly Timer _mTimer;
        private double[] _mAngles;
        private PointF _mCenterPoint;
        private Color _mColor;
        private Color[] _mColors;
        private int _mInnerCircleRadius;
        private bool _mIsTimerActive;
        private int _mNumberOfSpoke;
        private int _mOuterCircleRadius;
        private int _mProgressValue;
        private int _mSpokeThickness;

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:WayGUI.CustomControls.LoadingCircle" /> class.
        /// </summary>
        public LoadingCircle() {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _mColor = _defaultColor;

            GenerateColorsPallet();
            GetSpokesAngles();
            GetControlCenterPoint();

            _mTimer = new Timer();
            _mTimer.Tick += ATimerTick;
            ActiveTimer();

            Resize += LoadingCircleResize;
        }

        // Properties ========================================================
        /// <summary>
        ///   Gets or sets the lightest color of the circle.
        /// </summary>
        /// <value> The lightest color of the circle. </value>
        [TypeConverter("System.Drawing.ColorConverter")] [Category("LoadingCircle")] [Description("Sets the color of spoke.")] public Color Color {
            get { return _mColor; }
            set {
                _mColor = value;

                GenerateColorsPallet();
                Invalidate();
            }
        }

        /// <summary>
        ///   Gets or sets the outer circle radius.
        /// </summary>
        /// <value> The outer circle radius. </value>
        [Description("Gets or sets the radius of outer circle.")] [Category("LoadingCircle")] public int OuterCircleRadius {
            get {
                if(_mOuterCircleRadius == 0)
                    _mOuterCircleRadius = DefaultOuterCircleRadius;

                return _mOuterCircleRadius;
            }
            set {
                _mOuterCircleRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        ///   Gets or sets the inner circle radius.
        /// </summary>
        /// <value> The inner circle radius. </value>
        [Description("Gets or sets the radius of inner circle.")] [Category("LoadingCircle")] public int InnerCircleRadius {
            get {
                if(_mInnerCircleRadius == 0)
                    _mInnerCircleRadius = DefaultInnerCircleRadius;

                return _mInnerCircleRadius;
            }
            set {
                _mInnerCircleRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        ///   Gets or sets the number of spoke.
        /// </summary>
        /// <value> The number of spoke. </value>
        [Description("Gets or sets the number of spoke.")] [Category("LoadingCircle")] public int NumberSpoke {
            get {
                if(_mNumberOfSpoke == 0)
                    _mNumberOfSpoke = DefaultNumberOfSpoke;

                return _mNumberOfSpoke;
            }
            set {
                if(_mNumberOfSpoke != value && _mNumberOfSpoke > 0) {
                    _mNumberOfSpoke = value;
                    GenerateColorsPallet();
                    GetSpokesAngles();

                    Invalidate();
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref="T:WayGUI.CustomControls.LoadingCircle" /> is active.
        /// </summary>
        /// <value> <c>true</c> if active; otherwise, <c>false</c> . </value>
        [Description("Gets or sets the number of spoke.")] [Category("LoadingCircle")] public bool Active {
            get { return _mIsTimerActive; }
            set {
                _mIsTimerActive = value;
                ActiveTimer();
            }
        }

        /// <summary>
        ///   Gets or sets the spoke thickness.
        /// </summary>
        /// <value> The spoke thickness. </value>
        [Description("Gets or sets the thickness of a spoke.")] [Category("LoadingCircle")] public int SpokeThickness {
            get {
                if(_mSpokeThickness <= 0)
                    _mSpokeThickness = DefaultSpokeThickness;

                return _mSpokeThickness;
            }
            set {
                _mSpokeThickness = value;
                Invalidate();
            }
        }

        /// <summary>
        ///   Gets or sets the rotation speed.
        /// </summary>
        /// <value> The rotation speed. </value>
        [Description("Gets or sets the rotation speed. Higher the slower.")] [Category("LoadingCircle")] public int RotationSpeed {
            get { return _mTimer.Interval; }
            set {
                if(value > 0)
                    _mTimer.Interval = value;
            }
        }

        // Construtor ========================================================

        // Events ============================================================
        /// <summary>
        ///   Handles the Resize event of the LoadingCircle control.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> The <see cref="T:System.EventArgs" /> instance containing the event data. </param>
        private void LoadingCircleResize(object sender, EventArgs e) {
            GetControlCenterPoint();
        }

        /// <summary>
        ///   Handles the Tick event of the aTimer control.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> The <see cref="T:System.EventArgs" /> instance containing the event data. </param>
        private void ATimerTick(object sender, EventArgs e) {
            _mProgressValue = ++_mProgressValue % _mNumberOfSpoke;
            Invalidate();
        }

        /// <summary>
        ///   Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e"> A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e) {
            if(_mNumberOfSpoke > 0) {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                var intPosition = _mProgressValue;
                for(var intCounter = 0; intCounter < _mNumberOfSpoke; intCounter++) {
                    intPosition = intPosition % _mNumberOfSpoke;
                    DrawLine(e.Graphics, GetCoordinate(_mCenterPoint, _mInnerCircleRadius, _mAngles[intPosition]), GetCoordinate(_mCenterPoint, _mOuterCircleRadius, _mAngles[intPosition]), _mColors[intCounter], _mSpokeThickness);
                    intPosition++;
                }
            }

            base.OnPaint(e);
        }

        // Overridden Methods ================================================
        /// <summary>
        ///   Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize"> The custom-sized area for a control. </param>
        /// <returns> An ordered pair of type <see cref="T:System.Drawing.Size"></see> representing the width and height of a rectangle. </returns>
        public override Size GetPreferredSize(Size proposedSize) {
            proposedSize.Width = (_mOuterCircleRadius + _mSpokeThickness) * 2;

            return proposedSize;
        }

        // Methods ===========================================================
        /// <summary>
        ///   Darkens a specified color.
        /// </summary>
        /// <param name="objColor"> Color to darken. </param>
        /// <param name="intPercent"> The percent of darken. </param>
        /// <returns> The new color generated. </returns>
        private static Color Darken(Color objColor, int intPercent) {
            int intRed = objColor.R;
            int intGreen = objColor.G;
            int intBlue = objColor.B;
            return Color.FromArgb(intPercent, Math.Min(intRed, byte.MaxValue), Math.Min(intGreen, byte.MaxValue), Math.Min(intBlue, byte.MaxValue));
        }

        /// <summary>
        ///   Generates the colors pallet.
        /// </summary>
        private void GenerateColorsPallet() {
            _mColors = GenerateColorsPallet(_mColor, Active, _mNumberOfSpoke);
        }

        /// <summary>
        ///   Generates the colors pallet.
        /// </summary>
        /// <param name="objColor"> Color of the lightest spoke. </param>
        /// <param name="blnShadeColor"> if set to <c>true</c> the color will be shaded on X spoke. </param>
        /// <param name="intNbSpoke"> </param>
        /// <returns> An array of color used to draw the circle. </returns>
        private Color[] GenerateColorsPallet(Color objColor, bool blnShadeColor, int intNbSpoke = 0) {
            var objColors = new Color[NumberSpoke];

            // Value is used to simulate a gradient feel... For each spoke, the 
            // color will be darken by value in intIncrement.
            var bytIncrement = (byte) (byte.MaxValue / NumberSpoke);

            //Reset variable in case of multiple passes
            byte percentageOfDarken = 0;

            for(var intCursor = 0; intCursor < NumberSpoke; intCursor++) {
                if(blnShadeColor) {
                    if(intCursor == 0 || intCursor < NumberSpoke - intNbSpoke)
                        objColors[intCursor] = objColor;
                    else {
                        // Increment alpha channel color
                        percentageOfDarken += bytIncrement;

                        // Ensure that we don't exceed the maximum alpha
                        // channel value (255)
                        if(percentageOfDarken > byte.MaxValue)
                            percentageOfDarken = byte.MaxValue;

                        // Determine the spoke forecolor
                        objColors[intCursor] = Darken(objColor, percentageOfDarken);
                    }
                }
                else
                    objColors[intCursor] = objColor;
            }

            return objColors;
        }

        /// <summary>
        ///   Gets the control center point.
        /// </summary>
        private void GetControlCenterPoint() {
            _mCenterPoint = GetControlCenterPoint(this);
        }

        /// <summary>
        ///   Gets the control center point.
        /// </summary>
        /// <returns> PointF object </returns>
        private static PointF GetControlCenterPoint(Control objControl) {
            return new PointF(objControl.Width / 2, objControl.Height / 2 - 1);
        }

        /// <summary>
        ///   Draws the line with GDI+.
        /// </summary>
        /// <param name="objGraphics"> The Graphics object. </param>
        /// <param name="objPointOne"> The point one. </param>
        /// <param name="objPointTwo"> The point two. </param>
        /// <param name="objColor"> Color of the spoke. </param>
        /// <param name="intLineThickness"> The thickness of spoke. </param>
        private static void DrawLine(Graphics objGraphics, PointF objPointOne, PointF objPointTwo, Color objColor, int intLineThickness) {
            using(var objPen = new Pen(new SolidBrush(objColor), intLineThickness)) {
                objPen.StartCap = LineCap.Round;
                objPen.EndCap = LineCap.Round;
                objGraphics.DrawLine(objPen, objPointOne, objPointTwo);
            }
        }

        /// <summary>
        ///   Gets the coordinate.
        /// </summary>
        /// <param name="objCircleCenter"> The Circle center. </param>
        /// <param name="intRadius"> The radius. </param>
        /// <param name="indblAngle"> The angle. </param>
        /// <returns> </returns>
        private static PointF GetCoordinate(PointF objCircleCenter, int intRadius, double indblAngle) {
            var dblAngle = Math.PI * indblAngle / NumberOfDegreesInHalfCircle;

            return new PointF(objCircleCenter.X + intRadius * (float) Math.Cos(dblAngle), objCircleCenter.Y + intRadius * (float) Math.Sin(dblAngle));
        }

        /// <summary>
        ///   Gets the spokes angles.
        /// </summary>
        private void GetSpokesAngles() {
            _mAngles = GetSpokesAngles(NumberSpoke);
        }

        /// <summary>
        ///   Gets the spoke angles.
        /// </summary>
        /// <returns> An array of angle. </returns>
        private static double[] GetSpokesAngles(int intNumberSpoke) {
            var angles = new double[intNumberSpoke];
            var dblAngle = NumberOfDegreesInCircle / intNumberSpoke;

            for(var shtCounter = 0; shtCounter < intNumberSpoke; shtCounter++)
                angles[shtCounter] = (shtCounter == 0 ? dblAngle : angles[shtCounter - 1] + dblAngle);

            return angles;
        }

        /// <summary>
        ///   Actives the timer.
        /// </summary>
        private void ActiveTimer() {
            if(_mIsTimerActive)
                _mTimer.Start();
            else {
                _mTimer.Stop();
                _mProgressValue = 0;
            }

            GenerateColorsPallet();
            Invalidate();
        }

        /// <summary>
        ///   Sets the circle appearance.
        /// </summary>
        /// <param name="numberSpoke"> The number spoke. </param>
        /// <param name="spokeThickness"> The spoke thickness. </param>
        /// <param name="innerCircleRadius"> The inner circle radius. </param>
        /// <param name="outerCircleRadius"> The outer circle radius. </param>
        public void SetCircleAppearance(int numberSpoke, int spokeThickness, int innerCircleRadius, int outerCircleRadius) {
            NumberSpoke = numberSpoke;
            SpokeThickness = spokeThickness;
            InnerCircleRadius = innerCircleRadius;
            OuterCircleRadius = outerCircleRadius;

            Invalidate();
        }
    }
}