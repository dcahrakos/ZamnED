Class MainWindow 
    Public OpenRom As New Microsoft.Win32.OpenFileDialog()


    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)
        Dim result? As Boolean = OpenRom.ShowDialog()
        OpenRom.FileName = "Zombies Ate My Neighbors (U)"
        OpenRom.DefaultExt = ".smc"
        OpenRom.Filter = "SNES Rom File (.smc)|*.smc"

        If result = True Then
            Dim filename As String = OpenRom.FileName
            Me.Title = "ZamnED - " + filename
        End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class
