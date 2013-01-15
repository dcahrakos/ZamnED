'Imports System.IO
''Rom clas also borrowed from another ZAMN editor. proper links and credit will be inserted soon.
'Public Class ROM

'    Public path As String
'    Public regLvlCount As Integer
'    Public maxLvlNum As Integer
'    Public bonusLvls As New List(Of Integer)
'    Public names As New Dictionary(Of Integer, String)
'    Public failed As Boolean = False
'    Public TitlePageGFX As TitleGFX
'    Public OpenRom As New Microsoft.Win32.OpenFileDialog()


'    Public hacked As Boolean = False

'    Private Shared offsetPos As Integer() = {&H1C, &H1E, &H20, &H36, &H38, &H3A}

'    Public Sub New(ByVal path As String)
'        Try
'            Me.path = path
'            Dim s As New FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read)
'            If Shrd.HasHeader(s) Then
'                Shrd.RemoveHeader(s)
'                MsgBox("This ROM had a header which was automatically removed")
'            End If
'            s.Seek(Ptr.LevelPointers, SeekOrigin.Begin)
'            regLvlCount = s.ReadByte() + s.ReadByte() * &H100 - 1
'            maxLvlNum = regLvlCount
'            s.Seek(Ptr.BonusLvlNums, SeekOrigin.Begin)
'            Dim num As Integer
'            Dim curLvl As Integer = 0
'            For l As Integer = 0 To maxLvlNum
'                num = s.ReadByte() + s.ReadByte() * &H100
'                If num <> 0 Then
'                    bonusLvls.Add(num)
'                    maxLvlNum = Math.Max(maxLvlNum, num)
'                End If
'                curLvl += 1
'            Next
'            TitlePageGFX = New TitleGFX(s)
'            'Get level names
'            Dim ptrs As DList(Of Integer, Integer) = GetAllLvlPtrs(s)
'            For l As Integer = 0 To ptrs.L1.Count - 1
'                Try
'                    names.Add(ptrs.L1(l), GetLevelTitle(s, ptrs.L2(l)))
'                Catch ex As Exception
'                    names.Add(ptrs.L1(l), "ERROR: " & ex.Message)
'                End Try
'            Next

'            'Testing 4 byte level pointers

'            's.Seek(0, SeekOrigin.Begin)
'            'If s.ReadByte <> 159 Then
'            '    s.Seek(Ptr.LevelPointers + 2, SeekOrigin.Begin)
'            '    Dim lenDiff As Integer = (maxLvlNum + 1) * 2
'            '    Shrd.InsertBytes(s, lenDiff)
'            '    'Dim ptrs As DList(Of Integer, Integer) = GetAllLvlPtrs(s)
'            '    s.Seek(lvlPtrs + 2, SeekOrigin.Begin)
'            '    For l As Integer = 0 To maxLvlNum
'            '        If ptrs.L1.IndexOf(l) > -1 Then
'            '            s.Write(Shrd.ConvertAddr(ptrs.FromSecond(l) + lenDiff), 0, 4)
'            '            ptrs.L2(ptrs.L1.IndexOf(l)) += lenDiff
'            '        Else
'            '            s.Seek(4, SeekOrigin.Current)
'            '        End If
'            '    Next
'            '    For l As Integer = 0 To ptrs.L1.Count - 1
'            '        For m As Integer = 0 To offsetPos.Length - 1
'            '            s.Seek(ptrs.L2(l) + offsetPos(m), SeekOrigin.Begin)
'            '            Dim ptr As Integer = s.ReadByte + s.ReadByte * &H100 + lenDiff
'            '            s.Seek(-2, SeekOrigin.Current)
'            '            s.WriteByte(ptr Mod &H100)
'            '            s.WriteByte(ptr \ &H100)
'            '        Next
'            '    Next
'            '    s.Seek(0, SeekOrigin.Begin)
'            '    s.WriteByte(159)
'            'End If
'            'hacked = True

'            s.Close()
'        Catch ex As Exception
'            failed = True
'            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
'        End Try
'    End Sub

'    Public Function GetLevelTitle(ByVal s As Stream, ByVal ptr As Integer)
'        s.Seek(ptr + &H36, SeekOrigin.Begin)
'        Shrd.GoToRelativePointer(s, &H9F)
'        Dim TP1 As New TitlePage(s)
'        s.Seek(ptr + &H38, SeekOrigin.Begin)
'        Shrd.GoToRelativePointer(s, &H9F)
'        Dim TP2 As New TitlePage(s)
'        Return Shrd.FormatTitleString(TP1.ToString & " " & TP2.ToString)
'    End Function

'    Public Function GetLvlPtr(ByVal num As Integer, ByVal s As Stream) As Integer
'        If hacked Then
'            s.Seek(Ptr.LevelPointers + 2 + num * 4, SeekOrigin.Begin)
'            Return Shrd.ReadFileAddr(s)
'        Else
'            s.Seek(Ptr.LevelPointers + 2 + num * 2, SeekOrigin.Begin)
'            Return Shrd.ReadRelativeFileAddr(s, &H9F)
'        End If
'    End Function

'    Public Function GotoLvlPtr(ByVal num As Integer, ByVal s As Stream) As Integer
'        s.Seek(GetLvlPtr(num, s), SeekOrigin.Begin)
'    End Function

'    Public Function GetLevel(ByVal num As Integer, ByVal name As String) As Level
'        Dim s As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
'        GotoLvlPtr(num, s)
'        Return New Level(s, name, num)
'    End Function

'    Public Function GetAllLvlPtrs(ByVal s As Stream) As DList(Of Integer, Integer)
'        Dim ptrs As New DList(Of Integer, Integer)
'        For l As Integer = 0 To regLvlCount
'            ptrs.Add(l, GetLvlPtr(l, s))
'        Next
'        For Each num As Integer In bonusLvls
'            ptrs.Add(num, GetLvlPtr(num, s))
'        Next
'        Return ptrs
'    End Function

'    Public Sub SaveLevel(ByVal lvl As Level)
'        Dim fs As New FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read)
'        Dim ROMSize As Long = fs.Length
'        Dim data As LevelWriteData = lvl.GetWriteData()
'        'Dim fs2 As New FileStream(Application.StartupPath + "\lvl.bin", FileMode.Create)
'        'fs2.Write(data.data, 0, data.data.Length)
'        'fs2.Close()
'        Dim ptrs As DList(Of Integer, Integer) = GetAllLvlPtrs(fs)
'        ptrs.SortBySecond()
'        Dim lvlPtr As Integer = GetLvlPtr(lvl.num, fs)
'        fs.Seek(lvlPtr + 4, SeekOrigin.Begin)
'        Shrd.GoToPointer(fs)
'        Dim SNESAddr As Byte() = Shrd.ConvertAddr(fs.Position)
'        data.data(4) = SNESAddr(0)
'        data.data(5) = SNESAddr(1)
'        data.data(6) = SNESAddr(2)
'        For y As Integer = 0 To lvl.Height - 1
'            For x As Integer = 0 To lvl.Width - 1
'                fs.WriteByte(lvl.Tiles(x, y))
'                fs.WriteByte(0)
'            Next
'        Next
'        fs.Seek(lvlPtr, SeekOrigin.Begin)
'        For l As Integer = 0 To 5
'            SNESAddr = Shrd.ConvertAddr(data.addrOffsets(l) + lvlPtr)
'            data.data(offsetPos(l)) = SNESAddr(0)
'            data.data(offsetPos(l) + 1) = SNESAddr(1)
'        Next
'        Dim lenDiff As Integer = 0
'        If ptrs.L2.IndexOf(lvlPtr) < ptrs.L2.Count - 1 Then
'            lenDiff = data.data.Length - (ptrs.L2(ptrs.L2.IndexOf(lvlPtr) + 1) - lvlPtr)
'            Shrd.InsertBytes(fs, lenDiff)
'        End If
'        fs.Seek(lvlPtr, SeekOrigin.Begin)
'        fs.Write(data.data, 0, data.data.Length)
'        fs.Seek(lvlPtr + &H3C, SeekOrigin.Begin)
'        Dim tempptr As Integer
'        Dim bossIndex As Integer = 0
'        Do 'set pointers for special boss monsters
'            tempptr = Shrd.ReadFileAddr(fs)
'            If Ptr.SpBossMonsters.Contains(tempptr) Then
'                fs.Write(Shrd.ConvertAddr(data.bossDataPtr(bossIndex) + lvlPtr), 0, 4)
'                bossIndex += 1
'            ElseIf tempptr = -1 Then
'                Exit Do
'            Else
'                fs.Seek(4, SeekOrigin.Current)
'            End If
'        Loop
'        Dim donePtrs As New List(Of Integer)
'        For l As Integer = ptrs.L2.IndexOf(lvlPtr) + 1 To ptrs.L2.Count - 1 'update level pointers
'            fs.Seek(Ptr.LevelPointers + 2 + ptrs.L1(l) * 2, SeekOrigin.Begin)
'            Dim NewPtr As Integer = fs.ReadByte + fs.ReadByte * &H100 + lenDiff
'            fs.Seek(-2, SeekOrigin.Current)
'            fs.WriteByte(NewPtr Mod &H100)
'            fs.WriteByte(NewPtr \ &H100)
'            If Not donePtrs.Contains(NewPtr) Then
'                donePtrs.Add(NewPtr)
'                NewPtr += &HF0000
'                fs.Seek(NewPtr, SeekOrigin.Begin)
'                Dim NewPtr2 As Integer
'                For m As Integer = 0 To 5 'Update pointers within level files
'                    fs.Seek(NewPtr + offsetPos(m), SeekOrigin.Begin)
'                    NewPtr2 = fs.ReadByte + fs.ReadByte * &H100 + lenDiff
'                    If NewPtr2 > 0 And NewPtr2 > lenDiff Then
'                        fs.Seek(-2, SeekOrigin.Current)
'                        fs.WriteByte(NewPtr2 Mod &H100)
'                        fs.WriteByte(NewPtr2 \ &H100)
'                    End If
'                Next
'                fs.Seek(NewPtr + &H3C, SeekOrigin.Begin)
'                Do 'Update special boss monsters
'                    NewPtr2 = Shrd.ReadFileAddr(fs)
'                    If Ptr.SpBossMonsters.Contains(NewPtr2) Then
'                        NewPtr2 = Shrd.ReadFileAddr(fs) + lenDiff
'                        fs.Seek(-4, SeekOrigin.Current)
'                        fs.Write(Shrd.ConvertAddr(NewPtr2), 0, 4)
'                    ElseIf NewPtr2 = -1 Then
'                        Exit Do
'                    End If
'                Loop
'            End If
'        Next
'        fs.SetLength(ROMSize)
'        names(lvl.num) = GetLevelTitle(fs, lvlPtr)
'        OpenLevel.SetName(lvl.num, names(lvl.num))
'        fs.Close()
'    End Sub
'End Class
