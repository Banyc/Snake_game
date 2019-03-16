Public Enum MovementJudge
    Normal
    Crashed
    Promoted
End Enum

Public Class Map
    Private _map(,) As Node
    Private _size As Point

    Private Enum NodeState
        None
        Bonus
        SnakeBody
        Block
    End Enum

    Private Structure Node
        Dim State As NodeState
        Dim SnakeId As Integer
    End Structure

    Private Function NewNode()
        Dim node = New Node
        node.State = NodeState.None
        node.SnakeId = -1
        Return node
    End Function

    Public Sub New(size As Point)
        _size = size
        ReDim _map(size.X, size.Y)

        For y = 0 To size.Y
            For x = 0 To size.X
                _map(x, y) = NewNode()
            Next
        Next

        InitWalls(size)
    End Sub

    ' judges the interaction of the snake with the map
    ' `pos` is the pending step of the snake
    ' returns enum `MovementJudge`
    Public Function Judge(pos As Point) As MovementJudge
        If _map(pos.X, pos.Y).State = NodeState.SnakeBody Or _map(pos.X, pos.Y).State = NodeState.Block Then
            Return MovementJudge.Crashed
        ElseIf _map(pos.X, pos.Y).State = NodeState.Bonus Then
            Return MovementJudge.Promoted
        Else    'ElseIf _map(pos.X, pos.Y).State = NodeState.None Then
            Return MovementJudge.Normal
        End If
    End Function

    Public Sub CleanNode(pos As Point)
        _map(pos.X, pos.Y).State = NodeState.None
    End Sub

    Public Sub AddSnakeNode(pos As Point, Optional snakeId As Integer = -1)
        _map(pos.X, pos.Y).State = NodeState.SnakeBody
        _map(pos.X, pos.Y).SnakeId = snakeId
    End Sub

    Public Sub AddBonusNode(pos As Point)
        _map(pos.X, pos.Y).State = NodeState.Bonus
    End Sub

    Public Sub AddBlockNode(pos As Point)
        _map(pos.X, pos.Y).State = NodeState.Block
    End Sub

    Private Sub InitWalls(size As Point)
        Dim x, y As Integer
        For x = 0 To size.X
            AddBlockNode(New Point(x, 0))
            AddBlockNode(New Point(x, size.Y))
        Next
        For y = 0 To size.Y
            AddBlockNode(New Point(0, y))
            AddBlockNode(New Point(size.X, y))
        Next
    End Sub

    Public Function GetSize() As Point
        Return _size
    End Function

    Public Sub Clear()

    End Sub

End Class
