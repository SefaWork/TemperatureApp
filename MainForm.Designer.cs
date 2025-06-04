namespace TemperatureApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            SelectFolderButton = new Button();
            FolderLocation = new Label();
            ControlGroupBox = new GroupBox();
            CalculateButton = new Button();
            GlobalListBox = new CheckedListBox();
            ControlListBox = new CheckedListBox();
            ResultsMessage = new RichTextBox();
            ControlGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // SelectFolderButton
            // 
            SelectFolderButton.Location = new Point(12, 12);
            SelectFolderButton.Name = "SelectFolderButton";
            SelectFolderButton.Size = new Size(125, 29);
            SelectFolderButton.TabIndex = 0;
            SelectFolderButton.Text = "Select Folder";
            SelectFolderButton.UseVisualStyleBackColor = true;
            SelectFolderButton.Click += SelectFolderButton_Click;
            // 
            // FolderLocation
            // 
            FolderLocation.BackColor = Color.White;
            FolderLocation.Font = new Font("Ubuntu Mono", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FolderLocation.ForeColor = Color.FromArgb(125, 125, 125);
            FolderLocation.Location = new Point(143, 9);
            FolderLocation.Name = "FolderLocation";
            FolderLocation.Size = new Size(645, 32);
            FolderLocation.TabIndex = 1;
            FolderLocation.Text = "Not selected.";
            FolderLocation.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ControlGroupBox
            // 
            ControlGroupBox.Controls.Add(CalculateButton);
            ControlGroupBox.Controls.Add(GlobalListBox);
            ControlGroupBox.Controls.Add(ControlListBox);
            ControlGroupBox.Location = new Point(12, 47);
            ControlGroupBox.Name = "ControlGroupBox";
            ControlGroupBox.Size = new Size(776, 207);
            ControlGroupBox.TabIndex = 2;
            ControlGroupBox.TabStop = false;
            ControlGroupBox.Text = "Controls";
            // 
            // CalculateButton
            // 
            CalculateButton.Location = new Point(32, 170);
            CalculateButton.Name = "CalculateButton";
            CalculateButton.Size = new Size(125, 29);
            CalculateButton.TabIndex = 3;
            CalculateButton.Text = "Calculate";
            CalculateButton.UseVisualStyleBackColor = true;
            CalculateButton.Click += CalculateButton_Click;
            // 
            // GlobalListBox
            // 
            GlobalListBox.BackColor = SystemColors.Control;
            GlobalListBox.BorderStyle = BorderStyle.None;
            GlobalListBox.CheckOnClick = true;
            GlobalListBox.ColumnWidth = 300;
            GlobalListBox.Font = new Font("Ubuntu Mono", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GlobalListBox.FormattingEnabled = true;
            GlobalListBox.Location = new Point(454, 26);
            GlobalListBox.Name = "GlobalListBox";
            GlobalListBox.Size = new Size(266, 138);
            GlobalListBox.TabIndex = 1;
            GlobalListBox.ThreeDCheckBoxes = true;
            // 
            // ControlListBox
            // 
            ControlListBox.BackColor = SystemColors.Control;
            ControlListBox.BorderStyle = BorderStyle.None;
            ControlListBox.CheckOnClick = true;
            ControlListBox.ColumnWidth = 300;
            ControlListBox.Font = new Font("Ubuntu Mono", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ControlListBox.FormattingEnabled = true;
            ControlListBox.Location = new Point(56, 26);
            ControlListBox.Name = "ControlListBox";
            ControlListBox.Size = new Size(266, 138);
            ControlListBox.TabIndex = 0;
            ControlListBox.ThreeDCheckBoxes = true;
            // 
            // ResultsMessage
            // 
            ResultsMessage.BackColor = Color.White;
            ResultsMessage.BorderStyle = BorderStyle.FixedSingle;
            ResultsMessage.Font = new Font("Ubuntu Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResultsMessage.Location = new Point(12, 260);
            ResultsMessage.Multiline = false;
            ResultsMessage.Name = "ResultsMessage";
            ResultsMessage.ReadOnly = true;
            ResultsMessage.ScrollBars = RichTextBoxScrollBars.Horizontal;
            ResultsMessage.Size = new Size(776, 33);
            ResultsMessage.TabIndex = 3;
            ResultsMessage.Text = "Results will be displayed here.";
            ResultsMessage.WordWrap = false;
            ResultsMessage.LinkClicked += ResultsMessage_LinkClicked;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 305);
            Controls.Add(ResultsMessage);
            Controls.Add(ControlGroupBox);
            Controls.Add(FolderLocation);
            Controls.Add(SelectFolderButton);
            Name = "MainForm";
            Text = "Temperature App";
            ControlGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button SelectFolderButton;
        private Label FolderLocation;
        private GroupBox ControlGroupBox;
        private CheckedListBox ControlListBox;
        private CheckedListBox GlobalListBox;
        private Button CalculateButton;
        private RichTextBox ResultsMessage;
    }
}
