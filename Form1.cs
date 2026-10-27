using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lab2Grupp1GUI
{
    public partial class Form1 : Form
    {
        private ReservationsController _controller;
        private Användare _aktivAnvändare;
        private bool _visarAnvändarVy = false;

        // Paneler för att växla mellan vyer
        private Panel pnlLogin;
        private Panel pnlRegister;
        private Panel pnlMain;

        // Container-paneler för högersidans knappar (förhindrar överlappning)
        private Panel pnlStudentActions;
        private Panel pnlAdminActions;

        // Inloggningskontroller
        private TextBox txtEpost;
        private TextBox txtLösenord;

        // Registreringskontroller
        private TextBox txtRegNamn;
        private TextBox txtRegEpost;
        private TextBox txtRegLösen;

        // Huvudmeny-kontroller
        private Label lblVälkommen;
        private Label lblRubrik;
        private ListBox lstAnnonser;
        private ListBox lstAnvändare;

        // Student-knappar
        private Button btnReservera;
        private Button btnKöp;
        private Button btnSkapaAnnons;

        // Admin-knappar
        private Button btnAdminVäxlaVy;
        private Button btnAdminTaBortAnnons;
        private Button btnAdminRedigeraAnnons;
        private Button btnAdminSkapaAnvändare;
        private Button btnAdminRedigeraAnvändare;
        private Button btnAdminTaBortAnvändare;

        private Button btnLoggaUt;

        public Form1()
        {
            InitializeComponent();
            _controller = new ReservationsController();

            // Fönsterinställningar
            this.Text = "BookSwap - Digital handel av kurslitteratur";
            this.Width = 840;
            this.Height = 620;
            this.StartPosition = FormStartPosition.CenterScreen;

            SkapaInloggningsVy();
            SkapaRegistreringsVy();
            SkapaHuvudmenyVy();

            VisaInloggningsVy();
        }

        // ==================== 1. INLOGGNINGSVY ====================
        private void SkapaInloggningsVy()
        {
            pnlLogin = new Panel() { Dock = DockStyle.Fill };

            Label lblTitel = new Label() { Text = "VÄLKOMMEN TILL BOOKSWAP", Font = new Font("Arial", 16, FontStyle.Bold), Left = 220, Top = 50, Width = 400 };
            Label lblEpost = new Label() { Text = "E-post:", Left = 220, Top = 140, Width = 100 };
            txtEpost = new TextBox() { Left = 320, Top = 140, Width = 200 };

            Label lblLösen = new Label() { Text = "Lösenord:", Left = 220, Top = 180, Width = 100 };
            txtLösenord = new TextBox() { Left = 320, Top = 180, Width = 200, PasswordChar = '*' };

            Button btnLoggaIn = new Button() { Text = "Logga in", Left = 320, Top = 230, Width = 100, Height = 35, BackColor = Color.LightGreen };
            btnLoggaIn.Click += (s, e) =>
            {
                _aktivAnvändare = _controller.LoggaIn(txtEpost.Text, txtLösenord.Text);
                if (_aktivAnvändare == null)
                {
                    MessageBox.Show("Felaktig e-post eller lösenord!", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txtEpost.Clear();
                    txtLösenord.Clear();
                    _visarAnvändarVy = false;
                    UppdateraVyer();
                    UppdateraMenyEfterRoll();
                    VisaHuvudmenyVy();
                }
            };

            Button btnGåTillReg = new Button() { Text = "Registrera nytt konto", Left = 430, Top = 230, Width = 140, Height = 35 };
            btnGåTillReg.Click += (s, e) => { pnlRegister.BringToFront(); };

            pnlLogin.Controls.AddRange(new Control[] { lblTitel, lblEpost, txtEpost, lblLösen, txtLösenord, btnLoggaIn, btnGåTillReg });
            this.Controls.Add(pnlLogin);
        }

        // ==================== 2. REGISTRERINGSVY ====================
        private void SkapaRegistreringsVy()
        {
            pnlRegister = new Panel() { Dock = DockStyle.Fill };

            Label lblTitel = new Label() { Text = "REGISTRERA NYTT STUDENTKONTO", Font = new Font("Arial", 14, FontStyle.Bold), Left = 220, Top = 50, Width = 400 };
            Label lblNamn = new Label() { Text = "Fullständigt namn:", Left = 200, Top = 120, Width = 120 };
            txtRegNamn = new TextBox() { Left = 330, Top = 120, Width = 200 };

            Label lblEpost = new Label() { Text = "E-post:", Left = 200, Top = 160, Width = 120 };
            txtRegEpost = new TextBox() { Left = 330, Top = 160, Width = 200 };

            Label lblLösen = new Label() { Text = "Lösenord:", Left = 200, Top = 200, Width = 120 };
            txtRegLösen = new TextBox() { Left = 330, Top = 200, Width = 200, PasswordChar = '*' };

            Button btnRegistrera = new Button() { Text = "Skapa konto", Left = 330, Top = 250, Width = 100, Height = 35, BackColor = Color.LightBlue };
            btnRegistrera.Click += (s, e) =>
            {
                try
                {
                    _aktivAnvändare = _controller.RegistreraStudent(txtRegNamn.Text, txtRegEpost.Text, txtRegLösen.Text);
                    MessageBox.Show("Studentkonto skapat!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRegNamn.Clear();
                    txtRegEpost.Clear();
                    txtRegLösen.Clear();
                    _visarAnvändarVy = false;
                    UppdateraVyer();
                    UppdateraMenyEfterRoll();
                    VisaHuvudmenyVy();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fel vid registrering", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            Button btnTillbaka = new Button() { Text = "Tillbaka till inloggning", Left = 440, Top = 250, Width = 140, Height = 35 };
            btnTillbaka.Click += (s, e) => { pnlLogin.BringToFront(); };

            pnlRegister.Controls.AddRange(new Control[] { lblTitel, lblNamn, txtRegNamn, lblEpost, txtRegEpost, lblLösen, txtRegLösen, btnRegistrera, btnTillbaka });
            this.Controls.Add(pnlRegister);
        }

        // ==================== 3. HUVUDMENY VY ====================
        private void SkapaHuvudmenyVy()
        {
            pnlMain = new Panel() { Dock = DockStyle.Fill };

            lblVälkommen = new Label() { Font = new Font("Arial", 11, FontStyle.Bold), Left = 20, Top = 15, Width = 400 };
            lblRubrik = new Label() { Font = new Font("Arial", 10, FontStyle.Bold), Left = 20, Top = 60, Width = 350, Text = "Tillgängliga annonser i systemet:" };

            // Sökfält för annonser (sök på titel eller ID)
            TextBox txtSök = new TextBox() { Left = 20, Top = 85, Width = 200, PlaceholderText = "Sök titel eller ID..." };
            Button btnSökAktivera = new Button() { Text = "Sök", Left = 230, Top = 83, Width = 90, Height = 25 };
            btnSökAktivera.Click += (s, e) =>
            {
                if (_visarAnvändarVy) return;
                lstAnnonser.Items.Clear();
                var resultat = _controller.SökAnnonser(txtSök.Text);
                foreach (var a in resultat)
                {
                    string kursInfo = a.TillhörandeKurs != null ? $" | Kurs: {a.TillhörandeKurs.Kursnamn}" : "";
                    lstAnnonser.Items.Add($"[ID: {a.Id}] [{a.Status}] {a.Titel}{kursInfo} - {a.Pris} kr | Skick: {a.Skick} | Säljare: {a.Säljare.Namn}");
                }
            };

            Button btnVisaAlla = new Button() { Text = "Visa alla", Left = 330, Top = 83, Width = 90, Height = 25 };
            btnVisaAlla.Click += (s, e) => UppdateraVyer();

            // Listboxar
            lstAnnonser = new ListBox() { Left = 20, Top = 120, Width = 510, Height = 420 };
            lstAnvändare = new ListBox() { Left = 20, Top = 120, Width = 510, Height = 420, Visible = false };

            // --- STUDENT PANEL ---
            pnlStudentActions = new Panel() { Left = 550, Top = 120, Width = 240, Height = 380 };

            btnReservera = new Button() { Text = "Reservera vald annons", Left = 0, Top = 0, Width = 230, Height = 35 };
            btnReservera.Click += (s, e) =>
            {
                if (lstAnnonser.SelectedIndex >= 0)
                {
                    var annonser = _controller.TillgängligaAnnonser;
                    if (lstAnnonser.SelectedIndex < annonser.Count)
                    {
                        var vald = annonser[lstAnnonser.SelectedIndex];
                        try
                        {
                            _controller.ReserveraAnnons(vald, (Student)_aktivAnvändare);
                            MessageBox.Show("Annonsen har reserverats!", "Klart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UppdateraVyer();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Fel", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    }
                }
                else { MessageBox.Show("Välj en annons i listan först."); }
            };

            btnKöp = new Button() { Text = "Bekräfta köp (Reserverad)", Left = 0, Top = 45, Width = 230, Height = 35 };
            btnKöp.Click += (s, e) =>
            {
                if (lstAnnonser.SelectedIndex >= 0)
                {
                    var alla = _controller.AllaAnnonser;
                    var vald = alla[lstAnnonser.SelectedIndex];
                    try
                    {
                        Affär kvitto = _controller.BekräftaKöp(vald, (Student)_aktivAnvändare);
                        MessageBox.Show($"Köp genomfört! Kvitto skapat för '{kvitto.Annons.Titel}'.", "Grattis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UppdateraVyer();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Fel", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
            };

            btnSkapaAnnons = new Button() { Text = "Skapa ny annons", Left = 0, Top = 90, Width = 230, Height = 35, BackColor = Color.LightGreen };
            btnSkapaAnnons.Click += (s, e) =>
            {
                Form adForm = new Form() { Width = 350, Height = 280, Text = "Skapa ny annons", StartPosition = FormStartPosition.CenterParent };

                TextBox tTitel = new TextBox() { Left = 20, Top = 20, Width = 290, PlaceholderText = "Boktitel" };
                TextBox tPris = new TextBox() { Left = 20, Top = 60, Width = 290, PlaceholderText = "Pris (kr)" };

                ComboBox cSkick = new ComboBox() { Left = 20, Top = 100, Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
                cSkick.Items.AddRange(Enum.GetValues(typeof(AnnonsSkick)).Cast<object>().ToArray());
                cSkick.SelectedIndex = 0;

                TextBox tKurs = new TextBox() { Left = 20, Top = 140, Width = 290, PlaceholderText = "Kursnamn" };

                Button bSparaAnnons = new Button() { Text = "Publicera", Left = 20, Top = 185, Width = 100, Height = 30 };
                bSparaAnnons.Click += (sender, args) =>
                {
                    try
                    {
                        int pris = int.Parse(tPris.Text);
                        _controller.SkapaAnnons((Student)_aktivAnvändare, tTitel.Text, pris, (AnnonsSkick)cSkick.SelectedItem, tKurs.Text);
                        MessageBox.Show("Annons skapad!");
                        UppdateraVyer();
                        adForm.Close();
                    }
                    catch (Exception ex) { MessageBox.Show("Fel: " + ex.Message); }
                };

                adForm.Controls.AddRange(new Control[] { tTitel, tPris, cSkick, tKurs, bSparaAnnons });
                adForm.ShowDialog();
            };

            pnlStudentActions.Controls.AddRange(new Control[] { btnReservera, btnKöp, btnSkapaAnnons });


            // --- ADMIN PANEL ---
            pnlAdminActions = new Panel() { Left = 550, Top = 120, Width = 240, Height = 380, Visible = false };

            btnAdminVäxlaVy = new Button() { Text = "🔄 Växla till Användarlista", Left = 0, Top = 0, Width = 230, Height = 40, BackColor = Color.LightSteelBlue, Font = new Font("Arial", 9, FontStyle.Bold) };
            btnAdminVäxlaVy.Click += (s, e) =>
            {
                _visarAnvändarVy = !_visarAnvändarVy;
                UppdateraAdminVyStatus();
            };

            // Admin - Annonshantering
            btnAdminTaBortAnnons = new Button() { Text = "[ADMIN] Ta bort annons", Left = 0, Top = 50, Width = 230, Height = 35, BackColor = Color.MistyRose };
            btnAdminTaBortAnnons.Click += (s, e) =>
            {
                if (lstAnnonser.SelectedIndex >= 0)
                {
                    var vald = _controller.AllaAnnonser[lstAnnonser.SelectedIndex];
                    try
                    {
                        _controller.AdminTaBortAnnons((Administratör)_aktivAnvändare, vald);
                        MessageBox.Show("Annonsen har tagits bort av administratören.");
                        UppdateraVyer();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else { MessageBox.Show("Välj en annons i listan först."); }
            };

            btnAdminRedigeraAnnons = new Button() { Text = "[ADMIN] Redigera annons", Left = 0, Top = 95, Width = 230, Height = 35, BackColor = Color.LemonChiffon };
            btnAdminRedigeraAnnons.Click += (s, e) =>
            {
                if (lstAnnonser.SelectedIndex >= 0)
                {
                    var valdAnnons = _controller.AllaAnnonser[lstAnnonser.SelectedIndex];

                    Form editForm = new Form() { Width = 320, Height = 270, Text = "Redigera annons", StartPosition = FormStartPosition.CenterParent };
                    TextBox txtTitel = new TextBox() { Left = 20, Top = 20, Width = 260, Text = valdAnnons.Titel };
                    TextBox txtPris = new TextBox() { Left = 20, Top = 60, Width = 260, Text = valdAnnons.Pris.ToString() };

                    ComboBox cmbSkick = new ComboBox() { Left = 20, Top = 100, Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
                    cmbSkick.Items.AddRange(Enum.GetValues(typeof(AnnonsSkick)).Cast<object>().ToArray());
                    cmbSkick.SelectedItem = valdAnnons.Skick;

                    Button bSpara = new Button() { Text = "Spara ändringar", Left = 20, Top = 150, Width = 120, Height = 30 };
                    bSpara.Click += (sender, args) =>
                    {
                        try
                        {
                            int pris = int.Parse(txtPris.Text);
                            _controller.AdminRedigeraAnnons((Administratör)_aktivAnvändare, valdAnnons, txtTitel.Text, pris, (AnnonsSkick)cmbSkick.SelectedItem);
                            MessageBox.Show("Annonsen har uppdaterats!");
                            UppdateraVyer();
                            editForm.Close();
                        }
                        catch (Exception ex) { MessageBox.Show("Fel vid redigering: " + ex.Message); }
                    };

                    editForm.Controls.AddRange(new Control[] { txtTitel, txtPris, cmbSkick, bSpara });
                    editForm.ShowDialog();
                }
                else { MessageBox.Show("Välj en annons i listan först."); }
            };

            // Admin - Användarhantering
            btnAdminSkapaAnvändare = new Button() { Text = "[ADMIN] Skapa ny användare", Left = 0, Top = 50, Width = 230, Height = 35, BackColor = Color.LightYellow };
            btnAdminSkapaAnvändare.Click += (s, e) =>
            {
                Form form = new Form() { Width = 320, Height = 280, Text = "Skapa användare", StartPosition = FormStartPosition.CenterParent };
                TextBox tNamn = new TextBox() { Left = 20, Top = 20, Width = 260, PlaceholderText = "Namn" };
                TextBox tEpost = new TextBox() { Left = 20, Top = 60, Width = 260, PlaceholderText = "E-post" };
                TextBox tLösen = new TextBox() { Left = 20, Top = 100, Width = 260, PasswordChar = '*', PlaceholderText = "Lösenord" };

                CheckBox chkAdmin = new CheckBox() { Text = "Skapa som Administratör", Left = 20, Top = 140, Width = 220 };

                Button bSpara = new Button() { Text = "Spara", Left = 20, Top = 180, Width = 100 };
                bSpara.Click += (sender, args) =>
                {
                    try
                    {
                        _controller.AdminSkapaAnvändare((Administratör)_aktivAnvändare, tNamn.Text, tEpost.Text, tLösen.Text, chkAdmin.Checked);
                        MessageBox.Show("Användare skapad!");
                        UppdateraVyer();
                        form.Close();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                };
                form.Controls.AddRange(new Control[] { tNamn, tEpost, tLösen, chkAdmin, bSpara });
                form.ShowDialog();
            };

            btnAdminRedigeraAnvändare = new Button() { Text = "[ADMIN] Redigera användare", Left = 0, Top = 95, Width = 230, Height = 35, BackColor = Color.LemonChiffon };
            btnAdminRedigeraAnvändare.Click += (s, e) =>
            {
                if (lstAnvändare.SelectedIndex >= 0)
                {
                    var valdAnvändare = _controller.AllaAnvändare[lstAnvändare.SelectedIndex];

                    Form editUserForm = new Form() { Width = 330, Height = 280, Text = "Redigera användare", StartPosition = FormStartPosition.CenterParent };
                    TextBox tNamn = new TextBox() { Left = 20, Top = 20, Width = 260, Text = valdAnvändare.Namn };
                    TextBox tEpost = new TextBox() { Left = 20, Top = 60, Width = 260, Text = valdAnvändare.Epost };

                    CheckBox chkAdmin = new CheckBox() { Text = "Är Administratör", Left = 20, Top = 105, Width = 220, Checked = valdAnvändare is Administratör };

                    Button bSpara = new Button() { Text = "Spara ändringar", Left = 20, Top = 150, Width = 130, Height = 30 };
                    bSpara.Click += (sender, args) =>
                    {
                        try
                        {
                            _controller.AdminRedigeraAnvändare((Administratör)_aktivAnvändare, valdAnvändare, tNamn.Text, tEpost.Text, chkAdmin.Checked);
                            MessageBox.Show("Användaren har uppdaterats!");
                            UppdateraVyer();
                            editUserForm.Close();
                        }
                        catch (Exception ex) { MessageBox.Show("Fel vid redigering: " + ex.Message); }
                    };

                    editUserForm.Controls.AddRange(new Control[] { tNamn, tEpost, chkAdmin, bSpara });
                    editUserForm.ShowDialog();
                }
                else { MessageBox.Show("Välj en användare i listan först."); }
            };

            btnAdminTaBortAnvändare = new Button() { Text = "[ADMIN] Ta bort användare", Left = 0, Top = 140, Width = 230, Height = 35, BackColor = Color.MistyRose };
            btnAdminTaBortAnvändare.Click += (s, e) =>
            {
                if (lstAnvändare.SelectedIndex >= 0)
                {
                    var valdAnvändare = _controller.AllaAnvändare[lstAnvändare.SelectedIndex];
                    try
                    {
                        _controller.AdminTaBortAnvändare((Administratör)_aktivAnvändare, valdAnvändare);
                        MessageBox.Show("Användaren har tagits bort.");
                        UppdateraVyer();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else { MessageBox.Show("Välj en användare i listan först."); }
            };

            pnlAdminActions.Controls.AddRange(new Control[] { btnAdminVäxlaVy, btnAdminTaBortAnnons, btnAdminRedigeraAnnons, btnAdminSkapaAnvändare, btnAdminRedigeraAnvändare, btnAdminTaBortAnvändare });


            // Gemensam Logga ut-knapp längst ner till höger
            btnLoggaUt = new Button() { Text = "Logga ut", Left = 550, Top = 510, Width = 230, Height = 30 };
            btnLoggaUt.Click += (s, e) =>
            {
                _aktivAnvändare = null;
                pnlLogin.BringToFront();
            };

            pnlMain.Controls.AddRange(new Control[] { lblVälkommen, lblRubrik, txtSök, btnSökAktivera, btnVisaAlla, lstAnnonser, lstAnvändare, pnlStudentActions, pnlAdminActions, btnLoggaUt });
            this.Controls.Add(pnlMain);
        }

        private void VisaInloggningsVy()
        {
            pnlLogin.BringToFront();
        }

        private void VisaHuvudmenyVy()
        {
            lblVälkommen.Text = $"Inloggad som: {_aktivAnvändare.Namn} ({_aktivAnvändare.GetType().Name})";
            pnlMain.BringToFront();
        }

        private void UppdateraVyer()
        {
            lstAnnonser.Items.Clear();
            foreach (var a in _controller.AllaAnnonser)
            {
                string kursInfo = a.TillhörandeKurs != null ? $" | Kurs: {a.TillhörandeKurs.Kursnamn}" : "";
                lstAnnonser.Items.Add($"[ID: {a.Id}] [{a.Status}] {a.Titel}{kursInfo} - {a.Pris} kr | Skick: {a.Skick} | Säljare: {a.Säljare.Namn}");
            }

            lstAnvändare.Items.Clear();
            foreach (var u in _controller.AllaAnvändare)
            {
                string roll = u is Administratör ? "Administratör" : "Student";
                lstAnvändare.Items.Add($"{u.Namn} ({u.Epost}) - Roll: {roll}");
            }
        }

        private void UppdateraMenyEfterRoll()
        {
            if (_aktivAnvändare is Administratör)
            {
                pnlStudentActions.Visible = false;
                pnlAdminActions.Visible = true;
                _visarAnvändarVy = false;
                UppdateraAdminVyStatus();
            }
            else
            {
                pnlStudentActions.Visible = true;
                pnlAdminActions.Visible = false;

                lstAnnonser.Visible = true;
                lstAnvändare.Visible = false;
                lblRubrik.Text = "Tillgängliga annonser i systemet:";
                UppdateraVyer();
            }
        }

        private void UppdateraAdminVyStatus()
        {
            if (_visarAnvändarVy)
            {
                btnAdminVäxlaVy.Text = "🔄 Växla till Annonslista";
                lblRubrik.Text = "Systemets registrerade användare:";
                lstAnnonser.Visible = false;
                lstAnvändare.Visible = true;

                // Visa användar-admin knappar, dölj annons-admin knappar
                btnAdminTaBortAnnons.Visible = false;
                btnAdminRedigeraAnnons.Visible = false;

                btnAdminSkapaAnvändare.Visible = true;
                btnAdminRedigeraAnvändare.Visible = true;
                btnAdminTaBortAnvändare.Visible = true;
            }
            else
            {
                btnAdminVäxlaVy.Text = "🔄 Växla till Annonslista";
                lblRubrik.Text = "Alla annonser i systemet (Admin-läge):";
                lstAnnonser.Visible = true;
                lstAnvändare.Visible = false;

                // Visa annons-admin knappar, dölj användar-admin knappar
                btnAdminTaBortAnnons.Visible = true;
                btnAdminRedigeraAnnons.Visible = true;

                btnAdminSkapaAnvändare.Visible = false;
                btnAdminRedigeraAnvändare.Visible = false;
                btnAdminTaBortAnvändare.Visible = false;
            }
        }
    }
}
