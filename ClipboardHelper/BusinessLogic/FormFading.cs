using ClipboardHelperRegEx.Properties;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    /// <summary>
    ///     A class which helps hiding/showing a form by fading it out/in or instant hiding/showing.
    /// </summary>
    public sealed class FormFading : IDisposable
    {
        public delegate void DgEventRaiserEventHandler(object sender, EventArgs e);

        public enum ApplyWhen
        {
            WhenMouseLeavesForm,
            Instantly,
            NotDefined
        }

        public enum ChangeVisibility
        {
            Gradually,
            Instantly,
            NotDefined
        }

        public enum Visibility
        {
            Hiding,
            Showing,
            ValueInSettings
        }

        private static readonly object Locker = new object();
        private readonly Timer _hideTimer;
        private readonly Stopwatch _measureHowManySecondsSinceMouseCursorLeftForm = new Stopwatch();
        private ApplyWhen _applyWhen;
        private ChangeVisibility _changeVisibility;
        private bool _mouseAlreadyRegisteredInside;

        private Visibility _storedOpacity;

        /// <summary>
        ///     A class which helps hiding/showing a form by fading it out/in or hiding/showing it instantly.
        /// </summary>
        /// <param name="form">The form which needs to be hidden.</param>
        // ReSharper disable once UnusedParameter.Local
        public FormFading(Form form)
        {
            Form = form;
            CursorTimeOutsideFormUntilStartHiding = 30;
            _hideTimer = new Timer
            {
                Interval = 15
            };
            _hideTimer.Tick += FadeFormByChangingOpacity;
            CheckPeriodicallyIfMouseCursorIsInsideForm();
        }

        private Form Form { get; }
        public int CursorTimeOutsideFormUntilStartHiding { get; set; }
        public bool Enabled { get; set; }
        private Visibility VisibilityTo { get; set; }

        /// <summary>
        ///     Event occurs when state has changed.
        /// </summary>
        public event DgEventRaiserEventHandler WorkFinished;

        /// <summary>
        ///     Action triggered when form is shown at full opacity.
        /// </summary>
        public event DgEventRaiserEventHandler Shown;

        /// <summary>
        ///     Sets a new status for the form visibility.
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="changeVisibility"></param>
        /// <param name="applyWhen"></param>
        public void SetStatusTo(Visibility visibility, ChangeVisibility changeVisibility = ChangeVisibility.NotDefined,
            ApplyWhen applyWhen = ApplyWhen.NotDefined)
        {
            lock (Locker)
            {
                _applyWhen = applyWhen;
                _changeVisibility = changeVisibility;
                Change(visibility);
            }
        }

        /// <summary>
        ///     Change the current form visibility status with a new one.
        /// </summary>
        /// <param name="changeVisibilityTo"></param>
        private async void Change(Visibility changeVisibilityTo)
        {
            _hideTimer.Enabled = false;
            Enabled = false;
            VisibilityTo = changeVisibilityTo;
            _storedOpacity = changeVisibilityTo;

            if (_applyWhen == ApplyWhen.Instantly)
            {
                _hideTimer.Enabled = true;
                Enabled = true;
                return;
            }

            switch (VisibilityTo)
            {
                case Visibility.Hiding when MouseInsideForm():
                    _hideTimer.Enabled = false;
                    _mouseAlreadyRegisteredInside = false;
                    break;
                case Visibility.Hiding when !MouseInsideForm():
                    _mouseAlreadyRegisteredInside = false;
                    break;
                case Visibility.Showing when MouseInsideForm():
                    _hideTimer.Enabled = true;
                    _mouseAlreadyRegisteredInside = false;
                    break;
                case Visibility.ValueInSettings:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(changeVisibilityTo),
                        Resources.FormFading_Change_An_error_occured_while_changing_the_current_form_visibility_status_with_a_new_one_);
            }

            if (!Enabled)
            {
                Enabled = true;
                CheckPeriodicallyIfMouseCursorIsInsideForm();
            }

            while (Enabled)
                await Task.Run(async () => { await Task.Delay(200).ConfigureAwait(true); }).ConfigureAwait(true);
        }

        /// <summary>
        ///     Checks whether mouse is outside form and enables a timer to change the form visibility.
        /// </summary>
        private async void CheckPeriodicallyIfMouseCursorIsInsideForm()
        {
            while (Enabled)
            {
                if (MouseInsideForm() && !_mouseAlreadyRegisteredInside && VisibilityTo != Visibility.Showing)
                {
                    _mouseAlreadyRegisteredInside = true;
                    _storedOpacity = VisibilityTo; //store for later retrieval when mouse is outside again.
                    VisibilityTo = Visibility.Showing;
                }
                else if (!MouseInsideForm() && _mouseAlreadyRegisteredInside &&
                         _measureHowManySecondsSinceMouseCursorLeftForm.Elapsed.Seconds >=
                         CursorTimeOutsideFormUntilStartHiding)
                {
                    _mouseAlreadyRegisteredInside = false;
                    VisibilityTo = _storedOpacity;
                    if (!_hideTimer.Enabled) _hideTimer.Enabled = true;
                }

                await Task.Run(async () => { await Task.Delay(200).ConfigureAwait(true); }).ConfigureAwait(true);
            }
        }

        /// <summary>
        ///     Is mouse inside the form boundaries.
        /// </summary>
        /// <returns></returns>
        private bool MouseInsideForm()
        {
            if (Form.Bounds.Contains(Control.MousePosition) && Form.Visible) //cursor inside window
            {
                _measureHowManySecondsSinceMouseCursorLeftForm.Reset();
                return true;
            }

            _measureHowManySecondsSinceMouseCursorLeftForm.Start(); //cursor outside window
            return false;
        }

        /// <summary>
        ///     A method run by a timer to change the forms opacity and visibility.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FadeFormByChangingOpacity(object sender, EventArgs e)
        {
            switch (VisibilityTo)
            {
                case Visibility.Hiding:
                    switch (_changeVisibility)
                    {
                        case ChangeVisibility.Gradually:
                            if (Form.Opacity <= 0)
                            {
                                Finish();
                                Form.Hide();
                            }
                            else
                            {
                                Form.Opacity -= 0.006;
                            }

                            break;
                        case ChangeVisibility.Instantly:
                            Form.Hide();
                            Form.Opacity = 0;
                            Finish();
                            break;
                        case ChangeVisibility.NotDefined:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(sender),
                                Resources.FormFading_FadeFormByChangingOpacity_An_error_occured_while_changing_the_forms_opacity_and_visibility);
                    }

                    break;
                case Visibility.Showing:
                    switch (_changeVisibility)
                    {
                        case ChangeVisibility.Gradually:
                            if (!Form.Visible) Form.Show();
                            if (Form.Opacity >= 0.99)
                            {
                                if (_storedOpacity == Visibility.Showing)
                                {
                                    Finish();
                                    Shown?.Invoke(this, EventArgs.Empty);
                                }
                            }
                            else
                            {
                                Form.Opacity += 0.08;
                            }

                            break;
                        case ChangeVisibility.Instantly:
                            Form.Opacity = 0.99;
                            if (!Form.Visible) Form.Show();
                            Finish();
                            Shown?.Invoke(this, EventArgs.Empty);
                            break;
                        case ChangeVisibility.NotDefined:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(sender),
                                Resources.FormFading_FadeFormByChangingOpacity_An_error_occured_while_changing_the_forms_opacity_and_visibility);
                    }

                    break;
                case Visibility.ValueInSettings:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sender),
                        Resources.FormFading_FadeFormByChangingOpacity_An_error_occured_while_changing_the_forms_opacity_and_visibility);
            }
        }

        /// <summary>
        ///     Finish the current visibility change job.
        /// </summary>
        private void Finish()
        {
            WorkFinished?.Invoke(this, EventArgs.Empty);
            _hideTimer.Enabled = false;
            Enabled = false;
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing) _hideTimer.Dispose();
            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}