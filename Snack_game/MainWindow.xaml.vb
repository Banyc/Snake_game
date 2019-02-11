Class MainWindow
    Private _IsInGame As Boolean
    Private _length As Integer = 6
    Private _snake As New List(Of Point)  ' by fake positions
    Private _bonus As New List(Of Point)

    Private _border As New Point(60, 60)

    'Private _Timer As Threading.DispatcherTimer
    Private _Timer As Timers.Timer
    Private _rnd As New Random()
    Private _direc As Direction = Direction.None
    Private _prevDirec As Direction = Direction.None
    Private Enum Direction
        None
        Up
        Down
        Left
        Right
    End Enum
    Private _speed As Speed = Speed.Fast
    Private Enum Speed
        DamnFast = 10
        Fast = 30
        Moderate = 50
        Slow = 100
        DamnSlow = 500
    End Enum
    Private _thick = Thickness.Thick
    Private Enum Thickness
        Thick = 5
        Thin = 1
        Custom = 10
    End Enum

    Public Sub New()
        InitializeComponent()

        ' initiates the map
        Me.Width = (_border.X + 1) * _thick + 15
        Me.Height = (_border.Y + 1) * _thick + 38
        PaintBorder(_border)
        NewSnake()
        NewBouns()

        ' initiates a new timer
        '_Timer = New System.Windows.Threading.DispatcherTimer()
        'AddHandler _Timer.Tick, AddressOf Timer_tick
        ''_Timer.Interval = New TimeSpan(Int(_speed))
        'Dim span As TimeSpan = TimeSpan.FromMilliseconds(60)
        '_Timer.Interval = span
        '_Timer.Start()

        ' initiates a new timer
        _Timer = New Timers.Timer()
        AddHandler _Timer.Elapsed, AddressOf Timer_tick
        _Timer.Interval = _speed
        _Timer.Start()
        'Me.Dispatcher.BeginInvoke(Sub() Timer_tick())

        ' initiates a new timer
        '_Timer = New Threading.Timer(AddressOf Timer_tick, Nothing, 60, Threading.Timeout.Infinite)

        _IsInGame = True
    End Sub

    Private Sub InitiateGame()
        ' cleans historical map
        Canvas1.Children.Clear()
        Background.Children.Clear()
        _bonus.Clear()
        _snake.Clear()
        _direc = Direction.None
        _prevDirec = Direction.None

        ' initiates the map
        Me.Width = (_border.X + 1) * _thick + 15
        Me.Height = (_border.Y + 1) * _thick + 38
        PaintBorder(_border)
        NewSnake()
        NewBouns()

        ' launchs the game
        _IsInGame = True
        _Timer.Start()
    End Sub

    Private Sub Timer_tick()
        'Private Sub Timer_tick(state As Object)

        Me.Dispatcher.BeginInvoke(Sub() SnakeMove())
        'SnakeMove()
    End Sub

    Private Sub OnKeyDownDD(sender As Object, e As KeyEventArgs)
        If _IsInGame Then
            Select Case e.Key
                Case Key.Left
                    If (_prevDirec <> Direction.Right) Then
                        _direc = Direction.Left
                    End If
                Case Key.Right
                    If (_prevDirec <> Direction.Left) Then
                        _direc = Direction.Right
                    End If
                Case Key.Up
                    If (_prevDirec <> Direction.Down) Then
                        _direc = Direction.Up
                    End If
                Case Key.Down
                    If (_prevDirec <> Direction.Up) Then
                        _direc = Direction.Down
                    End If
            End Select
        Else
            InitiateGame()
        End If
        e.Handled = True
    End Sub

    Private Sub NewBouns()
        NewBouns(_border)
    End Sub

    ' paints a new bouns
    Private Sub NewBouns(border As Point)
        Dim bounsPos = New Point(_rnd.Next(1, border.X), _rnd.Next(1, border.Y))
        ' prevents the new bouns from being on the body of the snake
        While (_snake.IndexOf(bounsPos) > -1)
            bounsPos = New Point(_rnd.Next(1, border.X), _rnd.Next(1, border.Y))
        End While
        ' draws the dot on canvas
        PaintForeDot(bounsPos, IsBouns:=True)
        _bonus.Add(bounsPos)
    End Sub

    Private Sub NewSnake()
        NewSnake(_border)
    End Sub

    ' paints a new snake
    Private Sub NewSnake(border As Point)
        Dim startPos As New Point(_rnd.Next(1, border.X), _rnd.Next(1, border.Y))
        ' draws the dot on canvas
        PaintForeDot(startPos, IsBouns:=False)
        _snake.Add(startPos)
    End Sub

    Private Sub PaintBorder(border As Point)
        Dim x, y As Integer
        For x = 0 To border.X
            PaintBackDot(New Point(x, 0))
            PaintBackDot(New Point(x, border.Y))
        Next
        For y = 0 To border.Y
            PaintBackDot(New Point(0, y))
            PaintBackDot(New Point(border.X, y))
        Next

    End Sub

    ' stretch the head of the snack and deletes the tail of it
    Private Sub SnakeMove()
        ' renew direction
        _prevDirec = _direc

        ' finds out the next step of the snake
        Dim newPoint As Point
        Select Case _direc
            Case Direction.Left
                newPoint = New Point(_snake.Last.X - 1, _snake.Last.Y)
            Case Direction.Right
                newPoint = New Point(_snake.Last.X + 1, _snake.Last.Y)
            Case Direction.Up
                newPoint = New Point(_snake.Last.X, _snake.Last.Y - 1)
            Case Direction.Down
                newPoint = New Point(_snake.Last.X, _snake.Last.Y + 1)
            Case Else
                newPoint = Nothing
        End Select

        If newPoint <> Nothing Then
            ' checks if the snake hits itself
            If _snake.IndexOf(newPoint) > -1 Then  ' if hits itself
                GameOver()
            End If

            ' checks If the snake hits the border
            If (newPoint.X < _border.X And newPoint.Y < _border.Y And newPoint.X > 0 And newPoint.Y > 0) Then
                ' if does not hits the border
                _snake.Add(newPoint)
                PaintForeDot(newPoint, IsBouns:=False)
            Else
                GameOver()
            End If

            ' checks if the snake should preserve its tail
            If _bonus.IndexOf(newPoint) = -1 And _snake.Count >= _length Then
                ' if does not hit the bonus AND the snake is shorter than default value
                CutTail()
            End If

            ' paints a new bonus
            If _bonus.IndexOf(newPoint) > -1 Then
                Canvas1.Children.RemoveAt(_bonus.IndexOf(newPoint))
                _bonus.RemoveAt(_bonus.IndexOf(newPoint))
                NewBouns()
            End If

        Else

        End If


    End Sub

    ' only stretch the head of snake
    Private Function StretchSnake(direc As Direction) As Boolean
        _prevDirec = _direc
        Dim newPoint As Point
        Select Case direc
            Case Direction.Left
                newPoint = New Point(_snake.Last.X - 1, _snake.Last.Y)
            Case Direction.Right
                newPoint = New Point(_snake.Last.X + 1, _snake.Last.Y)
            Case Direction.Up
                newPoint = New Point(_snake.Last.X, _snake.Last.Y - 1)
            Case Direction.Down
                newPoint = New Point(_snake.Last.X, _snake.Last.Y + 1)
        End Select

        ' checks if the new point is in the size of canvas
        Dim size As Point = GetFakeCanvSize()
        If (newPoint.X < size.X And newPoint.Y < size.Y And newPoint.X > 0 And newPoint.Y > 0) Then
            _snake.Add(newPoint)
            PaintForeDot(newPoint, IsBouns:=False)
            Return True
        Else
            Return False
        End If
    End Function
    ' cuts the last dot of the body of snake
    Private Sub CutTail()
        _snake.RemoveAt(0)
        Canvas1.Children.RemoveAt(1)  ' the first element is bonus
    End Sub

    ' draws the dot on canvas of foreground
    Private Sub PaintForeDot(fakePoint As Point, IsBouns As Boolean)
        Dim rect = GetRealDot()
        Canvas.SetLeft(rect, GetRealPos(fakePoint).X)
        Canvas.SetTop(rect, GetRealPos(fakePoint).Y)
        If (IsBouns) Then
            Canvas1.Children.Insert(0, rect)
        Else
            Canvas1.Children.Add(rect)
        End If
    End Sub

    ' draws the dot on canvas of foreground
    Private Sub PaintBackDot(fakePoint As Point)
        Dim rect = GetRealDot()
        Canvas.SetLeft(rect, GetRealPos(fakePoint).X)
        Canvas.SetTop(rect, GetRealPos(fakePoint).Y)

        Background.Children.Add(rect)
    End Sub

    ' gets the real pixel `Point`
    Private Function GetRealPos(point As Point)
        Return New Point(point.X * _thick, point.Y * _thick)
    End Function

    Private Function GetFakeCanvSize()
        Return New Point(Canvas1.ActualWidth / _thick, Canvas1.ActualHeight / _thick)
    End Function

    Private Function GetRealDot() As Rectangle
        Dim rect As New Rectangle
        rect.Width = _thick + 1
        rect.Height = _thick + 1
        rect.Fill = Brushes.White
        rect.Stroke = Nothing
        Return rect
    End Function

    Private Sub GameOver()
        _Timer.Stop()
        _IsInGame = False
    End Sub
End Class
