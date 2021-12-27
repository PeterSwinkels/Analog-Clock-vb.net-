<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InterfaceWindow
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InterfaceWindow))
      Me.MenuBar = New System.Windows.Forms.MenuStrip()
      Me.ProgramMainMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.InformationMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.QuitMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.MenuBar.SuspendLayout()
      Me.SuspendLayout()
      '
      'MenuBar
      '
      Me.MenuBar.ImageScalingSize = New System.Drawing.Size(20, 20)
      Me.MenuBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgramMainMenu})
      Me.MenuBar.Location = New System.Drawing.Point(0, 0)
      Me.MenuBar.Name = "MenuBar"
      Me.MenuBar.Size = New System.Drawing.Size(282, 28)
      Me.MenuBar.TabIndex = 0
      Me.MenuBar.Text = "MenuBar"
      '
      'ProgramMainMenu
      '
      Me.ProgramMainMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InformationMenu, Me.QuitMenu})
      Me.ProgramMainMenu.Name = "ProgramMainMenu"
      Me.ProgramMainMenu.Size = New System.Drawing.Size(78, 24)
      Me.ProgramMainMenu.Text = "&Program"
      '
      'InformationMenu
      '
      Me.InformationMenu.Name = "InformationMenu"
      Me.InformationMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
      Me.InformationMenu.Size = New System.Drawing.Size(208, 26)
      Me.InformationMenu.Text = "&Information"
      '
      'QuitMenu
      '
      Me.QuitMenu.Name = "QuitMenu"
      Me.QuitMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
      Me.QuitMenu.Size = New System.Drawing.Size(208, 26)
      Me.QuitMenu.Text = "&Quit"
      '
      'InterfaceWindow
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(282, 255)
      Me.Controls.Add(Me.MenuBar)
      Me.DoubleBuffered = True
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
      Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
      Me.KeyPreview = True
      Me.MainMenuStrip = Me.MenuBar
      Me.MaximizeBox = False
      Me.Name = "InterfaceWindow"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.MenuBar.ResumeLayout(False)
      Me.MenuBar.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub

   Friend WithEvents MenuBar As System.Windows.Forms.MenuStrip
   Friend WithEvents ProgramMainMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents InformationMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents QuitMenu As System.Windows.Forms.ToolStripMenuItem
End Class
