
namespace Les12Salopards.IHM
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCalculerPrime = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMontantPrime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCalculerPrime
            // 
            this.btnCalculerPrime.Location = new System.Drawing.Point(94, 70);
            this.btnCalculerPrime.Name = "btnCalculerPrime";
            this.btnCalculerPrime.Size = new System.Drawing.Size(296, 51);
            this.btnCalculerPrime.TabIndex = 0;
            this.btnCalculerPrime.Text = "Calculer prime";
            this.btnCalculerPrime.UseVisualStyleBackColor = true;
            this.btnCalculerPrime.Click += new System.EventHandler(this.btnCalculerPrime_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Montant de la prime : ";
            // 
            // lblMontantPrime
            // 
            this.lblMontantPrime.AutoSize = true;
            this.lblMontantPrime.Location = new System.Drawing.Point(255, 178);
            this.lblMontantPrime.Name = "lblMontantPrime";
            this.lblMontantPrime.Size = new System.Drawing.Size(16, 17);
            this.lblMontantPrime.TabIndex = 2;
            this.lblMontantPrime.Text = "ff";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblMontantPrime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCalculerPrime);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCalculerPrime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMontantPrime;
    }
}

