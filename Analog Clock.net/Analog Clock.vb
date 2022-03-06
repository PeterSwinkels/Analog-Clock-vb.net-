'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports System.DateTime
Imports System.Drawing
Imports System.Math
Imports System.Windows.Forms

'This control contains the procedures for drawing an analog clock.
Public Class AnalogClockControl
   Private Const CLOCK_LINE_WIDTH As Integer = 2            'Defines the width of the lines used to draw the clock.
   Private Const CLOCK_SIZE As Integer = 120                'Defines the clock face's diameter in pixels.
   Private Const HAND_NUT_SIZE As Integer = 3               'Defines the hands' nut size.
   Private Const HOURS_TO_DEGREES As Integer = 30           'Defines the value used to convert hours to degrees.
   Private Const LARGE_MARK_INTERVAL As Integer = 3         'Defines the interval between the large marks in hours.
   Private Const MINUTES_TO_DEGREES As Integer = 6          'Defines the value used to convert minutes to degrees.
   Private Const MINUTES_TO_FRACTION As Double = 1 / 60     'Defines the value used to convert minutes to the fractional part of an hour.
   Private Const SECONDS_TO_DEGREES As Integer = 6          'Defines the value used to convert seconds to degrees.
   Private Const TWELVE_HOUR_ANGLE As Integer = -90         'Defines the angle for noon/midnight in degrees.

   Private Const CLOCK_X As Integer = CInt(CLOCK_SIZE * 1.1)                   'Defines the clock face's horizontal center in pixels.
   Private Const CLOCK_Y As Integer = CInt(CLOCK_SIZE * 1.1)                   'Defines the clock face's vertical center in pixels.
   Private Const DEGREES_PER_RADIAN As Double = 180 / PI                       'Defines the number of degrees per radian.
   Private Const HOUR_HAND_LENGTH As Integer = CInt(CLOCK_SIZE / 1.6)          'Defines the hour hand's length.
   Private Const LARGE_MARK_LENGTH As Integer = CInt(HOUR_HAND_LENGTH / 2.5)   'Defines the size of the markings used to mark every third hour.
   Private Const MINUTE_HAND_LENGTH As Integer = CInt(HOUR_HAND_LENGTH * 1.5)  'Defines the minutes hand's length.
   Private Const SECOND_HAND_LENGTH As Integer = CInt(HOUR_HAND_LENGTH * 1.5)  'Defines the seconds hand's length.
   Private Const SMALL_MARK_LENGTH As Integer = CInt(LARGE_MARK_LENGTH / 2)    'Defines the size of the marking's used to mark the hours.

   'This structure defines the time displayed by the clock.
   Private Structure TimeStr
      Public Hour As Integer    'Contains the hour.
      Public Minute As Integer  'Contains the minute.
      Public Second As Integer  'Contains the second.
   End Structure

   Public Event HandleError(ExceptionO As Exception)                                          'Defines the error event.
   Private ControlToolTip As New ToolTip                                                       'Contains this control's tooltip.
   Private WithEvents AnalogClockTimer As New Timer With {.Enabled = True, .Interval = 1000}   'Contains the timer that powers the clock.

   'This procedure initializes this control.
   Public Sub New()
      Try
         InitializeComponent()
         DrawClock(CurrentTime(Advance:=False))
         ControlToolTip.SetToolTip(Me, "Click near the face's edge or use the plus key to set the time.")
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure handles the user's keystrokes.
   Private Sub AnalogClockControl_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
      Try
         Dim Time As TimeStr = CurrentTime()

         If e.KeyCode = Keys.Add Then
            With Time
               If e.Shift Then
                  If .Hour = 11 Then .Hour = 0 Else .Hour += 1
               Else
                  If .Minute = 59 Then
                     .Minute = 0
                     If .Hour = 11 Then .Hour = 0 Else .Hour += 1
                  Else
                     .Minute += +1
                  End If
               End If
               DrawClock(CurrentTime(, NewHour:= .Hour, NewMinute:= .Minute))
            End With
         End If
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure gives the command to change the time being displayed.
   Private Sub AnalogClockControl_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
      Try
         Select Case e.Button
            Case MouseButtons.Middle
               CurrentTime(, NewHour:=CInt((GetAngle(e.X, e.Y) - TWELVE_HOUR_ANGLE) / HOURS_TO_DEGREES), NewMinute:=0)
            Case MouseButtons.Left
               CurrentTime(, NewHour:=CInt((GetAngle(e.X, e.Y) - TWELVE_HOUR_ANGLE) / HOURS_TO_DEGREES))
            Case MouseButtons.Right
               CurrentTime(, , NewMinute:=CInt((GetAngle(e.X, e.Y) - TWELVE_HOUR_ANGLE) / MINUTES_TO_DEGREES))
         End Select

         DrawClock(CurrentTime(Advance:=False))
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure sets this control's size.
   Private Sub AnalogClockControl_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
      Try
         Me.Size = New Size(CInt(CLOCK_SIZE * 2.2), CInt(CLOCK_SIZE * 2.2))
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure gives the command to display an analog clock.
   Private Sub AnalogClockTimer_Tick(sender As Object, e As EventArgs) Handles AnalogClockTimer.Tick
      Try
         DrawClock(CurrentTime(Advance:=True))
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure manages the time displayed by the clock.
   Private Function CurrentTime(Optional Advance As Boolean = False, Optional NewHour As Integer? = Nothing, Optional NewMinute As Integer? = Nothing, Optional NewSecond As Integer? = Nothing) As TimeStr
      Try
         Static Time As New TimeStr With {.Hour = Now.Hour(), .Minute = Now.Minute(), .Second = Now.Second()}

         With Time
            If Advance Then
               If .Second = 59 Then
                  .Second = 0
                  If .Minute = 59 Then
                     .Minute = 0
                     .Hour = If(.Hour = 11, 0, .Hour + 1)
                  Else
                     .Minute += 1
                  End If
               Else
                  .Second += 1
               End If
            Else
               If NewHour IsNot Nothing Then .Hour = NewHour.Value
               If NewMinute IsNot Nothing Then .Minute = NewMinute.Value
               If NewSecond IsNot Nothing Then .Second = NewSecond.Value
            End If
         End With

         Return Time
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure draws an analog clock displaying the specified time.
   Private Sub DrawClock(DisplayedTime As TimeStr)
      Try
         Dim HourAsRadians As New Double
         Dim MarkLength As New Integer
         Dim MinuteAsRadians As New Double
         Dim SecondAsRadians As New Double

         Static HourHandX As New Integer
         Static HourHandY As New Integer
         Static MinuteHandX As New Integer
         Static MinuteHandY As New Integer
         Static SecondHandX As New Integer
         Static SecondHandY As New Integer

         With Me.CreateGraphics
            .DrawLine(New Pen(Me.BackColor, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, HourHandX, HourHandY)
            .DrawLine(New Pen(Me.BackColor, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, MinuteHandX, MinuteHandY)
            .DrawLine(New Pen(Me.BackColor, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, SecondHandX, SecondHandY)

            For HourOnFace As Integer = 0 To 11
               HourAsRadians = ((HourOnFace * HOURS_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN
               MarkLength = If(HourOnFace Mod LARGE_MARK_INTERVAL = 0, LARGE_MARK_LENGTH, SMALL_MARK_LENGTH)
               .DrawLine(New Pen(Color.Yellow, CLOCK_LINE_WIDTH), CInt((Cos(HourAsRadians) * CLOCK_SIZE) + CLOCK_X), CInt((Sin(HourAsRadians) * CLOCK_SIZE) + CLOCK_Y), CInt((Cos(HourAsRadians) * (CLOCK_SIZE - MarkLength)) + CLOCK_X), CInt((Sin(HourAsRadians) * (CLOCK_SIZE - MarkLength)) + CLOCK_Y))
            Next HourOnFace
            .DrawEllipse(New Pen(Color.Blue, CLOCK_LINE_WIDTH), 12, 12, CLOCK_SIZE * 2, CLOCK_SIZE * 2)

            With DisplayedTime
               HourAsRadians = (((.Hour + .Minute * MINUTES_TO_FRACTION) * HOURS_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN
               SecondAsRadians = ((.Second * SECONDS_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN
               MinuteAsRadians = ((.Minute * MINUTES_TO_DEGREES) + TWELVE_HOUR_ANGLE) / DEGREES_PER_RADIAN
            End With

            HourHandX = CInt((Cos(HourAsRadians) * HOUR_HAND_LENGTH) + CLOCK_X)
            HourHandY = CInt((Sin(HourAsRadians) * HOUR_HAND_LENGTH) + CLOCK_Y)
            MinuteHandX = CInt((Cos(MinuteAsRadians) * MINUTE_HAND_LENGTH) + CLOCK_X)
            MinuteHandY = CInt((Sin(MinuteAsRadians) * MINUTE_HAND_LENGTH) + CLOCK_Y)
            SecondHandX = CInt((Cos(SecondAsRadians) * SECOND_HAND_LENGTH) + CLOCK_X)
            SecondHandY = CInt((Sin(SecondAsRadians) * SECOND_HAND_LENGTH) + CLOCK_Y)

            .DrawLine(New Pen(Color.Green, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, HourHandX, HourHandY)
            .DrawLine(New Pen(Color.Green, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, MinuteHandX, MinuteHandY)
            .DrawLine(New Pen(Color.Red, CLOCK_LINE_WIDTH), CLOCK_X, CLOCK_Y, SecondHandX, SecondHandY)
            .DrawEllipse(Pens.White, CInt(CLOCK_X - (HAND_NUT_SIZE / 2)), CInt(CLOCK_Y - (HAND_NUT_SIZE / 2)), HAND_NUT_SIZE, HAND_NUT_SIZE)
         End With
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure converts the specified hand tip position to an angle relative to the clock's center.
   Private Function GetAngle(HandTipX As Integer, HandTipY As Integer) As Integer
      Try
         Dim HandTip As Point = New Point(HandTipX - CLOCK_X, HandTipY - CLOCK_Y)
         Dim Hypotenuse As Double = Sqrt((HandTip.X ^ 2) + (HandTip.Y ^ 2))
         Dim Cosine As Double = Min(HandTip.X, Hypotenuse) / Max(HandTip.X, Hypotenuse)
         Dim Sine As Double = Min(HandTip.Y, Hypotenuse) / Max(HandTip.Y, Hypotenuse)
         Dim Angle As Double = Asin(Sine)

         If (Sine <= 0 AndAlso Cosine <= 0) OrElse (Sine >= 0 AndAlso Cosine <= 0) Then
            Angle = PI - Angle
         ElseIf Sine <= 0 AndAlso Cosine >= 0 Then
            Angle = (PI * 2) + Angle
         ElseIf Sine >= 0 AndAlso Cosine >= 0 Then
            Angle = Angle
         Else
            Angle = 0
         End If

         Return CInt(Angle * (180 / PI))
      Catch ExceptionO As Exception
         RaiseEvent HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function
End Class
