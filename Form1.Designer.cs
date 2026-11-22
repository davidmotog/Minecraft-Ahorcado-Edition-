namespace Ahorcado
{
    partial class Form1
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.start = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.exit = new System.Windows.Forms.Button();
            this.mostrar = new System.Windows.Forms.Button();
            this.hangman = new System.Windows.Forms.PictureBox();
            this.guess = new System.Windows.Forms.TextBox();
            this.doguess = new System.Windows.Forms.Button();
            this.input = new System.Windows.Forms.TextBox();
            this.debug = new System.Windows.Forms.Label();
            this.Juego = new System.Windows.Forms.GroupBox();
            this.timesGuessed = new System.Windows.Forms.Label();
            this.tries = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hangman)).BeginInit();
            this.Juego.SuspendLayout();
            this.SuspendLayout();
            // 
            // start
            // 
            resources.ApplyResources(this.start, "start");
            this.start.Name = "start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.exit);
            this.groupBox1.Controls.Add(this.mostrar);
            this.groupBox1.Controls.Add(this.start);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // exit
            // 
            resources.ApplyResources(this.exit, "exit");
            this.exit.Name = "exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // mostrar
            // 
            resources.ApplyResources(this.mostrar, "mostrar");
            this.mostrar.Name = "mostrar";
            this.mostrar.UseVisualStyleBackColor = true;
            this.mostrar.Click += new System.EventHandler(this.mostrar_Click);
            // 
            // hangman
            // 
            resources.ApplyResources(this.hangman, "hangman");
            this.hangman.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hangman.Image = global::Ahorcado.Properties.Resources.Hangman_0;
            this.hangman.Name = "hangman";
            this.hangman.TabStop = false;
            this.hangman.Click += new System.EventHandler(this.hangman_Click);
            // 
            // guess
            // 
            resources.ApplyResources(this.guess, "guess");
            this.guess.Name = "guess";
            this.guess.ReadOnly = true;
            this.guess.TextChanged += new System.EventHandler(this.guess_TextChanged);
            // 
            // doguess
            // 
            resources.ApplyResources(this.doguess, "doguess");
            this.doguess.Name = "doguess";
            this.doguess.UseVisualStyleBackColor = true;
            this.doguess.Click += new System.EventHandler(this.doguess_Click);
            // 
            // input
            // 
            resources.ApplyResources(this.input, "input");
            this.input.Name = "input";
            this.input.TextChanged += new System.EventHandler(this.input_TextChanged);
            // 
            // debug
            // 
            resources.ApplyResources(this.debug, "debug");
            this.debug.Name = "debug";
            // 
            // Juego
            // 
            resources.ApplyResources(this.Juego, "Juego");
            this.Juego.Controls.Add(this.timesGuessed);
            this.Juego.Controls.Add(this.tries);
            this.Juego.Controls.Add(this.doguess);
            this.Juego.Controls.Add(this.input);
            this.Juego.Name = "Juego";
            this.Juego.TabStop = false;
            // 
            // timesGuessed
            // 
            resources.ApplyResources(this.timesGuessed, "timesGuessed");
            this.timesGuessed.Name = "timesGuessed";
            // 
            // tries
            // 
            resources.ApplyResources(this.tries, "tries");
            this.tries.Name = "tries";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Juego);
            this.Controls.Add(this.debug);
            this.Controls.Add(this.guess);
            this.Controls.Add(this.hangman);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hangman)).EndInit();
            this.Juego.ResumeLayout(false);
            this.Juego.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button mostrar;
        private System.Windows.Forms.PictureBox hangman;
        private System.Windows.Forms.TextBox guess;
        private System.Windows.Forms.Button doguess;
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Label debug;
        private System.Windows.Forms.GroupBox Juego;
        private System.Windows.Forms.Label tries;
        private System.Windows.Forms.Label timesGuessed;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
