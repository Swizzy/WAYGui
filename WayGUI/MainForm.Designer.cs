namespace WayGUI
{
    using System;
    using WayGUI.CustomControls;

    internal sealed partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.operationsbox = new System.Windows.Forms.GroupBox();
            this.releasebtn = new System.Windows.Forms.Button();
            this.initbtn = new System.Windows.Forms.Button();
            this.erasenorbtn = new System.Windows.Forms.Button();
            this.write = new System.Windows.Forms.Button();
            this.readbtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchprotectmode = new System.Windows.Forms.ToolStripMenuItem();
            this.switchEraseMode = new System.Windows.Forms.ToolStripMenuItem();
            this.selectPythonPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNORwaypyNANDWaypyPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verifynor = new System.Windows.Forms.ToolStripMenuItem();
            this.NANDInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.KillPython = new System.Windows.Forms.ToolStripMenuItem();
            this.clearlog = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.hclink = new System.Windows.Forms.ToolStripStatusLabel();
            this.colinklabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.outgroupbox = new System.Windows.Forms.GroupBox();
            this.logstrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsbox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.diffwrite = new System.Windows.Forms.CheckBox();
            this.wordwriteubm = new System.Windows.Forms.CheckBox();
            this.wordwrite = new System.Windows.Forms.CheckBox();
            this.writeverify = new System.Windows.Forms.CheckBox();
            this.memory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comports = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.noradressbox = new System.Windows.Forms.NumericUpDown();
            this.lengthbox = new System.Windows.Forms.NumericUpDown();
            this.offsetbox = new System.Windows.Forms.NumericUpDown();
            this.dumpcount = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.OutputBox = new WayGUI.CustomControls.ThreadSafeRichTextBox();
            this.loadingCircle1 = new LoadingCircle();
            this.operationsbox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.outgroupbox.SuspendLayout();
            this.logstrip.SuspendLayout();
            this.settingsbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.noradressbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dumpcount)).BeginInit();
            this.SuspendLayout();
            // 
            // operationsbox
            // 
            this.operationsbox.Controls.Add(this.loadingCircle1);
            this.operationsbox.Controls.Add(this.releasebtn);
            this.operationsbox.Controls.Add(this.initbtn);
            this.operationsbox.Controls.Add(this.erasenorbtn);
            this.operationsbox.Controls.Add(this.write);
            this.operationsbox.Controls.Add(this.readbtn);
            this.operationsbox.Location = new System.Drawing.Point(12, 129);
            this.operationsbox.Name = "operationsbox";
            this.operationsbox.Size = new System.Drawing.Size(543, 72);
            this.operationsbox.TabIndex = 0;
            this.operationsbox.TabStop = false;
            this.operationsbox.Text = "Operations";
            // 
            // releasebtn
            // 
            this.releasebtn.Enabled = false;
            this.releasebtn.Location = new System.Drawing.Point(350, 19);
            this.releasebtn.Name = "releasebtn";
            this.releasebtn.Size = new System.Drawing.Size(101, 47);
            this.releasebtn.TabIndex = 3;
            this.releasebtn.Text = "Release/Reset";
            this.releasebtn.UseVisualStyleBackColor = true;
            this.releasebtn.Click += new System.EventHandler(this.ReleasebtnClick);
            // 
            // initbtn
            // 
            this.initbtn.Location = new System.Drawing.Point(6, 19);
            this.initbtn.Name = "initbtn";
            this.initbtn.Size = new System.Drawing.Size(75, 47);
            this.initbtn.TabIndex = 2;
            this.initbtn.Text = "Initalize";
            this.initbtn.UseVisualStyleBackColor = true;
            this.initbtn.Click += new System.EventHandler(this.InitBtnClick);
            // 
            // erasenorbtn
            // 
            this.erasenorbtn.Enabled = false;
            this.erasenorbtn.Location = new System.Drawing.Point(269, 19);
            this.erasenorbtn.Name = "erasenorbtn";
            this.erasenorbtn.Size = new System.Drawing.Size(75, 47);
            this.erasenorbtn.TabIndex = 2;
            this.erasenorbtn.Text = "Erase NOR";
            this.erasenorbtn.UseVisualStyleBackColor = true;
            this.erasenorbtn.Click += new System.EventHandler(this.ErasenorbtnClick);
            // 
            // write
            // 
            this.write.Enabled = false;
            this.write.Location = new System.Drawing.Point(178, 19);
            this.write.Name = "write";
            this.write.Size = new System.Drawing.Size(85, 47);
            this.write.TabIndex = 1;
            this.write.Text = "Write/Flash";
            this.write.UseVisualStyleBackColor = true;
            this.write.Click += new System.EventHandler(this.WriteClick);
            // 
            // readbtn
            // 
            this.readbtn.Enabled = false;
            this.readbtn.Location = new System.Drawing.Point(87, 19);
            this.readbtn.Name = "readbtn";
            this.readbtn.Size = new System.Drawing.Size(85, 47);
            this.readbtn.TabIndex = 0;
            this.readbtn.Text = "Read/Dump";
            this.readbtn.UseVisualStyleBackColor = true;
            this.readbtn.Click += new System.EventHandler(this.ReadbtnClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.donateToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(567, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchprotectmode,
            this.switchEraseMode,
            this.selectPythonPathToolStripMenuItem,
            this.selectNORwaypyNANDWaypyPathToolStripMenuItem,
            this.verifynor,
            this.NANDInfo,
            this.KillPython,
            this.clearlog});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // switchprotectmode
            // 
            this.switchprotectmode.Name = "switchprotectmode";
            this.switchprotectmode.Size = new System.Drawing.Size(286, 22);
            this.switchprotectmode.Text = "Disable Address Correction";
            this.switchprotectmode.Click += new System.EventHandler(this.SwitchAddressCorrectionMode);
            // 
            // switchEraseMode
            // 
            this.switchEraseMode.Name = "switchEraseMode";
            this.switchEraseMode.Size = new System.Drawing.Size(286, 22);
            this.switchEraseMode.Text = "Enable Erase";
            this.switchEraseMode.Click += new System.EventHandler(this.SwitchEraseAllowed);
            // 
            // selectPythonPathToolStripMenuItem
            // 
            this.selectPythonPathToolStripMenuItem.Name = "selectPythonPathToolStripMenuItem";
            this.selectPythonPathToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
            this.selectPythonPathToolStripMenuItem.Text = "Select Python Path";
            this.selectPythonPathToolStripMenuItem.Click += new System.EventHandler(this.SelectPythonPathToolStripMenuItemClick);
            // 
            // selectNORwaypyNANDWaypyPathToolStripMenuItem
            // 
            this.selectNORwaypyNANDWaypyPathToolStripMenuItem.Name = "selectNORwaypyNANDWaypyPathToolStripMenuItem";
            this.selectNORwaypyNANDWaypyPathToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
            this.selectNORwaypyNANDWaypyPathToolStripMenuItem.Text = "Select NORway.py && NANDWay.py Path";
            this.selectNORwaypyNANDWaypyPathToolStripMenuItem.Click += new System.EventHandler(this.SelectNoRwaypyNANDWaypyPathToolStripMenuItemClick);
            // 
            // verifynor
            // 
            this.verifynor.Enabled = false;
            this.verifynor.Name = "verifynor";
            this.verifynor.Size = new System.Drawing.Size(286, 22);
            this.verifynor.Text = "Verify NOR";
            this.verifynor.Click += new System.EventHandler(this.VerifyNORClick);
            // 
            // NANDInfo
            // 
            this.NANDInfo.Enabled = false;
            this.NANDInfo.Name = "NANDInfo";
            this.NANDInfo.Size = new System.Drawing.Size(286, 22);
            this.NANDInfo.Text = "NAND Info";
            this.NANDInfo.Click += new System.EventHandler(this.NANDInfoClick);
            // 
            // KillPython
            // 
            this.KillPython.Enabled = false;
            this.KillPython.Name = "KillPython";
            this.KillPython.Size = new System.Drawing.Size(286, 22);
            this.KillPython.Text = "Kill Python (Fixes Deadlock)";
            this.KillPython.Click += new System.EventHandler(this.KillPythonClick);
            // 
            // clearlog
            // 
            this.clearlog.Enabled = false;
            this.clearlog.Name = "clearlog";
            this.clearlog.Size = new System.Drawing.Size(286, 22);
            this.clearlog.Text = "Clear Log";
            this.clearlog.Click += new System.EventHandler(this.ClearlogClick);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.donateToolStripMenuItem.Text = "Donate";
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.DonateToolStripMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hclink,
            this.colinklabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(567, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // hclink
            // 
            this.hclink.Name = "hclink";
            this.hclink.Size = new System.Drawing.Size(276, 17);
            this.hclink.Spring = true;
            this.hclink.Text = "www.homebrew-connection.org";
            this.hclink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.hclink.Click += new System.EventHandler(this.HclinkClick);
            // 
            // colinklabel
            // 
            this.colinklabel.Name = "colinklabel";
            this.colinklabel.Size = new System.Drawing.Size(276, 17);
            this.colinklabel.Spring = true;
            this.colinklabel.Text = "www.consoleopen.com";
            this.colinklabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.colinklabel.Click += new System.EventHandler(this.ColinklabelClick);
            // 
            // outgroupbox
            // 
            this.outgroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outgroupbox.Controls.Add(this.OutputBox);
            this.outgroupbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outgroupbox.Location = new System.Drawing.Point(12, 207);
            this.outgroupbox.Name = "outgroupbox";
            this.outgroupbox.Size = new System.Drawing.Size(543, 256);
            this.outgroupbox.TabIndex = 1;
            this.outgroupbox.TabStop = false;
            this.outgroupbox.Text = "Output Console";
            this.outgroupbox.Resize += new System.EventHandler(this.OutgroupboxResize);
            // 
            // logstrip
            // 
            this.logstrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.logstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLogToolStripMenuItem});
            this.logstrip.Name = "logstrip";
            this.logstrip.ShowImageMargin = false;
            this.logstrip.Size = new System.Drawing.Size(97, 26);
            this.logstrip.Opening += new System.ComponentModel.CancelEventHandler(this.LogstripOpening);
            // 
            // saveLogToolStripMenuItem
            // 
            this.saveLogToolStripMenuItem.Name = "saveLogToolStripMenuItem";
            this.saveLogToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.saveLogToolStripMenuItem.Text = "Save Log";
            this.saveLogToolStripMenuItem.Click += new System.EventHandler(this.SaveLogToolStripMenuItemClick);
            // 
            // settingsbox
            // 
            this.settingsbox.Controls.Add(this.label5);
            this.settingsbox.Controls.Add(this.label4);
            this.settingsbox.Controls.Add(this.label3);
            this.settingsbox.Controls.Add(this.diffwrite);
            this.settingsbox.Controls.Add(this.wordwriteubm);
            this.settingsbox.Controls.Add(this.wordwrite);
            this.settingsbox.Controls.Add(this.writeverify);
            this.settingsbox.Controls.Add(this.memory);
            this.settingsbox.Controls.Add(this.label2);
            this.settingsbox.Controls.Add(this.comports);
            this.settingsbox.Controls.Add(this.label1);
            this.settingsbox.Controls.Add(this.noradressbox);
            this.settingsbox.Controls.Add(this.lengthbox);
            this.settingsbox.Controls.Add(this.offsetbox);
            this.settingsbox.Controls.Add(this.dumpcount);
            this.settingsbox.Controls.Add(this.label8);
            this.settingsbox.Location = new System.Drawing.Point(12, 27);
            this.settingsbox.Name = "settingsbox";
            this.settingsbox.Size = new System.Drawing.Size(543, 96);
            this.settingsbox.TabIndex = 1;
            this.settingsbox.TabStop = false;
            this.settingsbox.Text = "Settings";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(404, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Address:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(410, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Length:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(417, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Offset:";
            // 
            // diffwrite
            // 
            this.diffwrite.AutoSize = true;
            this.diffwrite.Enabled = false;
            this.diffwrite.Location = new System.Drawing.Point(270, 45);
            this.diffwrite.Name = "diffwrite";
            this.diffwrite.Size = new System.Drawing.Size(104, 17);
            this.diffwrite.TabIndex = 4;
            this.diffwrite.Text = "Differential Flash";
            this.diffwrite.UseVisualStyleBackColor = true;
            // 
            // wordwriteubm
            // 
            this.wordwriteubm.AutoSize = true;
            this.wordwriteubm.Enabled = false;
            this.wordwriteubm.Location = new System.Drawing.Point(150, 73);
            this.wordwriteubm.Name = "wordwriteubm";
            this.wordwriteubm.Size = new System.Drawing.Size(248, 17);
            this.wordwriteubm.TabIndex = 6;
            this.wordwriteubm.Text = "Use Word Programming (Unlock Bypass Mode)";
            this.wordwriteubm.UseVisualStyleBackColor = true;
            // 
            // wordwrite
            // 
            this.wordwrite.AutoSize = true;
            this.wordwrite.Enabled = false;
            this.wordwrite.Location = new System.Drawing.Point(6, 73);
            this.wordwrite.Name = "wordwrite";
            this.wordwrite.Size = new System.Drawing.Size(138, 17);
            this.wordwrite.TabIndex = 5;
            this.wordwrite.Text = "Use Word Programming";
            this.wordwrite.UseVisualStyleBackColor = true;
            // 
            // writeverify
            // 
            this.writeverify.AutoSize = true;
            this.writeverify.Checked = true;
            this.writeverify.CheckState = System.Windows.Forms.CheckState.Checked;
            this.writeverify.Location = new System.Drawing.Point(269, 19);
            this.writeverify.Name = "writeverify";
            this.writeverify.Size = new System.Drawing.Size(105, 17);
            this.writeverify.TabIndex = 3;
            this.writeverify.Text = "Verify After Write";
            this.writeverify.UseVisualStyleBackColor = true;
            // 
            // memory
            // 
            this.memory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.memory.FormattingEnabled = true;
            this.memory.Location = new System.Drawing.Point(87, 46);
            this.memory.Name = "memory";
            this.memory.Size = new System.Drawing.Size(176, 21);
            this.memory.TabIndex = 2;
            this.memory.SelectedIndexChanged += new System.EventHandler(this.MemorySelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Memory:";
            // 
            // comports
            // 
            this.comports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comports.FormattingEnabled = true;
            this.comports.Location = new System.Drawing.Point(180, 17);
            this.comports.Name = "comports";
            this.comports.Size = new System.Drawing.Size(83, 21);
            this.comports.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(138, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Port:";
            // 
            // noradressbox
            // 
            this.noradressbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noradressbox.Enabled = false;
            this.noradressbox.Hexadecimal = true;
            this.noradressbox.Increment = new decimal(new int[] {
            131072,
            0,
            0,
            0});
            this.noradressbox.Location = new System.Drawing.Point(466, 70);
            this.noradressbox.Maximum = new decimal(new int[] {
            16646144,
            0,
            0,
            0});
            this.noradressbox.Name = "noradressbox";
            this.noradressbox.Size = new System.Drawing.Size(71, 20);
            this.noradressbox.TabIndex = 8;
            // 
            // lengthbox
            // 
            this.lengthbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lengthbox.Hexadecimal = true;
            this.lengthbox.Location = new System.Drawing.Point(466, 44);
            this.lengthbox.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.lengthbox.Name = "lengthbox";
            this.lengthbox.Size = new System.Drawing.Size(71, 20);
            this.lengthbox.TabIndex = 8;
            // 
            // offsetbox
            // 
            this.offsetbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.offsetbox.Enabled = false;
            this.offsetbox.Hexadecimal = true;
            this.offsetbox.Location = new System.Drawing.Point(468, 18);
            this.offsetbox.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.offsetbox.Name = "offsetbox";
            this.offsetbox.Size = new System.Drawing.Size(69, 20);
            this.offsetbox.TabIndex = 7;
            // 
            // dumpcount
            // 
            this.dumpcount.Location = new System.Drawing.Point(87, 18);
            this.dumpcount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.dumpcount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dumpcount.Name = "dumpcount";
            this.dumpcount.Size = new System.Drawing.Size(45, 20);
            this.dumpcount.TabIndex = 0;
            this.dumpcount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "N° Dump(s):";
            // 
            // bw
            // 
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BWDoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BWRunWorkerCompleted);
            // 
            // OutputBox
            // 
            this.OutputBox.BackColor = System.Drawing.Color.Black;
            this.OutputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OutputBox.ContextMenuStrip = this.logstrip;
            this.OutputBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.OutputBox.DetectUrls = false;
            this.OutputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputBox.ForeColor = System.Drawing.Color.Green;
            this.OutputBox.Location = new System.Drawing.Point(3, 16);
            this.OutputBox.MaximumSize = new System.Drawing.Size(537, 237);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.Size = new System.Drawing.Size(537, 237);
            this.OutputBox.TabIndex = 0;
            this.OutputBox.Text = "";
            this.OutputBox.WordWrap = false;
            // 
            // loadingCircle1
            // 
            this.loadingCircle1.Active = false;
            this.loadingCircle1.Color = System.Drawing.Color.Black;
            this.loadingCircle1.InnerCircleRadius = 8;
            this.loadingCircle1.Location = new System.Drawing.Point(457, 19);
            this.loadingCircle1.Name = "loadingCircle1";
            this.loadingCircle1.NumberSpoke = 10;
            this.loadingCircle1.OuterCircleRadius = 20;
            this.loadingCircle1.RotationSpeed = 100;
            this.loadingCircle1.Size = new System.Drawing.Size(80, 47);
            this.loadingCircle1.SpokeThickness = 2;
            this.loadingCircle1.TabIndex = 4;
            this.loadingCircle1.Text = "loading";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(567, 488);
            this.Controls.Add(this.settingsbox);
            this.Controls.Add(this.outgroupbox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.operationsbox);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.Text = "NORWay & NANDWay GUI {0}.{1} {2}";
            this.Load += new System.EventHandler(this.Form1Load);
            this.operationsbox.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.outgroupbox.ResumeLayout(false);
            this.logstrip.ResumeLayout(false);
            this.settingsbox.ResumeLayout(false);
            this.settingsbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.noradressbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dumpcount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox operationsbox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button readbtn;
        private System.Windows.Forms.Button write;
        private System.Windows.Forms.Button releasebtn;
        private System.Windows.Forms.Button erasenorbtn;
        private System.Windows.Forms.GroupBox outgroupbox;
        private System.Windows.Forms.GroupBox settingsbox;
        private System.Windows.Forms.NumericUpDown dumpcount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox memory;
        private System.Windows.Forms.CheckBox writeverify;
        private System.Windows.Forms.CheckBox wordwrite;
        private System.Windows.Forms.CheckBox wordwriteubm;
        private System.Windows.Forms.CheckBox diffwrite;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown offsetbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown lengthbox;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel colinklabel;
        private System.ComponentModel.BackgroundWorker bw;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown noradressbox;
        private System.Windows.Forms.Button initbtn;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchprotectmode;
        private System.Windows.Forms.ToolStripMenuItem switchEraseMode;
        private System.Windows.Forms.ToolStripMenuItem selectPythonPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNORwaypyNANDWaypyPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel hclink;
        private CustomControls.ThreadSafeRichTextBox OutputBox;
        internal System.Windows.Forms.ComboBox comports;
        private System.Windows.Forms.ToolStripMenuItem verifynor;
        private System.Windows.Forms.ToolStripMenuItem NANDInfo;
        private System.Windows.Forms.ContextMenuStrip logstrip;
        private System.Windows.Forms.ToolStripMenuItem saveLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem KillPython;
        private System.Windows.Forms.ToolStripMenuItem clearlog;
        private LoadingCircle loadingCircle1;
    }
}

