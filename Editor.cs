namespace TimeTracker
{
    public partial class Editor : Form
    {
        public Record Record { get; set; } = new();

        List<TextBox> _tbs = new();

        public Editor()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Dynamically create controls.
            int x = 30;
            int xinc = 48;
            int i = 0;
            Font font = new("Consolas", 14F, FontStyle.Regular, GraphicsUnit.Point);

            foreach (var day in Record.DAYS)
            {
                var lbl = new Label
                {
                    AutoSize = true,
                    Font = font,
                    Location = new Point(x, 11),
                    Name = "lbl" + day,
                    Size = new Size(36, 20),
                    Text = day
                };
                Controls.Add(lbl);

                var tb = new TextBox
                {
                    Font = font,
                    Location = new Point(x, 40),
                    Margin = new Padding(3, 4, 3, 4),
                    Name = "tb" + day,
                    Size = new Size(40, 27),
                    TabIndex = i + 10,
                    Text = Record.DayMinutes[i].ToString()
                };
                tb.KeyPress += (object? sender, KeyPressEventArgs e) => TestForInteger_KeyPress(sender!, e);
                Controls.Add(tb);
                _tbs.Add(tb);

                x += xinc;
                i++;
            }

            Width = x + xinc;

            base.OnLoad(e);
        }

        void Ok_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _tbs.Count; i++)
            {
                Record.DayMinutes[i] = int.Parse(_tbs[i].Text);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void TestForInteger_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Determine whether the keystroke is an integer.
            char c = e.KeyChar;
            e.Handled = !((c >= '0' && c <= '9') || (c == '\b'));
        }
    }
}
