Public Enum Thickness
    Thick = 5
    Thin = 1
    Custom = 10
End Enum

Public Enum Direction
    None
    Up
    Down
    Left
    Right
End Enum

Public Enum NumOfPlayer
    SinglePlayer = 1
    MultiplePlayers
End Enum

Public Enum BonusSpeed
    DamnSlow

End Enum

Public Class GameManager
    Private _IsInGame As Boolean

    Private _thick  ' of the graphics

    Private _mapSize As Point  ' the size of the map

    Private _Timer As Timers.Timer

    Private _map As Map

    Private _minNumBonus As Integer  ' minimal number of bonus
    Private _bonus As Bonus
    Private _bonusColor As Color = Colors.Yellow

    Private _numOfPlayer As NumOfPlayer
    Private _whichMoves As Integer
    Private _snakes As List(Of Snake)

    Private _wall As Wall

    Private _direc() As Direction

    Private _snakesColor As List(Of Color)

    Public Sub New(canvasPos As UIElementCollection, timer As Timers.Timer, thickness As Thickness, mapSize As Point, numOfPlayer As NumOfPlayer, Optional minNumBonus As Integer = 1)
        _mapSize = mapSize
        _thick = thickness


        _map = New Map(_mapSize)

        _wall = New Wall(canvasPos, _thick, Me, _map)

        _minNumBonus = minNumBonus

        _bonus = New Bonus(canvasPos, _thick, Me, _map, _bonusColor, _minNumBonus)

        ' modify the color of the snakes HERE!
        _snakesColor = New List(Of Color)
        _snakesColor.Add(Colors.LightBlue)
        _snakesColor.Add(Colors.LightPink)

        _numOfPlayer = numOfPlayer
        ReDim _direc(_numOfPlayer - 1)
        _snakes = New List(Of Snake)
        For i = 0 To _numOfPlayer - 1
            Dim snake = New Snake(canvasPos, _thick, Me, _map, _snakesColor.Item(i), i + 1)
            _snakes.Add(snake)
            _direc(0) = Direction.None
        Next

        _Timer = timer
        _Timer.Interval /= _numOfPlayer

        InitiateGame()
    End Sub

    Private Sub InitiateGame()
        _IsInGame = False
        _Timer.Stop()

        ' cleans historical map and momentum
        ResetDirec()
        For Each snake In _snakes
            snake.ClearSnake()
        Next
        _bonus.ClearAll()
        _wall.ClearAll()
        _map.Clear()

        ' initiates the map elements
        _wall.InitBorder()
        For Each snake In _snakes
            snake.NewSnake()
        Next
        _bonus.NewBonus()

        ' launches the game
        _IsInGame = True
        _Timer.Start()
    End Sub

    Public Sub Timer_tick()
        Dim snakeIndex As Integer = WhichSnakeMoves()
        _snakes(snakeIndex).SnakeMove(_direc(snakeIndex))
    End Sub

    ' returns an index num whose snake is in turn to move
    Private Function WhichSnakeMoves()
        _whichMoves += 1
        If _whichMoves >= _snakes.Count Then
            _whichMoves -= _snakes.Count
        End If
        Return _whichMoves
    End Function

    Public Sub GameOver()
        _IsInGame = False
        _Timer.Stop()
    End Sub

    Public Sub Restart()
        InitiateGame()
    End Sub

    Public Sub UpdateDirec(direc As Direction, Optional snakeId As Integer = -1)
        If snakeId = -1 Then
            _direc(0) = direc
        Else
            If snakeId <= _numOfPlayer Then
                _direc(snakeId - 1) = direc
            End If
        End If
    End Sub

    Private Sub ResetDirec()
        For i = 0 To _direc.Count() - 1
            _direc(i) = Direction.None
        Next
    End Sub

    Public Function GetMapSize()
        Return _mapSize
    End Function

    ' `bonusPos` is the pos of a bonus needed to be killed
    Public Sub ResetBonus(bonusPos As Point)
        _bonus.ResetBonus(bonusPos)
    End Sub
End Class
