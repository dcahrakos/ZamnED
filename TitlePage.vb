Public Class TitlePage

    Public words As New List(Of Word)

    Public Sub New(ByVal s As IO.Stream)
        Do
            words.Add(New Word(s))
            If words.Last.last Then Exit Do
        Loop
    End Sub

    Public Sub New(ByVal tp As TitlePage)
        For Each w As Word In tp.words
            words.Add(New Word(w))
        Next
    End Sub

    Public Sub Write(ByVal lst As List(Of Byte), ByVal pageNum As Byte)
        For Each w As Word In words
            lst.AddRange(New Byte() {w.x, w.y, w.font, pageNum})
            For Each c As Integer In w.chars
                lst.Add(c)
            Next
            lst.Add(&HFF)
            If w Is words.Last Then
                lst(lst.Count - 1) = 0
            End If
        Next
    End Sub

    Public Sub Sort()
        Dim newlist As New List(Of Word)
        Do Until words.Count = 0
            Dim minValue As Integer = &H10000
            Dim minWord As Word
            For Each w As Word In words
                If w.y * &H100 + w.x < minValue Then
                    minValue = w.y * &H100 + w.x
                    minWord = w
                End If
            Next
            If minWord.chars.Count > 0 Then
                newlist.Add(minWord)
            End If
            words.Remove(minWord)
        Loop
        words = newlist
    End Sub

    Public Overrides Function ToString() As String
        Dim str As String = ""
        For Each w As Word In words
            str &= w.ToString & " "
        Next
        Return str.Trim()
    End Function
End Class

Public Class Word

    Public x As Byte
    Public y As Byte
    Public font As Byte
    Public chars As New List(Of Byte)
    Public last As Boolean

    Public Shared Strs As String() = {"", "", "", "", "", "", "", "", "", "", "", "", "", "F", "", "", _
                                      "J", "", "", "H", "", "", "N", "", "", "L", "", "", "P", "", "", "", _
                                      " ", "NT", "TH", "TE", "STE", "IT", "ET", "ST", "RT", "TO", "NTS", "TY", "CTO", "T", "TA", "PTS", _
                                      "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "O", "Q", "", "", "", "", _
                                      "", "A", "B", "C", "D", "E", "F", "G", "H", "A", "J", "K", "L", "M", "N", "O", _
                                      "P", "R", "R", "S", "P", "U", "EI", "LI", "X", "Y", "Z", "E", "S", "M", "", "", _
                                      "", "R", "E", "E", "I", "O", "S", "!", "F", "E", "R", "", "L", "E", "W", "", _
                                      "I", "R", "H", "H", "T", "V", "E", "", "I", "", "O", "N", "", " ", "", ""}

    Public Sub New(ByVal s As IO.Stream)
        Me.x = s.ReadByte
        Me.y = s.ReadByte
        Me.font = s.ReadByte
        s.ReadByte() 'skip page number
        Do
            Dim num As Byte = s.ReadByte
            If num = &HFF Or num = 0 Then
                last = (num = 0)
                Return
            End If
            chars.Add(num)
        Loop
    End Sub

    Public Sub New(ByVal w As Word)
        Me.x = w.x
        Me.y = w.y
        Me.font = w.font
        For Each b As Byte In w.chars
            Me.chars.Add(b)
        Next
    End Sub

    Public Sub New()

    End Sub

    Public Overrides Function ToString() As String
        Dim str As String = ""
        For l As Integer = 0 To chars.Count - 1
            If l > 0 AndAlso (chars(l) = &H2E And chars(l - 1) <> &H7B) Then
                str &= "ITA"
            Else
                str &= Strs(chars(l))
            End If
        Next
        Return str
    End Function
End Class