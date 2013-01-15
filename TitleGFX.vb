'Imports System.Drawing
'Imports System.Drawing.Imaging

'Public Class TitleGFX

'    Public LetterImgs(&H5F) As Bitmap
'    Public plt(&H80) As Color
'    Private widths As Integer() = {3, 2, 6}

'    Public Sub New(ByVal fs As IO.FileStream)
'        fs.Seek(Ptr.TitlePalette, IO.SeekOrigin.Begin)
'        plt = Shrd.ReadPalette(fs, &H80, False)
'        fs.Seek(Ptr.TitleGraphics, IO.SeekOrigin.Begin)
'        Dim GFX As Byte() = Tileset.DecompressMap16(fs)
'        Dim LinGFX(511)(,) As Byte
'        For l As Integer = 0 To 511
'            LinGFX(l) = Shrd.PlanarToLinear(GFX, l * &H20)
'        Next
'        Dim tilePos(&H5F) As Point
'        fs.Seek(Ptr.TitleCharWidth, IO.SeekOrigin.Begin)
'        For l As Integer = 0 To &H5F
'            tilePos(l) = New Point(fs.ReadByte, fs.ReadByte)
'            If tilePos(l).X = 255 Then
'                tilePos(l) = Point.Empty
'            End If
'        Next
'        Dim curImg As Bitmap
'        For l As Integer = 0 To &H5F
'            Dim index As Integer = Ptr.TitleTileMap + tilePos(l).Y * &H600
'            Dim width As Integer
'            If tilePos(l).Y < 3 Then
'                width = widths(tilePos(l).Y)
'            End If
'            curImg = New Bitmap(width * 8, 48, PixelFormat.Format8bppIndexed)
'            Dim imgData As BitmapData = curImg.LockBits(New Rectangle(Point.Empty, curImg.Size), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed)
'            index += width * tilePos(l).X * 2
'            fs.Seek(index, IO.SeekOrigin.Begin)
'            For y As Integer = 0 To 5
'                For x As Integer = 0 To width - 1
'                    Dim t As Byte = fs.ReadByte
'                    Shrd.DrawTile(imgData, x * 8, y * 8, fs.ReadByte, t, LinGFX)
'                Next
'                fs.Seek(&H100 - width * 2, IO.SeekOrigin.Current)
'            Next
'            curImg.UnlockBits(imgData)
'            LetterImgs(l) = curImg
'        Next
'    End Sub
'End Class
