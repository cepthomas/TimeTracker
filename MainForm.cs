using Microsoft.VisualBasic;

namespace TimeTracker
{
    public partial class MainForm : Form
    {
        string _dataPath = "";
        string _dataFile = "TT_data.csv";
        string _logFile = "TT_log.txt";
        string _configFile = "config.txt";
        bool _dirty = false;
        List<Record> _records = new();

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            var cfig = File.ReadAllLines(_configFile);
            foreach (var l in cfig)
            {
                if (l.StartsWith("#"))
                {
                    // skip comment
                }
                else
                {
                    var parts = l.Split('=', StringSplitOptions.RemoveEmptyEntries);
                    if (parts[0] == "data")
                    {
                        _dataPath = parts[1];
                    }
                }
            }

            _dataFile = Path.Combine(_dataPath, _dataFile);
            _logFile = Path.Combine(_dataPath, _logFile);

            Log("Starting.");

            if (File.Exists(_dataFile))
            {
                _records.Clear();
                var lines = File.ReadAllLines(_dataFile);
                foreach (var l in lines)
                {
                    try
                    {
                        var rec = Record.Parse(l);
                        _records.Add(rec);
                    }
                    catch (Exception)
                    {
                        Log($"Bad record:{l}");
                        MessageBox.Show("Bad record");
                    }
                }
            }

            // Ensure that we have a record for current week.
            DateOnly monday = DateOnly.FromDateTime(DateTime.Now);

            while(monday.DayOfWeek != DayOfWeek.Monday)
            {
                monday = monday.AddDays(-1);
            }

            if (_records.Count == 0 || _records[0].Date != monday)
            {
                _records.Insert(0, new Record() { Date = monday });
            }

            _records.ForEach(x => { lbRecords.Items.Add(x.FormatForDisplay()); });

            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_dirty)
            {
                // Save file. Do some backups first.
                try { File.Copy(_dataFile.Replace(".", "_2."), _dataFile.Replace(".", "_3."), true); } catch { }
                try { File.Copy(_dataFile.Replace(".", "_1."), _dataFile.Replace(".", "_2."), true); } catch { }
                try { File.Copy(_dataFile, _dataFile.Replace(".", "_1."), true); } catch { }

                var srecs = new List<string>();
                _records.ForEach(r => srecs.Add(r.FormatForPersist()));
                File.WriteAllLines(_dataFile, srecs);
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
                    Text = $"Edit minutes for week of {_records[i].Date.ToString(Record.DT_FORMAT)}"
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

        public void Log(string msg)
        {
            File.AppendAllText(_logFile, $"{DateTime.Now} {msg} {Environment.NewLine}");
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