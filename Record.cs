namespace TimeTracker
{
    public class Record
    {
        public static string DT_FORMAT = "MMM-dd-yyyy";

        public static string[] DAYS = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        public DateOnly Date { get; set; }

        public int[] DayMinutes { get; set; } = new int[DAYS.Length];

        public int Total { get; set; } = 0;

        /// <summary>Copy - clone.</summary>
        public Record Clone()
        {
            Record record = new Record();
            record.Date = Date;
            record.DayMinutes = (int[])DayMinutes.Clone();
            record.Total = Total;
            return record;
        }

        public static Record Parse(string srecord)
        {
            var record = new Record();
            var parts = srecord.Split(',', StringSplitOptions.RemoveEmptyEntries);
            record.Date = DateOnly.ParseExact(parts[0], DT_FORMAT, null);
            for (int i = 0; i < DAYS.Length; i++)
            {
                record.DayMinutes[i] = int.Parse(parts[i + 1]);
            }

            return record;
        }

        public string FormatForDisplay()
        {
            int total = 0;
            var ls = new List<string>
            {
                Date.ToString(DT_FORMAT)
            };

            foreach(int m in DayMinutes)
            {
                ls.Add(FormatInt(m));
                total += m;
            }
            ls.Add(" ");
            ls.Add(FormatInt(total));
            return string.Join(' ', ls);
        }

        public string FormatForPersist()
        {
            var ls = new List<string>
            {
                Date.ToString(DT_FORMAT)
            };

            foreach (int m in DayMinutes)
            {
                ls.Add(m.ToString());
            }
            return string.Join(',', ls);
        }

        string FormatInt(int i) => i switch
        {
            0 => "  0",
            (>= 1) and (<= 9) => $"  {i}",
            (>= 10) and (<= 99) => $" {i}",
            (>= 100) and (<= 999) => $"{i}",
            _ => "????",
        };
    }
}
