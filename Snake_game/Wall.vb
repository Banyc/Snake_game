Public Class Wall
    Inherits Mapping

    Private _map As Map

    Public Sub New(canvasPos As UIElementCollection, thickness As Thickness, gM As GameManager, map As Map, Optional color As Color = Nothing)
        MyBase.New(canvasPos, thickness, GameElementType.Wall, map, color)

        _map = map
    End Sub

    Public Sub InitBorder()
        Dim x, y As Integer
        For x = 0 To _map.GetSize().X
            AppendDot(New Point(x, 0))
            AppendDot(New Point(x, _map.GetSize().Y))
        Next
        For y = 0 To _map.GetSize().Y
            AppendDot(New Point(0, y))
            AppendDot(New Point(_map.GetSize().X, y))
        Next
    End Sub

    Public Sub ClearAll()
        MyBase.Clear()
    End Sub
End Class
