Class MainWindow
    Private _length As Integer = 6  ' initial length of the snake

    Private _mapSize As New Point(60, 60)  ' the size of the map

    Private _numOfPlayer As NumOfPlayer = NumOfPlayer.MultiplePlayers

    Private _minNumBonus As Integer = 12

    Private _Timer As Timers.Timer

    Private _gM As GameManager

    Private _speed As Speed = Speed.Moderate
    Private Enum Speed
        DamnFast = 10
        Fast = 30
        Moderate = 50
        Slow = 100
        DamnSlow = 500
    End Enum

    Private _thick = Thickness.DamnThick  ' of the graphics
    Private Enum Thickness
        Thick = 5
        Thin = 1
        DamnThick = 10
    End Enum

    Public Sub New()
        InitializeComponent()

        ' initiates the map
        Me.Width = (_mapSize.X + 1) * _thick + 15
        Me.Height = (_mapSize.Y + 1) * _thick + 38

        ' initiates a new timer
        _Timer = New Timers.Timer()
        AddHandler _Timer.Elapsed, AddressOf Timer_tick
        _Timer.Interval = _speed

        ' initiates gameManager
        _gM = New GameManager(CanvasGrid.Children, _Timer, _thick, _mapSize, _numOfPlayer, _minNumBonus)

    End Sub

    Private Sub Timer_tick()
        Me.Dispatcher.BeginInvoke(Sub() _gM.Timer_tick())
    End Sub

    Private Sub OnKeyDownDD(sender As Object, e As KeyEventArgs)

        Select Case e.Key
            ' first snake
            Case Key.Left
                _gM.UpdateDirec(Direction.Left, 1)
            Case Key.Right
                _gM.UpdateDirec(Direction.Right, 1)
            Case Key.Up
                _gM.UpdateDirec(Direction.Up, 1)
            Case Key.Down
                _gM.UpdateDirec(Direction.Down, 1)

            ' second snake
            Case Key.A
                _gM.UpdateDirec(Direction.Left, 2)
            Case Key.D
                _gM.UpdateDirec(Direction.Right, 2)
            Case Key.W
                _gM.UpdateDirec(Direction.Up, 2)
            Case Key.S
                _gM.UpdateDirec(Direction.Down, 2)

            Case Key.F2
                _gM.Restart()
        End Select

        e.Handled = True
    End Sub
End Class
