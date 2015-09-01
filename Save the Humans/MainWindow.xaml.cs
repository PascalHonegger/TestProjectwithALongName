using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Save_the_Humans
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// 
    ///     Commits have Swag!
    /// 
    /// Wag by Aladinasfdasdf
    /// 
    ///     Created by Pascal Honegger, Alain Keller and Seraphin Rihm as a test Project.
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer _enemyTimer = new DispatcherTimer();
        private readonly Random _random = new Random();
        private readonly DispatcherTimer _targetTimer = new DispatcherTimer();
        private bool _humanCaptured;

        public MainWindow()
        {
            InitializeComponent();

            _enemyTimer.Tick += _enemyTimer_Tick;
            _enemyTimer.Interval = TimeSpan.FromSeconds(2);

            _targetTimer.Tick += _targetTimer_Tick;
            _targetTimer.Interval = TimeSpan.FromSeconds(.1);
        }

        private void _targetTimer_Tick(object sender, EventArgs e)
        {
            ProgressBar.Value += 1;
            if (ProgressBar.Value >= ProgressBar.Maximum)
                EndTheGame();
        }

        private void EndTheGame()
        {
            if (PlayArea.Children.Contains(GameOverText)) return;

            _enemyTimer.Stop();
            _targetTimer.Stop();
            _humanCaptured = false;
            PlayArea.Children.Clear();
            StartButton.Visibility = Visibility.Visible;
            PlayArea.Children.Add(GameOverText);
            GameOverText.Visibility = Visibility.Visible;
        }

        private void _enemyTimer_Tick(object sender, EventArgs e)
        {
            AddEnemy();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void AddEnemy()
        {
            var enemy = new ContentControl {Template = Resources["EnemyTemplate"] as ControlTemplate};
            AnimateEnemy(enemy, 0, PlayArea.ActualWidth - 100, "(Canvas.Left)");
            AnimateEnemy(enemy, _random.Next((int) PlayArea.ActualHeight - 100),
                _random.Next((int) PlayArea.ActualHeight - 100), "(Canvas.Top)");
            PlayArea.Children.Add(enemy);

            enemy.MouseEnter += enemy_MouseEnter;
        }

        private void enemy_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_humanCaptured)
                EndTheGame();
        }

        private void AnimateEnemy(ContentControl enemy, double from, double to, string propertyToAnimate)
        {
            var storyboard = new Storyboard {AutoReverse = true, RepeatBehavior = RepeatBehavior.Forever};

            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(_random.Next(4, 6)))
            };
            Storyboard.SetTarget(animation, enemy);
            Storyboard.SetTargetProperty(animation, new PropertyPath(propertyToAnimate));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private void StartGame()
        {
            Human.IsHitTestVisible = true;
            _humanCaptured = false;
            ProgressBar.Value = 0;
            StartButton.Visibility = Visibility.Hidden;
            PlayArea.Children.Clear();
            PlayArea.Children.Add(Target);
            PlayArea.Children.Add(Human);
            _enemyTimer.Start();
            _targetTimer.Start();
        }

        private void human_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_enemyTimer.IsEnabled) return;

            _humanCaptured = true;
            Human.IsHitTestVisible = true;
        }

        private void target_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_targetTimer.IsEnabled || !_humanCaptured) return;

            ProgressBar.Value = 0;
            Canvas.SetLeft(Target, _random.Next(100, (int) PlayArea.ActualWidth - 100));
            Canvas.SetTop(Target, _random.Next(100, (int) PlayArea.ActualHeight - 100));
            Canvas.SetLeft(Human, _random.Next(100, (int) PlayArea.ActualWidth - 100));
            Canvas.SetTop(Human, _random.Next(100, (int) PlayArea.ActualHeight - 100));
            _humanCaptured = false;
            Human.IsHitTestVisible = true;
        }

        private void playArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_humanCaptured) return;

            var pointerPosition = e.GetPosition(null);
            var relativePosition = Grid.TransformToVisual(PlayArea).Transform(pointerPosition);
            if ((Math.Abs(relativePosition.X - Canvas.GetLeft(Human))) > Human.ActualWidth*3 ||
                (Math.Abs(relativePosition.Y - Canvas.GetTop(Human))) > Human.ActualHeight*3)
            {
                _humanCaptured = false;
                Human.IsHitTestVisible = true;
            }
            else
            {
                Canvas.SetLeft(Human, relativePosition.X - Human.ActualWidth/2);
                Canvas.SetTop(Human, relativePosition.Y - Human.ActualHeight/2);
            }
        }

        private void playArea_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_humanCaptured)
            {
                EndTheGame();
            }
        }
    }
}