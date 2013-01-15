'Imports System.Runtime.InteropServices
'Imports System.Drawing.Imaging


'Public Class Shrd

'    Public Shared PxFont As New Fonts("Small Fonts", 9)

'    Public Shared Function ReadFileAddr(ByVal s As IO.Stream) As Integer
'        Dim part2 As Integer = s.ReadByte() + s.ReadByte() * &H100
'        Dim Banknum As Integer = s.ReadByte()
'        s.ReadByte()
'        If Banknum < &H80 Or part2 < &H8000 Then
'            Return -1
'        End If
'        Return (Banknum - &H80) * &H8000 + part2 - &H8000
'    End Function

'    Public Shared Sub GoToPointer(ByVal s As IO.Stream)
'        Dim addr As Integer = ReadFileAddr(s)
'        If addr = -1 Then
'            ErrorLog.Report()
'        Else
'            s.Seek(addr, IO.SeekOrigin.Begin)
'        End If
'    End Sub

'    Public Shared Function GetFileAddr(ByVal arr As Byte(), ByVal idx As Integer) As Integer
'        Dim part2 As Integer = arr(idx) + arr(idx + 1) * &H100
'        Dim Banknum As Integer = arr(idx + 2)
'        If Banknum < &H80 Or part2 < &H8000 Then
'            Return -1
'        End If
'        Return (Banknum - &H80) * &H8000 + part2 - &H8000
'    End Function

'    Public Shared Function ReadRelativeFileAddr(ByVal s As IO.Stream, ByVal bank As Byte) As Integer
'        Dim part2 As Integer = s.ReadByte + s.ReadByte * &H100
'        If bank < &H80 Or part2 < &H8000 Then
'            Return -1
'        End If
'        Return (bank - &H80) * &H8000 + part2 - &H8000
'    End Function

'    Public Shared Sub GoToRelativePointer(ByVal s As IO.Stream, ByVal bank As Byte)
'        Dim addr As Integer = ReadRelativeFileAddr(s, bank)
'        If addr = -1 Then
'            ErrorLog.Report()
'        Else
'            s.Seek(addr, IO.SeekOrigin.Begin)
'        End If
'    End Sub

'    Public Shared Function ConvertAddr(ByVal address As Integer) As Byte()
'        If address = 0 Or address = -1 Then
'            Return New Byte() {0, 0, 0, 0}
'        End If
'        Dim bank As Integer = address \ &H8000
'        Dim part2 As Integer = address - bank * &H8000 + &H8000
'        Return New Byte() {part2 Mod &H100, part2 \ &H100, bank + &H80, 0}
'    End Function

'    Public Shared Function RGBtoSNESLo(ByVal RGB As Color) As Byte
'        Return (RGB.B \ 8 * &H400 + RGB.G \ 8 * &H20 + RGB.R \ 8) Mod &H100
'    End Function

'    Public Shared Function RGBtoSNESHi(ByVal RGB As Color) As Byte
'        Return (RGB.B \ 8 * &H400 + RGB.G \ 8 * 32 + RGB.R \ 8) \ &H100
'    End Function

'    Public Shared Function SNEStoRGB(ByVal LoByte As Byte, ByVal HiByte As Byte) As Color
'        Dim v As Integer = LoByte + &H100 * HiByte
'        Return Color.FromArgb((v Mod &H20) * 8, ((v \ &H20) Mod &H20) * 8, ((v \ &H400) Mod &H20) * 8)
'    End Function

'    Public Shared Function ReadPalette(ByVal s As IO.Stream, ByVal colorCount As Integer, ByVal transparant As Boolean) As Color()
'        Dim plt(colorCount - 1) As Color
'        For l As Integer = 0 To colorCount - 1
'            plt(l) = SNEStoRGB(s.ReadByte, s.ReadByte)
'            If transparant AndAlso l Mod 16 = 0 Then
'                plt(l) = Color.FromArgb(0, plt(l))
'            End If
'        Next
'        Return plt
'    End Function

'    Public Shared Function PlanarToLinear(ByVal bytes As Byte(), ByVal index As Integer) As Byte(,)
'        Dim result(7, 7) As Byte
'        Dim line As Integer = 0
'        Dim bit As Integer = 0
'        For l As Integer = index To index + &H1F Step 2
'            For m As Integer = 0 To 7
'                If (bytes(l) And (1 << m)) <> 0 Then
'                    result(line, 7 - m) = result(line, 7 - m) Or (1 << bit)
'                End If
'                If (bytes(l + 1) And (1 << m)) <> 0 Then
'                    result(line, 7 - m) = result(line, 7 - m) Or (1 << bit + 1)
'                End If
'            Next
'            line += 1
'            If line = 8 Then
'                line = 0
'                bit += 2
'            End If
'        Next
'        Return result
'    End Function

'    Public Shared Sub DrawTile(ByVal bmp As Bitmap, ByVal x As Integer, ByVal y As Integer, ByVal gfx As Byte(), ByVal gfxindex As Integer, ByVal palette As Color(), ByVal palIndex As Integer, ByVal xFlip As Boolean, ByVal yFlip As Boolean)
'        Dim tile As Byte(,) = PlanarToLinear(gfx, gfxindex)
'        Dim xStep As Integer = 1, yStep As Integer = 1
'        If xFlip Then
'            x += 7
'            xStep = -1
'        End If
'        Dim xOrig As Integer = x
'        If yFlip Then
'            y += 7
'            yStep = -1
'        End If
'        For l As Integer = 0 To 7
'            For m As Integer = 0 To 7
'                If palette(palIndex + tile(l, m)).A > 0 Then
'                    bmp.SetPixel(x, y, palette(palIndex + tile(l, m)))
'                End If
'                x += xStep
'            Next
'            y += yStep
'            x = xOrig
'        Next
'    End Sub

'    Public Shared Sub DrawTile(ByVal bmp As Bitmap, ByVal x As Integer, ByVal y As Integer, ByVal s As IO.Stream, ByVal palette As Color(), ByVal palIndex As Integer, ByVal xFlip As Boolean, ByVal yFlip As Boolean)
'        Dim gfx(31) As Byte
'        s.Read(gfx, 0, 32)
'        DrawTile(bmp, x, y, gfx, 0, palette, palIndex, xFlip, yFlip)
'    End Sub

'    Public Shared Sub DrawTile(ByVal bmp As BitmapData, ByVal x As Integer, ByVal y As Integer, ByVal data As Byte, ByVal tileIndex As Integer, ByVal tiles As Byte()(,))
'        Dim palIndex As Byte = &H10 * ((data \ 4) And 7)
'        If (data And 1) = 1 Then tileIndex += &H100
'        Dim xFlip As Boolean = (data And &H40) > 1
'        Dim yFlip As Boolean = (data And &H80) > 1
'        Dim xStep As Integer = 1, yStep As Integer = 1
'        If xFlip Then
'            x += 7
'            xStep = -1
'        End If
'        Dim xOrig As Integer = x
'        If yFlip Then
'            y += 7
'            yStep = -1
'        End If
'        For l As Integer = 0 To 7
'            For m As Integer = 0 To 7
'                Marshal.WriteByte(bmp.Scan0, y * bmp.Stride + x, palIndex + tiles(tileIndex)(l, m))
'                x += xStep
'            Next
'            y += yStep
'            x = xOrig
'        Next
'    End Sub

'    Public Shared Sub FillPalette(ByVal bmp As Bitmap, ByVal colors As Color())
'        Dim pal As ColorPalette = bmp.Palette
'        For l As Integer = 0 To colors.Length - 1
'            pal.Entries(l) = colors(l)
'        Next
'        bmp.Palette = pal
'    End Sub

'    Public Shared Sub MakePltTransparent(ByVal bmp As Bitmap)
'        Dim pal As ColorPalette = bmp.Palette
'        For l As Integer = 0 To pal.Entries.Length - 1 Step 16
'            pal.Entries(l) = Color.Transparent
'        Next
'        bmp.Palette = pal
'    End Sub

'    Public Shared Function RealSize(ByVal rect As Rectangle) As Rectangle
'        Return New Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
'    End Function

'    Public Shared Function HexL(ByVal n As Integer, ByVal length As Integer) As String
'        Dim str As String = Hex(n)
'        If str.Length < length Then
'            Return StrDup(length - str.Length, "0") & str
'        Else
'            Return str
'        End If
'    End Function

'    Public Shared Sub InsertBytes(ByVal s As IO.Stream, ByVal byteCount As Integer)
'        If byteCount < 0 Then
'            s.Seek(-byteCount, IO.SeekOrigin.Current)
'        End If
'        Dim rest(s.Length - s.Position - 1) As Byte
'        Dim start As Long = s.Position
'        s.Read(rest, 0, rest.Length)
'        s.Seek(start + byteCount, IO.SeekOrigin.Begin)
'        s.Write(rest, 0, rest.Length)
'    End Sub

'    Public Shared Function ToText(ByVal data As Byte()) As String
'        Dim str As String = ""
'        For l As Integer = 0 To data.Length - 1
'            str &= HexL(data(l), 2)
'        Next
'        Return str
'    End Function

'    Public Shared Function FromText(ByVal str As String) As Byte()
'        Dim data(str.Length \ 2 - 1) As Byte
'        For l As Integer = 0 To data.Length - 1
'            data(l) = CByte("&H" & Mid(str, l * 2 + 1, 2))
'        Next
'        Return data
'    End Function

'    Private Shared nameLCase As String() = {"The", "A", "An", "And", "But", "As", "At", "By", "For", "From", "In", "Into", "Of", "Off", "On", "Onto", "Over", "Past", "To", "Upon", "With", "Vs"}

'    Public Shared Function PropperCase(ByVal str As String) As String
'        Dim pos As Integer = 1
'        Dim len As Integer
'        Dim rstr As String = UCase(str(0)) & LCase(Mid(str, 2))
'        Do While InStr(pos, rstr, " ") > 0
'            pos = InStr(pos, rstr, " ")
'            rstr = Mid(rstr, 1, pos) & UCase(rstr(pos)) & Mid(rstr, pos + 2)
'            pos += 1
'        Loop
'        For l As Integer = 0 To nameLCase.Length - 1
'            len = nameLCase(l).Length
'            pos = 1
'            Do While InStr(pos, rstr, nameLCase(l))
'                pos = InStr(pos, rstr, nameLCase(l))
'                If (pos >= 3 AndAlso rstr(pos - 3) <> ":") And (pos = rstr.Length - len + 1 OrElse rstr(pos + len - 1) = " ") Then
'                    rstr = Mid(rstr, 1, pos - 1) & LCase(nameLCase(l)) & Mid(rstr, pos + len)
'                End If
'                pos += 1
'            Loop
'        Next
'        Return rstr
'    End Function

'    Public Shared Function ReplaceFirst(ByVal str As String, ByVal findStr As String, ByVal replaceStr As String) As String
'        Dim pos As Integer = InStr(str, findStr)
'        If pos > 0 Then
'            Return Mid(str, 1, pos - 1) & replaceStr & Mid(str, pos + findStr.Length)
'        End If
'        Return str
'    End Function

'    Public Shared Function InStrN(ByVal str As String, ByVal find As String, ByVal n As Integer) As Integer
'        Dim p As Integer = 1
'        Do While InStr(p, str, find) > 0 And n > 0
'            p = InStr(p, str, find) + 1
'            n -= 1
'        Loop
'        Return p - 1
'    End Function

'    Public Shared Function ArrayToString(ByVal a As Array) As String
'        Dim result As String = "{"
'        For l As Integer = 0 To a.Length - 1
'            result &= a.GetValue(l).ToString & ","
'        Next
'        Return Mid(result, 1, result.Length - 1) & "}"
'    End Function

'    Public Shared Function FormatCopyStr(ByVal str As String) As String
'        Return "ZAMNClip|" & str & "|EndClip"
'    End Function

'    Public Shared Function IsZAMNClip(ByVal str As String) As Boolean
'        str = Trim(str)
'        Return str.StartsWith("ZAMNClip|") And (InStr(10, str, "|") > 0)
'    End Function

'    Public Shared Function UnFormatCopyStr(ByVal str As String) As String
'        str = Trim(str)
'        Return Mid(str, 10, InStr(10, str, "|") - 10)
'    End Function

'    Public Shared Function GetBytes(ByVal value As Integer) As Byte()
'        Dim result As Byte() = BitConverter.GetBytes(value)
'        If result(3) = 0 Then
'            ReDim Preserve result(2)
'        End If
'        If result(2) = 0 Then
'            ReDim Preserve result(1)
'        End If
'        If result(1) = 0 Then
'            ReDim Preserve result(0)
'        End If
'        Return result
'    End Function

'    Public Shared Function FormatTitleString(ByVal str As String) As String
'        str = str.Replace("LEVEI", "LEVEL")
'        If str.Contains("CREDIT") Then
'            str = Mid(str, InStrRev(str, "CREDIT"))
'            str = Shrd.ReplaceFirst(str, "LEVEL", "LEVEL:")
'        ElseIf str.Contains("BONUS") Then
'            str = Mid(str, InStrRev(str, "BONUS"))
'            str = Shrd.ReplaceFirst(str, "LEVEL", "LEVEL:")
'        ElseIf str.Contains("LEVEL") Then
'            str = Mid(str, InStrRev(str, "LEVEL"))
'            Dim p As Integer = InStr(InStr(str, " ") + 1, str, " ") - 1
'            str = Mid(str, 1, p) & ":" & Mid(str, p + 1)
'        End If
'        str = Shrd.PropperCase(str.Replace("  ", " "))
'        Return str
'    End Function

'    Public Shared Function GetRoundRect(ByVal Rect As Rectangle, ByVal CurveRadius As Integer) As Drawing2D.GraphicsPath
'        Dim gp As New Drawing2D.GraphicsPath
'        Dim pt2x As Integer = Rect.X + Rect.Width - 1
'        Dim pt2y As Integer = Rect.Y + Rect.Height - 1
'        CurveRadius *= 2

'        gp.AddArc(pt2x - CurveRadius, Rect.Y, CurveRadius, CurveRadius, 270, 90)
'        gp.AddArc(pt2x - CurveRadius, pt2y - CurveRadius, CurveRadius, CurveRadius, 0, 90)
'        gp.AddArc(Rect.X, pt2y - CurveRadius, CurveRadius, CurveRadius, 90, 90)
'        gp.AddArc(Rect.X, Rect.Y, CurveRadius, CurveRadius, 180, 90)
'        gp.CloseFigure()
'        Return gp
'    End Function

'    Public Shared Function GetRoundRectTop(ByVal Rect As Rectangle, ByVal CurveRadius As Integer) As Drawing2D.GraphicsPath
'        Dim gp As New Drawing2D.GraphicsPath
'        Dim pt2x As Integer = Rect.X + Rect.Width - 1
'        Dim pt2y As Integer = Rect.Y + Rect.Height - 1
'        CurveRadius *= 2

'        gp.AddArc(Rect.X, Rect.Y, CurveRadius, CurveRadius, 180, 90)
'        gp.AddArc(pt2x - CurveRadius, Rect.Y, CurveRadius, CurveRadius, 270, 90)
'        gp.AddLine(pt2x, pt2y, Rect.X, pt2y)
'        gp.CloseFigure()
'        Return gp
'    End Function

'    Public Shared Function CreateList(Of T)(ByVal item As T) As List(Of T)
'        Dim l As New List(Of T)
'        l.Add(item)
'        Return l
'    End Function

'    Public Shared Sub DrawWithPlt(ByVal g As Graphics, ByVal x As Integer, ByVal y As Integer, ByVal bmp As Bitmap, ByVal plt As Color(), ByVal colorIdx As Integer, ByVal colorCount As Integer)
'        Dim pal As ColorPalette = bmp.Palette
'        For l As Integer = 0 To colorCount - 1
'            pal.Entries(l) = plt((l + colorIdx) Mod plt.Length)
'        Next
'        bmp.Palette = pal
'        g.DrawImage(bmp, x, y)
'    End Sub

'    Public Shared Function HasHeader(ByVal s As IO.Stream) As Boolean
'        Dim extraBytes As Integer = s.Length Mod &H8000L
'        Return extraBytes = &H200
'    End Function

'    Public Shared Sub RemoveHeader(ByVal s As IO.Stream)
'        Dim extraBytes As Integer = s.Length Mod &H8000L
'        s.Seek(0, IO.SeekOrigin.Begin)
'        Shrd.InsertBytes(s, -extraBytes)
'        s.SetLength(s.Length - extraBytes)
'    End Sub
'End Class
