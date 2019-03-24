Public Class Bonus
    Inherits Mapping

    Private _minNum As Integer

    Public Sub New(canvasPos As UIElementCollection, thickness As Thickness, gM As GameManager, map As Map, Optional color As Color = Nothing, Optional minNum As Integer = 1)
        MyBase.New(canvasPos, thickness, GameElementType.Bonus, map, color)

        _minNum = minNum
    End Sub

    Public Sub NewBonus()
        For i = 0 To _minNum
            AppendRanDot()
        Next
    End Sub

    Public Sub ResetBonus(pos As Point)
        CleanDot(pos)

        AppendRanDot()
    End Sub

    Public Overrides Sub Clear()
        MyBase.Clear()
    End Sub
End Class
