'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports System.Drawing
Imports System.Environment
Imports System.Windows.Forms

'This module contains this program's main interface window.
Public Class InterfaceWindow

   Private WithEvents AnalogClock As New AnalogClockControl  'Contains a reference to an instance of the analog clock control.

   'This procedure initializes this windows.
   Public Sub New()
      Try
         InitializeComponent()

         With My.Application.Info
            Me.Text = $"{ .Title} v{ .Version} - by: { .CompanyName}"
         End With

         Me.Controls.Add(AnalogClock)
         AnalogClock.Top = MenuBar.Height

         Me.ClientSize = New Size(AnalogClock.Width, AnalogClock.Height + Me.MenuBar.Height)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure handles any errors that occur.
   Private Sub HandleError(ExceptionO As Exception) Handles AnalogClock.HandleError
      Try
         MessageBox.Show(ExceptionO.Message, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
      Catch
         [Exit](0)
      End Try
   End Sub

   'This procedure displays information about this program.
   Private Sub InformationMenu_Click(sender As Object, e As EventArgs) Handles InformationMenu.Click
      Try
         MessageBox.Show(My.Application.Info.Description, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure closes this window.
   Private Sub QuitMenu_Click(sender As Object, e As EventArgs) Handles QuitMenu.Click
      Try
         Me.Close()
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub
End Class
