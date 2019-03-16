Public Enum GameElementType
    Snake
    Bonus
    Wall
End Enum

Public Class Mapping
    Private drawGround As Canvas
    Private _bodies As List(Of Point)
    Private _thickness As Thickness
    Private _gEType As GameElementType
    Private _map As Map
    Private _rnd As Random
    Private _id As Integer
    Private _color As Color

    Public Sub New(canvasPos As UIElementCollection, thickness As Thickness, gEType As GameElementType, map As Map, Optional color As Color = Nothing, Optional id As Integer = -1)
        _bodies = New List(Of Point)

        _gEType = gEType

        _id = id

        If color <> Nothing Then
            _color = color
        Else
            _color = Colors.White
        End If

        _map = map

        _thickness = thickness

        _rnd = New Random()

        drawGround = New Canvas()

        canvasPos.Add(drawGround)
    End Sub

    Protected Function GetBodies()
        Return _bodies
    End Function

    Protected Function GetLastBody()
        Return _bodies.Last()
    End Function

    Protected Sub TurnBodyRed()
        For Each rect In drawGround.Children
            rect.Fill = Brushes.Red
        Next
    End Sub

    Protected Sub CleanDot(pos As Point)
        _map.CleanNode(pos)
        Dim index As Integer
        index = _bodies.IndexOf(pos)
        _bodies.RemoveAt(index)
        drawGround.Children.RemoveAt(index)
    End Sub

    Protected Sub CleanOldestDot()
        _map.CleanNode(_bodies(0))
        _bodies.RemoveAt(0)
        drawGround.Children.RemoveAt(0)
    End Sub

    Protected Sub AppendRanDot()
        Dim newPoint As Point
        newPoint = New Point(_rnd.Next(0, _map.GetSize().X), _rnd.Next(0, _map.GetSize().Y))
        ' TODO: If the map full
        While _map.Judge(newPoint) <> MovementJudge.Normal
            newPoint = New Point(_rnd.Next(0, _map.GetSize().X), _rnd.Next(0, _map.GetSize().Y))
        End While

        AppendDot(newPoint)
    End Sub

    ' draws the dot on canvas
    Protected Sub AppendDot(fakePos As Point)
        _bodies.Add(fakePos)

        Dim rect = GetRealDot()
        Canvas.SetLeft(rect, GetRealPos(fakePos).X)
        Canvas.SetTop(rect, GetRealPos(fakePos).Y)

        Select Case _gEType
            Case GameElementType.Snake
                _map.AddSnakeNode(fakePos, _id)
            Case GameElementType.Bonus
                _map.AddBonusNode(fakePos)
            Case GameElementType.Wall
                _map.AddBlockNode(fakePos)
        End Select

        drawGround.Children.Add(rect)
    End Sub

    ' gets the real pixel `Point`
    Private Function GetRealPos(point As Point)
        Return New Point(point.X * _thickness, point.Y * _thickness)
    End Function

    ' gets a new rectangle as a dot on the canvas
    Private Function GetRealDot() As Rectangle
        Dim rect As New Rectangle With {
            .Width = _thickness,
            .Height = _thickness,
            .Fill = New SolidColorBrush(_color),
            .Stroke = Nothing,
            .UseLayoutRounding = True  ' http://10rem.net/blog/2010/04/08/layout-rounding
        }
        Return rect
    End Function

    Protected Sub Clear()
        For Each pos In _bodies
            _map.CleanNode(pos)
        Next
        _bodies.Clear()
        drawGround.Children.Clear()
    End Sub

    ' gets fake canvas size
    Protected Function GetCanvSize()
        Return New Point(drawGround.ActualWidth / _thickness, drawGround.ActualHeight / _thickness)
    End Function
End Class
