<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmhlp
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grihlp = New System.Windows.Forms.DataGridView
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.txtCerca = New System.Windows.Forms.TextBox
        Me.lb_cerca = New System.Windows.Forms.Label
        Me.lbChiave = New System.Windows.Forms.Label
        Me.btnAggiorna = New System.Windows.Forms.Button
        CType(Me.grihlp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grihlp
        '
        Me.grihlp.AllowUserToAddRows = False
        Me.grihlp.AllowUserToDeleteRows = False
        Me.grihlp.AllowUserToOrderColumns = True
        Me.grihlp.AllowUserToResizeRows = False
        Me.grihlp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grihlp.Location = New System.Drawing.Point(8, 12)
        Me.grihlp.Name = "grihlp"
        Me.grihlp.RowHeadersVisible = False
        Me.grihlp.Size = New System.Drawing.Size(408, 256)
        Me.grihlp.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.64103!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.35897!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.txtCerca, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lb_cerca, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(2, 294)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(423, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'txtCerca
        '
        Me.txtCerca.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.txtCerca.Location = New System.Drawing.Point(103, 4)
        Me.txtCerca.Name = "txtCerca"
        Me.txtCerca.Size = New System.Drawing.Size(280, 20)
        Me.txtCerca.TabIndex = 1
        '
        'lb_cerca
        '
        Me.lb_cerca.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lb_cerca.AutoSize = True
        Me.lb_cerca.Location = New System.Drawing.Point(28, 8)
        Me.lb_cerca.Name = "lb_cerca"
        Me.lb_cerca.Size = New System.Drawing.Size(43, 13)
        Me.lb_cerca.TabIndex = 1
        Me.lb_cerca.Text = "Campo:"
        '
        'lbChiave
        '
        Me.lbChiave.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lbChiave.AutoSize = True
        Me.lbChiave.Location = New System.Drawing.Point(30, 316)
        Me.lbChiave.Name = "lbChiave"
        Me.lbChiave.Size = New System.Drawing.Size(0, 13)
        Me.lbChiave.TabIndex = 2
        Me.lbChiave.Visible = False
        '
        'btnAggiorna
        '
        Me.btnAggiorna.Location = New System.Drawing.Point(10, 270)
        Me.btnAggiorna.Name = "btnAggiorna"
        Me.btnAggiorna.Size = New System.Drawing.Size(62, 24)
        Me.btnAggiorna.TabIndex = 3
        Me.btnAggiorna.Text = "Aggiorna"
        Me.btnAggiorna.UseVisualStyleBackColor = True
        '
        'frmhlp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(425, 335)
        Me.Controls.Add(Me.btnAggiorna)
        Me.Controls.Add(Me.grihlp)
        Me.Controls.Add(Me.lbChiave)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmhlp"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmhlp"
        CType(Me.grihlp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grihlp As System.Windows.Forms.DataGridView
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txtCerca As System.Windows.Forms.TextBox
    Friend WithEvents lb_cerca As System.Windows.Forms.Label
    Friend WithEvents lbChiave As System.Windows.Forms.Label
    Friend WithEvents btnAggiorna As System.Windows.Forms.Button

End Class
