Public Class Snake
    Inherits Mapping

    Private _IsGG As Boolean

    Private _gM As GameManager

    Private _map As Map

    Private _intiLeng As Integer ' initial length of the snake

    Private _rnd As Random

    Private _prevDirec As Direction  ' previous direction

    Public Sub New(canvasPos As UIElementCollection, thickness As Thickness, gM As GameManager, map As Map, Optional color As Color = Nothing, Optional snakeId As Integer = -1, Optional startPos As Point = Nothing)
        MyBase.New(canvasPos, thickness, GameElementType.Snake, map, color)

        _IsGG = False

        _gM = gM

        _map = map

        _rnd = New Random()

        _intiLeng = 6  ' initial length of the snake

        _prevDirec = Direction.None  ' previous direction

        If startPos <> Nothing Then
            NewSnake(startPos)
        Else
            NewSnake()
        End If
    End Sub

    ' paints a new snake
    Public Sub NewSnake()
        ClearSnake()
        _IsGG = False

        ' draws the dot on canvas
        AppendRanDot()
    End Sub

    ' paints a new snake
    Public Sub NewSnake(startPos As Point)
        ClearSnake()
        _IsGG = False

        ' draws the dot on canvas
        AppendDot(startPos)
    End Sub

    ' stretch the head of the snack and deletes the tail of it
    ' returns if it obeys rules
    Public Sub SnakeMove(direc As Direction)
        If Not _IsGG Then
            If IsOpposite(direc, _prevDirec) Then
                direc = _prevDirec
            End If

            ' renew direction
            _prevDirec = direc

            ' finds out the next step of the snake
            Dim newPoint As Point
            Select Case direc
                Case Direction.Left
                    newPoint = New Point(GetLastBody().X - 1, GetLastBody().Y)
                Case Direction.Right
                    newPoint = New Point(GetLastBody().X + 1, GetLastBody().Y)
                Case Direction.Up
                    newPoint = New Point(GetLastBody().X, GetLastBody().Y - 1)
                Case Direction.Down
                    newPoint = New Point(GetLastBody().X, GetLastBody().Y + 1)
                Case Else
                    newPoint = Nothing
            End Select

            If newPoint <> Nothing Then

                Dim pendingState As MovementJudge = _map.Judge(newPoint)
                Select Case pendingState
                    Case MovementJudge.Normal
                        If GetBodies().Count >= _intiLeng Then
                            CutTail()
                        End If
                        AppendDot(newPoint)
                    Case MovementJudge.Crashed
                        ' GameOver()
                        KillSnake()
                    Case MovementJudge.Promoted
                        AppendDot(newPoint)
                        _gM.ResetBonus(newPoint)

                End Select
            End If
        End If

    End Sub

    Private Function IsOpposite(d1 As Direction, d2 As Direction) As Boolean
        If d1 = Direction.Up And d2 = Direction.Down Then
            Return True
        ElseIf d1 = Direction.Down And d2 = Direction.Up Then
            Return True
        ElseIf d1 = Direction.Left And d2 = Direction.Right Then
            Return True
        ElseIf d1 = Direction.Right And d2 = Direction.Left Then
            Return True
        End If
        Return False
    End Function

    ' cuts the last dot of the body of snake
    Private Sub CutTail()
        CleanOldestDot()
    End Sub

    Private Sub KillSnake()
        ' TODO: changes color
        _IsGG = True

        TurnBodyRed()

        _gM.GameOver()

    End Sub

    Public Sub ClearSnake()
        _prevDirec = Direction.None

        MyBase.Clear()
    End Sub
End Class



