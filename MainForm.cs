namespace TimeTracker
{
    public partial class MainForm : Form
    {
        string _fp = "TT_data.csv";

        bool _dirty = false;

        List<Record> _records = new();

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Ensure user dir.
            string localdir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = Path.Combine(localdir, "Ephemera", "TimeTracker");
            DirectoryInfo di = new(path);
            di.Create();

            _fp = Path.Combine(path, _fp);

            if (File.Exists(_fp))
            {
                _records.Clear();
                File.ReadAllLines(_fp).ToList().ForEach(l => _records.Add(Record.Parse(l)));
            }

            // Ensure that we have a record for current week.
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            if (_records.Count == 0 || _records[0].Date != today)
            {
                _records.Insert(0, new Record() { Date = today });
            }

            _records.ForEach(x => { lbRecords.Items.Add(x.FormatForDisplay()); });

            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_dirty)
            {
                // Save file. Do some backups first.
                try { File.Copy(_fp.Replace(".", "_2."), _fp.Replace(".", "_3."), true); } catch { }
                try { File.Copy(_fp.Replace(".", "_1."), _fp.Replace(".", "_2."), true); } catch { }
                try { File.Copy(_fp, _fp.Replace(".", "_1."), true); } catch { }

                var srecs = new List<string>();
                _records.ForEach(r => srecs.Add(r.FormatForPersist()));
                File.WriteAllLines(_fp, srecs);
            }

            base.OnFormClosing(e);
        }

        void Records_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Get selected index and open the editor dialog.
            int i = lbRecords.SelectedIndex;

            if (i >= 0 && i < _records.Count)
            {
                using Editor ed = new()
                {
                    Record = _records[i].Clone(),
                    Text = $"Edit inutes for week of {_records[i].Date.ToString(Record.DT_FORMAT)}"
                };

                if (ed.ShowDialog() == DialogResult.OK)
                {
                    // Check for edits.
                    for (int j = 0; j < _records[i].DayMinutes.Length; j++)
                    {
                        _dirty |= (_records[i].DayMinutes[j] != ed.Record.DayMinutes[j]);
                    }
                    _records[i] = ed.Record;
                    lbRecords.Items[i] = _records[i].FormatForDisplay();
                }
            }
        }

        void MakeFake()
        {
            Random r = new();
            for (int i = 1; i < 30; i++)
            {
                var record = new Record();
                for (int d = 0; d < Record.DAYS.Length; d++)
                {
                    record.DayMinutes[d] = r.Next() % 100;
                }

                record.Date = new DateOnly(2023, 1, i);
                _records.Add(record);
            }
        }
    }
}