//using Atlas.UI.Extensions;
//using System.Collections;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Input;

//namespace Atlas.UI.Controls
//{
//    public class SearchBox : Control
//    {
//        private TextBox _userInputTextBox;
//        private Popup _suggestionsPopup;
//        private ListView _selectorListView;

//        private bool _ignoreTextChanges;
//        private bool _ignoreSelectionClosure;

//        public delegate IEnumerable FilterAction(IEnumerable itemsSource, string filter);

//        public static readonly DependencyProperty ItemsSourceProperty = Dependency.Register<IEnumerable>(nameof(ItemsSource));
//        public static readonly DependencyProperty FilteredItemsSourceProperty = Dependency.Register<IEnumerable>(nameof(FilteredItemsSource));
//        public static readonly DependencyProperty FilterProperty = Dependency.Register<FilterAction>(nameof(Filter));
//        public static readonly DependencyProperty DefaultToAllOnFilterFailureProperty = Dependency.Register<bool>(nameof(DefaultToAllOnFilterFailure));
//        public static readonly DependencyProperty SelectedItemProperty = Dependency.Register<object>(nameof(SelectedItem));

//        public IEnumerable ItemsSource
//        {
//            get => (IEnumerable)GetValue(ItemsSourceProperty);
//            set => SetValue(ItemsSourceProperty, value);
//        }

//        public IEnumerable FilteredItemsSource
//        {
//            get => (IEnumerable)GetValue(FilteredItemsSourceProperty);
//            internal set => SetValue(FilteredItemsSourceProperty, value);
//        }

//        public FilterAction Filter
//        {
//            get => (FilterAction)GetValue(FilterProperty);
//            set => SetValue(FilterProperty, value);
//        }

//        public bool DefaultToAllOnFilterFailure
//        {
//            get => (bool)GetValue(DefaultToAllOnFilterFailureProperty);
//            set => SetValue(DefaultToAllOnFilterFailureProperty, value);
//        }

//        public object SelectedItem
//        {
//            get => GetValue(SelectedItemProperty);
//            set => SetValue(SelectedItemProperty, value);
//        }

//        static SearchBox()
//        {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
//        }

//        public override void OnApplyTemplate()
//        {
//            base.OnApplyTemplate();

//            _userInputTextBox = GetTemplateChild("PART_UserInputTextBox") as TextBox;
//            _suggestionsPopup = GetTemplateChild("PART_SuggestionsPopup") as Popup;
//            _selectorListView = GetTemplateChild("PART_SelectorListView") as ListView;

//            _userInputTextBox.TextChanged += _userInputTextBox_TextChanged;
//            _userInputTextBox.PreviewKeyDown += _userInputTextBox_PreviewKeyDown;
//            _userInputTextBox.LostKeyboardFocus += _userInputTextBox_LostKeyboardFocus;
//            _selectorListView.SelectionChanged += _selectorListView_SelectionChanged;
//        }

//        private void _userInputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
//        {
//            base.OnPreviewKeyDown(e);

//            switch (e.Key)
//            {
//                case Key.Down:
//                    GoDown();
//                    break;

//                case Key.Up:
//                    GoUp();
//                    break;

//                case Key.Escape:
//                    Cancel();
//                    break;

//                case Key.Enter:
//                case Key.Tab:
//                    Select();
//                    break;
//            }
//        }

//        private void GoDown()
//        {
//            _ignoreSelectionClosure = true;
//            if (_selectorListView.SelectedIndex == _selectorListView.Items.Count - 1)
//            {
//                _selectorListView.SelectedIndex = -1;
//            }
//            else
//            {
//                _selectorListView.SelectedIndex++;
//            }
//            _ignoreSelectionClosure = false;
//        }

//        private void GoUp()
//        {
//            _ignoreSelectionClosure = true;
//            if (_selectorListView.SelectedIndex == -1)
//            {
//                _selectorListView.SelectedIndex = _selectorListView.Items.Count - 1;
//            }
//            else
//            {
//                _selectorListView.SelectedIndex -= 1;
//            }
//            _ignoreSelectionClosure = false;
//        }

//        private void Cancel()
//        {
//            _suggestionsPopup.IsOpen = false;
//            Keyboard.ClearFocus();
//        }

//        private void Select()
//        {
//            if (_selectorListView.SelectedIndex == -1)
//                return;

//            _ignoreTextChanges = true;
//            SelectedItem = _selectorListView.SelectedItem = _selectorListView.Items[_selectorListView.SelectedIndex];
//            _suggestionsPopup.IsOpen = false;
//            _ignoreTextChanges = false;
//        }

//        private void _userInputTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
//        {
//            if (!_userInputTextBox.IsKeyboardFocusWithin)
//            {
//                _ignoreTextChanges = true;
//                _userInputTextBox.Text = SelectedItem?.ToString();
//                _ignoreTextChanges = false;
//            }
//        }

//        private void _selectorListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            if (e.AddedItems.Count > 0)
//            {
//                _ignoreTextChanges = true;
//                SelectedItem = e.AddedItems[0];
//                _userInputTextBox.Text = SelectedItem.ToString();

//                if (!_ignoreSelectionClosure)
//                    _suggestionsPopup.IsOpen = false;

//                _ignoreTextChanges = false;
//            }
//        }

//        private void _userInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
//        {
//            if (_ignoreTextChanges)
//                return;

//            if (!string.IsNullOrEmpty(_userInputTextBox.Text))
//            {
//                FilteredItemsSource = Filter?.Invoke(ItemsSource, _userInputTextBox.Text);
//                _suggestionsPopup.IsOpen = true;
//            }
//            else
//            {
//                if (DefaultToAllOnFilterFailure)
//                {
//                    FilteredItemsSource = ItemsSource;
//                    _suggestionsPopup.IsOpen = true;
//                }
//                else
//                {

//                    _suggestionsPopup.IsOpen = false;
//                }
//            }
//        }
//    }
//}
